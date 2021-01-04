using game.save;
using game.save.snapshot;
using player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace skilltree
{
	public class SkillTreeSnapshot : MonoBehaviour, IGameSaveDataHolder
	{
		private SaveSkillTree _snapshot;
		[SerializeField] private SkillMenu _menu = default;
		[SerializeField] private Player _player = default;

		public void SaveSnapshot()
		{
			_snapshot = _menu._skillTree.GetSnapshot();
			SaveFromGameSaveManager();

			// Load skills to player
			_player.LoadSkills(_snapshot._skills);
		}

		public void LoadSnapshot()
		{
			if (_snapshot != null)
			{
				_menu._skillTree.LoadSnapshot(_snapshot);
			}
		}

		public void ResetSnapshot()
		{
			if (_snapshot != null)
			{
				_menu._skillTree.LoadSnapshot(_snapshot);
				SkillCategory[] categories = _menu._skillTree.GetCategories();
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
			_snapshot = snapshot.SkillTree;

			LoadSnapshot();
		}

        public void Save(GameSnapshotBase snapshot)
        {
			GameSnapshot gameSnapshot = ((GameSnapshot)snapshot);
			gameSnapshot.SkillTree = _snapshot;
			gameSnapshot.PlayerSkills = _snapshot._skills;
		}
    }
}