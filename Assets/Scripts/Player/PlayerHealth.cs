using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerHealth
    {
        public int MaxHealth { get; private set; }
        public int Health { get; private set; }

        // METHODS
        public void Initialize()
        {
            MaxHealth = 100;
            Health = MaxHealth;
        }

        public bool IsDead()
        {
            return Health <= 0;
        }

        public void AddHealth(int healthToAdd)
        {
            Health += healthToAdd;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }
        }

        public void RemoveHealth(int healthToRemove)
        {
            Health -= healthToRemove;
            if (Health < 0)
            {
                Health = 0;
            }
        }
    }
}
