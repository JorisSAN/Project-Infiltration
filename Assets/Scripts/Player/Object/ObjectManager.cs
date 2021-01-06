using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.obj
{
    public class ObjectManager : MonoBehaviour
    {
        public static ObjectManager Instance { get; private set; } // Singleton for the moment : to change

        private void Awake()
        {
            Instance = this;
        }

        public void UseObject(string objectUuid)
        {
            switch (objectUuid)
            {
                case "Object_1":
                    Debug.Log("Object 1 !");
                    break;

                case "Object_2":
                    Debug.Log("Object 2 !");
                    break;
            }
        }
    }
}
