using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SilverHand.Core
{
	[JsonObject]
	public struct UserId
	{
		public ulong Id { get; init; }

		public UserId(ulong id) => Id = id;
		public UserId(SocketUser user) => Id = user.Id;

		public bool Equals(UserId obj) => Id == obj.Id;
		public override bool Equals(object? obj) => obj is UserId id && Id == id.Id;
		public override int GetHashCode() => HashCode.Combine(Id);

		public static bool operator ==(UserId left, UserId right) => left.Equals(right);
		public static bool operator !=(UserId left, UserId right) => !left.Equals(right);
	}

	[JsonObject(MemberSerialization.OptIn)]
	public class UserRegistry
	{
		public event EventHandler<UserId>? OnUserCreated;
		public event EventHandler<UserId>? OnUserDeleted;

		public bool Dirty { get; private set; }

		[JsonProperty("regis")]
		private readonly HashSet<ulong> registry;

		public UserRegistry()
		{
			registry = new HashSet<ulong>();
			OnUserCreated += (_, _) => Dirty = true;
			OnUserDeleted += (_, _) => Dirty = true;
		}

		public ICollection<ulong> Users => registry;

		public void Register(SocketUser user)
		{
			registry.Add(user.Id);
			OnUserCreated?.Invoke(this, new UserId(user));
		}

		public bool Deregister(SocketUser user)
		{
			if (!registry.Remove(user.Id)) return false;

			OnUserDeleted?.Invoke(this, new UserId(user));
			return true;
		}

		public bool IsRegistered(SocketUser user)
		{
			return registry.Contains(user.Id);
		}

		public static async Task<UserRegistry> LoadAsync(string filename)
		{
			using var stream = new StreamReader(filename);
			var registryData = await stream.ReadToEndAsync();
			return JsonConvert.DeserializeObject<UserRegistry>(registryData);
		}

		public static Task StoreAsync(string filename, UserRegistry userRegistry)
		{
			if (!userRegistry.Dirty) return Task.CompletedTask;

			userRegistry.Dirty = false;
			using var stream = new StreamWriter(filename);
			var registryData = JsonConvert.SerializeObject(userRegistry, Formatting.Indented);
			return stream.WriteAsync(registryData);
		}
	}
}
