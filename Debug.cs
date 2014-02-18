using System;
using TextAdventure.Language;

namespace TextAdventure
{
	public static class Debug
	{
		public static void Log (object log)
		{
			Log (log, Globals.isDebug);
		}

		public static void Log (object log, bool isDebug)
		{
			if (!isDebug)
			{
				return;
			}
			Output.Print (log);
		}

		public static void LogWarning (object log)
		{
			LogWarning (log, Globals.isDebug);
		}

		public static void LogWarning (object log, bool isDebug)
		{
			ConsoleColor color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Yellow;
			Log ("WARNING: " + log.ToString (), isDebug);
			Console.ForegroundColor = color;
		}

		public static void LogError (object log)
		{
			LogError (log, Globals.isDebug);
		}

		public static void LogError (object log, bool isDebug)
		{
			ConsoleColor color = Console.ForegroundColor;
			Console.ForegroundColor = ConsoleColor.Red;
			string logStr = log.ToString ();
			if (!logStr.ToLower ().Contains ("exception"))
			{
				logStr = "ERROR: " + logStr;
			}
			Log (logStr, isDebug);
			Console.ForegroundColor = color;
		}

		public static void LogException (Exception e)
		{
			LogError ("Exception! " + e, true);
			Environment.Exit (1);
		}
	}
}

