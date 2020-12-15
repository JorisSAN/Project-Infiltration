using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class Player : MonoBehaviour
    {
        private PlayerHealth _playerHealth;
        private PlayerMoney _playerMoney;
        private PlayerDiscretion _playerDiscretion;

        // METHODS
        public void Awake()
        {
            _playerHealth = new PlayerHealth();
            _playerMoney = new PlayerMoney();
            _playerDiscretion = new PlayerDiscretion();
            Initialize();
        }

        public void Initialize()
        {
            _playerHealth.Initialize();
            _playerMoney.Initialize();
            _playerDiscretion.Initialize();
        }

        public void TakeDamage(int damage)
        {
            _playerHealth.RemoveHealth(damage);
            if (_playerHealth.IsDead())
            {
                // Death animation
            }
        }

        public void Heal(int heal)
        {
            _playerHealth.AddHealth(heal);
        }

        public void IncreaseMoney(int money)
        {
            _playerMoney.AddMoney(money);
        }

        public void DecreaseMoney(int money)
        {
            _playerMoney.RemoveMoney(money);
        }
    }
}
