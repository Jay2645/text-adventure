using System;
using System.Collections.Generic;
using TextAdventure.Language;

namespace TextAdventure.GameObjects
{
	public class Backpack : GameObject
	{
		public Backpack ()
		{
			name = "Backpack";
			Language.Processor.AddCommand ("backpack", BACKPACK_HELP_STRING, PrintContents);
			Language.Processor.AddCommand ("bp", BACKPACK_HELP_STRING, PrintContents);
			Language.Processor.AddCommand ("i", BACKPACK_HELP_STRING, PrintContents);
			Language.Processor.AddCommand ("inventory", BACKPACK_HELP_STRING, PrintContents);
		}
		private List<Item> backpackItems = new List<Item> ();

		private const string YOU_HAVE_STRING = "You have: ";
		private const string NOTHING_STRING = "Your backpack is empty.";
		private const string BACKPACK_HELP_STRING = "Lists the contents of the player's inventory.";

		public void AddItem (Item i)
		{
			backpackItems.Add (i);
		}

		public void RemoveItem (Item i)
		{
			backpackItems.Remove (i);
		}

		public Item[] GetItems ()
		{
			return backpackItems.ToArray ();
		}

		public static void PrintContents (string param)
		{
			Item[] backpackItems = Characters.Player.player.backpack.GetItems ();
			if (backpackItems.Length == 0)
			{
				Language.Output.Print (NOTHING_STRING);
				return;
			}
			Language.Output.Print ("You have: ");
			// Count how many of each item is in the backpack
			Dictionary<string, int> itemCount = new Dictionary<string, int> ();
			foreach (Item i in backpackItems)
			{
				string name = i.name.ToLower ();
				if (itemCount.ContainsKey (name))
				{
					itemCount [name]++;
				}
				else
				{
					itemCount.Add (name, 1);
				}
			}
			foreach (KeyValuePair<string, int> kvp in itemCount)
			{
				string key = kvp.Key;
				int value = kvp.Value;
				Output.Print (Grammar.MakeItemGrammar (key, value, true));
			}
		}
	}
}

