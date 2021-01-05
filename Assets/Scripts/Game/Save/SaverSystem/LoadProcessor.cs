using game.save.saver;
using game.save.snapshot;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadProcessor 
{
    private IDataSaver _playerPrefsDataSaver;

    private GameSnapshot _loadTarget;
    private IGameSaverOnLoad _onLoadFinished;

    private const int MAX_LOAD_TRIES = 5;
    private int _loadTries;

    public void Initialize(IDataSaver playerPrefsDataSaver)
    {
        _playerPrefsDataSaver = playerPrefsDataSaver;
    }

    public void Load(GameSnapshotBase loadTarget, IGameSaverOnLoad onLoadFinished)
    {
        _loadTarget = (GameSnapshot)loadTarget;
        _onLoadFinished = onLoadFinished;
        RunLoad();
    }

    public void LoadFrom(ILoader systemLoader)
    {
        if (_loadTarget == null || _onLoadFinished == null)
        {
            OnLoadDone(false);
            return;
        }

        SavableGameSnapshot gameSnapshot = systemLoader.LoadGameSave<SavableGameSnapshot>();
        if (gameSnapshot == null || !gameSnapshot.IsValid())
        {
            OnLoadDone(false);
            return;
        }
        _loadTarget.LoadDataFromSavable(gameSnapshot);
        OnLoadDone(true);
    }

    private void OnLoadDone(bool isLoadASuccess)
    {
        _onLoadFinished(isLoadASuccess, _loadTarget);

        _loadTarget = null;
        _onLoadFinished = null;
    }

    private void RunLoad()
    {
        _playerPrefsDataSaver.Load(RetryIfFailOnLoadDone);
    }

    private void RetryIfFailOnLoadDone(bool isLoadASuccess)
    {
        if (!isLoadASuccess)
        {
            RetryLoadOnFail();
            return;
        }
        else
        {
            LoadFrom(_playerPrefsDataSaver);
        }
    }

    private void RetryLoadOnFail()
    {
        _loadTries++;
        if (_loadTries > MAX_LOAD_TRIES)
        {
            Debug.LogError("Loading failed");
        }
        RunLoad();
    }
}
