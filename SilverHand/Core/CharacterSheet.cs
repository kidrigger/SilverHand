using Newtonsoft.Json;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Threading;

namespace SilverHand.Core
{
	[JsonObject]
	public record Stats
	{
		[Accessor("Intelligence", "INT")]
		public int Intelligence { get; set; }

		[Accessor("Reflexes", "Reflex", "REF")]
		public int Reflexes { get; set; }

		[Accessor("TECH","Technology")]
		public int Tech { get; set; }

		[Accessor("Cool", "Will")]
		public int Cool { get; set; }

		[Accessor("ATTR", "Attract", "Attractive", "Attractiveness")]
		public int Attractiveness { get; set; }

		[Accessor("Luck", "Lucky")]
		public int Luck { get; set; }

		[Accessor("Move","Movement","MA","Movement Allowance", "MovementAllowance")]
		public int MovementAllowance { get; set; }

		[Accessor("Bod", "Body", "Body Type", "BodyType")]
		public int Body { get; set; }

		[Accessor("EMP", "Empath", "Empathy")]
		public int Empathy { get; set; }
		
		[Accessor("Run", "Running", "Run Speed")]
		public int Running { get; set; }

		[Accessor("Jump", "Leap")]
		public int Leap { get; set; }

		[Accessor("Lift", "DeadLift")]
		public int Lift { get; set; }

		private static Dictionary<string, PropertyInfo> _propertyMap = new ();
		public static void Init()
		{
			_propertyMap = (from prop in typeof(Stats).GetProperties()
						   from attr in prop.GetCustomAttributes(typeof(AccessorAttribute))
						   from name in ((AccessorAttribute)attr).AccessorNames
						   select (name, prop)).ToDictionary((v)=>v.name, (v)=>v.prop);
		}

		public int? GetStat(in string key)
		{
			if (_propertyMap.TryGetValue(key.ToLower(), out var prop))
				return prop.GetValue(this) as int?;

			return null;
		}

		public bool SetStat(in string key, int value)
		{
			if (!_propertyMap.TryGetValue(key.ToLower(), out var prop)) return false;

			prop.SetValue(this, value);
			OnSetDirty?.Invoke();
			return true;
		}

		public static string? GetCanonicalName(string key) => _propertyMap.TryGetValue(key.ToLower(), out var prop) ? prop.Name : null;

		public event Action? OnSetDirty;
	}

	public record Armor
	{
		public int Head { get; set; }
		public int Torso { get; set; }
		public int RightArm { get; set; }
		public int LeftArm { get; set; }
		public int RightLeg { get; set; }
		public int LeftLeg { get; set; }
	}

	[JsonObject]
	public record Skills
	{
		[JsonObject]
		public record SpecialSkill
		{
			[Accessor("Authority")]
			public int Authority { get; set; }

			[Accessor("CharismaticLeadership", "Charismatic Leadership", "CharismaticLead", "ChrLead")]
			public int CharismaticLeadership { get; set; }

			[Accessor("CombatSense", "Combat", "Combat Sense")]
			public int CombatSense { get; set; }

			[Accessor("Cred", "Credibility")]
			public int Credibility { get; set; }

			[Accessor("Fam", "Family")]
			public int Family { get; set; }

			[Accessor("Interface", "Face")]
			public int Interface { get; set; }

			[Accessor("JuryRig", "Rig", "Jury Rig")]
			public int JuryRig { get; set; }

			[Accessor("MedTech", "MedicalTech", "MTech")]
			public int MedicalTech { get; set; }

			[Accessor("Resource", "Resources")]
			public int Resources { get; set; }

			[Accessor("StreetDeal")]
			public int StreetDeal { get; set; }
		}

		[JsonObject]
		public record AttractivenessSkill
		{
			[Accessor("Grooming", "PersonalGrooming", "Personal Grooming")]
			public int PersonalGrooming { get; set; }

			[Accessor("Style", "Wardrobe")]
			public int Style { get; set; }
		}

