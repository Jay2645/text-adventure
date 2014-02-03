using System;
using TextAdventure.Language;
using System.Collections.Generic;
using TextAdventure.GameObjects;
using TextAdventure.IO;

namespace TextAdventure.Environments
{
	public class Room
	{
		protected Room ()
		{
		}
		public Room (string name, string description, string[] items, Dictionary<Direction, string> neighborRooms)
		{
			SetRoom (name, description, items, neighborRooms);
		}

		public string name = "";
		public string description = "";
		public List<Item> items = null;
		public Dictionary<Direction, string> neighborRooms = null;
		private static Dictionary<string, Room> allRooms = new Dictionary<string, Room> ();

		private const string MULTI_EXIT_STRING = "Exits are to the ";
		private const string SINGLE_EXIT_STRING = "There is an exit to the ";
		private const string NO_ENTRY = "You cannot go that way.";
		private const string NO_ITEMS = "There is nothing here.";
		private const string COULD_NOT_FIND_ITEM = "You cannot find a ";
		private const string ITEMS = "There is ";
		private const string EXITS_HELP = "Prints all exits to this room.";
		private const string GO_HELP = "Move in the specified direction.";
		private const string PICK_UP_HELP = "Pick up an item.";

		public static void InitRooms (string jsonPath)
		{
			Processor.AddCommand ("exits", EXITS_HELP, PrintExits);
			Processor.AddCommand ("go", GO_HELP, Travel);
			Processor.AddCommand ("pick up", PICK_UP_HELP, GetItem);

			string roomJSON = System.IO.File.ReadAllText (jsonPath);
			JSONArray parsedJSON = JSON.Parse (roomJSON).AsArray;
			for (int i = 0; i < parsedJSON.Count; i++)
			{
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
				new Room (roomName, roomDesc, items, exitDic);
			}
		}

		public static Room GetRoom (string name)
		{
			name = name.ToLower ();
			if (allRooms.ContainsKey (name))
			{
				return allRooms [name];
			}
			return null;
		}

		public void SetRoom (string name, string description, string[] items, Dictionary<Direction, string> neighborRooms)
		{
			this.name = name;
			this.description = description;
			this.neighborRooms = neighborRooms;
			this.items = new List<Item> ();
			for (int i = 0; i < items.Length; i++)
			{
				this.items.Add (new Item (items [i]));
			}
			allRooms.Add (this.name.ToLower (), this);
		}

		public virtual void OnRoomEnter ()
		{
			PrintRoom ();
			Input.ProcessLine ();
		}

		public virtual void OnRoomExit ()
		{
		}

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
		}

		public static void GetItem (Item i)
		{
			GameObjects.Characters.Player.player.AddItemToBackpack (i);
			Globals.room.items.Remove (i);
		}

		public virtual void PrintRoom ()
		{
			Output.Print (name.ToUpper () + ":");
			Output.Print (description);
			PrintItems (items);
			PrintExits (neighborRooms);
		}

		public static void PrintExits (string param)
		{
			PrintExits (Globals.room.neighborRooms);
		}

		public static void PrintItems (string param)
		{
			PrintItems (Globals.room.items);
		}

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