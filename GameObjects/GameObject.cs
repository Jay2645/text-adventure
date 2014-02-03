using System;
using TextAdventure.Observers;

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
		protected Subject subject = new Subject ();

		public override string ToString ()
		{
			return name;
		}

		/// <summary>
		/// Notifies all observers under our control of the specified event.
		/// </summary>
		/// <param name='entity'>
		/// An entity relating to the event.
		/// </param>
		/// <param name='eventType'>
		/// A unique string identifying the event.
		/// </param>
		public void Notify (GameObject entity, string eventType)
		{
			subject.Notify (entity, eventType);
		}

		/// <summary>
		/// Adds an observer to recieve our notifications.
		/// </summary>
		/// <param name='os'>
		/// An array of Observers to add.
		/// </param>
		public void AddObserver (Observer[] os)
		{
			subject.AddObserver (os);
		}

		/// <summary>
		/// Adds an observer to recieve our notifications.
		/// </summary>
		/// <param name='o'>
		/// An Observer to add.
		/// </param>
		public void AddObserver (Observer o)
		{
			subject.AddObserver (o);
		}

		/// <summary>
		/// Removes a specified observer.
		/// </summary>
		/// <param name='o'>
		/// The observer to remove.
		/// </param>
		public void RemoveObserver (Observer o)
		{
			subject.RemoveObserver (o);
		}
	}
}