		[JsonObject]
		public record BodySkill
		{
			[Accessor("Endurance")]
			public int Endurance { get; set; }

			[Accessor("Strength", "StrengthFeat", "Strength Feat")]
			public int StrengthFeat { get; set; }

			[Accessor("Swimming", "Swim")]
			public int Swimming { get; set; }
		}

		[JsonObject]
		public record CoolSkill
		{
			[Accessor("Interrogation", "Interrogate")]
			public int Interrogation { get; set; }

			[Accessor("Intimidation", "Intimidate")]
			public int Intimidate { get; set; }

			[Accessor("Oratory", "Speech")]
			public int Oratory { get; set; }

			[Accessor("Resist", "Resist Torture", "Resist Drugs", "ResistTorture", "ResistDrugs")]
			public int Resist { get; set; }

			[Accessor("StreetWise")]
			public int StreetWise { get; set; }
		}

		[JsonObject]
		public record EmpathySkill
		{
			[Accessor("Perception", "Human Perception")]
			public int HumanPerception { get; set; }

			[Accessor("Interview")]
			public int Interview { get; set; }

			[Accessor("Leadership", "Lead")]
			public int Leadership { get; set; }

			[Accessor("Seduction", "Seduce")]
			public int Seduction { get; set; }

			[Accessor("Social")]
			public int Social { get; set; }

			[Accessor("Persuade", "Persuasion", "Fast Talk", "FastTalk")]
			public int Persuasion { get; set; }

			[Accessor("Perform", "Performance")]
			public int Perform { get; set; }
		}

		// TODO: Language

		[JsonObject]
		public record IntelligenceSkill
		{
			[Accessor("Accounting")]
			public int Accounting { get; set; }

			[Accessor("Anthropology")]
			public int Anthropology { get; set; }

			[Accessor("Awareness", "Notice")]
			public int Awareness { get; set; }

			[Accessor("Biology")]
			public int Biology { get; set; }

			[Accessor("Botany")]
			public int Botany { get; set; }

			[Accessor("Chemistry")]
			public int Chemistry { get; set; }

			[Accessor("Composition")]
			public int Composition { get; set; }

			[Accessor("DiagnoseIllness", "Diagnosis", "Diagnose", "Diagnose Illness")]
			public int DiagnoseIllness { get; set; }

			[Accessor("Education", "GK", "General Knowledge", "GenKnow", "GeneralKnowledge")]
			public int Education { get; set; }

			[Accessor("Expert")]
			public int Expert { get; set; }

			[Accessor("Gamble")]
			public int Gamble { get; set; }

			[Accessor("Geology")]
			public int Geology { get; set; }

			[Accessor("Hide", "Evade")]
			public int Hide { get; set; }

			[Accessor("History")]
			public int History { get; set; }

			[Accessor("LibrarySearch", "Library")]
			public int LibrarySearch { get; set; }

			[Accessor("Maths", "Mathematics", "Math")]
			public int Mathematics { get; set; }

			[Accessor("Physics")]
			public int Physics { get; set; }

			[Accessor("Programming", "Coding")]
			public int Programming { get; set; }

			[Accessor("Shadow", "Track")]
			public int Shadow { get; set; }

			[Accessor("Stock Market", "StockMarket")]
			public int StockMarket { get; set; }

			[Accessor("SystemKnowledge", "System Knowledge")]
			public int SystemKnowledge { get; set; }

			[Accessor("Teaching")]
			public int Teaching { get; set; }

			[Accessor("Survival")]
			public int Survival { get; set; }

			[Accessor("Zoology")]
			public int Zoology { get; set; }
		}

		public SpecialSkill Special { get; set; } = new();
		public AttractivenessSkill Attractiveness { get; set; } = new();
		public BodySkill Body { get; set; } = new();
		public CoolSkill Cool { get; set; } = new();
		public EmpathySkill Empathy { get; set; } = new();
		public IntelligenceSkill Intelligence { get; set; } = new();

