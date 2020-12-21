using player;
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

        public override void ClearSnapshot()
        {
            PlayerHealth = new PlayerHealth();
        }

        public void LoadDataFromSavable(SavableGameSnapshot savableSnapshot)
        {
            PlayerHealth = savableSnapshot.PlayerHealth;
        }
    }
}
