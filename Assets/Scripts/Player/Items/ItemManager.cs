using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.item
{
    public class ItemManager : MonoBehaviour
    {
        public static ItemManager Instance { get; private set; } // Singleton for the moment : to change

        private void Awake()
        {
            Instance = this;
        }

        public void UseItem(string itemUuid)
        {
            switch (itemUuid)
            {
                case "ProPistol":
                    Debug.Log("ProPistol !");
                    break;

                case "Crossbow":
                    Debug.Log("Crossbow !");
                    break;
            }
        }
    }
}
