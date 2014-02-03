using System;
using System.IO;
using System.Reflection;
using TextAdventure.Environments;
using TextAdventure.Language;

namespace TextAdventure
{
	class MainClass
	{
		private const string WELCOME_STRING = "Welcome to ";
		public const string GAME_NAME = "Text Adventure";
		private const string SELECT_MAP_STRING = "Please enter the name of the map you'd like to load.";
		private const string FILE_NOT_FOUND = "File not found: ";

		public static void Main (string[] args)
		{
			// Welcome player
			Output.Print (WELCOME_STRING + GAME_NAME + "!");

			// Load the map.
			LoadMap ();

			// Start game loop
			Globals.player.EnterRoom (Room.GetRoom ("start"));
		}

		private static void LoadMap ()
		{
			string roomPath = Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location);
			roomPath = Path.Combine (roomPath, "Rooms");
			Output.Print (SELECT_MAP_STRING);
			string input = Input.GetLine ();
			if (!input.Contains (".map"))
			{
				input += ".map";
			}
			input = Path.Combine (roomPath, input);
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
