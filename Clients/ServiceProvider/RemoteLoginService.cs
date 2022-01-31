using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;

using DreamRecorder.Directory.Logic;
using DreamRecorder.Directory.Logic.Tokens;

namespace DreamRecorder.Directory.ServiceProvider
{

    public class RemoteLoginService<TCredential> : ILoginService where TCredential : class
    {

        public Func<HttpClient> HttpClientFactory { get; set; } = () => new HttpClient();

        public string Server { get; set; }

        public int Port { get; set; }

        public Guid Type { get; set ; }

        public LoginToken Login(object credential) => Login(credential as TCredential);

        public void CheckToken(AccessToken token, LoginToken tokenToCheck)
        {
            HttpClient client = HttpClientFactory();

            client.DefaultRequestHeaders.Add("token", JsonSerializer.Serialize(token));

            HttpResponseMessage response = client.PostAsJsonAsync(
                                                                    new UriBuilder(
                                                                    Uri.UriSchemeHttps,
                                                                    Server,
                                                                    Port,
                                                                    $"{Type}/{nameof(CheckToken)}").Uri,
                                                                    tokenToCheck).
                                                    Result;

            response.EnsureSuccessStatusCode();
        }

        public void DisposeToken(LoginToken token)
        {
            HttpClient client = HttpClientFactory();

            HttpResponseMessage response = client.PostAsJsonAsync(
                                                                    new UriBuilder(
                                                                    Uri.UriSchemeHttps,
                                                                    Server,
                                                                    Port,
																	$"{Type}/{nameof(DisposeToken)}").Uri,
                                                                    token).
                                                    Result;

            response.EnsureSuccessStatusCode();
        }

        public virtual LoginToken Login(TCredential credential)
        {
            HttpClient client = HttpClientFactory();

            HttpResponseMessage response = client.PostAsJsonAsync(
                                                                    new UriBuilder(
                                                                    Uri.UriSchemeHttps,
                                                                    Server,
                                                                    Port,
																	$"{Type}/{nameof(Login)}").Uri,
                                                                    credential).
                                                    Result;

            response.EnsureSuccessStatusCode();

            LoginToken result = response.Content.ReadAsAsync<LoginToken>().Result;

            return result;
        }

    }

}
