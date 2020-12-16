using player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace hud
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Player _player = default;
        [SerializeField] private HealthBar _healthBar = default;
        [SerializeField] private Money _money = default;
        [SerializeField] private DiscretionBar _discretionBar = default;

        // METHODS
        public void Awake()
        {
            Initialize();
        }

        public void Update()
        {
            _healthBar.SetHealth(_player.PlayerHealth.Health);
            _money.SetAmount(_player.PlayerMoney.Money.ToString());
            _discretionBar.SetDiscretion(_player.PlayerDiscretion.Discretion);
        }

        public void Initialize()
        {
            _healthBar.SetMaxHealth(_player.PlayerHealth.MaxHealth);
            _healthBar.SetHealth(_player.PlayerHealth.Health);

            _money.SetAmount(_player.PlayerMoney.Money.ToString());

            _discretionBar.SetMaxDiscretion(_player.PlayerDiscretion.DiscretionMax);
            _discretionBar.SetDiscretion(_player.PlayerDiscretion.Discretion);
        }

        public void AddHealth(int health)
        {
            _player.Heal(health);
        }

        public void RemoveHealth(int health)
        {
            _player.TakeDamage(health);
        }

        public void AddMoney(int money)
        {
            _player.IncreaseMoney(money);
        }

        public void RemoveMoney(int money)
        {
            _player.DecreaseMoney(money);
        }

        public void AddDiscretion(int discretion)
        {
            _player.IncreaseDiscretion(discretion);
        }

        public void RemoveDiscretion(int discretion)
        {
            _player.DecreaseDiscretion(discretion);
        }
    }
}
