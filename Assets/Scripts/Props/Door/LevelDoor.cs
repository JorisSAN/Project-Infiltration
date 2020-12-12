using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace props.door
{
    public class LevelDoor : Door
    {
        [SerializeField] private int _levelNumber = 0;

        public int LevelNumber
        {
            get
            {
                return _levelNumber;
            }
        }

        public override void OnTriggerEnter(Collider collider)
        {
            OpenDoor();
        }

        public override void OnTriggerExit(Collider collider)
        {
            CloseDoor();
        }
    }
}
