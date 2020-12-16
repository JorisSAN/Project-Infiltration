using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class Player : MonoBehaviour
    {
        private Vector3 _startingPos;
        public PlayerHealth PlayerHealth { get; private set; }
        public PlayerMoney PlayerMoney { get; private set; }
        public PlayerDiscretion PlayerDiscretion { get; private set; }

        // METHODS
        public void Awake()
        {
            PlayerHealth = new PlayerHealth();
            PlayerMoney = new PlayerMoney();
            PlayerDiscretion = new PlayerDiscretion();
            Initialize();
        }

        public void Initialize()
        {
            SetStartingPos(this.gameObject.transform.localPosition);
            PlayerHealth.Initialize();
            PlayerMoney.Initialize();
            PlayerDiscretion.Initialize();
        }

        public void SetStartingPos(Vector3 pos)
        {
            Debug.Log("Starting pos = " + pos);
            _startingPos = pos;
        }

        public void TakeDamage(int damage)
        {
            PlayerHealth.RemoveHealth(damage);
            if (PlayerHealth.IsDead())
            {
                // Death animation

            }
        }

        public void Heal(int heal)
        {
            PlayerHealth.AddHealth(heal);
        }

        public void IncreaseMoney(int money)
        {
            PlayerMoney.AddMoney(money);
        }

        public void DecreaseMoney(int money)
        {
            PlayerMoney.RemoveMoney(money);
        }

        public void IncreaseDiscretion(int discretion)
        {
            PlayerDiscretion.AddDiscretion(discretion);
        }

        public void DecreaseDiscretion(int discretion)
        {
            PlayerDiscretion.RemoveDiscretion(discretion);
        }
    }
}
