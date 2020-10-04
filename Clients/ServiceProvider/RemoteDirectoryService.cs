using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Logic.Tokens;

namespace DreamRecorder.Directory.ServiceProvider
{

	public class RemoteDirectoryService : IDirectoryService
	{

		public string Server { get; set; }

		public int Port { get; set; }

		public EntityToken Login(LoginToken token)
		{
			HttpClient client = new HttpClient();

			HttpResponseMessage response = client.PostAsJsonAsync(new UriBuilder(Uri.UriSchemeHttps, Server, Port, nameof(Login)).Uri, token).Result;

			response.EnsureSuccessStatusCode();

			EntityToken result = response.Content.ReadAsAsync<EntityToken>().Result;

			return result;
		}

		public EntityToken UpdateToken(EntityToken token)
		{
			HttpClient client = new HttpClient();

			client.DefaultRequestHeaders.Add("token", System.Text.Json.JsonSerializer.Serialize(token));

			HttpResponseMessage response = client.PostAsync(nameof(UpdateToken), null).Result;

			response.EnsureSuccessStatusCode();

			EntityToken result = response.Content.ReadAsAsync<EntityToken>().Result;

			return result;
		}

		public void DisposeToken(EntityToken token)
		{
			HttpClient client = new HttpClient();

			HttpResponseMessage response = client.PostAsJsonAsync(new UriBuilder(Uri.UriSchemeHttps, Server, Port, nameof(DisposeToken)).Uri, token).Result;

			response.EnsureSuccessStatusCode();

		}

		public AccessToken Access(EntityToken token, Guid target)
		{
			HttpClient client = new HttpClient();

			HttpResponseMessage response = client.PostAsXmlAsync($"{nameof(Access)}/{target}", token).Result;

			response.EnsureSuccessStatusCode();

			AccessToken result = response.Content.ReadAsAsync<AccessToken>().Result;

			return result;

		}

		public string GetProperty(EntityToken token, Guid target, string name)
		{
			HttpClient client = new HttpClient();

			HttpResponseMessage response = client.PostAsJsonAsync($"{nameof(GetProperty)}/{target}/{name}", token).Result;

			response.EnsureSuccessStatusCode();

			string result = response.Content.ReadAsStringAsync().Result;

			return result;
		}

		public void SetProperty(EntityToken token, Guid target, string name, string value)
		{
			HttpClient client = new HttpClient();

			HttpResponseMessage response = client.PostAsXmlAsync($"{nameof(SetProperty)}/{target}/{name}", token).Result;

			response.EnsureSuccessStatusCode();
		}

		public AccessType AccessProperty(EntityToken token, Guid target, string name) 
		{
			
		}

		public AccessType GrantRead(EntityToken token, Guid target, string name, Guid access) { throw new NotImplementedException(); }

		public AccessType GrantWrite(EntityToken token, Guid target, string name, Guid access) { throw new NotImplementedException(); }

		public bool Contain(EntityToken token, Guid @group, Guid target) { throw new NotImplementedException(); }

		public ICollection<Guid> ListGroup(EntityToken token, Guid @group) { throw new NotImplementedException(); }

		public void AddToGroup(EntityToken token, Guid @group, Guid target) { throw new NotImplementedException(); }

		public void RemoveFromGroup(EntityToken token, Guid @group, Guid target) { throw new NotImplementedException(); }

		public void CheckToken(EntityToken token, AccessToken tokenToCheck) { throw new NotImplementedException(); }

		public void CheckToken(EntityToken token, EntityToken tokenToCheck) { throw new NotImplementedException(); }

		public Guid CreateUser(EntityToken token) { throw new NotImplementedException(); }

		public Guid CreateGroup(EntityToken token) { throw new NotImplementedException(); }

		public void RegisterLogin(EntityToken loginServiceToken, LoginToken targetToken) { throw new NotImplementedException(); }

	}

}
