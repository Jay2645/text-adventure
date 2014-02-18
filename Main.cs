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
		private const string SELECT_MAP_STRING = "Please enter the name of the map you'd like to load.";
		private const string FILE_NOT_FOUND = "File not found: ";

		public static void Main (string[] args)
		{
			// Welcome player
			Output.Print (WELCOME_STRING + GAME_NAME + "!");

			try
			{
				// Load the map.
				LoadMap ();

				Console.Clear ();

				// Start game loop
				Globals.player.EnterRoom (Room.GetRoom ("start"));
			}
			catch (Exception e)
			{
				Debug.LogException (e); // This will exit for us
			}
		}

		/// <summary>
		/// Asks the user what map they want to load, then loads the map.
		/// </summary>
		private static void LoadMap ()
		{
			Output.Print (SELECT_MAP_STRING);
			string input = Input.GetLine ();
			if (input.ToLower () == "debug")
			{
				Globals.isDebug = true;
				Debug.LogWarning ("Debug mode initialized.");
				LoadMap ();
				return;
			}
			if (!File.Exists (input) && File.Exists (input + ".map"))
			{
				input += ".map";
			}
			else if (Directory.Exists (input))
			{
				string[] directoryFiles = Directory.GetFiles (input);
				foreach (string file in directoryFiles)
				{
					if (file.EndsWith (".map"))
					{
						input = file;
						break;
					}
				}
			}
			else if (!File.Exists (input))
			{
				string roomPath = Path.GetDirectoryName (Assembly.GetEntryAssembly ().Location);
				roomPath = Path.Combine (roomPath, "Maps");
				input = Path.Combine (roomPath, input, input + ".map");
			}
			if (File.Exists (input))
			{
				Room.InitRooms (input);
			}
			else
			{
				Debug.LogError (FILE_NOT_FOUND + input, true);
				LoadMap ();
			}
		}
	}
}
