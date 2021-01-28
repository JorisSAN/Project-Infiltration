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
		[SerializeField] private bool _consommable = default;

		[TextArea(3, 5)]
		[SerializeField] private string _description = default;

		[SerializeField] private int _levelRequirement = default;
		[SerializeField] private int _cost = default;
		[SerializeField] private Rarity _rarity = default;
		[SerializeField] private float _cooldown = default;

		[SerializeField] private Sprite _icon = default;

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

		public bool Consommable
		{
			get
			{
				return _consommable;
			}
			set
			{
				_consommable = value;
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

		public float Cooldown
		{
			get
			{
				return _cooldown;
			}
		}

		public Sprite Icon
		{
			get
			{
				return _icon;
			}
		}

        public int Stock { get; set; } = 0;

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
		/// Visual print out of requirements
		/// </summary>
		/// <returns>The requirements.</returns>
		public string GetRequirements()
		{
			string requirements = "";
			ItemCategory category = transform.parent.parent.GetComponent<ItemCategory>();

			if (_levelRequirement > 0)
				requirements += string.Format("* {0} Skill Lv {1} \n", category.displayName, _levelRequirement);


			return requirements;
		}

		/// <summary>
		/// Loops through all requirements to check if this item is available for purchase
		/// </summary>
		/// <returns><c>true</c> if this instance is requirements; otherwise, <c>false</c>.</returns>
		public bool IsAvailable()
		{
			int level = 0;
			int playerMoney = Shop.PlayerMoney;
			return (level >= LevelRequirement) && (playerMoney >= _cost);
		}
	}
}
