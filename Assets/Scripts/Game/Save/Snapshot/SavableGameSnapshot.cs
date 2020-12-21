using game.save.saver;
using player;
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

        public void InitializeFromGameSnapshot(GameSnapshot gameSnapshot)
        {
            PlayerHealth = gameSnapshot.PlayerHealth;
        }

        public override bool IsValid()
        {
            return true;
        }
    }
}
