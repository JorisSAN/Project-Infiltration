﻿using game.save;
using game.save.snapshot;
using itemshop.save;
using player.inventory;
using player.item;
using player.skill;
using skilltree;
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
        public PlayerSkillCollection PlayerSkillCollection { get; private set; }
        public PlayerItemCollection PlayerItemCollection { get; private set; }
        public PlayerInventory PlayerInventory { get; private set; }

        [SerializeField] private TopDownMovement _playerMovement = default;

        public bool Initialized { get; private set; }

        // METHODS
        public void Awake()
        {
            Initialized = false;
            Initialize();
        }

        public void Update()
        {
            /* Discretion update */
            bool isWalking = _playerMovement.IsWalking;
            bool isRunning = _playerMovement.IsRunning;
            bool isCrouching = _playerMovement.IsCrouching;

            PlayerDiscretion.ActualDiscretionTimer -= Time.deltaTime;
            if (PlayerDiscretion.ActualDiscretionTimer < 0)
            {
                PlayerDiscretion.UpdateDiscretionWithMovement(isWalking, isRunning, isCrouching);
                PlayerDiscretion.ActualDiscretionTimer = PlayerDiscretion.BaseDiscretionTimer;
            }
        }

        public void Initialize()
        {
            SetStartingPos(this.gameObject.transform.localPosition);

            PlayerHealth = new PlayerHealth();
            PlayerMoney = new PlayerMoney();
            PlayerDiscretion = new PlayerDiscretion();
            PlayerSkillCollection = new PlayerSkillCollection();
            PlayerItemCollection = new PlayerItemCollection();
            PlayerInventory = new PlayerInventory();

            PlayerMoney.Initialize();
            PlayerDiscretion.Initialize();
            PlayerInventory.Initialize();

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

        public void UpdateMoney(int money)
        {
            if (money < 0)
            {
                money = 0;
            }
            PlayerMoney.UpdateMoney(money);
        }

        public void IncreaseDiscretion(int discretion)
        {
            PlayerDiscretion.AddDiscretion(discretion);
        }

        public void DecreaseDiscretion(int discretion)
        {
            PlayerDiscretion.RemoveDiscretion(discretion);
        }

        public void SelectSkill(string skillUuid)
        {
            PlayerSkillCollection.SelectSkill(skillUuid);

            PlayerInventory.SelectSkill(PlayerSkillCollection.CurrentSkill);
        }

        public void UseSkill()
        {
            PlayerInventory.UseSkill();
        }

        public void LoadSkills(List<SaveSkill> skills)
        {
            PlayerSkillCollection.Load(skills);
        }

        public void SelectItem(string itemUuid)
        {
            PlayerItemCollection.SelectItem(itemUuid);

            PlayerInventory.SelectItem(PlayerItemCollection.CurrentItem);
        }

        public void UseItem()
        {
            PlayerItemCollection.UpdateStockItem();
            PlayerInventory.UseItem();

            //SaveFromGameSaveManager();
        }

        public void LoadItems(List<SaveItem> items)
        {
            PlayerItemCollection.Load(items);
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
            LoadSkills(snapshot.PlayerSkills);
            LoadItems(snapshot.PlayerItems);
        }

        public void Save(GameSnapshotBase snapshot)
        {
            Debug.Log("Saving data from the game save");
            GameSnapshot gameSnapshot = ((GameSnapshot)snapshot);
            gameSnapshot.PlayerHealth = this.PlayerHealth;
            gameSnapshot.PlayerItems = this.PlayerItemCollection.GetSnapshot();
        }
    }
}
