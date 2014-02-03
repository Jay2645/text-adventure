using System;
using TextAdventure.Environments;

namespace TextAdventure.GameObjects.Characters
{
	/// <summary>
	/// A class representing the player.
	/// </summary>
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

		/// <summary>
		/// Enters a room. Will raise OnRoomExit and OnRoomEnter.
		/// </summary>
		/// <param name='current'>
		/// The room to enter.
		/// </param>
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

		/// <summary>
		/// Adds an item to our backpack.
		/// </summary>
		/// <param name='i'>
		/// The item to add.
		/// </param>
		public void AddItemToBackpack (Item i)
		{
			backpack.AddItem (i);
		}

		/// <summary>
		/// Removes an item from our backpack.
		/// </summary>
		/// <param name='i'>
		/// The item to remove.
		/// </param>
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

		/// <summary>
		/// Gets or sets the current room.
		/// Will also update Globals.
		/// </summary>
		/// <value>
		/// The present room.
		/// </value>
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
		#endregion
	}
}

