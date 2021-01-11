using game.save;
using game.save.snapshot;
using intemshop.ui;
using itemshop.save;
using player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace itemshop.ui
{
	public class ItemShopSnapshot : MonoBehaviour, IGameSaveDataHolder
	{
		private SaveItemShop _snapshot;
		[SerializeField] private ItemShopMenu _menu = default;
		[SerializeField] private Player _player = default;

		public void SaveSnapshot()
		{
			Debug.Log("Save shop 1");
			_snapshot = _menu._itemShop.GetSnapshot();
			SaveFromGameSaveManager();

			// Load items to player
			if (_player != null)
            {
				_player.LoadItems(_snapshot._items);
				_player.UpdateMoney(_snapshot._playerMoney);
			}
		}

		public void LoadSnapshot()
		{
			if (_snapshot != null)
			{
				_menu._itemShop.LoadSnapshot(_snapshot);
			}
		}

		public void ResetSnapshot()
		{
			if (_snapshot != null)
			{
				_menu._itemShop.LoadSnapshot(_snapshot);
				ItemCategory[] categories = _menu._itemShop.GetCategories();
				_menu.ShowCategory(categories[0]);
			}
		}

		public void SaveFromGameSaveManager()
		{
			GameSaveManager.Instance.Save();
		}

		public void Load(GameSnapshotBase save)
		{
			GameSnapshot snapshot = (GameSnapshot)save;
			_snapshot = snapshot.ItemShop;

			//LoadSnapshot();
		}

		public void Save(GameSnapshotBase snapshot)
		{
			Debug.Log("Save shop");
			GameSnapshot gameSnapshot = ((GameSnapshot)snapshot);
			gameSnapshot.ItemShop = _snapshot;
			gameSnapshot.PlayerItems = _snapshot._items;
		}
	}
}
