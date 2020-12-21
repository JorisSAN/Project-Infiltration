using game.save;
using game.save.snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class Player : MonoBehaviour, IGameSaveDataHolder
    {
        private Vector3 _startingPos;
        public PlayerHealth PlayerHealth { get; private set; }
        public PlayerMoney PlayerMoney { get; private set; }
        public PlayerDiscretion PlayerDiscretion { get; private set; }
        public bool Initialized { get; private set; }

        // METHODS
        public void Awake()
        {
            Initialized = false;
            Initialize();
        }

        public void Initialize()
        {
            SetStartingPos(this.gameObject.transform.localPosition);

            PlayerHealth = new PlayerHealth();
            PlayerMoney = new PlayerMoney();
            PlayerDiscretion = new PlayerDiscretion();

            //PlayerHealth.Initialize();
            PlayerMoney.Initialize();
            PlayerDiscretion.Initialize();

            Initialized = true;
        }

        public void SetStartingPos(Vector3 pos)
        {
            _startingPos = pos;
        }

        public void TakeDamage(int damage)
        {
            PlayerHealth.RemoveHealth(damage);
            if (PlayerHealth.IsDead())
            {
                // Death animation

            }
            SaveFromGameSaveManager();
        }

        public void Heal(int heal)
        {
            PlayerHealth.AddHealth(heal);
            SaveFromGameSaveManager();
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

        public void SaveFromGameSaveManager()
        {
            GameSaveManager.Instance.Save();
        }

        public void Load(GameSnapshotBase save)
        {
            Debug.Log("Loading data from the game save");
            GameSnapshot snapshot = (GameSnapshot)save;

            //this.Initialize();
            PlayerHealth = snapshot.PlayerHealth;
            Debug.Log("PlayerHealth == null ? " + PlayerHealth is null);
            Debug.Log("PlayerHealth.MaxHealth = " + PlayerHealth.MaxHealth);
            Debug.Log("PlayerHealth.Health = " + PlayerHealth.Health);
        }

        public void Save(GameSnapshotBase snapshot)
        {
            Debug.Log("Saving data from the game save");
            GameSnapshot gameSnapshot = ((GameSnapshot)snapshot);
            gameSnapshot.PlayerHealth = this.PlayerHealth;
        }
    }
}
