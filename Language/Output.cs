using System;

namespace TextAdventure.Language
{
	/// <summary>
	/// Hooks into whatever output is being used for the game -- console, etc.
	/// </summary>
	public static class Output
	{
		/// <summary>
		/// Print the specified object.
		/// </summary>
		/// <param name='obj'>
		/// The object to print.
		/// </param>
		public static void Print (object obj)
		{
			Console.WriteLine (obj);
		}
	}
}

