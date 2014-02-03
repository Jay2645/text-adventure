using System;

namespace TextAdventure.GameObjects.Characters
{
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

