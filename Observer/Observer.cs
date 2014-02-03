namespace TextAdventure
{
	namespace Observers
	{
		public abstract class Observer
		{
			public abstract void OnNotify (GameObjects.GameObject entity, string eventType);
		}
	}
}