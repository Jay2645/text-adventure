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
			_backpack = new Backpack ();
		}

		protected Backpack _backpack = null;
		/// <summary>
		/// Gets this character's backpack.
		/// </summary>
		/// <value>
		/// This character's backpack.
		/// </value>
		public Backpack backpack
		{
			get
			{
				return _backpack;
			}
		}

		protected Goals.Goal goal = null;

		/// <summary>
		/// Say the specified speech.
		/// </summary>
		/// <param name='speech'>
		/// What to say.
		/// </param>
		public void Say (string speech)
		{
			Language.Output.Print (name.ToUpper () + ": \"" + speech + "\"");
		}

		/// <summary>
		/// Checks if a goal is satisfied. Nulls the goal on completion.
		/// </summary>
		/// <returns>
		/// TRUE if the goal is satisfied or null, else FALSE.
		/// </returns>
		public bool CheckGoal ()
		{
			if (goal == null)
			{
				return true;
			}
			if (goal.CheckGoal ())
			{
				goal = null;
				return true;
			}
			return false;
		}
	}
}

