using System;
using System.IO;
using System.Reflection;
using TextAdventure.Environments;
using TextAdventure.Language;

namespace TextAdventure
{
	/// <summary>
	/// The main class.
	/// </summary>
	class MainClass
	{
		private const string WELCOME_STRING = "Welcome to ";
		public const string GAME_NAME = "Text Adventure";
		private const string SELECT_MAP_STRING = "Please enter the name of the map you'd like to load, without extension.";
		private const string FILE_NOT_FOUND = "File not found: ";

		public static void Main (string[] args)
		{
			// Welcome player
			Output.Print (WELCOME_STRING + GAME_NAME + "!");

			// Load the map.
			LoadMap ();

			// Initialize scripting logic
			TextAdventure.IO.LuaSystem.LuaManager.DoAllFiles ();

			Console.Clear ();

			// Start game loop
			Globals.player.EnterRoom (Room.GetRoom ("start"));
		}

		/// <summary>
		/// Asks the user what map they want to load, then loads the map.
		/// </summary>
		private static void LoadMap ()
		{
			string roomPath = Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location);
			roomPath = Path.Combine (roomPath, "Maps");
			Output.Print (SELECT_MAP_STRING);
			string input = Input.GetLine ();
			if (input.ToLower () == "debug")
			{
				Output.Print ("Debug mode initialized.");
				Globals.isDebug = true;
				LoadMap ();
				return;
			}
			input = Path.Combine (roomPath, input, input + ".map");
			if (File.Exists (input))
			{
				Room.InitRooms (input);
			}
			else
			{
				Output.Print (FILE_NOT_FOUND + input);
				LoadMap ();
			}
		}
	}
}
