using System;
using Discord.Commands;
using SilverHand.Core;
using System.Threading.Tasks;

namespace SilverHand.Modules
{
	[Group("stat")]
	public class PlayerStatModule : ModuleBase<SocketCommandContext>
	{
		[Command("get")]
		[Summary("Gets the stat mentioned stat of the user")]
		public Task GetStatAsync([Summary("Stat name")][Remainder]string stat)
		{
			var user = Context.Message.Author;
			var nick = Context.Guild.GetUser(user.Id).Nickname;

			var cm = CharacterManager.Instance;

			var charSheet = cm.GetCharacterSheet(user);
			if (charSheet == null)
			{
				if (!cm.IsUserRegistered(user)) return ReplyAsync($"{nick} is not a registered user. Register with `.server reg`");
				if (!cm.IsUserActive(user)) return ReplyAsync($"{nick} is not playing as a character. Roleplay with `.char use <charactername>`");
				return ReplyAsync($"Character not found");
			}

			stat = stat.Trim();
			try
			{
				stat = CharacterSheet.GetStatName(stat);
				var statValue = charSheet.GetStat(stat);
				return ReplyAsync(statValue != int.MinValue ? $"{charSheet.Handle}'s {stat} : {statValue}" : $"{charSheet.Handle}'s {stat} is not set!");
			}
			catch (Exception e)
			{
				return ReplyAsync(e.Message);
			}
		}

		[Command("set")]
		[Summary("Sets the stat mentioned stat of the user")]
		public Task SetStatAsync([Summary("Value of stat")] int val, [Summary("Stat name")][Remainder] string stat)
		{
			var user = Context.Message.Author;
			var nick = Context.Guild.GetUser(user.Id).Nickname;

			var cm = CharacterManager.Instance;

			var charSheet = cm.GetCharacterSheet(user);
			if (charSheet == null)
			{
				if (!cm.IsUserRegistered(user))
					return ReplyAsync($"{nick} is not a registered user. Register with `.server reg`");
				if (!cm.IsUserActive(user))
					return ReplyAsync($"{nick} is not playing as a character. Roleplay with `.char use <charactername>`");
				return ReplyAsync("Character not found");

			}

			stat = stat.Trim();
			try
			{
				stat = CharacterSheet.GetStatName(stat);
				charSheet.SetStat(stat, val);
				return Task.WhenAll(CharacterManager.StoreCharacterManager(),
					ReplyAsync($"{charSheet.Handle}'s {stat} <- {val}"));
			}
			catch (Exception e)
			{
				return ReplyAsync($"{e.Message}");
			}
		}
	}

	[Group("skill")]
	public class PlayerSkillModule : ModuleBase<SocketCommandContext>
	{
		[Command("get")]
		[Summary("Gets the mentioned skill of the user")]
		public Task GetSkillAsync([Summary("skill name")][Remainder] string skill)
		{
			var user = Context.Message.Author;
			var nick = Context.Guild.GetUser(user.Id).Nickname;

			var cm = CharacterManager.Instance;

			var charSheet = cm.GetCharacterSheet(user);
			if (charSheet == null)
			{
				if (!cm.IsUserRegistered(user)) return ReplyAsync($"{nick} is not a registered user. Register with `.server reg`");
				if (!cm.IsUserActive(user)) return ReplyAsync($"{nick} is not playing as a character. Roleplay with `.char use <charactername>`");
				return ReplyAsync($"Character not found");
			}

			skill = skill.Trim();
			try
			{
				skill = CharacterSheet.GetSkillName(skill);
				var statValue = charSheet.GetSkill(skill);
				return ReplyAsync(statValue != int.MinValue
					? $"{charSheet.Handle}'s {skill} : {statValue}"
					: $"{charSheet.Handle}'s {skill} is not set!");
			}
			catch (Exception e)
			{
				return ReplyAsync(e.Message);
			}
		}

		[Command("set")]
		[Summary("Sets the skill mentioned by the user")]
		public Task SetSkillAsync([Summary("Value of skill")] int val, [Summary("skill name")][Remainder] string skill)
		{
			var user = Context.Message.Author;
			var nick = Context.Guild.GetUser(user.Id).Nickname;

			var cm = CharacterManager.Instance;

			var charSheet = cm.GetCharacterSheet(user);
			if (charSheet == null)
			{
				if (!cm.IsUserRegistered(user))
					return ReplyAsync($"{nick} is not a registered user. Register with `.server reg`");
				if (!cm.IsUserActive(user))
					return ReplyAsync($"{nick} is not playing as a character. Roleplay with `.char use <charactername>`");
				return ReplyAsync("Character not found");

			}
			
			try
			{
				skill = CharacterSheet.GetSkillName(skill);
				charSheet.SetSkill(skill, val);
				return Task.WhenAll(CharacterManager.StoreCharacterManager(),
					ReplyAsync($"{charSheet.Handle}'s {skill} <- {val}"));
			}
			catch (Exception e)
			{
				return ReplyAsync($"{e.Message}");
			}
		}

		[Command("roll")]
		[Summary("Rolls the skill mentioned by the user")]
		public Task RollSkillAsync([Summary("Skill name")] [Remainder] string skill)
		{
			var user = Context.Message.Author;
			var nick = Context.Guild.GetUser(user.Id).Nickname;

			var cm = CharacterManager.Instance;

			var charSheet = cm.GetCharacterSheet(user);
			if (charSheet == null)
			{
				if (!cm.IsUserRegistered(user)) return ReplyAsync($"{nick} is not a registered user. Register with `.server reg`");
				if (!cm.IsUserActive(user)) return ReplyAsync($"{nick} is not playing as a character. Roleplay with `.char use <charactername>`");
				return ReplyAsync($"Character not found");
			}

			skill = skill.Trim();
			try
			{
				skill = CharacterSheet.GetSkillName(skill);

				if (Skills.GetSkillStatName(skill) is not { } stat) throw new ArgumentException($"Skill {skill} doesn't exist or does not have a stat");

				var statValue = charSheet.GetStat(stat);
				var skillValue = charSheet.GetSkill(skill);
				var roll = (int)DiceRoller.Roll(new DiceRoll(1, 10));

				return ReplyAsync($"{charSheet.Handle}'s {skill} roll : [1d10 ({roll}) + {stat} ({statValue}) + {skill} ({skillValue})] => **`{roll + skillValue + statValue}`**");
			}
			catch (Exception e)
			{
				return ReplyAsync(e.Message);
			}
		}
	}
}
