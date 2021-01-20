using hud.selectionwheel;
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
        [SerializeField, NoNull] private ItemWheel _itemWheel = default;
        [SerializeField, NoNull] private SkillWheel _skillWheel = default;

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
                UseSkill();
            }

            /* Use item */
            if (Input.GetKeyDown(KeyCode.O))
            {
                UseItem();
            }

            /* Show item wheel */
            if (Input.GetKeyDown(KeyCode.L))
            {
                _itemWheel.Show();
            }

            /* Hide item wheel */
            if (Input.GetKeyUp(KeyCode.L))
            {
                _itemWheel.Hide();
            }

            /* Show skill wheel */
            if (Input.GetKeyDown(KeyCode.M))
            {
                _skillWheel.Show();
            }

            /* Hide skill wheel */
            if (Input.GetKeyUp(KeyCode.M))
            {
                _skillWheel.Hide();
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

            _itemWheel.LoadWheel(_player.PlayerItemCollection.Items);
            _skillWheel.LoadWheel(_player.PlayerSkillCollection.Skills);
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

        public void SelectSkill(string skillUuid)
        {
            _player.SelectSkill(skillUuid);
            _inventory.SelectSkill(_player.PlayerSkillCollection.CurrentSkill);
        }

        public void UseSkill()
        {
            _player.UseSkill();
            _inventory.UseSkill();
        }

        public void SelectItem(string itemUuid)
        {
            _player.SelectItem(itemUuid);
            _inventory.SelectItem(_player.PlayerItemCollection.CurrentItem);
        }

        public void UseItem()
        {
            _player.UseItem();
            _inventory.UseItem();
        }
    }
}
