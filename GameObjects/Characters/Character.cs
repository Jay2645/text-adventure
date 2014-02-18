using System;
using System.Collections.Generic;
using TextAdventure.States;

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
			characterList.Add (name, this);
			characterBinding = new TextAdventure.IO.LuaSystem.LuaBinding (name);
			AddObserver (characterBinding);
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
		protected static Dictionary<string, Character> characterList = new Dictionary<string, Character> ();
		protected IO.LuaSystem.LuaBinding characterBinding;
		public State state = null;

		/// <summary>
		/// Say the specified speech.
		/// </summary>
		/// <param name='speech'>
		/// What to say.
		/// </param>
		public void Say (string speech)
		{
			string say;
			if (speech.EndsWith ("\""))
			{
				say = name.ToUpper () + ": " + speech;
			}
			else
			{
				say = name.ToUpper () + ": \"" + speech + "\"";
			}
			Language.Output.Print (say);
			Notify (speech.ToLower (), Observers.EventList.OnCharacterSpeak);
		}

		public static Character GetCharacter (string name)
		{
			if (characterList.ContainsKey (name))
			{
				return characterList [name];
			}
			return new Character (name);
		}

		public void SetAcquireGoal (string wanted, string owner)
		{
			SetGoal (new TextAdventure.Goals.Acquire (new Item (wanted), GetCharacter (owner)));
		}

		public void SetPerformGoal (string action, string param)
		{
			SetGoal (new TextAdventure.Goals.Perform (action, param));
		}

		private void SetGoal (Goals.Goal g)
		{
			if (Globals.isDebug)
			{
				Language.Output.Print ("Changing goal to " + g);
			}
			goal = g;
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

