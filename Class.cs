using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http;
using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RemAPIWrapper
{
	public class RemAPI
	{
		private string Token;
		private ulong ClientId;

		private HttpClient Client = new HttpClient();

		public RemAPI(string Token, ulong ClientId)
		{
			this.Token = Token;
			this.ClientId = ClientId;
		}

		public enum ImageType
		{
			Rem = 0,
			Emilia = 1,
			Ram = 2,
			Thighs = 3,
			Neko = 4
		}
		/// <summary>Get an image with a certain type
		/// <para>Returns <see cref="string"/></para>
		/// </summary>
		public async Task<string> GetImage(ImageType Type, bool IsNSFW)
		{
			if (Type == ImageType.Thighs && !IsNSFW)
			{
				throw new Exception("NSFW must be true for Thighs endpoint");
			}
			else if (Type == ImageType.Neko && !IsNSFW)
			{
				throw new Exception("NSFW must be true for Neko endpoint");
			}

			Client.DefaultRequestHeaders.Add("Authorization", Token);
			Client.DefaultRequestHeaders.Add("ClientId", ClientId.ToString());

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("https://api.rembot.cc/api/v1/images"),
				Content = new StringContent($@"{{
				""ImageType"":""{Type}"",
				""IsNsfw"":""{IsNSFW}""}}", Encoding.UTF8, "application/json"),
			};
			var response = await Client.SendAsync(request).ConfigureAwait(false);
			response.EnsureSuccessStatusCode();
			var responseInfo = await response.Content.ReadAsStringAsync();
			return responseInfo;
		}
	}
}
