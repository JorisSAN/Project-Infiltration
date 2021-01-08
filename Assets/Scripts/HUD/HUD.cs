using player;
using player.skill;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using utils.noNull;

namespace hud
{
    public class Hud : MonoBehaviour
    {
        [SerializeField, NoNull] private Player _player = default;
        [SerializeField, NoNull] private HealthBar _healthBar = default;
        [SerializeField, NoNull] private MoneyContainer _moneyContainer = default;
        [SerializeField, NoNull] private DiscretionBar _discretionBar = default;
        [SerializeField, NoNull] private Inventory _inventory = default;

        // METHODS
        public void Start()
        {
            StartCoroutine(Initialize());
        }

        public void Update()
        {
            _discretionBar.SetDiscretion(_player.PlayerDiscretion.Discretion);

            /* Use skill */
            if (Input.GetKeyDown(KeyCode.P))
            {
                SelectSkill();
                UseSkill();
            }

            /* Use object */
            if (Input.GetKeyDown(KeyCode.O))
            {
                SelectObject();
                UseObject();
            }
        }

        public IEnumerator Initialize()
        {
            yield return new WaitUntil(() => _player.Initialized);

            _healthBar.SetMaxHealth(_player.PlayerHealth.MaxHealth);
            _healthBar.SetHealth(_player.PlayerHealth.Health);

            _moneyContainer.SetAmount(_player.PlayerMoney.Money.ToString());

            _discretionBar.SetMaxDiscretion(_player.PlayerDiscretion.DiscretionMax);
            _discretionBar.SetDiscretion(_player.PlayerDiscretion.Discretion);
        }

        public void AddHealth(int health)
        {
            _player.Heal(health);
            _healthBar.SetHealth(_player.PlayerHealth.Health);
        }

        public void RemoveHealth(int health)
        {
            _player.TakeDamage(health);
            _healthBar.SetHealth(_player.PlayerHealth.Health);
        }

        public void AddMoney(int money)
        {
            _player.IncreaseMoney(money);
            _moneyContainer.DisplayCurrencyAmountChange(_player.PlayerMoney.Money);
        }

        public void RemoveMoney(int money)
        {
            _player.DecreaseMoney(money);
            _moneyContainer.DisplayCurrencyAmountChange(_player.PlayerMoney.Money);
        }

        public void AddDiscretion(int discretion)
        {
            _player.IncreaseDiscretion(discretion);
            _discretionBar.SetDiscretion(_player.PlayerDiscretion.Discretion);
        }

        public void RemoveDiscretion(int discretion)
        {
            _player.DecreaseDiscretion(discretion);
            _discretionBar.SetDiscretion(_player.PlayerDiscretion.Discretion);
        }

        public void SelectSkill()
        {
            _player.SelectSkill();
            _inventory.SelectSkill(_player.PlayerSkillCollection.CurrentSkill);
        }

        public void UseSkill()
        {
            _player.UseSkill();
            _inventory.UseSkill();
        }

        public void SelectObject()
        {
            _player.SelectItem();
            _inventory.SelectItem(_player.PlayerItemCollection.CurrentItem);
        }

        public void UseObject()
        {
            _player.UseItem();
            _inventory.UseItem();
        }
    }
}
