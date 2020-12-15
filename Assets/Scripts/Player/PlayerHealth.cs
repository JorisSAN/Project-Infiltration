using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerHealth
    {
        private int _healthMax;
        public int Health { get; private set; }

        // METHODS
        public void Initialize()
        {
            _healthMax = 100;
            Health = _healthMax;
        }

        public bool IsDead()
        {
            return Health <= 0;
        }

        public void AddHealth(int healthToAdd)
        {
            Health += healthToAdd;
            if (Health > _healthMax)
            {
                Health = _healthMax;
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
