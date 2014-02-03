namespace TextAdventure
{
	namespace Observers
	{
		public abstract class Observer
		{
			public abstract void OnNotify (object entity, EventList eventType);
		}
	}
}