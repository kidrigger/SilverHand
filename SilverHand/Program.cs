using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using Discord.Commands;
using SilverHand.Core;

new SilverHand.Program().MainAsync().GetAwaiter().GetResult();

namespace SilverHand
{
	public class Program
	{
        private readonly DiscordSocketClient client;
        private readonly CommandService commands;
        private readonly ConfigFile config;
        private const string ConfigFile = "silverhand.config.json";

        public Program()
        {
            client = new DiscordSocketClient();
            commands = new CommandService(new CommandServiceConfig());
			using var reader = new StreamReader(ConfigFile);
			config = JsonConvert.DeserializeObject<ConfigFile>(reader.ReadToEnd());
		}

        private static async Task<string> GetTokenAsync()
        {
	        using var stream = new StreamReader("token.txt");
	        return await stream.ReadToEndAsync();
        }

        public async Task MainAsync()
        {
            _ = await CharacterManager.InitCharacterManagerAsync(config);
            Stats.Init();
            Skills.Init();

            client.Log += Log;
            commands.Log += Log;

            var token = await GetTokenAsync();

            var cmd = new CommandHandler(client, commands);
            await cmd.InstallCommandsAsync();

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            
            // Block this task until the program is closed.
            await Task.Delay(-1);

            await CharacterManager.StoreCharacterManager();
        }

        private static Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}
	}
}
