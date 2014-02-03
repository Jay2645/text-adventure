using System;
using TextAdventure.GameObjects.Characters;

namespace TextAdventure.IO.LuaSystem
{
	public class MainBinding : LuaBinding
	{
		public MainBinding ()
		{
			SetBinding ("main");
		}

		public Character GetCharacter (string name)
		{
			return Character.GetCharacter (name);
		}
	}
}

