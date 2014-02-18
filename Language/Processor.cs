using System;
using System.Collections.Generic;
using LuaInterface;

namespace TextAdventure.Language
{
	/// <summary>
	/// Proccesses any commands from the console.
	/// This class transforms user input into machine-readable code.
	/// </summary>
	public static class Processor
	{
		private static Dictionary<string, LuaFunction> luaCommands
		{
			get
			{
				if (_luaCommands == null)
				{
					InitializeDictionaries ();
				}
				return _luaCommands;
			}
		}
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
		private static Dictionary<string, LuaFunction> _luaCommands = null;
		private static Dictionary<string, string> _commandHelp = null;
		private static Dictionary<string, string> commandIndex = new Dictionary<string, string> ();

		/// <summary>
		/// A message stating that the game does not understand the input provided.
		/// </summary>
		public const string DO_NOT_UNDERSTAND = "I do not understand that.";
		private const string HELP_DESC = "Gives help to the user.";
		private const string QUIT_DESC = "Quits the game.";

		/// <summary>
		/// Initializes the dictionaries and adds the base commands.
		/// </summary>
		private static void InitializeDictionaries ()
		{
			_commands = new Dictionary<string, Action<string>> ();
			_luaCommands = new Dictionary<string, LuaFunction> ();
			_commandHelp = new Dictionary<string, string> ();
			_commands.Add ("help", Help);
			_commandHelp.Add ("help", HELP_DESC);
			_commandHelp.Add ("quit", QUIT_DESC);
			_commandHelp.Add ("q", QUIT_DESC);
		}

		/// <summary>
		/// Adds a command to the dictionary.
		/// </summary>
		/// <param name='name'>
		/// What the user types in to invoke the command.
		/// </param>
		/// <param name='helpDesc'>
		/// What the user sees when they type "help command".
		/// </param>
		/// <param name='perform'>
		/// The Method to invoke. Anything after the command string will be passed as a parameter.
		/// </param>
		public static void AddCommand (string name, string helpDesc, Action<string> perform)
		{
			name = name.ToLower ();
			if (commandHelp.ContainsKey (name))
			{
				return;
			}
			commands.Add (name, perform);
			commandHelp.Add (name, helpDesc);
		}

		public static void AddCommand (string name, string helpDesc, LuaFunction perform)
		{
			name = name.ToLower ();
			if (commandHelp.ContainsKey (name))
			{
				return;
			}
			commandHelp.Add (name, helpDesc);
			luaCommands.Add (name, perform);
		}

		/// <summary>
		/// Processes user input.
		/// </summary>
		/// <param name='input'>
		/// What the user has typed into the console.
		/// </param>
		public static void ProcessInput (string input)
		{
			int firstSpace = input.IndexOf (' ');
			string command; 
			string param;

			if (firstSpace > -1)
			{
				command = input.Substring (0, firstSpace).ToLower ();
				param = input.Substring (firstSpace + 1);
				if (!commands.ContainsKey (command) && !luaCommands.ContainsKey (command))
				{
					// No command found, might have gotten spacing wrong
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
			command = command.ToLower ().Trim ();
			param = param.Trim ();
			// Special case: User wants to quit the game
			if (command == "quit" || command == "q")
			{
				Environment.Exit (0);
				return;
			}
			// Invoke the command.
			if (commands.ContainsKey (command))
			{
				NotifyObservers (command, param); // Notify FIRST, otherwise "go" command notifications never get processed
				commands [command].Invoke (param);
			}
			else if (luaCommands.ContainsKey (command))
			{
				NotifyObservers (command, param);
				luaCommands [command].Call (param);
			}
			else
			{
				// Could not find the specified command.
				Output.Print (DO_NOT_UNDERSTAND);
			}
			// Back to the game loop.
			Input.ProcessLine ();
		}

		private static void NotifyObservers (string command, string param)
		{
			if (commandIndex.ContainsKey (command))
			{
				commandIndex [command] = param;
			}
			else
			{
				commandIndex.Add (command, param);
			}
			Globals.player.Notify (command, Observers.EventList.OnPlayerCommand);
		}

		/// <summary>
		/// Gets the last parameters for the given command.
		/// For example, if the player typed "go south" then "go north," the command "go" would return "north."
		/// </summary>
		/// <param name='command'>
		/// The command to check.
		/// </param>
		public static string GetCommandParameters (string command)
		{
			command = command.ToLower ();
			if (commandIndex.ContainsKey (command))
			{
				return commandIndex [command];
			}
			return "";
		}

		/// <summary>
		/// Provides help based on the specified input.
		/// If the input is empty, will provide all help.
		/// </summary>
		/// <param name='input'>
		/// The subject where help is needed.
		/// </param>
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

