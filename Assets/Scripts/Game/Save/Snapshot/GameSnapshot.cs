using itemshop.save;
using player;
using skilltree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.save.snapshot
{
    public class GameSnapshot : GameSnapshotBase
    {
        public PlayerHealth PlayerHealth
        {
            get; set;
        }

        public SaveSkillTree SkillTree
        {
            get; set;
        }

        public List<SaveSkill> PlayerSkills
        {
            get; set;
        }

        public SaveItemShop ItemShop
        {
            get; set;
        }

        public List<SaveItem> PlayerItems
        {
            get; set;
        }

        public override void ClearSnapshot()
        {
            PlayerHealth = new PlayerHealth();
            SkillTree = new SaveSkillTree();
            PlayerSkills = new List<SaveSkill>();
            ItemShop = new SaveItemShop();
            PlayerItems = new List<SaveItem>();
        }

        public void LoadDataFromSavable(SavableGameSnapshot savableSnapshot)
        {
            PlayerHealth = savableSnapshot.PlayerHealth;
            SkillTree = savableSnapshot.SkillTree;
            PlayerSkills = savableSnapshot.PlayerSkills;
            ItemShop = savableSnapshot.ItemShop;
            PlayerItems = savableSnapshot.PlayerItems;
        }
    }
}
