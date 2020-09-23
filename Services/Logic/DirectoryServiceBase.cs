using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Logic.Tokens;
using DreamRecorder.Directory.ServiceProvider;
using DreamRecorder.Directory.Services.Logic.Entities;

using JetBrains.Annotations;

using ILoginProvider = DreamRecorder.Directory.Logic.ILoginProvider;

namespace DreamRecorder.Directory.Services.Logic
{

	[PublicAPI]
	public class DirectoryServiceBase : IDirectoryService,IDirectoryServiceInternal
	{

		public IAccessTokenProvider AccessTokenProvider { get; set; }

		public IEntityTokenProvider EntityTokenProvider { get; set; }

		public RNGCryptoServiceProvider RngProvider { get; set; } = new RNGCryptoServiceProvider();

		public KnownSpecialGroups KnownSpecialGroups { get ; }

		public         DirectoryService   ServiceEntity      { get; set; }

		public EntityToken ServiceToken { get; }

		public virtual ILoginServiceProvider LoginServiceProvider { get; }

		public virtual IDirectoryServiceProvider DirectoryServiceProvider { get; }

		public void Start() {  }

		public ITokenPolicy TokenPolicy { get; set; }

		public AccessToken IssueAccessToken(
			[NotNull] Entity entity,
			[NotNull] Entity accessTarget,
			TimeSpan lifetime)
		{
			if (entity == null)
			{
				throw new ArgumentNullException(nameof(entity));
			}

			if (accessTarget == null)
			{
				throw new ArgumentNullException(nameof(accessTarget));
			}

			DateTimeOffset now = DateTimeOffset.UtcNow;

			AccessToken token = new AccessToken
			{
				Owner = entity.Guid,
				Target = accessTarget.Guid,
				NotBefore = now,
				NotAfter = now + lifetime,
				Issuer = ServiceEntity.Guid,
				Secret = new byte[1024],
			};

			RngProvider.GetBytes(token.Secret);

			IssuedAccessTokens.Add(token);

			return token;
		}

		/// <summary>
		/// Directly issue entity token
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
			};

			RngProvider.GetBytes(token.Secret);

			IssuedEntityTokens.Add(token);

