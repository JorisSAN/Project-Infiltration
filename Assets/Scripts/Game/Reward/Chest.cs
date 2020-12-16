using hud;
using player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.rewards
{
    public class Chest : MonoBehaviour
    {
        private bool _canOpen = false;
        private bool _isOpen = false;
        private ChestContent _chestContent;

        [SerializeField] private Animator _chestAnimation = default;

        [SerializeField] private Player _player = default; // TO CHANGE WHEN IMPLEMENT GAMEFLOW
        [SerializeField] private Hud _hud = default; // TO CHANGE WHEN IMPLEMENT GAMEFLOW

        public void Awake()
        {
            _chestContent = new ChestContent();
        }

        public void Update()
        {
            /* Inputs handler */
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (_canOpen && !_isOpen)
                {
                    OpenChest();
                    _isOpen = true;
                }
            }
        }

        public void OnTriggerEnter(Collider collider)
        {
            _canOpen = true;
        }

        public void OnTriggerExit(Collider collider)
        {
            _canOpen = false;
        }

        private void OpenChest()
        {
            _chestAnimation.SetBool("isOpen", true);
            GiveRewardToPlayer();
        }

        private void CloseChest()
        {
            _chestAnimation.SetBool("isOpen", false);
        }

        private void GiveRewardToPlayer()
        {
            int amount = _chestContent.GenerateReward();
            _player.IncreaseMoney(amount);
            _hud.AddMoney(amount);
        }
    }
}
