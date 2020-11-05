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
			Thighs = 3
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

			HttpClient Client = new HttpClient();
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

		public class User
		{
			public ulong UserId { get; private set; }
			/// <summary>The amount of money a user has
			/// </summary>
			public BigInteger Bal { get; private set; }
			/// <summary>The amount of experience points a user has
			/// </summary>
			public BigInteger Xp { get; private set; }

			/// <summary>Checks if the user is blacklisted, If the user is blacklisted you will not be able to update them.
			/// </summary>
			public bool Blacklisted { get; private set; }
			/// <summary>The level a user is, The more experience points they have the higher the level 
			/// </summary>
			public int Level { get; private set; }
		}

		/// <summary>Get a users information
		/// <para>Returns <see cref="User"/></para>
		/// </summary>
		public async Task<User> GetUser(ulong UserId)
		{
			HttpClient Client = new HttpClient();
			Client.DefaultRequestHeaders.Add("Authorization", Token);
			Client.DefaultRequestHeaders.Add("ClientId", ClientId.ToString());

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("https://api.rembot.cc/api/v1/economy/user"),
				Content = new StringContent($@"{{
				""UserId"":""{UserId}""}}", Encoding.UTF8, "application/json"),
			};
			var response = await Client.SendAsync(request).ConfigureAwait(false);
			response.EnsureSuccessStatusCode();
			var responseInfo = await response.Content.ReadAsStringAsync();
			User User = JsonSerializer.Deserialize<User>(responseInfo);
			return User;
		}

		/// <summary>Update a users balance, you can only update the same user every 15 seconds
		/// </summary>
		public async Task UpdateUser(ulong UserId, BigInteger Bal)
		{
			HttpClient Client = new HttpClient();
			Client.DefaultRequestHeaders.Add("Authorization", Token);
			Client.DefaultRequestHeaders.Add("ClientId", ClientId.ToString());

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Put,
				RequestUri = new Uri("https://api.rembot.cc/api/v1/economy/user"),
				Content = new StringContent($@"{{
				""UserId"":""{UserId}"",
				""Bal"":""{Bal}""}}", Encoding.UTF8, "application/json"),
			};
			var response = await Client.SendAsync(request).ConfigureAwait(false);
			response.EnsureSuccessStatusCode();
			return;
		}
	}
}
