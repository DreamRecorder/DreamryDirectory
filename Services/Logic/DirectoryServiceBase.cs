using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Logic.Tokens;
using DreamRecorder.Directory.ServiceProvider;
using DreamRecorder.Directory.Services.General;
using DreamRecorder.Directory.Services.Logic.Entities;
using DreamRecorder.Directory.Services.Logic.Permissions;
using DreamRecorder.ToolBox.General;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using KnownPermissionGroups = DreamRecorder . Directory . Services . Logic . Entities . KnownPermissionGroups ;
using Permission = DreamRecorder.Directory.Services.Logic.Permissions.Permission;
using PermissionGroup = DreamRecorder.Directory.Logic.PermissionGroup;

namespace DreamRecorder.Directory.Services.Logic
{

	[PublicAPI]
	public class DirectoryServiceBase : IDirectoryService, IDirectoryServiceInternal
	{

		public ILogger<DirectoryServiceBase> Logger { get; set; } = StaticServiceProvider.Provider.
			GetService<ILoggerFactory>().
			CreateLogger<DirectoryServiceBase>();

		public IAccessTokenProvider AccessTokenProvider { get; set; }

		public IEntityTokenProvider EntityTokenProvider { get; set; }

		public RNGCryptoServiceProvider RngProvider { get; set; } = new RNGCryptoServiceProvider();

		public EntityToken ServiceToken { get; }

		public virtual ILoginServiceProvider LoginServiceProvider { get; }

		public virtual IDirectoryServiceProvider DirectoryServiceProvider { get; }

		public EntityToken AnonymousToken { get; set; }

		public TokenStorage<EntityToken> IssuedEntityTokens { get; set; }

		public TokenStorage<AccessToken> IssuedAccessTokens { get; set; }

		public EntityToken Login(LoginToken token)
		{
			if (token == null)
			{
				return AnonymousToken;
			}

			token.CheckTokenTime();

			LoginService issuer =
				DirectoryDatabase.LoginServices.SingleOrDefault(
																	loginProvider
																		=> loginProvider.Guid == token.Issuer);

			if (!(issuer is null))
			{
				Entity target = DirectoryDatabase.FindEntity(token.Owner);

				if (target != null)
				{
					if (target.GetIsDisabled())
					{
						throw new EntityDisabledException(target.Guid);
					}

					if (target.GetCanLoginFrom().Contains(issuer.Guid))
					{
						ILoginService loginService = LoginServiceProvider.GetLoginService(issuer);

						loginService.CheckToken(AccessTokenProvider.Access(issuer.Guid), token);

						EntityToken resultToken = IssueEntityToken(
																	target,
																	TokenPolicy.EntityTokenLife(target));

						return resultToken;
					}
					else
					{
						throw new PermissionDeniedException();
					}
				}
				else
				{
					throw new EntityNotFoundException(token);
				}
			}
			else
			{
				throw new InvalidIssuerException();
			}
		}

