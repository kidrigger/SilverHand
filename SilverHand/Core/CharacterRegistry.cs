using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace SilverHand.Core
{
	[JsonArray]
	class OwnershipDict : Dictionary<CharacterId, UserId> { }

	[JsonObject(MemberSerialization.OptIn)]
	public struct CharacterId
	{
		public int Id { get; private set; }

		[JsonProperty("Key")]
		public string Key { get; init; }

		public CharacterId(string name)
		{
			Key = name.Trim();
			Id = Key.ToLower().GetHashCode();
		}

		[OnDeserialized]
		void OnDeserialized(StreamingContext _)
		{
			Id = Key.ToLower().GetHashCode();
		}

		public bool Equals(CharacterId obj) => Id == obj.Id;
		public override bool Equals(object? obj) => obj is CharacterId id && Id == id.Id;
		public override int GetHashCode() => Id;

		public static bool operator ==(CharacterId left, CharacterId right) => left.Equals(right);
		public static bool operator !=(CharacterId left, CharacterId right) => !left.Equals(right);
	}

	[JsonObject(MemberSerialization.OptIn)]
	public class CharacterRegistry
	{
		public event EventHandler<CharacterId>? OnCharacterCreated;
		public event EventHandler<CharacterId>? OnCharacterDeleted;

		public bool Dirty { get; private set; } = false;

		[JsonProperty("regis")]
		private readonly HashSet<CharacterId> registry;

//#pragma warning disable IDE0051 // Remove unused private members
//		private IList<(CharacterId, UserId)> OnlyForSerializationDoNotUse
//		{
//			get => (from entry in ownership select (entry.Key, entry.Value)).ToList();
//			set => ownership = (from entry in value select entry).ToDictionary((e) => e.Item1, (e) => e.Item2);
//		}
//#pragma warning restore IDE0051 // Remove unused private members

		[JsonProperty("ownership")]
		private OwnershipDict ownership;

		public CharacterRegistry()
		{
			registry = new();
			ownership = new();

			OnCharacterCreated += (_, _) => Dirty = true;
			OnCharacterDeleted += (_, _) => Dirty = true;
		}

		public ICollection<CharacterId> Characters { get => registry; }

		public bool Register(UserId user, string character)
		{
			CharacterId cid = new (character);
			if (!registry.Contains(cid))
			{
				registry.Add(cid);
				ownership.Add(cid, user);
				OnCharacterCreated?.Invoke(this, cid);
				return true;
			}
			return false;
		}

		public bool Deregister(UserId user, string character)
		{
			CharacterId cid = new (character);
			if (registry.Contains(cid) && ownership[cid] == user && registry.Remove(cid))
			{
				ownership.Remove(cid);
				OnCharacterDeleted?.Invoke(this, cid);
				return true;
			}
			return false;
		}

		public bool IsOwner(CharacterId character, UserId user) => ownership.TryGetValue(character, out UserId owner) && owner == user;

		public bool IsRegistered(string character)
		{
			return registry.Contains(new (character));
		}

		public static async Task<CharacterRegistry> LoadAsync(string filename)
		{
			using var stream = new StreamReader(filename);
			var data = await stream.ReadToEndAsync();
			return JsonConvert.DeserializeObject<CharacterRegistry>(data);
		}

		public static Task StoreAsync(string filename, CharacterRegistry sheet)
		{
			if (sheet.Dirty)
			{
				sheet.Dirty = false;
				using var stream = new StreamWriter(filename);
				return stream.WriteAsync(JsonConvert.SerializeObject(sheet, Formatting.Indented));
			}
			return Task.CompletedTask;
		}
	}
}
