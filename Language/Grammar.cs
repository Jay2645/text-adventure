using System;

namespace TextAdventure.Language
{
	public static class Grammar
	{
		private const string SINGLUAR_ARTICLE = "A ";
		private const string SINGLUAR_ARTICLE_ALT = "An ";
		private const string PLURAL = "s";
		private const string AND_STRING = "and ";

		public static string MakeItemGrammar (string name, int count, bool beginSentence)
		{
			if (name == "")
			{
				return "";
			}
			string output = "";
			if (count == 1)
			{
				char vowel = name [0];
				if (vowel == 'a' || vowel == 'e' || vowel == 'i' || vowel == 'u')
				{
					string prefix = Grammar.SINGLUAR_ARTICLE_ALT;
					if (!beginSentence)
					{
						prefix = prefix.ToLower ();
					}
					output = prefix + name;
				}
				else
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
				output = count + "x " + name;
			}
			return output;
		}

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

