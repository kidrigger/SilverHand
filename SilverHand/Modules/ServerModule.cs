using Discord.Commands;
using SilverHand.Core;
using System.Linq;
using System.Threading.Tasks;

namespace SilverHand.Modules
{
	[Group("server")]
	public class ServerModule : ModuleBase<SocketCommandContext>
	{
		[Command("reg")]
		[Summary("Registers the user in the server")]
		public Task RegisterUserAsync()
		{
			var user = Context.Message.Author;
			var nick = Context.Guild.GetUser(user.Id).Nickname;
			if (!CharacterManager.Instance.RegisterUser(user))
			{
				return ReplyAsync($"{nick} `({user.Username}#{user.Discriminator})` is already registered.");
			}			
			return Task.WhenAll(CharacterManager.StoreCharacterManager(), ReplyAsync($"{nick} `({user.Username}#{user.Discriminator})` registered."));
		}

		[Command("dereg")]
		[Summary("Deregisters the user in the server")]
		public Task DeregisterUserAsync()
		{
			var user = Context.Message.Author;
			var nick = Context.Guild.GetUser(user.Id).Nickname;
			return !CharacterManager.Instance.DeregisterUser(user) ? ReplyAsync($"{nick} `({user.Username}#{user.Discriminator})` is not registered.") : Task.WhenAll(CharacterManager.StoreCharacterManager(), ReplyAsync($"{nick} `({user.Username}#{user.Discriminator})` deregistered."));
		}

		[Command("players")]
		[Summary("Lists the registered players in the server")]
		public Task ListUsersAsync()
		{
			var users = (from userId in CharacterManager.Instance.RegisteredUsers select $"\n - {Context.Guild.GetUser(userId).Nickname}").ToList();
			return ReplyAsync(users.Count == 0 ? "No users registered" : string.Join(string.Empty, users));
		}
	}
}
