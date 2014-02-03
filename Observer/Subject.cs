﻿using System.Collections.Generic;
namespace TextAdventure
{
	namespace Observers
	{
		/// <summary>
		/// A "subject" class to be used for the Subject-Observer "observer pattern."
		/// </summary>
		public class Subject
		{
			/// <summary>
			/// This is a list of Observer classes which will recieve Notify events from us.
			/// </summary>
			protected List<Observer> observerList = new List<Observer> ();

			/// <summary>
			/// Notifies all observers under our control of the specified event.
			/// </summary>
			/// <param name='entity'>
			/// An entity relating to the event.
			/// </param>
			/// <param name='eventType'>
			/// A unique string identifying the event.
			/// </param>
			public void Notify (GameObjects.GameObject entity, string eventType)
			{
				List<Observer> allObservers = new List<Observer> ();
				foreach (Observer o in observerList)
				{
					if (o == null)
					{
						continue;
					}
					allObservers.Add (o);
				}
				observerList = allObservers;
				foreach (Observer o in observerList)
				{
					o.OnNotify (entity, eventType);
				}
			}

			/// <summary>
			/// Adds an observer to recieve our notifications.
			/// </summary>
			/// <param name='os'>
			/// An array of Observers to add.
			/// </param>
			public void AddObserver (Observer[] os)
			{
				foreach (Observer o in os)
				{
					AddObserver (o);
				}
			}

			/// <summary>
			/// Adds an observer to recieve our notifications.
			/// </summary>
			/// <param name='o'>
			/// An Observer to add.
			/// </param>
			public void AddObserver (Observer o)
			{
				if (!observerList.Contains (o))
				{
					observerList.Add (o);
				}
			}

			/// <summary>
			/// Removes a specified observer.
			/// </summary>
			/// <param name='o'>
			/// The observer to remove.
			/// </param>
			public void RemoveObserver (Observer o)
			{
				if (observerList.Contains (o))
				{
					observerList.Remove (o);
				}
			}
		}
	}
}