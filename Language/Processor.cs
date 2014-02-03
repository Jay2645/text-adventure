using System;
using System.Collections.Generic;

namespace TextAdventure.Language
{
	public static class Processor
	{
		private static Dictionary<string, Action<string>> commands
		{
			get
			{
				if (_commands == null)
				{
					InitializeDictionaries ();
				}
				return _commands;
			}
		}
		private static Dictionary<string, string> commandHelp
		{
			get
			{
				if (_commandHelp == null)
				{
					InitializeDictionaries ();
				}
				return _commandHelp;
			}
		}

		private static Dictionary<string, Action<string>> _commands = null;
		private static Dictionary<string, string> _commandHelp = null;

		public const string DO_NOT_UNDERSTAND = "I do not understand that.";
		private const string HELP_DESC = "Gives help to the user.";
		private const string QUIT_DESC = "Quits the game.";

		private static void InitializeDictionaries ()
		{
			_commands = new Dictionary<string, Action<string>> ();
			_commandHelp = new Dictionary<string, string> ();
			_commands.Add ("help", Help);
			_commandHelp.Add ("help", HELP_DESC);
			_commandHelp.Add ("quit", QUIT_DESC);
			_commandHelp.Add ("q", QUIT_DESC);
		}

		public static void AddCommand (string name, string helpDesc, Action<string> perform)
		{
			name = name.ToLower ();
			commands.Add (name, perform);
			commandHelp.Add (name, helpDesc);
		}

		public static void ProcessInput (string input)
		{
			int firstSpace = input.IndexOf (' ');

			string command; 
			string param;
			if (firstSpace > -1)
			{
				command = input.Substring (0, firstSpace).ToLower ();
				param = input.Substring (firstSpace + 1);
				if (!commands.ContainsKey (command)) // No command found, might have gotten spacing wrong
				{
					firstSpace = param.IndexOf (' ');
					if (firstSpace > -1)
					{
						command += " " + param.Substring (0, firstSpace);
						param = param.Substring (firstSpace);
					}
				}
			}
			else
			{
				// User may have entered direction without "go"
				Environments.Direction dir = TextAdventure.Environments.DirectionConverter.FromString (input);
				if (dir == TextAdventure.Environments.Direction.None)
				{
					command = input;
					param = "";
				}
				else
				{
					command = "go";
					param = input;
				}
			}
			if (command == "quit" || command == "q")
			{
				Environment.Exit (0);
				return;
			}
			if (commands.ContainsKey (command))
			{
				commands [command].Invoke (param);
			}
			else
			{
				Output.Print (DO_NOT_UNDERSTAND);
			}
			Input.ProcessLine ();
		}

		public static void Help (string input)
		{
			if (input == "")
			{
				foreach (KeyValuePair<string, string> kvp in _commandHelp)
				{
					Help (kvp.Key);
				}
			}
			else if (commandHelp.ContainsKey (input))
			{
				Output.Print (input + ": " + commandHelp [input]);
			}
		}
	}
}

