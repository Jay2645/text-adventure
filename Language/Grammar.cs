using System;

namespace TextAdventure.Language
{
	/// <summary>
	/// A class specializing in making things have proper grammar.
	/// </summary>
	public static class Grammar
	{
		private const string SINGLUAR_ARTICLE = "A ";
		private const string SINGLUAR_ARTICLE_ALT = "An ";
		private const string PLURAL = "s";
		private const string AND_STRING = "and ";

		/// <summary>
		/// Returns a string formatted as if you have n instances of an item.
		/// i.e. "a foo," "42x foo"
		/// </summary>
		/// <returns>
		/// A string formatted for n instances of an item.
		/// </returns>
		/// <param name='name'>
		/// The name of the item.
		/// </param>
		/// <param name='count'>
		/// How many you have of it.
		/// </param>
		/// <param name='beginSentence'>
		/// Is this the beginning of a sentence? If so, we should capitalize the beginning.
		/// </param>
		public static string MakeItemGrammar (string name, int count, bool beginSentence)
		{
			// No item to add, return an empty string
			if (name == "")
			{
				return "";
			}
			string output = "";
			if (count == 1) // Only 1 item here
			{
				char vowel = name [0];
				if (vowel == 'a' || vowel == 'e' || vowel == 'i' || vowel == 'u') // Use "an" rather than "a"
				{
					string prefix = Grammar.SINGLUAR_ARTICLE_ALT;
					if (!beginSentence)
					{
						prefix = prefix.ToLower ();
					}
					output = prefix + name;
				}
				else // Use "a"
				{
					string prefix = Grammar.SINGLUAR_ARTICLE;
					if (!beginSentence)
					{
						prefix = prefix.ToLower ();
					}
					output = prefix + name;
				}
			}
			else
			{
				/*int firstSpace = name.IndexOf (' ');
				string subject;
				string predicate;
				if (firstSpace > -1)
				{
					subject = name.Substring (0, firstSpace);
					predicate = name.Substring (firstSpace);
				}
				else
				{
					subject = name;
					predicate = "";
				}
				output = count + " " + subject + PLURAL + predicate;*/
				output = count + "x " + name; // 42x flower
			}
			return output;
		}

		/// <summary>
		/// Makes an array of strings into a list of items.
		/// </summary>
		/// <returns>
		/// A list of items. For example, "a boy, a dog, and a horse."
		/// </returns>
		/// <param name='itemStrings'>
		/// The base strings from which to make the list.
		/// </param>
		public static string MakeItemList (string[] itemStrings)
		{
			if (itemStrings == null || itemStrings.Length == 0)
			{
				return "";
			}
			int count = 0;
			string output = "";
			foreach (string item in itemStrings)
			{
				output += item;
				count++;
				if (count == itemStrings.Length)
				{
					if (!item.EndsWith ("!") && !item.EndsWith ("?") && !item.EndsWith ("."))
					{
						output += ".";
					}
					break;
				}
				else
				{
					// Commas if there are more than 2 items
					if (itemStrings.Length != 2)
					{
						output += ",";
					}
					output += " ";
					// "And" if the next item is the last
					if (count == itemStrings.Length - 1)
					{
						output += AND_STRING;
					}
				}
			}
			return output;
		}
	}
}

