using contextswitcher.loader;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace props.door
{
    /// <summary>
    /// Launcher for level loader
    /// </summary>
    public class LevelDoorLauncher : MonoBehaviour
    {
        [SerializeField] private LevelDoor _levelDoor = default;
        [SerializeField] private LevelLoader _levelLoader = default;

        public void OnTriggerEnter(Collider collider)
        {
            if (_levelDoor.LoadLevelAsync)
            {
                _levelLoader.LoadLevelAsync(_levelDoor.SceneIndex);
            }
            else
            {
                _levelLoader.LoadLevel(_levelDoor.SceneIndex);
            }
        }

        public void OnTriggerExit(Collider collider)
        {
            // DO NOTHING
        }
    }
}
