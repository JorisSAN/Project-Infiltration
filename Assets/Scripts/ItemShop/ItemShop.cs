using itemshop.save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace itemshop
{
	public class ItemShop : MonoBehaviour
	{
		[DisplayName("ItemShopData")] public string displayName = "Item Shop";

		[TextArea(3, 5)]
		[SerializeField] private string _description;

		// Money of the player
		[SerializeField] private int _playerMoney = 0;

		[SerializeField] private Vector2 _gridCellSize = new Vector2(250f, 200f);

		[HideInInspector] public ItemCategory _currentCategory;

		/* TEMPORAIRE */
		public System.Type ItemCategory { get { return typeof(ItemCategory); } }
		public System.Type ItemCollection { get { return typeof(ItemCollection); } }
		public System.Type Item { get { return typeof(Item); } }
		/* TEMPORAIRE */

		private Dictionary<ItemCollection, List<ItemCollection>> _childParents;

		private Dictionary<string, ItemCategory> _categoryLib = new Dictionary<string, ItemCategory>();
		private Dictionary<string, ItemCollection> _skillCollectionLib = new Dictionary<string, ItemCollection>();
		private Dictionary<string, Item> _itemLib = new Dictionary<string, Item>();

		private Dictionary<string, ItemCategory> _categoryUuidLib = new Dictionary<string, ItemCategory>();
		private Dictionary<string, ItemCollection> _collectionUuidLib = new Dictionary<string, ItemCollection>();
		private Dictionary<string, Item> _itemUuidLib = new Dictionary<string, Item>();

		private bool _initialized = false;

		public int PlayerMoney
		{
			get
			{
				return _playerMoney;
			}
			set
			{
				_playerMoney = value;
			}
		}

		public Vector2 GridCellSize
		{
			get
			{
				return _gridCellSize;
			}
		}

		public bool Initialized
		{
			get
			{
				return _initialized;
			}
		}

		// METHODS

		public void Awake()
		{
			if (!_initialized)
			{
				Initialize();
			}
		}

		public void Initialize()
		{
			foreach (ItemCategory cat in GetCategories())
			{
				if (!string.IsNullOrEmpty(cat.Id)) _categoryLib[cat.Id] = cat;
				_categoryUuidLib[cat.Uuid] = cat;
			}

			foreach (ItemCollection col in GetItemCollections())
			{
				if (!string.IsNullOrEmpty(col.Id)) _skillCollectionLib[col.Id] = col;
				_collectionUuidLib[col.Uuid] = col;
			}

			foreach (Item item in GetItems())
			{
				if (!string.IsNullOrEmpty(item.Id)) _itemLib[item.Id] = item;
				_itemUuidLib[item.Uuid] = item;
			}

			_childParents = GetParentData();

			_initialized = true;
		}

		/// <summary>
		/// Loops through all collections to determine parent elements. Warning, quite expensive.
		/// </summary>
		/// <returns>The parent data.</returns>
		private Dictionary<ItemCollection, List<ItemCollection>> GetParentData()
		{
			Dictionary<ItemCollection, List<ItemCollection>> childParents = new Dictionary<ItemCollection, List<ItemCollection>>();

			foreach (ItemCategory category in GetComponentsInChildren<ItemCategory>())
			{
				foreach (ItemCollection parent in category.GetComponentsInChildren<ItemCollection>())
				{
					foreach (ItemCollection child in parent.ChildItems)
					{
						if (!childParents.ContainsKey(child))
						{
							childParents[child] = new List<ItemCollection>();
						}

						childParents[child].Add(parent);
					}
				}
			}

			return childParents;
		}


		/// <summary>
		/// Retrieve all active categories. Warning, expensive
		/// </summary>
		/// <returns>The categories.</returns>
		public ItemCategory[] GetCategories()
		{
			return GetComponentsInChildren<ItemCategory>();
		}

		/// <summary>
		/// Returns all item collecitons. Warning, expensive
		/// </summary>
		/// <returns>The item collections.</returns>
		public ItemCollection[] GetItemCollections()
		{
			return GetComponentsInChildren<ItemCollection>();
		}

		/// <summary>
		/// Returns all items. Warning expensive
		/// </summary>
		/// <returns>The items.</returns>
		public Item[] GetItems()
		{
			return GetComponentsInChildren<Item>();
		}

		/// <summary>
		/// Returns a category from the user assigned ID
		/// </summary>
		/// <returns>The category.</returns>
		/// <param name="categoryId">Category identifier.</param>
		public ItemCategory GetCategory(string categoryId)
		{
			return _categoryLib[categoryId];
		}

		/// <summary>
		/// Returns a collection from the user assigned ID
		/// </summary>
		/// <returns>The collection.</returns>
		/// <param name="collectionId">Collection identifier.</param>
		public ItemCollection GetCollection(string collectionId)
		{
			return _skillCollectionLib[collectionId];
		}

		/// <summary>
		/// Returns an item from the user assigned ID
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="itemId">Skill identifier.</param>
		public Item GetItem(string itemId)
		{
			return _itemLib[itemId];
		}

		// <summary>
		/// Declare the current level of a specific category
		/// </summary>
		/// <param name="categoryId">Category identifier.</param>
		/// <param name="lvl">Lv.</param>
		public void SetCategoryLvl(string categoryId, int lvl)
		{
			ItemCategory cat = GetCategory(categoryId);
			cat.ItemLevel = lvl;
		}

		/// <summary>
		/// Add money to actual playerMoney
		/// </summary>
		/// <param name="moneyToAdd"></param>
		public void AddMoney(int moneyToAdd)
		{
			_playerMoney += moneyToAdd;
		}

		/// <summary>
		/// Remove money to actual playerMoney
		/// </summary>
		/// <param name="moneyToRemove"></param>
		public void RemoveMoney(int moneyToRemove)
		{
			_playerMoney -= moneyToRemove;
			if (_playerMoney < 0)
			{
				_playerMoney = 0;
			}
		}

		/// <summary>
		/// Check if a specific item has been unlocked
		/// </summary>
		/// <returns><c>true</c> if this instance is unlocked the specified itemId; otherwise, <c>false</c>.</returns>
		/// <param name="itemId">Skill identifier.</param>
		public bool IsUnlocked(string itemId)
		{
			return GetItem(itemId).Unlocked;
		}

		public ItemCollectionGrid GetGrid(ItemCategory category)
		{
			ItemCollection[] collect = category.GetComponentsInChildren<ItemCollection>();
			Vector2 min = new Vector2(Mathf.Infinity, Mathf.Infinity);
			Vector2 max = new Vector2(Mathf.NegativeInfinity, Mathf.NegativeInfinity);

			// Find the min x, min y, max x, and max y
			foreach (ItemCollection col in collect)
			{
				if (col.WindowRect.x < min.x) min.x = col.WindowRect.x;
				if (col.WindowRect.x > max.x) max.x = col.WindowRect.x;
				if (col.WindowRect.y < min.y) min.y = col.WindowRect.y;
				if (col.WindowRect.y > max.y) max.y = col.WindowRect.y;
			}

			int x, y;
			int width = Mathf.CeilToInt(Mathf.Abs(min.x - max.x) / GridCellSize.x) + 1;
			int height = Mathf.CeilToInt(Mathf.Abs(min.y - max.y) / GridCellSize.y) + 1;
			ItemCollection[,] grid = new ItemCollection[width, height];
			foreach (ItemCollection col in collect)
			{
				x = Mathf.RoundToInt((col.WindowRect.x - min.x) / GridCellSize.x);
				y = Mathf.RoundToInt((col.WindowRect.y - min.y) / GridCellSize.y);
				grid[x, y] = col;
			}

			return new ItemCollectionGrid(grid);
		}

		/* SAVE */

		/// <summary>
		/// Returns a snapshot of this item shop current state
		/// </summary>
		/// <returns>The snapshot.</returns>
		virtual public SaveItemShop GetSnapshot()
		{
			List<SaveItem> items = new List<SaveItem>();
			List<SaveItem> playerItems = new List<SaveItem>();
			List<SaveItemCollection> itemCollections = new List<SaveItemCollection>();
			List<SaveItemCategory> itemsCategories = new List<SaveItemCategory>();

			foreach (Item i in GetItems())
			{
				string itemName = "";
				if (i.Icon != null)
				{
					itemName = i.Icon.name;

				}
				items.Add(new SaveItem
				{
					_uuid = i.Uuid,
					_unlocked = i.Unlocked,
					_consommable = i.Consommable,
					_cost = i.Cost,
					_icon = itemName,
					_rarity = i.Rarity,
					_stock = i.Stock
				});;
			}

			ItemCollection[] collections = GetItemCollections();

			foreach (ItemCollection col in collections)
			{
				Item i = col.LastBoughtItem;
				if (col.Item.Unlocked || col.Item.Consommable)
                {
					i = col.Item;
                }

				if (i != null && i.Stock > 0)
                {
					string itemName = "";
					if (i.Icon != null)
					{
						itemName = i.Icon.name;

					}

					playerItems.Add(new SaveItem
					{
						_uuid = i.Uuid,
						_unlocked = i.Unlocked,
						_consommable = i.Consommable,
						_cost = i.Cost,
						_icon = itemName,
						_rarity = i.Rarity,
						_stock = i.Stock
					});
				}
			}

			foreach (ItemCollection col in collections)
			{
				itemCollections.Add(new SaveItemCollection
				{
					_uuid = col.Uuid,
					_itemIndex = col.ItemIndex
				});
			}

			foreach (ItemCategory cat in GetCategories())
			{
				itemsCategories.Add(new SaveItemCategory
				{
					_uuid = cat.Uuid,
					_itemLevel = cat.ItemLevel
				});
			}

			return new SaveItemShop
			{
				_playerMoney = this._playerMoney,
				_items = items,
				_playerItems = playerItems,
				_collections = itemCollections,
				_categories = itemsCategories
			};
		}

		/// <summary>
		/// Restores a snapshot and overwrites the current item shop values with it
		/// </summary>
		/// <param name="snapshot">Snapshot.</param>
		virtual public void LoadSnapshot(SaveItemShop snapshot)
		{
			Initialize();

			this._playerMoney = snapshot._playerMoney;

			foreach (SaveItem i in snapshot._items)
			{
				_itemLib[i._uuid].Unlocked = i._unlocked;
				_itemLib[i._uuid].Stock = i._stock;
			}

			foreach (SaveItem it in snapshot._playerItems)
			{
				_itemLib[it._uuid].Unlocked = it._unlocked;
				_itemLib[it._uuid].Stock = it._stock;
			}

			foreach (SaveItemCollection c in snapshot._collections)
			{
				_skillCollectionLib[c._uuid].ItemIndex = c._itemIndex;
			}

			foreach (SaveItemCategory c in snapshot._categories)
			{
				_categoryLib[c._uuid].ItemLevel = c._itemLevel;
			}
		}

		/* SAVE */
	}
}
