using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RemAPIWrapper
{
	public class RemAPI
	{
		private string Token;

		public RemAPI(string Token)
		{
			this.Token = Token;
		}

		public async Task<string> Rem()
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Token", Token);
				HttpResponseMessage response = await client.GetAsync("https://api.rembot.cc/images/sfw/rem");
				HttpContent content = response.Content;

				string result = await content.ReadAsStringAsync();
				if (response.StatusCode == HttpStatusCode.OK)
				{
					return result;
				}
				else
				{
					throw new HttpListenerException(int.Parse(response.StatusCode.ToString()), $"Server returned an error. {response.StatusCode} {result}");
				}
			}
		}

		public async Task<string> LewdRem()
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Token", Token);
				HttpResponseMessage response = await client.GetAsync("https://api.rembot.cc/images/nsfw/rem");
				HttpContent content = response.Content;

				string result = await content.ReadAsStringAsync();
				if (response.StatusCode == HttpStatusCode.OK)
				{
					return result;
				}
				else
				{
					throw new HttpListenerException(int.Parse(response.StatusCode.ToString()), $"Server returned an error. {response.StatusCode} {result}");
				}
			}
		}

		public async Task<string> Thighs()
		{
			using (HttpClient client = new HttpClient())
			{
				client.DefaultRequestHeaders.Add("Token", Token);
				HttpResponseMessage response = await client.GetAsync("https://api.rembot.cc/images/nsfw/thighs");
				HttpContent content = response.Content;

				string result = await content.ReadAsStringAsync();
				if (response.StatusCode == HttpStatusCode.OK)
				{
					return result;
				}
				else
				{
					throw new HttpListenerException(int.Parse(response.StatusCode.ToString()), $"Server returned an error. {response.StatusCode} {result}");
				}
			}
		}
	}
}
