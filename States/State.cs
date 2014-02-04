using System;
using System.Collections.Generic;

namespace TextAdventure.States
{
	public class State
	{
		public State (string name)
		{
			this.name = name;
		}
		public string name;

		public List<string> allowedStates = new List<string> ();

		public void AddAllowedState (string state)
		{
			state = state.ToLower ();
			allowedStates.Add (state);
		}

		public void AddAllowedState (State state)
		{
			AddAllowedState (state.name);
		}

		public bool IsAllowedState (string stateName)
		{
			stateName = stateName.ToLower ();
			return allowedStates.Contains (stateName);
		}

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

