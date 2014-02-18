using System;
using TextAdventure.GameObjects.Characters;

namespace TextAdventure.Goals
{
	/// <summary>
	/// A Goal which is satisfied when the player types in a certain action with a certain parameter.
	/// </summary>
	public class Perform : Goal
	{
		public Perform (string action, string param)
		{
			this.action = action.ToLower ().Trim ();
			this.param = param.ToLower ().Trim ();
			Player.player.AddObserver (this);
		}

		string action = "";
		string param = "";

		private bool actionPerformed = false;

		public override void OnNotify (object entity, TextAdventure.Observers.EventList eventType)
		{
			if (eventType != TextAdventure.Observers.EventList.OnPlayerCommand)
			{
				return;
			}
			string currentAction = (string)entity;
			currentAction = currentAction.ToLower ().Trim ();
			if (action != currentAction)
			{
				return;
			}
			string currentParam = Language.Processor.GetCommandParameters (currentAction);
			currentParam = currentParam.ToLower ().Trim (); // Probably not needed but I'm being pedantic
			if (currentParam == param)
			{
				actionPerformed = true;
				Player.player.RemoveObserver (this);
			}
		}

		public override bool CheckGoal ()
		{
			return actionPerformed;
		}

		public override string ToString ()
		{
			return "Perform \"" + action + "\" with parameter \"" + param + "\"";
		}
	}
}

