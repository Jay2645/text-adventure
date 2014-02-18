using System;
using TextAdventure.GameObjects;
using TextAdventure.GameObjects.Characters;

namespace TextAdventure.Goals
{
	/// <summary>
	/// A goal specifying that this character wants to have an item from someone.
	/// When the specified owner has the specified object, the Goal is satisfied.
	/// </summary>
	public class Acquire : Goal
	{
		public Acquire (Item wanted, Character owner)
		{
			this.wanted = wanted;
			this.owner = owner;
		}
		private Item wanted;
		private Character owner;

		public override bool CheckGoal ()
		{
			return owner.backpack.ContainsItem (wanted);
		}

		public override string ToString ()
		{
			return owner + "needs to acquire " + wanted;
		}
	}
}

