using System;
using TextAdventure.GameObjects;
using TextAdventure.GameObjects.Characters;

namespace TextAdventure.Goals
{
	/// <summary>
	/// A goal specifying that this character wants to have an item from someone.
	/// When the specified owner has the specified object, the Goal is satisfied.
	/// </summary>
	public class Aquire : Goal
	{
		public Aquire (Character wantee, Item wanted, Character owner)
		{
			character = wantee;
			this.wanted = wanted;
			this.owner = owner;
		}
		private Item wanted;
		private Character owner;

		public override bool CheckGoal ()
		{
			return owner.backpack.ContainsItem (wanted);
		}
	}
}