		private static Dictionary<string, (PropertyInfo, PropertyInfo)> _propertyMap = new();
		public static void Init()
		{
			_propertyMap = (from stat in typeof(Skills).GetProperties()
				from skill in stat.PropertyType.GetProperties()
				from attr in skill.GetCustomAttributes(typeof(AccessorAttribute))
				from name in ((AccessorAttribute)attr).AccessorNames
				select (name, statSkill: (stat, skill))).ToDictionary((v) => v.name, (v) => v.statSkill);
		}

		public int? GetSkill(in string key)
		{
			if (_propertyMap.TryGetValue(key.ToLower(), out var statSkill))
			{
				var (stat, skill) = statSkill;
				if (skill.GetValue(stat.GetValue(this)) is int i)
					return i;
			}
			return null;
		}

		public bool SetSkill(string key, int value)
		{
			if (!_propertyMap.TryGetValue(key.ToLower(), out var statSkill)) return false;

			var (stat, skill) = statSkill;

			skill.SetValue(stat.GetValue(this), value);
			OnSetDirty?.Invoke();
			return true;
		}

		public static string? GetSkillStatName(in string key)
		{
			return _propertyMap.TryGetValue(key.ToLower(), out var statSkill) ? statSkill.Item1.Name.ToLower() : null;
		}

		public static string? GetCanonicalName(string key)
		{
			return _propertyMap.TryGetValue(key.ToLower(), out var statSkill) ? statSkill.Item2.Name.ToLower() : null;
		}
		
		public event Action? OnSetDirty;
	}

	public enum Role
	{ 
		Solo,
		Rocker,
		NetRunner,
		Media,
		Nomad,
		Fixer,
		Corp,
		Techie,
		MedTechie,
	}

	[JsonObject(MemberSerialization.OptIn)]
	public record CharacterSheet
	{
		[JsonProperty("Handle")]
		public string Handle { get => handle; set { SetDirty(); handle = value; } }

		[JsonProperty("Role")]
		public Role Role { get; init; }

		[JsonProperty("Stats")]
		public Stats Stats { get; init; }

		[JsonProperty("Skills")]
		public Skills Skills { get; init; }

		private bool dirty;
		private string handle;

		private void SetDirty() => dirty = true;

		public CharacterSheet()
		{
			handle = string.Empty;
			Stats = new Stats();
			Stats.OnSetDirty += SetDirty;

			Skills = new Skills();
			Skills.OnSetDirty += SetDirty;

			dirty = true;
		}

		public int GetStat(in string stat) => Stats.GetStat(stat) ?? throw new ArgumentException($"Stat {stat} doesn't exist");
		public static string GetStatName(in string stat) => Stats.GetCanonicalName(stat) ?? throw new ArgumentException($"Stat {stat} doesn't exist");

		public void SetStat(in string stat, in int value)
		{
			if (!Stats.SetStat(stat, value))
				throw new ArgumentException($"Stat {stat} doesn't exist");
		}

		public int GetSkill(in string skill) => Skills.GetSkill(skill) ?? throw new ArgumentException($"Skill {skill} doesn't exist");
		public static string GetSkillName(in string skill) => Skills.GetCanonicalName(skill) ?? throw new ArgumentException($"Skill {skill} doesn't exist");
		public void SetSkill(in string skill, in int value)
		{
			if (!Skills.SetSkill(skill, value))
				throw new ArgumentException($"Skill {skill} doesn't exist");
		}

		private static bool _isWriting = false;
		public static async Task<CharacterSheet> LoadAsync(string filename)
		{
			using var stream = new StreamReader(filename);
			var data = await stream.ReadToEndAsync();
			return JsonConvert.DeserializeObject<CharacterSheet>(data);
		}

		public static async Task StoreAsync(string filename, CharacterSheet sheet)
		{
			if (!sheet.dirty) return;

			while (_isWriting) await Task.Delay(100);

			await using var stream = new StreamWriter(filename);
			await stream.WriteAsync(JsonConvert.SerializeObject(sheet, Formatting.Indented));
			_isWriting = false;
		}
	}
}
