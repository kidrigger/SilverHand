using Discord.Commands;
using SilverHand.Core;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SilverHand.Modules
{
	[Group("char")]
	public class CharacterModule : ModuleBase<SocketCommandContext>
	{
		[Command("new")]
		[Summary("Creates a new character")]
		public Task NewCharAsync([Summary("The character name")][Remainder]string character)
		{
			character = character.Trim();
			var user = Context.Message.Author;
			var nick = Context.Guild.GetUser(user.Id).Nickname;
			if (!CharacterManager.Instance.IsUserRegistered(user))
			{
				return ReplyAsync($"User _{nick}_ is not registered. Please register yourself with `.server reg`.");
			}
			if (!CharacterManager.Instance.RegisterCharacter(user, character))
			{
				return ReplyAsync($"_{character}_ already exists.");
			}
			return Task.WhenAll(CharacterManager.StoreCharacterManager(), ReplyAsync($"_{character}_ registered by `{nick}`."));
		}

		[Command("use")]
		[Summary("Uses character")]
		public Task UseCharAsync([Summary("The character name")][Remainder] string character)
		{
			var user = Context.Message.Author;
			var nick = Context.Guild.GetUser(user.Id).Nickname;
			if (!CharacterManager.Instance.IsUserRegistered(user))
			{
				return ReplyAsync($"User _{nick}_ is not registered. Please register yourself with `.server reg`.");
			}
			if (!CharacterManager.Instance.IsCharacterRegistered(character))
			{
				return ReplyAsync($"_{character}_ is not a registered character create a new one with `.char new`.");
			}
			if (!CharacterManager.Instance.UseCharacter(user, character))
			{
				return ReplyAsync($"_{character}_ does not belong to _{nick}_");
			}
			return ReplyAsync($"_{nick}_ is now _{character}_");
		}

		[Command("list")]
		[Summary("Lists characters")]
		public Task ListCharsAsync()
		{
			var chars = CharacterManager.Instance.RegisteredCharacters;
			if (chars.Any()) {
				return ReplyAsync(string.Join(string.Empty, from character in chars select $"\n - {character.Key}"));
			}
			return ReplyAsync("No characters registered");
		}

		[Command("active")]
		[Summary("Lists active characters")]
		public Task ListActiveCharsAsync()
		{
			var chars = CharacterManager.Instance.ActiveCharacters;
			string GetNick(ulong v) => Context.Guild.GetUser(v).Nickname;
			if (chars.Count != 0)
			{
				return ReplyAsync(string.Join(string.Empty, from character in chars select $"\n - {GetNick(character.Key.Id)} -> {character.Value.Key}"));
			}
			return ReplyAsync("No characters registered");
		}

	}
}
