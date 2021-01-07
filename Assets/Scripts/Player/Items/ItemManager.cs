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
                case "Item_1":
                    Debug.Log("Item 1 !");
                    break;

                case "Item_2":
                    Debug.Log("Item 2 !");
                    break;
            }
        }
    }
}
