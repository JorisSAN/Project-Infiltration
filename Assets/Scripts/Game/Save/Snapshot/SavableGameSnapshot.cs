using game.save.saver;
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

        public void InitializeFromGameSnapshot(GameSnapshot gameSnapshot)
        {
            PlayerHealth = gameSnapshot.PlayerHealth;
            SkillTree = gameSnapshot.SkillTree;
            PlayerSkills = gameSnapshot.PlayerSkills;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
