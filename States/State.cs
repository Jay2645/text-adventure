using System;
using System.Collections.Generic;

namespace TextAdventure.States
{
	/// <summary>
	/// A class representing a character state.
	/// </summary>
	public class State
	{
		public State (string name)
		{
			this.name = name;
		}
		/// <summary>
		/// The name of this state.
		/// </summary>
		public string name;

		/// <summary>
		/// All allowed transition states.
		/// </summary>
		public List<string> allowedStates = new List<string> ();

		/// <summary>
		/// Adds a state to the allowed states.
		/// </summary>
		/// <param name='state'>
		/// The state to add.
		/// </param>
		public void AddAllowedState (string state)
		{
			state = state.ToLower ();
			if (allowedStates.Contains (state))
			{
				return;
			}
			allowedStates.Add (state);
		}

		/// <summary>
		/// Adds a state to the allowed states.
		/// </summary>
		/// <param name='state'>
		/// The state to add.
		/// </param>
		public void AddAllowedState (State state)
		{
			AddAllowedState (state.name);
		}

		/// <summary>
		/// Determines whether a state is allowed.
		/// </summary>
		/// <returns>
		/// <c>true</c> if the state is allowed; otherwise, <c>false</c>.
		/// </returns>
		/// <param name='stateName'>
		/// The name of the state.
		/// </param>
		public bool IsAllowedState (string stateName)
		{
			stateName = stateName.ToLower ();
			return allowedStates.Contains (stateName);
		}

		/// <summary>
		/// Prints the allowed states.
		/// </summary>
		public void PrintAllowedStates ()
		{
			foreach (string allowed in allowedStates)
			{
				Language.Output.Print (allowed);
			}
		}

		public override string ToString ()
		{
			return name;
		}
	}
}

