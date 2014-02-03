using System;
using System.Collections.Generic;

namespace TextAdventure.Environments
{
	/// <summary>
	/// An easy way to convert directions to strings and vice versa.
	/// </summary>
	public static class DirectionConverter
	{
		public const string NORTH = "north";
		public const string SOUTH = "south";
		public const string EAST = "east";
		public const string WEST = "west";
		public const string NORTHEAST = "northeast";
		public const string NORTHWEST = "northwest";
		public const string SOUTHEAST = "southeast";
		public const string SOUTHWEST = "southwest";

		public const string NORTH_SHORT = "n";
		public const string SOUTH_SHORT = "s";
		public const string EAST_SHORT = "e";
		public const string WEST_SHORT = "w";
		public const string NORTHEAST_SHORT = "ne";
		public const string NORTHWEST_SHORT = "nw";
		public const string SOUTHEAST_SHORT = "se";
		public const string SOUTHWEST_SHORT = "sw";

		private static Dictionary<string, Direction> directionLookup
		{
			get
			{
				if (_directionLookup == null)
				{
					CreateDictionary ();
				}
				return _directionLookup;
			}
		}
		private static Dictionary<Direction, string> reverseDirectionLookup
		{
			get
			{
				if (_reverseDirectionLookup == null)
				{
					CreateDictionary ();
				}
				return _reverseDirectionLookup;
			}
		}

		private static Dictionary<string, Direction> _directionLookup = null;
		private static Dictionary<Direction, string> _reverseDirectionLookup = null;

		/// <summary>
		/// Creates the dictionaries.
		/// </summary>
		private static void CreateDictionary ()
		{
			_directionLookup = new Dictionary<string, Direction> ();
			_directionLookup.Add (NORTH, Direction.North);
			_directionLookup.Add (SOUTH, Direction.South);
			_directionLookup.Add (EAST, Direction.East);
			_directionLookup.Add (WEST, Direction.West);
			_directionLookup.Add (NORTHEAST, Direction.Northeast);
			_directionLookup.Add (NORTHWEST, Direction.Northwest);
			_directionLookup.Add (SOUTHEAST, Direction.Southeast);
			_directionLookup.Add (SOUTHWEST, Direction.Southwest);

			_directionLookup.Add (NORTH_SHORT, Direction.North);
			_directionLookup.Add (SOUTH_SHORT, Direction.South);
			_directionLookup.Add (EAST_SHORT, Direction.East);
			_directionLookup.Add (WEST_SHORT, Direction.West);
			_directionLookup.Add (NORTHEAST_SHORT, Direction.Northeast);
			_directionLookup.Add (NORTHWEST_SHORT, Direction.Northwest);
			_directionLookup.Add (SOUTHEAST_SHORT, Direction.Southeast);
			_directionLookup.Add (SOUTHWEST_SHORT, Direction.Southwest);

			_reverseDirectionLookup = new Dictionary<Direction, string> ();
			_reverseDirectionLookup.Add (Direction.East, EAST);
			_reverseDirectionLookup.Add (Direction.North, NORTH);
			_reverseDirectionLookup.Add (Direction.Northeast, NORTHEAST);
			_reverseDirectionLookup.Add (Direction.Northwest, NORTHWEST);
			_reverseDirectionLookup.Add (Direction.South, SOUTH);
			_reverseDirectionLookup.Add (Direction.Southeast, SOUTHEAST);
			_reverseDirectionLookup.Add (Direction.Southwest, SOUTHWEST);
			_reverseDirectionLookup.Add (Direction.West, WEST);
		}

		/// <summary>
		/// Returns a direction based on a string input.
		/// </summary>
		/// <returns>
		/// The direction specified by the string.
		/// </returns>
		/// <param name='input'>
		/// The input.
		/// </param>
		public static Direction FromString (string input)
		{
			input = input.ToLower ();
			if (directionLookup.ContainsKey (input))
			{
				return directionLookup [input];
			}
			return Direction.None;
		}

		/// <summary>
		/// Returns a string based on a direction input.
		/// Will return an empty string if it can't find the direction's string for whatever reason.
		/// </summary>
		/// <returns>
		/// A string based on the specified direction.
		/// </returns>
		/// <param name='input'>
		/// The direction to look up.
		/// </param>
		public static string ToString (Direction input)
		{
			if (reverseDirectionLookup.ContainsKey (input))
			{
				return reverseDirectionLookup [input];
			}
			return "";
		}
	}
}