		public EntityToken UpdateToken(EntityToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity target = DirectoryDatabase.FindEntity(token.Owner);

			if (target != null)
			{
				if (target.GetIsDisabled())
				{
					throw new EntityDisabledException(target.Guid);
				}

				return IssueEntityToken(target, TimeSpan.FromHours(1));
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public void DisposeToken([NotNull] EntityToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			if (token.Issuer == ServiceEntity.Guid)
			{
				IssuedEntityTokens.DisposeToken(token);
			}
			else
			{
				DirectoryService issuer =
					DirectoryDatabase.DirectoryServices.FirstOrDefault(
																			directoryService
																				=> directoryService.Guid
																					== token.Issuer);
				if (!(issuer is null))
				{
					IDirectoryService issuerService = DirectoryServiceProvider.GetDirectoryProvider(issuer);

					issuerService.DisposeToken(token);
				}
			}
		}

		public void DisposeToken(LoginToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}


			LoginService issuer =
				DirectoryDatabase.LoginServices.FirstOrDefault(
																	directoryService
																		=> directoryService.Guid == token.Issuer);
			if (!(issuer is null))
			{
				ILoginService issuerService = LoginServiceProvider.GetLoginService(issuer);

				issuerService.DisposeToken(token);
			}
		}

		public void DisposeToken(AccessToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			if (token.Issuer == ServiceEntity.Guid)
			{
				IssuedAccessTokens.DisposeToken(token);
			}
			else
			{
				DirectoryService issuer =
					DirectoryDatabase.DirectoryServices.FirstOrDefault(
																			directoryService
																				=> directoryService.Guid
																					== token.Issuer);
				if (!(issuer is null))
				{
					IDirectoryService issuerService = DirectoryServiceProvider.GetDirectoryProvider(issuer);

					issuerService.DisposeToken(token);
				}
			}
		}

		public AccessToken Access(EntityToken token, Guid target)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Entity accessTarget = DirectoryDatabase.FindEntity(target);

				if (accessTarget != null)
				{
					return IssueAccessToken(
											requester,
											accessTarget,
											TokenPolicy.AccessTokenLife(requester, accessTarget));
				}
				else
				{
					throw new TargetEntityNotFoundException(target);
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public string GetProperty(EntityToken token, Guid target, string name)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Entity accessTarget = DirectoryDatabase.FindEntity(target);

				if (accessTarget != null)
				{
					name = name.Normalize(NormalizationForm.FormD);

					EntityProperty property =
						accessTarget.Properties.SingleOrDefault(prop => prop.Name == name);

					if (property != null)
					{
						AccessType validAccess = property.Access(requester);

						if (validAccess.HasFlag(AccessType.Read))
						{
							return property.Value;
						}
						else
						{
							throw new PermissionDeniedException();
						}
					}
					else
					{
						return null;
					}
				}
				else
				{
					throw new TargetEntityNotFoundException(target);
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}


		public void SetProperty(EntityToken token, Guid target, string name, string value)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Entity accessTarget = DirectoryDatabase.FindEntity(target);

				if (accessTarget != null)
				{
					name = name.Normalize(NormalizationForm.FormD);

					EntityProperty property =
						accessTarget.Properties.SingleOrDefault(prop => prop.Name == name);

					if (property != null)
					{
						AccessType validAccess = property.Access(requester);

						if (validAccess.HasFlag(AccessType.Write))
						{
							property.Value = value;
						}
						else
						{
							throw new PermissionDeniedException();
						}
					}
					else
					{
						accessTarget.Properties.Add(
														new EntityProperty
														{
															Name = name,
															Owner = requester,
															Value = value
														});
					}
				}
				else
				{
					throw new TargetEntityNotFoundException(target);
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public void TransferProperty ( EntityToken token , Guid target , string name , Guid newOwner )
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Entity propertyOwner = DirectoryDatabase.FindEntity(target);

				if (propertyOwner != null)
				{
					name = name.Normalize(NormalizationForm.FormD);

					EntityProperty property =
						propertyOwner.Properties.SingleOrDefault(prop => prop.Name == name);

					if (property != null)
					{
						if (property.Owner.Contain(requester))
						{
							Entity newOwnerEntity = DirectoryDatabase.FindEntity(newOwner);

							if ( newOwnerEntity != null )
							{
								property . Owner = newOwnerEntity ;
							}
							else
							{
								throw new TargetEntityNotFoundException(newOwner);
							}
						}
						else
						{
							throw new PermissionDeniedException();
						}
					}
					else
					{
						throw new TargetPropertyNotFoundException(target, name);
					}
				}
				else
				{
					throw new TargetEntityNotFoundException(target);
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public AccessType AccessProperty(EntityToken token, Guid target, string name)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Entity propertyTarget = DirectoryDatabase.FindEntity(target);

				if (propertyTarget != null)
				{
					name = name.Normalize(NormalizationForm.FormD);

					EntityProperty property =
						propertyTarget.Properties.SingleOrDefault(prop => prop.Name == name);

					if (property != null)
					{
						AccessType validAccess = property.Access(requester);

						return validAccess;
					}
					else
					{
						throw new TargetPropertyNotFoundException(target, name);
					}
				}
				else
				{
					throw new TargetEntityNotFoundException(target);
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public void SetPropertyPermission(EntityToken token, Guid target, string name, Guid permissionGroup)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Entity propertyOwner = DirectoryDatabase.FindEntity(target);

				if (propertyOwner != null)
				{
					name = name.Normalize(NormalizationForm.FormD);

					EntityProperty property =
						propertyOwner.Properties.SingleOrDefault(prop => prop.Name == name);

					if (property != null)
					{
						if (property.Owner.Contain(requester))
						{
							Permissions.PermissionGroup targetPermissionGroup = DirectoryDatabase.FindPermissionGroup(permissionGroup);

							if (targetPermissionGroup != null)
							{
								property.Permissions = targetPermissionGroup;
							}
							else
							{
								throw new TargetPermissionGroupNotFoundException(permissionGroup);
							}

						}
						else
						{
							throw new PermissionDeniedException();
						}
					}
					else
					{
						throw new TargetPropertyNotFoundException(target, name);
					}
				}
				else
				{
					throw new TargetEntityNotFoundException(target);
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public PermissionGroup GetPermissionGroup(EntityToken token, Guid target)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Permissions.PermissionGroup targetPermissionGroup = DirectoryDatabase.FindPermissionGroup(target);

				if (targetPermissionGroup != null)
				{
					return targetPermissionGroup.ToClientSidePermissionGroup();
				}
				else
				{
					throw new TargetPermissionGroupNotFoundException(target);
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public PermissionGroup UpdatePermissionGroup(EntityToken token, PermissionGroup target)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Permissions.PermissionGroup targetPermissionGroup = DirectoryDatabase.FindPermissionGroup(target.Guid);

				if (targetPermissionGroup != null)
				{
					if (targetPermissionGroup.Owner.Contain(requester))
					{
						targetPermissionGroup.Edit(target);
					}
					else
					{
						throw new PermissionDeniedException();
					}
				}
				else
				{
					targetPermissionGroup = new Permissions.PermissionGroup();
					targetPermissionGroup.Edit(target);
					targetPermissionGroup.Guid = Guid.NewGuid();

					DirectoryDatabase.PermissionGroups.Add(targetPermissionGroup);
				}

				return targetPermissionGroup.ToClientSidePermissionGroup();
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}


		public bool Contain(EntityToken token, Guid group, Guid target)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Group targetGroup = DirectoryDatabase.Groups.SingleOrDefault(grp => grp.Guid == group);

				if (targetGroup != null)
				{
					if (!targetGroup.GetMembersProperty().Access(requester).HasFlag(AccessType.Read))
					{
						throw new PermissionDeniedException();
					}

					Entity targetEntity = DirectoryDatabase.FindEntity(target);

					if (targetEntity != null)
					{
						return targetGroup.Contain(targetEntity);
					}
					else
					{
						throw new TargetEntityNotFoundException(target);
					}
				}
				else
				{
					throw new TargetEntityNotFoundException(group);
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public ICollection<Guid> ListGroup(EntityToken token, Guid group)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Group targetGroup = DirectoryDatabase.Groups.SingleOrDefault(grp => grp.Guid == group);

				if (targetGroup != null)
				{
					if (!targetGroup.GetMembersProperty().Access(requester).HasFlag(AccessType.Read))
					{
						throw new PermissionDeniedException();
					}

					return targetGroup.Members.Select(entity => entity.Guid).ToHashSet();
				}
				else
				{
					SpecialGroup targetSpecialGroup =
						DirectoryDatabase.KnownSpecialGroups.Entities.SingleOrDefault(
						specialGroup => specialGroup.Guid == group);

					if (targetSpecialGroup != null)
					{
						if ( targetSpecialGroup . GetMembersProperty ( ) .
												Access ( requester ) .
												HasFlag ( AccessType . Read ) )
						{
							return targetSpecialGroup . Members . Select ( entity => entity . Guid ) . ToHashSet ( ) ;
						}
						else
						{
							throw new PermissionDeniedException();

						}
					}
					else
					{
						throw new TargetEntityNotFoundException(group);
					}
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public void AddToGroup(EntityToken token, Guid group, Guid target)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Group targetGroup = DirectoryDatabase.Groups.SingleOrDefault(grp => grp.Guid == group);

				if (targetGroup != null)
				{
					if (!targetGroup.GetMembersProperty().
										Access(requester).
										HasFlag(AccessType.Write))
					{
						throw new PermissionDeniedException();
					}

					Entity targetEntity = DirectoryDatabase.FindEntity(target);

					if (targetEntity != null)
					{
						targetGroup.Members.Add(targetEntity);
					}
					else
					{
						throw new TargetEntityNotFoundException(target);
					}
				}
				else
				{
					throw new TargetEntityNotFoundException(group);
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public void RemoveFromGroup(EntityToken token, Guid group, Guid target)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Group targetGroup = DirectoryDatabase.Groups.SingleOrDefault(grp => grp.Guid == group);

				if (targetGroup != null)
				{
					if (!targetGroup.GetMembersProperty().
										Access(requester).
										HasFlag(AccessType.Write))
					{
						throw new PermissionDeniedException();
					}

					Entity targetEntity = DirectoryDatabase.FindEntity(target);

					if (targetEntity != null)
					{
						targetGroup.Members.Remove(targetEntity);
					}
					else
					{
						throw new TargetEntityNotFoundException(target);
					}
				}
				else
				{
					throw new TargetEntityNotFoundException(group);
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}


		public void CheckToken(EntityToken token, EntityToken tokenToCheck)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			if (tokenToCheck == null)
			{
				throw new ArgumentNullException(nameof(tokenToCheck));
			}

			CheckToken(token);

			DirectoryService directory =
				DirectoryDatabase.DirectoryServices.SingleOrDefault(
																		(entity => entity.Guid
																		   == token.Issuer));

			if (directory != null)
			{
				CheckToken(tokenToCheck);
			}
			else
			{
				throw new PermissionDeniedException();
			}
		}

		public void CheckToken([NotNull] EntityToken token, [NotNull] AccessToken tokenToCheck)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			if (tokenToCheck == null)
			{
				throw new ArgumentNullException(nameof(tokenToCheck));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				if (tokenToCheck.Target == requester.Guid
					|| (requester is DirectoryService))
				{
					CheckToken(tokenToCheck);
				}
				else
				{
					throw new PermissionDeniedException();
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public Guid CreateUser(EntityToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			LoginService requester =
				DirectoryDatabase.LoginServices.SingleOrDefault(entity => entity.Guid == token.Owner);

			if (requester != null)
			{
				User user = new User();

				DirectoryDatabase.Users.Add(user);

				user.AddCanLoginFrom(requester);

				return user.Guid;
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public Guid CreateGroup(EntityToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = DirectoryDatabase.FindEntity(token.Owner);

			if (requester != null)
			{
				Group group = new Group();

				EntityProperty memberProperty = group.GetMembersProperty();

				memberProperty . Owner = requester ;

				memberProperty . Permissions = KnownPermissionGroups . DirectoryServicesReadonly ;

				DirectoryDatabase.Groups.Add(group);

				return group.Guid;
			}
			else
			{
				throw new EntityNotFoundException();
			}
		}

		public void RegisterLogin(EntityToken token, LoginToken targetToken)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			if (targetToken == null)
			{
				throw new ArgumentNullException(nameof(targetToken));
			}

			LoginService requester =
				DirectoryDatabase.LoginServices.SingleOrDefault(entity => entity.Guid == token.Owner);

			if (requester != null)
			{
				CheckToken(targetToken);

				Entity targetEntity = DirectoryDatabase.FindEntity(targetToken.Owner);

				if (targetEntity != null)
				{
					targetEntity.AddCanLoginFrom(requester);
				}
				else
				{
					throw new EntityNotFoundException();
				}
			}
			else
			{
				throw new EntityNotFoundException();
			}
		}

		public TimeSpan GetLoginTokenLife(EntityToken token, Guid target)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			LoginService requester =
				DirectoryDatabase.LoginServices.SingleOrDefault(entity => entity.Guid == token.Owner);

			if (requester != null)
			{
				Entity targetEntity = DirectoryDatabase.FindEntity(target);

				if (targetEntity != null)
				{
					return TokenPolicy.LoginTokenLife(requester, targetEntity);
				}
				else
				{
					throw new TargetEntityNotFoundException(target);
				}
			}
			else
			{
				throw new EntityNotFoundException(token);
			}
		}

		public IDirectoryDatabase DirectoryDatabase { get; set; }

		public DirectoryService ServiceEntity { get; set; }

		public ITokenPolicy TokenPolicy { get; set; }

		public AccessToken IssueAccessToken(
			[NotNull] Entity entity,
			[NotNull] Entity accessTarget,
			TimeSpan lifetime)
		{
			if ( entity is null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			if ( accessTarget is null )
			{
				throw new ArgumentNullException ( nameof ( accessTarget ) ) ;
			}

			DateTimeOffset now = DateTimeOffset . UtcNow ;

			AccessToken token = new AccessToken
								{
									Owner     = entity . Guid ,
									Target    = accessTarget . Guid ,
									NotBefore = now ,
									NotAfter  = now + lifetime ,
									Issuer    = ServiceEntity . Guid ,
									Secret    = new byte[ 1024 ] ,
								} ;

			RngProvider . GetBytes ( token . Secret ) ;

			IssuedAccessTokens . AddToken ( token ) ;

			return token ;

		}

		/// <summary>
		///     Directly issue entity token
		/// </summary>
		/// <param name="entity"></param>
		/// <param name="lifetime"></param>
		/// <returns></returns>
		public EntityToken IssueEntityToken([NotNull] Entity entity, TimeSpan lifetime)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			DateTimeOffset now = DateTimeOffset.UtcNow;

			EntityToken token = new EntityToken
			{
				Owner = entity.Guid,
				NotBefore = now,
				NotAfter = now + lifetime,
				Issuer = ServiceEntity.Guid,
				Secret = new byte[1024],
				Guid = Guid.NewGuid(),
			};

			RngProvider.GetBytes(token.Secret);

			IssuedEntityTokens.AddToken(token);

			return token;
		}

		public void InitializeDatabase() { }

		public void Start() { }

		protected void CheckToken([NotNull] EntityToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			token.CheckTokenTime();

			if (token.Issuer == ServiceEntity.Guid)
			{
				IssuedEntityTokens.CheckToken(token);
			}
			else
			{
				DirectoryService issuer =
					DirectoryDatabase.DirectoryServices.FirstOrDefault(
																			directoryService
																				=> directoryService.Guid
																					== token.Issuer);
				if ( issuer is not null )
				{
					IDirectoryService issuerService = DirectoryServiceProvider . GetDirectoryProvider ( issuer ) ;

					issuerService . CheckToken ( EntityTokenProvider . GetToken ( ) , token ) ;
				}
				else
				{
					throw new InvalidIssuerException ( ) ;
				}
			}
		}

		protected void CheckToken([NotNull] AccessToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			token.CheckTokenTime();

			if (token.Issuer == ServiceEntity.Guid)
			{
				IssuedAccessTokens.CheckToken(token);
			}
			else
			{
				DirectoryService issuer =
					DirectoryDatabase.DirectoryServices.FirstOrDefault(
																			directoryService
																				=> directoryService.Guid
																					== token.Issuer);
				if ( issuer is not null )
				{
					IDirectoryService issuerService = DirectoryServiceProvider . GetDirectoryProvider ( issuer ) ;

					issuerService . CheckToken ( EntityTokenProvider . GetToken ( ) , token ) ;
				}
				else
				{
					throw new InvalidIssuerException ( ) ;
				}
			}
		}

		protected void CheckToken(LoginToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			token.CheckTokenTime();

			LoginService issuer =
				DirectoryDatabase.LoginServices.FirstOrDefault(
																	loginService
																		=> loginService.Guid == token.Issuer);
			if ( issuer is not null )
			{
				ILoginService issuerService = LoginServiceProvider . GetLoginService ( issuer ) ;

				issuerService . CheckToken ( AccessTokenProvider . Access ( issuer . Guid ) , token ) ;
			}
			else
			{
				throw new InvalidIssuerException ( ) ;
			}
		}

	}

}
