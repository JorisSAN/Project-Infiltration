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

        public override void ClearSnapshot()
        {
            PlayerHealth = new PlayerHealth();
            SkillTree = new SaveSkillTree();
            PlayerSkills = new List<SaveSkill>();
        }

        public void LoadDataFromSavable(SavableGameSnapshot savableSnapshot)
        {
            PlayerHealth = savableSnapshot.PlayerHealth;
            SkillTree = savableSnapshot.SkillTree;
            PlayerSkills = savableSnapshot.PlayerSkills;
        }
    }
}
