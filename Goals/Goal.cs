using System;
using TextAdventure.GameObjects.Characters;

namespace TextAdventure.Goals
{
	public abstract class Goal : Observers.Observer
	{
		public abstract bool CheckGoal ();

		public override void OnNotify (object entity, TextAdventure.Observers.EventList eventType)
		{
			/* EMPTY */
		}
	}
}

