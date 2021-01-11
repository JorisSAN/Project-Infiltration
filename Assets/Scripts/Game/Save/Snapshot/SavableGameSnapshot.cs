using game.save.saver;
using itemshop.save;
using player;
using skilltree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.save.snapshot
{
    [Serializable]
    public class SavableGameSnapshot : SavableObject, IGameSave
    {
        public int SaveVersion;
        int IGameSave.SaveVersion => SaveVersion;

        public PlayerHealth PlayerHealth;
        public SaveSkillTree SkillTree;
        public List<SaveSkill> PlayerSkills;
        public SaveItemShop ItemShop;
        public List<SaveItem> PlayerItems;

        public void InitializeFromGameSnapshot(GameSnapshot gameSnapshot)
        {
            PlayerHealth = gameSnapshot.PlayerHealth;
            SkillTree = gameSnapshot.SkillTree;
            PlayerSkills = gameSnapshot.PlayerSkills;
            ItemShop = gameSnapshot.ItemShop;
            PlayerItems = gameSnapshot.PlayerItems;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
