using game.save;
using game.save.snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameSaveManager _gameSaveManager = default;

    /// <summary>
    /// Initialize on start
    /// </summary>
    /// <returns></returns>
    public void Start()
    {
        StartCoroutine(InitializeGameSaveManager());
    }

    public IEnumerator InitializeGameSaveManager()
    {
        _gameSaveManager.Initialize(typeof(GameSnapshot));
        while (!_gameSaveManager.Initialized)
        {
            yield return null;
        }

        bool hasSucceedToLoad = false;
        if (_gameSaveManager.ContainSave())
        {
            Debug.LogWarning("GameSaveManager have a save, trying to load it");
            bool loadingSave = true;
            GameSnapshot loadTarget = new GameSnapshot();
            _gameSaveManager.Load(loadTarget, (bool success) =>
            {
                loadingSave = false;
                hasSucceedToLoad = success;
            });

            while (loadingSave)
            {
                yield return null;
            }
        }
        if (!hasSucceedToLoad)
        {
            // Do nothing for now
        }
    }
}
