using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace itemshop
{
	public enum Rarity
	{
		COMMON,
		RARE,
		EPIC,
		LEGENDARY
	}

	public class Item : MonoBehaviour
	{
		[DisplayName("Item")] public string displayName = "Item";

		[SerializeField] private string _id = default;
		[SerializeField] private string _uuid = default;
		[SerializeField] private bool _unlocked = default;

		[TextArea(3, 5)]
		[SerializeField] private string _description = default;

		[SerializeField] private int _levelRequirement = default;
		[SerializeField] private int _cost = default;
		[SerializeField] private Rarity _rarity = default;

		[SerializeField] private Sprite _icon = default;
		[SerializeField] private Rect _windowRect = default;

		private ItemCategory _category;
		private ItemCollection _collection;
		private ItemShop _shop;

		// GETTERS
		public string Id
		{
			get
			{
				return _id;
			}
		}

		public bool Unlocked
		{
			get
			{
				return _unlocked;
			}
			set
			{
				_unlocked = value;
			}
		}

		public string Description
		{
			get
			{
				return _description;
			}
		}

		public int LevelRequirement
		{
			get
			{
				return _levelRequirement;
			}
		}

		public int Cost
		{
			get
			{
				return _cost;
			}
		}

		public Rarity Rarity
		{
			get
			{
				return _rarity;
			}
		}

		public Sprite Icon
		{
			get
			{
				return _icon;
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

		public ItemCategory Category
		{
			get
			{
				if (_category == null) _category = transform.parent.parent.GetComponent<ItemCategory>();
				return _category;
			}
		}

		public ItemCollection Collection
		{
			get
			{
				if (_collection == null) _collection = GetComponentInParent<ItemCollection>();
				return _collection;
			}
		}

		public ItemShop Shop
		{
			get
			{
				if (_shop == null) _shop = Category.GetComponentInParent<ItemShop>();
				return _shop;
			}
		}

		public string Uuid
		{
			get
			{
				if (string.IsNullOrEmpty(_uuid))
				{
					//_uuid = System.Guid.NewGuid().ToString();
					_uuid = _id;
				}

				return _uuid;
			}

			set
			{
				_uuid = value;
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
		/// Loops through all requirements to check if this skill is available for purchase
		/// </summary>
		/// <returns><c>true</c> if this instance is requirements; otherwise, <c>false</c>.</returns>
		public bool IsAvailable()
		{
			int level = 0;
			int playerMoney = 0;
			return (level >= LevelRequirement) && (playerMoney >= _cost);
		}
	}
}
