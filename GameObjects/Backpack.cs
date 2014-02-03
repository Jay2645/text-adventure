using System;
using System.Collections.Generic;
using TextAdventure.Language;

namespace TextAdventure.GameObjects
{
	/// <summary>
	/// A class representing the player's "backpack" or inventory.
	/// </summary>
	public class Backpack : GameObject
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TextAdventure.GameObjects.Backpack"/> class.
		/// Will also add the proper commands to the language processor.
		/// </summary>
		public Backpack ()
		{
			name = "Backpack";
			Processor.AddCommand ("backpack", BACKPACK_HELP_STRING, PrintContents);
			Processor.AddCommand ("bp", BACKPACK_HELP_STRING, PrintContents);
			Processor.AddCommand ("i", BACKPACK_HELP_STRING, PrintContents);
			Processor.AddCommand ("inventory", BACKPACK_HELP_STRING, PrintContents);
		}
		private List<Item> backpackItems = new List<Item> ();

		private const string YOU_HAVE_STRING = "You have: ";
		private const string NOTHING_STRING = "Your backpack is empty.";
		private const string BACKPACK_HELP_STRING = "Lists the contents of the player's inventory.";

		/// <summary>
		/// Adds the item to our backpack.
		/// </summary>
		/// <param name='i'>
		/// The item to add.
		/// </param>
		public void AddItem (Item i)
		{
			backpackItems.Add (i);
		}

		/// <summary>
		/// Removes the item from our backpack.
		/// </summary>
		/// <param name='i'>
		/// The item to remove.
		/// </param>
		public void RemoveItem (Item i)
		{
			backpackItems.Remove (i);
		}

		/// <summary>
		/// Gets all items in this backpack.
		/// </summary>
		/// <returns>
		/// An array of all backpack items.
		/// </returns>
		public Item[] GetItems ()
		{
			return backpackItems.ToArray ();
		}

		/// <summary>
		/// Prints the contents of our backpack.
		/// </summary>
		/// <param name='param'>
		/// Ignored.
		/// </param>
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

