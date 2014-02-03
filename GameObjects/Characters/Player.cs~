using System;
using TextAdventure.Environments;

namespace TextAdventure.GameObjects.Characters
{
	public class Player : Character
	{
		public Player ()
		{
			this.name = "Player";
			_backpack = new Backpack ();
			_player = this;
		}

		private Backpack _backpack = null;
		private static Player _player = null;
		public Room room
		{
			get
			{
				return Globals.room;
			}
			set
			{
				Globals.room = value;
			}
		}

		public void EnterRoom (Room current)
		{
			if (room != null)
			{
				room.OnRoomExit ();
			}
			room = current;
			if (room == null)
			{
				return;
			}
			room.OnRoomEnter ();
		}

		public void AddItemToBackpack (Item i)
		{
			backpack.AddItem (i);
		}

		public void RemoveItemFromBackpack (Item i)
		{
			backpack.RemoveItem (i);
		}

		#region Getters
		/// <summary>
		/// Gets the player's backpack.
		/// </summary>
		/// <value>
		/// The player's backpack.
		/// </value>
		public Backpack backpack
		{
			get
			{
				return _backpack;
			}
		}
		/// <summary>
		/// Gets the player.
		/// </summary>
		/// <value>
		/// The player.
		/// </value>
		public static Player player
		{
			get
			{
				if (_player == null)
				{
					_player = new Player ();
				}
				return _player;
			}
		}
		#endregion
	}
}

