using System;

namespace TextAdventure.GameObjects
{
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

