using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace itemshop
{
	public class ItemCollection : MonoBehaviour
	{
		[DisplayName("ItemCollection")] public string displayName = "Item Collection";

		[SerializeField] private string _id = default;
		[SerializeField] private string _uuid = default;

		[TextArea(3, 5)]
		[SerializeField] private string _notes = default;

		[SerializeField] private List<ItemCollection> _childItems = new List<ItemCollection>();
		[SerializeField] private Rect _windowRect = default;

		private int _currentItem;

		// GETTERS

		public string Id
		{
			get
			{
				return _id;
			}
		}

		public int ItemIndex
		{
			get
			{
				return _currentItem;
			}

			set
			{
				if (value >= 0 && value < ItemCount)
				{
					_currentItem = value;
				}
			}
		}

		public List<ItemCollection> ChildItems
		{
			get
			{
				return _childItems;
			}
		}

		public Rect WindowRect
		{
			get
			{
				return _windowRect;
			}
			set
			{
				_windowRect = value;
			}
		}

		// How many items does this collection contain
		public int ItemCount
		{
			get
			{
				return transform.childCount;
			}
		}

		// Returns the current active item
		public Item Item
		{
			get
			{
				return transform.GetChild(_currentItem).GetComponent<Item>();
			}
		}

		// Returns the last bought item
		public Item LastBoughtItem
		{
			get
			{
				if ((_currentItem - 1) >= 0 && (_currentItem - 1) < ItemCount)
                {
					return transform.GetChild(_currentItem - 1).GetComponent<Item>();
				}
				return null;
			}
		}

		public string Uuid
		{
			get
			{
				if (string.IsNullOrEmpty(_uuid))
				{
					_uuid = System.Guid.NewGuid().ToString();
				}

				return _uuid;
			}

			set
			{
				_uuid = value;
			}
		}

		public ItemShop ItemShop
        {
			get
            {
				return transform.parent.parent.GetComponent<ItemShop>();
			}
        }

		// METHODS

		/// <summary>
		/// Set the x and y coords of the windows rect
		/// </summary>
		/// <param name="x">coord x</param>
		/// <param name="y">coord y</param>
		public void SetWindowRectPos(float x, float y)
		{
			_windowRect.x = x;
			_windowRect.y = y;
		}

		/// <summary>
		/// Get the item at a specific index
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="index">Index.</param>
		public Item GetItem(int index)
		{
			return transform.GetChild(index).GetComponent<Item>();
		}

		public Item[] GetAllItems()
        {
			Item[] itemsArray = transform.GetComponentsInChildren<Item>();

			return itemsArray;
        }

		/// <summary>
		/// Add an item collection to the itemChilds list
		/// </summary>
		/// <param name="itemCol">Skill collection</param>
		public void AddItemCollection(ItemCollection itemCol)
		{
			_childItems.Add(itemCol);

		}

		/// <summary>
		/// Unlock the currently active item and set the pointer to the next unlocked if available
		/// </summary>
		public void UnlockItem()
		{
			ItemShop itemShop = ItemShop;

			if (itemShop.PlayerMoney <= 0) return;

			itemShop.RemoveMoney(Item.Cost);

			Item.Unlocked = true;

			if (Item.Consommable)
            {
				Item.Stock += 1;
			}
			else
            {
				Item.Stock = 1;
				ItemIndex += 1;
			}
		}
	}
}
