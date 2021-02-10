using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SilverHand.Core
{
	public class CharacterManager
	{
		private readonly UserRegistry userRegistry;
		private readonly CharacterRegistry characterRegistry;
		private readonly CharacterSheetRepo characterRepo;

		private readonly Dictionary<UserId, CharacterId> activeCharacter;

		protected CharacterManager(UserRegistry uRegistry, CharacterRegistry cRegistry, CharacterSheetRepo cRepo)
		{
			userRegistry = uRegistry;
			characterRegistry = cRegistry;
			characterRepo = cRepo;
			activeCharacter = new();
		}

		public ICollection<ulong> RegisteredUsers => userRegistry.Users;
		public ICollection<CharacterId> RegisteredCharacters => characterRegistry.Characters;
		public IDictionary<UserId, CharacterId> ActiveCharacters => activeCharacter;

		public bool RegisterUser(SocketUser user)
		{
			if (!userRegistry.IsRegistered(user))
			{
				userRegistry.Register(user);
				return true;
			}
			return false;
		}
		public bool DeregisterUser(SocketUser user) => userRegistry.Deregister(user);
		public bool IsUserRegistered(SocketUser user) => userRegistry.IsRegistered(user);

		public bool RegisterCharacter(SocketUser user, string characterName) => characterRegistry.Register(new (user), characterName);
		public bool DeregisterCharacter(SocketUser user, string characterName) => characterRegistry.Deregister(new(user), characterName);
		public bool IsCharacterRegistered(string characterName) => characterRegistry.IsRegistered(characterName);

		public bool IsUserActive(SocketUser user) => activeCharacter.ContainsKey(new(user));
		public bool UseCharacter(SocketUser user, string characterName)
		{
			UserId uid = new(user);
			CharacterId cid = new(characterName);
			if (characterRegistry.IsOwner(cid, uid))
			{
				activeCharacter[uid] = cid;
				return true;
			}
			return false;
		}

		public CharacterSheet? GetCharacterSheet(SocketUser user)
		{
			UserId uid = new(user);
			if (activeCharacter.TryGetValue(uid, out CharacterId characterId))
			{
				return characterRepo.Get(characterId);
			}
			return null;
		}

		#region LoadStore
		private static ConfigFile configFile;
		private static CharacterManager? instance;
		public static CharacterManager Instance => instance ?? throw new NullReferenceException();

		public static async Task<CharacterManager> InitCharacterManagerAsync(ConfigFile config)
		{
			if (instance == null)
			{
				configFile = config;
				var userReg = await UserRegistry.LoadAsync(config.UserRegistry);
				var charReg = await CharacterRegistry.LoadAsync(config.CharacterRegistry);
				var charRepo = await CharacterSheetRepo.LoadAsync(config.CharacterSheetDirectory, charReg.Characters);
				charReg.OnCharacterCreated += (_, character) => charRepo.AddCharacter(character);
				charReg.OnCharacterDeleted += (_, character) => charRepo.DeleteCharacter(character);
				instance = new (
					uRegistry: userReg,
					cRegistry: charReg,
					cRepo: charRepo
				);
			}
			return instance;
		}

		public static Task StoreCharacterManager()
		{
			return Task.WhenAll(
				UserRegistry.StoreAsync(configFile.UserRegistry, Instance.userRegistry),
				CharacterRegistry.StoreAsync(configFile.CharacterRegistry, Instance.characterRegistry),
				CharacterSheetRepo.StoreAsync(configFile.CharacterSheetDirectory, Instance.characterRepo)
			);
		}
		#endregion
	}
}
