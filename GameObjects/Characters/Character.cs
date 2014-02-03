using System;

namespace TextAdventure.GameObjects.Characters
{
	/// <summary>
	/// A class representing a character.
	/// </summary>
	public class Character : GameObject
	{
		protected Character ()
		{
		}
		public Character (string name)
		{
			this.name = name;
		}
	}
}

