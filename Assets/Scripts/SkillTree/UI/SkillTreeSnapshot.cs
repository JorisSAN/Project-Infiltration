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
		private bool _wantToSave = false;
		[SerializeField] private SkillMenu _menu = default;
		[SerializeField] private Player _player = default;

		public void SaveSnapshot()
		{
			_wantToSave = true;
			_snapshot = _menu._skillTree.GetSnapshot();
			SaveFromGameSaveManager();

			// Load skills to player
			if (_player != null)
            {
				_player.LoadSkills(RetrieveSkillsUnlocked());
			}
		}

		public List<SaveSkill> RetrieveSkillsUnlocked()
		{
			List<SaveSkill> skills = new List<SaveSkill>();
			foreach (SaveSkill skill in _snapshot._skills)
			{
				if (skill._unlocked)
				{
					skills.Add(skill);
				}
			}

			return skills;
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

			//LoadSnapshot();
		}

        public void Save(GameSnapshotBase snapshot)
        {
			//if (_wantToSave)
            //{
				GameSnapshot gameSnapshot = ((GameSnapshot)snapshot);
				gameSnapshot.SkillTree = _snapshot;
				gameSnapshot.PlayerSkills = RetrieveSkillsUnlocked();
			//}
		}
    }
}