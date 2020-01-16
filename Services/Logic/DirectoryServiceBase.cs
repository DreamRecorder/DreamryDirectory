using System ;
using System.Collections ;
using System.Collections.Generic ;
using System.Linq ;
using System.Security.Cryptography ;

using DreamRecorder . Directory . Logic ;
using DreamRecorder.Directory.Logic.Tokens ;
using DreamRecorder . Directory . Services . Logic . Entities ;

using JetBrains.Annotations ;

namespace DreamRecorder . Directory . Services . Logic
{

	public class DirectoryServiceBase : IDirectoryProvider
	{

		public RNGCryptoServiceProvider RngProvider { get; set; } = new RNGCryptoServiceProvider();

		public DirectoryService ServiceEntity { get; set; }

		public EntityToken ServiceToken { get; set; }

		public virtual ILoginServiceProvider LoginServiceProvider { get; }


		public void Start() { Everyone = new Everyone ( ) { Guid = Guid . Parse ( "" ) } ; }

		public AccessToken IssueAccessToken( [NotNull] Entity entity, [NotNull] Entity accessTarget, TimeSpan lifetime)
		{
			if ( entity == null )
			{
				throw new ArgumentNullException ( nameof ( entity ) ) ;
			}

			if ( accessTarget == null )
			{
				throw new ArgumentNullException ( nameof ( accessTarget ) ) ;
			}

			DateTimeOffset now = DateTimeOffset.UtcNow;

			AccessToken token = new AccessToken
								{
									Owner     = entity.Guid,
									Target    =accessTarget.Guid,
									NotBefore = now,
									NotAfter  = now + lifetime,
									Issuer    = ServiceEntity.Guid,
									Secret    = new byte[1024],
									
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
									Owner     = entity.Guid,
									NotBefore = now,
									NotAfter  = now + lifetime,
									Issuer    = ServiceEntity.Guid,
									Secret    = new byte[1024],
								};

			RngProvider.GetBytes(token.Secret);

			IssuedEntityTokens.Add(token);

			return token;
		}

		public HashSet<User> Users { get; }

		public HashSet<Group> Groups { get; set; }

		public HashSet<Service> Services { get; set; }

		public HashSet<LoginProvider> LoginProviders { get; set; }

		public HashSet<DirectoryService> DirectoryServices { get; set; }

		public Everyone Everyone { get; set; }

		public AuthorizedUser AuthorizedUser { get; set; }

		public EntityToken EveryoneToken { get; set; }

		public IEnumerable<Entity> Entities
			=> Users.Union<Entity>(Groups).
					Union(Services).
					Union(LoginProviders).
					Union(DirectoryServices).
					Union(new Entity[] { Everyone, AuthorizedUser });

		public HashSet<EntityToken> IssuedEntityTokens { get; set; }

		public HashSet<AccessToken> IssuedAccessTokens { get; set; }

		public bool CheckTokenTime([NotNull] Token token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			return token.NotAfter      > DateTimeOffset.UtcNow
					&& token.NotBefore < DateTimeOffset.UtcNow;
		}



		public EntityToken Login(LoginToken token)
		{

			if (token == null)
			{
				return EveryoneToken;
			}

			if (CheckTokenTime(token))
			{
				LoginProvider issuer = LoginProviders.SingleOrDefault(loginProvider => loginProvider.Guid == token.Issuer);

				if (!(issuer is null))
				{

					var target= Entities . SingleOrDefault ( ( entity ) => entity . Guid == token . Owner ) ;

					if ( ! (target is null) )
					{
						target.GetIsDisabled()
					}

					ILoginProvider loginProvider = LoginServiceProvider . GetLoginProvider ( issuer ) ;

					AccessToken accessToken= IssueAccessToken ( ServiceEntity , issuer , TimeSpan . FromMinutes ( 1 ) ) ;

					if (loginProvider.CheckToken(accessToken,token)  )
					{



						var resultToken = IssueEntityToken ( ) ;

					}


				}

			}



		}

		public EntityToken ChangeToken(EntityToken token, Guid target)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			if (CheckToken(token))
			{
				if (Entities.FirstOrDefault((entity) => entity.Guid == target) is Entity requestEntity)
				{
					if (Entities.FirstOrDefault((entity) => entity.Guid == target) is Entity targetEntity)
					{
						if (targetEntity.Contain(requestEntity))
						{

							return IssueEntityToken ( targetEntity , TimeSpan . FromHours ( 1 ) ) ;
						}
					}
				}
			}

			return null;
		}

		public void DisposeToken([NotNull] EntityToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			IssuedEntityTokens.Remove(token);
		}

		public AccessToken Access(EntityToken token, Guid target) => throw new NotImplementedException();

		public string GetProperty(EntityToken token, Guid target, string name) => throw new NotImplementedException();

		public void SetProperty(EntityToken token, Guid target, string name, string value) { throw new NotImplementedException(); }

		public AccessType AccessProperty(EntityToken token, Guid target, string name) => throw new NotImplementedException();

		public bool Contain(EntityToken token, Guid @group, Guid entity) => throw new NotImplementedException();

		public ICollection<Guid> ListGroup(EntityToken token, Guid @group) => throw new NotImplementedException();

		public void AddToGroup(EntityToken token, Guid @group, Guid target) { throw new NotImplementedException(); }

		public void RemoveFromGroup(EntityToken token, Guid @group, Guid target) { throw new NotImplementedException(); }


		public bool CheckToken([NotNull] EntityToken token)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			if (CheckTokenTime(token))
			{
				if (token.Issuer == ServiceEntity.Guid)
				{
					return IssuedEntityTokens.Contains(token);
				}
				else
				{
					DirectoryService issuer =
						DirectoryServices.FirstOrDefault(
														(directoryService)
															=> directoryService.Guid == token.Issuer);
					if (!(issuer is null))
					{
						
					}

				}
			}
			return false;
		}

		public bool CheckToken([NotNull] EntityToken token, [NotNull] AccessToken tokenToCheck)
		{
			if (token == null)
			{
				throw new ArgumentNullException(nameof(token));
			}

			if (tokenToCheck == null)
			{
				throw new ArgumentNullException(nameof(tokenToCheck));
			}

			throw new NotImplementedException();
		}

		public bool CheckToken(EntityToken token, EntityToken tokenToCheck)
		{
			throw new NotImplementedException();
		}

		public Guid CreateUser(EntityToken token) => throw new NotImplementedException();

		public Guid CreateGroup(EntityToken token) => throw new NotImplementedException();

		public void RegisterLogin(EntityToken loginServiceToken, LoginToken targetToken) { }


	}

}