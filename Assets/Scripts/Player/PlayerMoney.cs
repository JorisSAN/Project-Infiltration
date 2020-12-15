using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerMoney
    {
        public int Money { get; private set; }

        // METHODS
        public void Initialize()
        {
            Money = 100;
        }

        public bool HasEnougthMoney(int moneyRequired)
        {
            return Money >= moneyRequired;
        }

        public void AddMoney(int moneyToAdd)
        {
            Money += moneyToAdd;
        }

        public void RemoveMoney(int moneyToRemove)
        {
            Money -= moneyToRemove;
            if (Money < 0)
            {
                Money = 0;
            }
        }
    }
}
