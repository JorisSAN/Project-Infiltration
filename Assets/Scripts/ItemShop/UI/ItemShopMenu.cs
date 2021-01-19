using intemshop.ui.carousel;
using itempshop.ui;
using itemshop;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace intemshop.ui
{
    public class ItemShopMenu : MonoBehaviour
    {
		private Dictionary<ItemCollection, ItemDisplayer> _itemDisplayersRef;
		[SerializeField] private List<ItemDisplayer> _itemDisplayers = new List<ItemDisplayer>();

		private List<ItemCollection> _collections = new List<ItemCollection>();

		[SerializeField] private ItemShopCarouselFiller _carouselFiller = default;

        public ItemShop _itemShop;

        [Header("Header")]
        [SerializeField] private Transform _categoryContainer = default;
        [SerializeField] private ButtonItemCategory _categoryButtonPrefab = default;
        [SerializeField] private Text _playerMoney = default;
        [SerializeField] private Text _categoryName = default;

		[Header("Context Sidebar")]
        [SerializeField] private RectTransform _sidebarContainer = default;
		[SerializeField] private Text _sidebarRarity = default;
		[SerializeField] private Text _sidebarTitle = default;
        [SerializeField] private Text _sidebarBody = default;
        [SerializeField] private Text _sidebarRequirements = default;
        [SerializeField] private Text _sidebarPurchasedMessage = default;
        [SerializeField] private Button _sidebarPurchase = default;
        [SerializeField] private Image _sidebarItemIcon = default;

		private void Start()
		{
			// Clear out test categories
			foreach (Transform child in _categoryContainer)
			{
				Destroy(child.gameObject);
			}

			// Populate categories
			ItemCategory[] itemCategories = _itemShop.GetCategories();
			foreach (ItemCategory category in itemCategories)
			{
				ButtonItemCategory buttonItemCat = Instantiate<ButtonItemCategory>(_categoryButtonPrefab);
				buttonItemCat.transform.SetParent(_categoryContainer);
				buttonItemCat.transform.localScale = Vector3.one;

				//buttonSkillCat.ResetPosition();
				buttonItemCat.ChangeContent(category.displayName);

				// Dump in a tmp variable to force capture the variable by the event
				ItemCategory tmpCat = category;
				buttonItemCat.RemoveAllListeners();
				buttonItemCat.AddListener(() =>
				{
					ShowCategory(tmpCat);
				});
			}

			if (itemCategories.Length > 0)
			{
				ShowCategory(itemCategories[0]);
			}
		}

		public void ShowCategory(ItemCategory category)
		{
			//_itemDisplayers = new List<ItemDisplayer>();
			_itemDisplayersRef = new Dictionary<ItemCollection, ItemDisplayer>();
			_categoryName.text = string.Format("{0}: Level {1}", category.displayName, category.ItemLevel);
			ClearDetails();

			CreateGrid(category);
			UpdatePlayerMoney();
			UpdateItemDisplayers();
		}

		public void CreateGrid(ItemCategory category)
        {
			_collections = category.GetRootItemCollections();

			int itemCount = 0;
			foreach (ItemCollection col in _collections)
            {
				itemCount += col.ItemCount;
            }

			List<ItemInfo> _infos = new List<ItemInfo>(itemCount);

			foreach (ItemCollection col in _collections)
            {
				Item item = col.Item;
				//if (!item.Unlocked)
				//{
					_infos.Add(new ItemInfo
					{
						Uuid = item.Uuid,
						Unlocked = item.Unlocked,
						Cost = item.Cost,
						Rarity = item.Rarity,
						SpriteName = item.Icon.name,
						ActionWhenClicked = ShowNodeDetails,
						Collection = col
					});
				//}
			}

			_carouselFiller.FillCarouselWithViews(_infos.ToArray());
		}

		private void UpdateItemDisplayers()
		{
			int itemCount = 0;
			foreach (ItemCollection col in _collections)
			{
				itemCount += col.ItemCount;
			}

			List<ItemInfo> _infos = new List<ItemInfo>(itemCount);

			foreach (ItemCollection col in _collections)
			{
				Item item = col.Item;
				//if (!item.Unlocked)
                //{
					_infos.Add(new ItemInfo
					{
						Uuid = item.Uuid,
						Unlocked = item.Unlocked,
						Cost = item.Cost,
						Rarity = item.Rarity,
						SpriteName = item.Icon.name,
						ActionWhenClicked = ShowNodeDetails,
						Collection = col
					});
				//}
			}

			_carouselFiller.FillCarouselWithViews(_infos.ToArray());
		}

		public void ShowNodeDetails(ItemDisplayer item)
		{
			ItemCollection itemCollection = item.Collection;
			NodeStatus status = item.GetStatus();

			_sidebarRarity.text = itemCollection.Item.Rarity.ToString();
			_sidebarRarity.color = item.GetColor(itemCollection.Item.Rarity);

			_sidebarTitle.text = string.Format("{0}: Lvl {1}", itemCollection.displayName, itemCollection.ItemIndex + 1);
			_sidebarBody.text = itemCollection.Item.Description;

			if (itemCollection.Item.Icon != null)
			{
				_sidebarItemIcon.sprite = itemCollection.Item.Icon;
				_sidebarItemIcon.gameObject.SetActive(true);
			}
			else
			{
				_sidebarItemIcon.gameObject.SetActive(false);
			}

			_sidebarItemIcon.preserveAspect = true;


			string requirements = itemCollection.Item.GetRequirements();
			if (string.IsNullOrEmpty(requirements))
			{
				_sidebarRequirements.gameObject.SetActive(false);
			}
			else
			{
				_sidebarRequirements.text = "<b>Requirements:</b> \n" + itemCollection.Item.GetRequirements();
				_sidebarRequirements.gameObject.SetActive(true);
			}

			if (status == NodeStatus.Purchasable)
			{
				_sidebarPurchasedMessage.gameObject.SetActive(false);
				_sidebarPurchase.gameObject.SetActive(true);
				_sidebarPurchase.onClick.RemoveAllListeners();
				_sidebarPurchase.onClick.AddListener(() =>
				{
					itemCollection.UnlockItem();
					UpdateItemDisplayers();
					ShowNodeDetails(item);
					UpdatePlayerMoney();
				});
			}
			else if (status == NodeStatus.Unlocked)
			{
				_sidebarPurchasedMessage.gameObject.SetActive(true);
				_sidebarPurchase.gameObject.SetActive(false);
			}
			else
			{
				_sidebarPurchasedMessage.gameObject.SetActive(false);
				_sidebarPurchase.gameObject.SetActive(false);
			}

			_sidebarContainer.gameObject.SetActive(true);
		}

		private void ClearDetails()
		{
			_sidebarContainer.gameObject.SetActive(false);
		}

		private void UpdatePlayerMoney()
		{
			_playerMoney.text = "Player Money: " + _itemShop.PlayerMoney;
		}
	}
}
