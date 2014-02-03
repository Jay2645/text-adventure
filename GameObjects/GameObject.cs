using System;

namespace TextAdventure.GameObjects
{
	/// <summary>
	/// A class representing a "Game Object."
	/// </summary>
	public class GameObject
	{
		protected GameObject ()
		{
		}
		public GameObject (string name)
		{
			this.name = name;
		}
		/// <summary>
		/// The name of this GameObject.
		/// </summary>
		public string name = "";

		public override string ToString ()
		{
			return name;
		}
	}
}