			return token;
		}

		public HashSet<User> Users { get; }

		public HashSet<Group> Groups { get; set; }

		public HashSet<Service> Services { get; set; }

		public HashSet<LoginService> LoginServices { get; set; }

		public HashSet<DirectoryService> DirectoryServices { get; set; }

		public EntityToken EveryoneToken { get; set; }

		public IEnumerable<Entity> Entities
			=> Users.Union<Entity>(Groups).
						Union(Services).
						Union(LoginServices).
						Union(DirectoryServices).
						Union(KnownSpecialGroups.Entities);

		public HashSet<EntityToken> IssuedEntityTokens { get; set; }

		public HashSet<AccessToken> IssuedAccessTokens { get; set; }


		/// <summary>
		/// Check if a token is in valid time
		/// </summary>
		/// <param name="token"></param>
		public void CheckTokenTime([NotNull] Token token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			if (token.NotAfter > DateTimeOffset.UtcNow
				&& token.NotBefore < DateTimeOffset.UtcNow)
			{
				return ;
			}
			else
			{
				throw new InvalidTimeException();
			}
		}


		public EntityToken Login(LoginToken token)
		{
			if (token == null)
			{
				return EveryoneToken;
			}

			CheckTokenTime(token);

			LoginService issuer =
				LoginServices.SingleOrDefault(loginProvider => loginProvider.Guid == token.Issuer);

			if (!(issuer is null))
			{
				Entity target = Entities.SingleOrDefault((entity) => entity.Guid == token.Owner);

				if (target != null)
				{
					if (target.GetIsDisabled())
					{
						throw new EntityDisabledException(target.Guid);
					}

					ILoginProvider loginProviderService = LoginServiceProvider.GetLoginProvider(issuer);

					loginProviderService.CheckToken(AccessTokenProvider.Access(issuer.Guid), token);

					EntityToken resultToken = IssueEntityToken(
																target,
																TokenPolicy.EntityTokenTimeSpan(target));

					return resultToken;
				}
				else
				{
					throw new EntityNotFoundException();
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

			Entity target = Entities.FirstOrDefault((entity) => entity.Guid == token.Owner);

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
				throw new EntityNotFoundException();
			}
		}

		public void DisposeToken([NotNull] EntityToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			IssuedEntityTokens.Remove(token);
		}

		public AccessToken Access(EntityToken token, Guid target)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = Entities.SingleOrDefault((entity) => entity.Guid == token.Owner);

			if (requester != null)
			{
				Entity accessTarget = Entities.SingleOrDefault((entity) => entity.Guid == target);

				if (accessTarget != null)
				{
					return IssueAccessToken(
											requester,
											accessTarget,
											TokenPolicy.AccessTokenLife(requester, accessTarget));
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

		public string GetProperty(EntityToken token, Guid target, string name)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = Entities.SingleOrDefault((entity) => entity.Guid == token.Owner);

			if (requester != null)
			{
				Entity accessTarget = Entities.SingleOrDefault((entity) => entity.Guid == target);

				if (accessTarget != null)
				{
					name = name.Normalize(NormalizationForm.FormD);

					EntityProperty property =
						accessTarget.Properties.SingleOrDefault(prop => prop.Name == name);

					if (property != null)
					{
						AccessType validAccess = property.Access(requester);

						if ((validAccess & AccessType.Read) == 0)
						{
							throw new PermissionDeniedException();
						}
						else
						{
							return property.Value;
						}
					}
					else
					{
						return null;
					}
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


		public void SetProperty(EntityToken token, Guid target, string name, string value)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = Entities.SingleOrDefault((entity) => entity.Guid == token.Owner);

			if (requester != null)
			{
				Entity accessTarget = Entities.SingleOrDefault((entity) => entity.Guid == target);

				if (accessTarget != null)
				{
					name = name.Normalize(NormalizationForm.FormD);

					EntityProperty property =
						accessTarget.Properties.SingleOrDefault(prop => prop.Name == name);

					if (property != null)
					{
						AccessType validAccess = property.Access(requester);

						if (validAccess != AccessType.ReadWrite)
						{
							throw new PermissionDeniedException();
						}
						else
						{
							property.Value = value;
						}
					}
					else
					{
						accessTarget.Properties.Add(
														new EntityProperty()
														{
															Guid = new Guid(),
															Name = name,
															Owner = requester,
															Value = value
														});
					}
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

		public AccessType AccessProperty(EntityToken token, Guid target, string name)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = Entities.SingleOrDefault((entity) => entity.Guid == token.Owner);

			if (requester != null)
			{
				Entity accessTarget = Entities.SingleOrDefault((entity) => entity.Guid == target);

				if (accessTarget != null)
				{
					name = name.Normalize(NormalizationForm.FormD);

					EntityProperty property =
						accessTarget.Properties.SingleOrDefault(prop => prop.Name == name);

					if (property != null)
					{
						AccessType validAccess = property.Access(requester);

						return validAccess;
					}
					else
					{
						throw new PropertyNotFoundException();
					}
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

		public bool Contain(EntityToken token, Guid @group, Guid target)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			Entity requester = Entities.SingleOrDefault((entity) => entity.Guid == token.Owner);

			if (requester != null)
			{
				Group groupTarget = Groups.SingleOrDefault(grp => grp.Guid == group);

				if ( AccessPro )
				{
					
				}

				if (groupTarget != null)
				{
					Entity accessTarget = Entities.SingleOrDefault((entity) => entity.Guid == target);

					if (accessTarget != null)
					{

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
			else
			{
				throw new EntityNotFoundException();
			}
		}

		public ICollection<Guid> ListGroup(EntityToken token, Guid @group)
		{
			throw new NotImplementedException();
		}

		public void AddToGroup(EntityToken token, Guid @group, Guid target) { CheckToken(token); }

		public void RemoveFromGroup(EntityToken token, Guid @group, Guid target)
		{
			throw new NotImplementedException();
		}

		protected void CheckToken([NotNull] EntityToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckTokenTime(token);

			if (token.Issuer == ServiceEntity.Guid)
			{
				if (IssuedEntityTokens.Contains(token))
				{
					return;
				}
				else
				{
					throw new InvalidTokenException();
				}
			}
			else
			{
				DirectoryService issuer =
					DirectoryServices.FirstOrDefault(
														(directoryService)
															=> directoryService.Guid == token.Issuer);
				if (!(issuer is null))
				{
					IDirectoryService issuerService = DirectoryServiceProvider.GetDirectoryProvider(issuer);

					issuerService.CheckToken(EntityTokenProvider.GetToken(), token);
				}
			}
		}

		protected void CheckToken([NotNull] AccessToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckTokenTime(token);

			if (token.Issuer == ServiceEntity.Guid)
			{
				if (IssuedAccessTokens.Contains(token))
				{
					return;
				}
				else
				{
					throw new InvalidTokenException();
				}
			}
			else
			{
				DirectoryService issuer =
					DirectoryServices.FirstOrDefault(
													(directoryService)
														=> directoryService.Guid == token.Issuer);
				if (!(issuer is null))
				{
					IDirectoryService issuerService = DirectoryServiceProvider.GetDirectoryProvider(issuer);

					issuerService.CheckToken(EntityTokenProvider.GetToken(), token);
				}
			}
		}

		protected void CheckToken(LoginToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckTokenTime(token);

			LoginService issuer =
				LoginServices.FirstOrDefault(
											(loginService)
												=> loginService.Guid == token.Issuer);
			if (!(issuer is null))
			{
				ILoginProvider issuerService = LoginServiceProvider.GetLoginProvider(issuer);

				issuerService.CheckToken(AccessTokenProvider.Access(issuer.Guid), token);
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
				DirectoryServices.SingleOrDefault((entity => entity.Guid == token.Issuer));

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

			Entity requester = Entities.SingleOrDefault((entity) => entity.Guid == token.Owner);

			if (requester != null)
			{
				if (tokenToCheck.Target == requester.Guid || (requester is DirectoryService directoryService && DirectoryServices.Contains(directoryService)))
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
				throw new EntityNotFoundException();
			}

		}

		public Guid CreateUser(EntityToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			CheckToken(token);

			User user = new User() { };

			Users.Add(user);



		}

		public Guid CreateGroup(EntityToken token) => throw new NotImplementedException();

		public void RegisterLogin(EntityToken loginServiceToken, LoginToken targetToken)
		{
			if (loginServiceToken == null)
			{
				throw new ArgumentNullException(nameof(loginServiceToken));
			}

			CheckToken(loginServiceToken);

			if (targetToken == null)
			{
				throw new ArgumentNullException(nameof(targetToken));
			}

			CheckToken(targetToken);
		}


	}

	public class PropertyNotFoundException : Exception
	{

	}

	public interface ITokenPolicy
	{

		TimeSpan EntityTokenTimeSpan(Entity entity);

		TimeSpan AccessTokenLife(Entity entity, Entity accessTarget);

	}

	public class TokenPolicy : ITokenPolicy
	{

		public TimeSpan EntityTokenTimeSpan(Entity entity) { return TimeSpan.FromMinutes(10); }

		public TimeSpan AccessTokenLife(Entity entity, Entity accessTarget)
		{
			return TimeSpan.FromMinutes(30);
		}

	}

	public class PermissionDeniedException : Exception
	{

	}

	public class EntityNotFoundException : AuthenticationException
	{

	}


	public class CannotCheckByIssuerException : AuthenticationException
	{

	}

	public class InvalidIssuerException : InvalidTokenException
	{

	}

	public class InvalidTimeException : InvalidTokenException
	{

	}

	public class AuthenticationException : Exception
	{

	}

	public class InvalidTokenException : AuthenticationException
	{

	}


	public class EntityDisabledException : AuthenticationException
	{

		public Guid EntityGuid { get; set; }

		public EntityDisabledException(Guid entityGuid) { EntityGuid = entityGuid; }

	}

}
