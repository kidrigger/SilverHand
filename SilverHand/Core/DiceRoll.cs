﻿using System;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace SilverHand.Core
{
	public static class DiceRoller {
		private readonly static Random rng = new Random();

		public static long Roll(DiceRoll dice)
		{
			long total = 0;
			for (int i = 0; i < dice.Count; ++i)
			{
				total += rng.Next(dice.Dice) + 1;
			}
			return total + dice.Modifier;
		}

		public static long Roll(DiceRoll dice, out string log)
		{
			StringBuilder sb = new("[");
			long total = 0;
			for (int i = 0; i < dice.Count; ++i)
			{
				var v = (rng.Next(dice.Dice) + 1);
				sb.Append($"{(i == 0 ? string.Empty : "+")}{v}");
				total += v;
			}
			sb.Append($"]");
			if (dice.Modifier != 0)
			{
				sb.Append($" {dice.Sign} {Math.Abs(dice.Modifier)}");
			}
			log = sb.ToString();
			return total + dice.Modifier;
		}
	}

	public struct DiceRoll
	{
		public int Count { get; init; }
		public int Dice { get; init; }
		public int Modifier { get; init; }
		public char Sign { get => Modifier > 0 ? '+' : '-'; }

		public DiceRoll(int dice) : this(1, dice) { }

		public DiceRoll(int count, int dice, int modifier = 0)
		{
			Count = count;
			Dice = dice;
			Modifier = modifier;
		}

		public override string ToString() => Modifier == 0 ? $"{Count}D{Dice}" : $"{Count} d{Dice} {Sign} {Math.Abs(Modifier)}";
		
		public static bool TryParse(string s, out DiceRoll diceRoll)
		{
			diceRoll = new DiceRoll { };
			int count = 1;
			s = s.ToLower().Trim();
			int dIdx = s.IndexOf('d');
			if (int.TryParse(s[..dIdx].Trim(), out int countResult))
			{
				count = countResult;
			}
			s = s[(dIdx + 1)..];
			int mIdx = Math.Max(s.IndexOf('+'), s.IndexOf('-'));
			int dice;
			int mod;
			if (mIdx < 0)
			{
				mod = 0;
				if (int.TryParse(s.Trim(), out int diceResult))
				{
					dice = diceResult;
				}
				else
				{
					return false;
				}
			}
			else
			{
				if (int.TryParse(s[..mIdx].Trim(), out int diceResult))
				{
					dice = diceResult;
				}
				else
				{
					return false;
				}

				if (int.TryParse(s[(mIdx + 1)..].Trim(), out int modResult))
				{
					mod = s[mIdx] == '+' ? modResult : -modResult;
				}
				else
				{
					return false;
				}
			}


			diceRoll = new DiceRoll
			{
				Count = count,
				Dice = dice,
				Modifier = mod,
			};
			return true;
		}
	}

	public class DiceRollTypeReader : TypeReader
	{
		public override Task<TypeReaderResult> ReadAsync(ICommandContext context, string input, IServiceProvider services)
		{
			if (DiceRoll.TryParse(input, out DiceRoll result))
			{
				return Task.FromResult(TypeReaderResult.FromSuccess(result));
			}

			return Task.FromResult(TypeReaderResult.FromError(CommandError.ParseFailed, "Input could not be parsed as a DiceRoll."));
		}
	}
}
