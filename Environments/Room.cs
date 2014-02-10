using System;
using TextAdventure.Language;
using System.Collections.Generic;
using TextAdventure.GameObjects;
using TextAdventure.IO;
using TextAdventure.IO.LuaSystem;
using TextAdventure.GameObjects.Characters;

namespace TextAdventure.Environments
{
	/// <summary>
	/// A class representing a "room" in the map.
	/// </summary>
	public class Room : Observers.Subject
	{
		protected Room ()
		{
		}
		public Room (string name, string description, string[] items, Dictionary<Direction, string> neighborRooms)
		{
			SetRoom (name, description, items, neighborRooms);
		}

		/// <summary>
		/// The name of this room.
		/// </summary>
		public string name = "";
		/// <summary>
		/// How this room is described to the player.
		/// </summary>
		public string description = "";
		/// <summary>
		/// All items in this room.
		/// </summary>
		public List<Item> items = null;
		public List<Character> characters = new List<Character> ();
		/// <summary>
		/// Any neighbor rooms and their direction.
		/// </summary>
		public Dictionary<Direction, string> neighborRooms = null;
		/// <summary>
		/// All rooms the game knows about.
		/// </summary>
		private static Dictionary<string, Room> allRooms = new Dictionary<string, Room> ();
		/// <summary>
		/// The LuaBinding for this room.
		/// </summary>
		protected LuaBinding roomBinding = null;

		// Strings to output to the console, to be localized at a later date
		private const string MULTI_EXIT_STRING = "Exits are to the ";
		private const string SINGLE_EXIT_STRING = "There is an exit to the ";
		private const string NO_ENTRY = "You cannot go that way.";
		private const string NO_ITEMS = "There is nothing here.";
		private const string COULD_NOT_FIND_ITEM = "You cannot find a ";
		private const string ITEMS = "There is ";
		private const string EXITS_HELP = "Prints all exits to this room.";
		private const string GO_HELP = "Move in the specified direction.";
		private const string PICK_UP_HELP = "Pick up an item.";

		private bool roomPrinted = false;

		/// <summary>
		/// Initializes all rooms in the game based on the path to a JSON file.
		/// </summary>
		/// <param name='jsonPath'>
		/// The path to the JSON file.
		/// </param>
		public static void InitRooms (string jsonPath)
		{
			// Add commands to the processor
			Processor.AddCommand ("exits", EXITS_HELP, PrintExits);
			Processor.AddCommand ("go", GO_HELP, Travel);
			Processor.AddCommand ("pick up", PICK_UP_HELP, GetItem);

			// Creates player for Lua
			new Player ();

			// Parse JSON
			string roomJSON = System.IO.File.ReadAllText (jsonPath);
			JSONArray parsedJSON = JSON.Parse (roomJSON).AsArray;
			string scriptsPath = System.IO.Path.GetDirectoryName (jsonPath);
			LuaManager.scriptsPath = System.IO.Path.Combine (scriptsPath, "scripts");
			for (int i = 0; i < parsedJSON.Count; i++)
			{
				// Each JSON object in the array is one room
				JSONClass jClass = parsedJSON [i].AsObject;
				string roomName = jClass ["name"];
				if (roomName == null)
				{
					continue;
				}
				string roomDesc = jClass ["desc"];
				JSONArray itemArray = jClass ["items"].AsArray;
				string[] items = new string[itemArray.Count];
				for (int j = 0; j < itemArray.Count; j++)
				{
					items [j] = itemArray [j];
				}
				JSONArray scripts = jClass ["scripts"].AsArray;
				if (scripts != null)
				{	
					for (int j = 0; j < scripts.Count; j++)
					{
						LuaManager.AddFilePath (scripts [j]);
					}
				}
				JSONClass exits = jClass ["exits"].AsObject;
				Dictionary<Direction, string> exitDic = new Dictionary<Direction, string> ();
				for (int j = 0; j < exits.Count; j++)
				{
					string directionStr = exits.GetKey (j);
					string otherRoom = exits [directionStr];
					Direction direction = DirectionConverter.FromString (directionStr);
					if (direction == Direction.None)
					{
						continue;
					}
					exitDic.Add (direction, otherRoom);
				}
				// The room will auto-add itself to the list of all rooms
				new Room (roomName, roomDesc, items, exitDic);
			}
			foreach (KeyValuePair<string, Room> kvp in allRooms)
			{
				kvp.Value.InitRoom ();
			}
		}



		/// <summary>
		/// Gets a room by name.
		/// </summary>
		/// <returns>
		/// The room, or null if there is no room by that name.
		/// </returns>
		/// <param name='name'>
		/// The room's name.
		/// </param>
		public static Room GetRoom (string name)
		{
			name = name.ToLower ();
			if (allRooms.ContainsKey (name))
			{
				return allRooms [name];
			}
			return null;
		}

		/// <summary>
		/// Sets values corresponding to this particular room.
		/// </summary>
		/// <param name='name'>
		/// The name of this room.
		/// </param>
		/// <param name='description'>
		/// The description of this room.
		/// </param>
		/// <param name='items'>
		/// Any items in this room.
		/// </param>
		/// <param name='neighborRooms'>
		/// All neighboring rooms.
		/// </param>
		protected void SetRoom (string name, string description, string[] items, Dictionary<Direction, string> neighborRooms)
		{
			this.name = name;
			this.description = description;
			this.neighborRooms = neighborRooms;
			this.items = new List<Item> ();
			for (int i = 0; i < items.Length; i++)
			{
				this.items.Add (new Item (items [i]));
			}
			roomBinding = new LuaBinding (name);
			AddObserver (roomBinding);
			allRooms.Add (this.name.ToLower (), this);
		}

