using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SilverHand.Core
{
	public class CharacterSheetRepo
	{
		private readonly string rootDirectory;
		private Dictionary<CharacterId, CharacterSheet> characterRepo;

		public CharacterSheetRepo(string rootDirectory)
		{
			this.rootDirectory = rootDirectory;
			if (!Directory.Exists(rootDirectory))
			{
				Directory.CreateDirectory(rootDirectory);
			}
			characterRepo = new ();
		}

		public CharacterSheet? Get(CharacterId character)
		{
			bool v = characterRepo.TryGetValue(character, out CharacterSheet? value);
			return v ? value : null;
		}

		public void AddCharacter(CharacterId character)
		{
			if (!characterRepo.ContainsKey(character))
			{
				characterRepo.Add(character, new CharacterSheet()
				{
					Handle = character.Key
				});
			}
		}

		public bool DeleteCharacter(CharacterId character)
		{
			if (!characterRepo.Remove(character)) return false;

			File.Delete($"{rootDirectory}/{character.Key}.json");
			return true;
		}

		public static async Task<CharacterSheetRepo> LoadAsync(string rootDir, IEnumerable<CharacterId> characters)
		{
			return new(rootDir)
			{
				characterRepo = (from sheet in await Task.WhenAll(from character in characters select CharacterSheet.LoadAsync($"{rootDir}/{character.Key}.json")) select sheet).ToDictionary((s) => new CharacterId(s.Handle))
			};
		}

		public static Task StoreAsync(string rootDir, CharacterSheetRepo characterSheetRepo)
		{
			return Task.WhenAll(from character in characterSheetRepo.characterRepo select CharacterSheet.StoreAsync($"{rootDir}/{character.Key.Key}.json", character.Value));
		}
	}
}
