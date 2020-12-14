using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace props.door
{
    public class LevelDoor : Door
    {
        [SerializeField] private int _sceneIndex = 0;
        [SerializeField] private bool _loadLevelAsync = false;

        public int SceneIndex
        {
            get
            {
                return _sceneIndex;
            }
        }

        public bool LoadLevelAsync
        {
            get
            {
                return _loadLevelAsync;
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
