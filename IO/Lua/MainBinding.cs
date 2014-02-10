using System;
using TextAdventure.GameObjects.Characters;
using TextAdventure.Environments;

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

		public Room CurrentRoom ()
		{
			return Globals.room;
		}

		public Player GetPlayer ()
		{
			return Globals.player;
		}

		public States.State CreateState (string stateStr)
		{
			States.State state = new States.State (stateStr);
			return state;
		}

		public void AddCommand (string command, string help, LuaInterface.LuaFunction function)
		{
			BindMessageFunction (function, command);
			TextAdventure.Language.Processor.AddCommand (command, help, function);
		}

		public void GameOver (string message)
		{
			Language.Output.Print ("Game over!");
			Language.Output.Print (message);
			Environment.Exit (0);
		}
	}
}

