using System;
using System.Collections.Generic;

namespace TextAdventure.Language
{
	/// <summary>
	/// Proccesses any commands from the console.
	/// This class transforms user input into machine-readable code.
	/// </summary>
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
			// Special case: User wants to quit the game
			if (command == "quit" || command == "q")
			{
				Environment.Exit (0);
				return;
			}
			// Invoke the command.
			if (commands.ContainsKey (command))
			{
				commands [command].Invoke (param);
			}
			else
			{
				// Could not find the specified command.
				Output.Print (DO_NOT_UNDERSTAND);
			}
			// Back to the game loop.
			Input.ProcessLine ();
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

