using System;
using TextAdventure.Environments;
using TextAdventure.GameObjects.Characters;

namespace TextAdventure
{
	/// <summary>
	/// A class full of some handy things.
	/// </summary>
	public static class Globals
	{
		public static Room room = null;
		public static Player player
		{
			get
			{
				return Player.player;
			}
		}

		public static bool isDebug = false;
	}
}

