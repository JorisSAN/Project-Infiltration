using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace props.door
{
    public class LevelDoorLauncher : MonoBehaviour
    {
        [SerializeField] private LevelDoor _levelDoor = default;

        public void OnTriggerEnter(Collider collider)
        {
            string levelName = "Level" + _levelDoor.LevelNumber;
            SceneManager.LoadScene(levelName, LoadSceneMode.Single);
        }

        public void OnTriggerExit(Collider collider)
        {
            //string levelName = "Level" + _levelDoor.LevelNumber;
            //SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        }
    }
}
