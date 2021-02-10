using Discord.Commands;
using SilverHand.Core;
using System.Threading.Tasks;

namespace SilverHand.Modules
{
	// Keep in mind your module **must** be public and inherit ModuleBase.
	// If it isn't, it will not be discovered by AddModulesAsync!
	public class WakeUpModule : ModuleBase<SocketCommandContext>
	{
		[Command("wake")]
		[Summary("Wakes you up")]
		public Task SayAsync()
		{
			return ReplyAsync("Wake the fuck up, Samurai! We have a city to burn.");
		}

		[Command("save")]
		[Summary("Save everything")]
		public Task SaveAsync()
		{
			return Task.WhenAny(CharacterManager.StoreCharacterManager(), ReplyAsync("Saving your mind from destruction"));
		}
	}
}
