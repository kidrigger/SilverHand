using Discord.Commands;
using SilverHand.Core;
using System.Threading.Tasks;

namespace SilverHand.Modules
{
	public class RollModule : ModuleBase<SocketCommandContext>
	{
		[Command("r")]
		[Summary("Rolls a dice")]
		public Task RollDiceAsync([Remainder][Summary("The dice roll")] DiceRoll diceRoll)
		{
			var nick = Context.Guild.GetUser(Context.Message.Author.Id).Nickname;
			var roll = DiceRoller.Roll(diceRoll, out var rollLog);
			return ReplyAsync($"{rollLog}\n {nick}'s roll {diceRoll} => **`{roll}`**");
		}
	}
}
