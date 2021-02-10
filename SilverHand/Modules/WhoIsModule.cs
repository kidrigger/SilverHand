using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace SilverHand.Modules
{
	public class WhoIsModule : ModuleBase<SocketCommandContext>
	{
		[Command("whois")]
		[Summary("Tells you the id of user")]
		public Task WhoIsAsync([Summary("The (optional) user to get info from")] SocketUser? user = null) {
			var userInfo = user ?? Context.Message.Author;
			return ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
		}

		[Command("whoami")]
		[Summary("Tells you who you are")]
		public Task WhoAmIAsync()
		{
			var userInfo = Context.Message.Author;
			return ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
		}
	}
}