		protected void InitRoom ()
		{
			Notify (this, TextAdventure.Observers.EventList.OnRoomInit);
		}

		/// <summary>
		/// Raises the room enter event.
		/// </summary>
		public virtual void OnRoomEnter ()
		{
			PrintRoom ();
			Input.ProcessLine ();
		}

		/// <summary>
		/// Raises the room exit event.
		/// </summary>
		public virtual void OnRoomExit ()
		{
		}

		public Character AddCharacter (string name)
		{
			Character c = Character.GetCharacter (name);
			characters.Add (c);
			return c;
		}

		/// <summary>
		/// Gets an item by partial name and places it into the player's inventory.
		/// Notifies the player that they picked up the item.
		/// </summary>
		/// <param name='s'>
		/// The item's partial or full name.
		/// </param>
		public static void GetItem (string s)
		{
			string itemName = s.ToLower ().Trim ();
			foreach (Item i in Globals.room.items)
			{
				string name = i.name.ToLower ();
				if (name.Contains (itemName))
				{
					GetItem (i);
					Output.Print ("You picked up the " + s.Trim () + ".");
					return;
				}
			}
			Output.Print (COULD_NOT_FIND_ITEM + s);
			return;
		}

		/// <summary>
		/// Gets an item directly, puts it into the player's inventory, and removes it from this room.
		/// </summary>
		/// <param name='i'>
		/// The item in question.
		/// </param>
		public static void GetItem (Item i)
		{
			GameObjects.Characters.Player.player.AddItemToBackpack (i);
			Globals.room.items.Remove (i);
		}

		/// <summary>
		/// Prints the contents of this room to the console.
		/// </summary>
		public virtual void PrintRoom ()
		{
			Output.Print (name.ToUpper () + ":");
			Output.Print (description);
			PrintCharacters (characters);
			PrintItems (items);
			PrintExits (neighborRooms);
			if (!roomPrinted)
			{
				Notify (this, Observers.EventList.OnRoomFirstPrint);
				roomPrinted = true;
			}
			Notify (this, Observers.EventList.OnRoomPrint);
		}

		/// <summary>
		/// Prints the exits to active room.
		/// </summary>
		/// <param name='param'>
		/// Ignored.
		/// </param>
		public static void PrintExits (string param)
		{
			PrintExits (Globals.room.neighborRooms);
		}

		/// <summary>
		/// Prints the items in the active room.
		/// </summary>
		/// <param name='param'>
		/// Ignored.
		/// </param>
		public static void PrintItems (string param)
		{
			PrintItems (Globals.room.items);
		}

		/// <summary>
		/// Prints a provided list of items.
		/// </summary>
		/// <param name='items'>
		/// A list of items.
		/// </param>
		public static void PrintItems (List<Item> items)
		{
			if (items == null || items.Count == 0)
			{
				return;
			}
			Dictionary<string, int> itemCount = new Dictionary<string, int> ();
			foreach (Item i in items)
			{
				string name = i.name;
				if (itemCount.ContainsKey (name))
				{
					itemCount [name]++;
				}
				else
				{
					itemCount.Add (name, 1);
				}
			}
			List<string> itemStrList = new List<string> ();
			foreach (KeyValuePair<string, int> kvp in itemCount)
			{
				string key = kvp.Key;
				int value = kvp.Value;
				itemStrList.Add (Grammar.MakeItemGrammar (key, value, false));
			}
			Output.Print (ITEMS + Grammar.MakeItemList (itemStrList.ToArray ()));
		}

		public static void PrintCharacters (List<Character> characters)
		{
			foreach (Character c in characters)
			{
				Output.Print ("There is someone named " + c.name + " here.");
			}
		}

		/// <summary>
		/// Prints the exits to this room.
		/// </summary>
		/// <param name='exitDic'>
		/// A generic Dictionary providing the exits to this room.
		/// Direction is the direction the exit is in, and string is the name of the next room.
		/// </param>
		public static void PrintExits (Dictionary<Direction, string> exitDic)
		{
			string exits;
			if (exitDic.Count == 1)
			{
				exits = SINGLE_EXIT_STRING;
			}
			else
			{
				exits = MULTI_EXIT_STRING;
			}
			List<string> directionStrings = new List<string> ();
			foreach (KeyValuePair<Direction, string> kvp in exitDic)
			{
				directionStrings.Add (DirectionConverter.ToString (kvp.Key));
			}
			exits += Grammar.MakeItemList (directionStrings.ToArray ());
			Output.Print (exits);
		}

		/// <summary>
		/// Travel in the specified direction.
		/// </summary>
		/// <param name='dir'>
		/// The direction to travel.
		/// </param>
		public static void Travel (string dir)
		{
			Direction direction = DirectionConverter.FromString (dir);
			Dictionary<Direction, string> exits = Globals.room.neighborRooms;
			if (direction == Direction.None)
			{
				Output.Print (Processor.DO_NOT_UNDERSTAND);
				PrintExits (exits);
				return;
			}
			if (exits.ContainsKey (direction))
			{
				string roomName = exits [direction];
				if (allRooms.ContainsKey (roomName))
				{
					GameObjects.Characters.Player.player.EnterRoom (allRooms [exits [direction]]);
				}
				else
				{
					// Houston, we have a problem.
					// The .json file *probably* has a typo in an exit somewhere.
					Output.Print ("Uh-oh. I can't find a room called " + roomName + ". I know I put the room around here somewhere...");
				}
			}
			else
			{
				Output.Print (NO_ENTRY);
				PrintExits (exits);
			}
		}
	}
}