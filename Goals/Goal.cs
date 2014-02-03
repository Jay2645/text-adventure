using System;
using TextAdventure.GameObjects.Characters;

namespace TextAdventure.Goals
{
	public abstract class Goal
	{
		public abstract bool CheckGoal ();

		public Character character = null;
	}
}

