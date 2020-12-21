using game.save.saver;
using game.save.snapshot;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaverSystem : IGameSaver
{
    private IDataSaver _playerPrefsDataSaver;

    private SaveProcessor _saveProcessor;
    private LoadProcessor _loadProcessor;
    private bool _initialized = false;

    public DateTime LastSaveTime
    {
        get
        {
            DateTime lastSaveTime = DateTime.MinValue;
            if (_playerPrefsDataSaver.ContainsSave())
            {
                lastSaveTime = _playerPrefsDataSaver.LastSaveTime();
            }
            return lastSaveTime;
        }
    }

    public bool Initialized
    {
        get
        {
            return _initialized;
        }
    }

    public bool ContainSave
    {
        get
        {
            return _playerPrefsDataSaver.ContainsSave();
        }
    }

    public void Initialize(string saveName)
    {
        _playerPrefsDataSaver = new PlayerPrefsDataSaver(saveName);
        _saveProcessor = new SaveProcessor();
        _loadProcessor = new LoadProcessor();

        _saveProcessor.Initialize(_playerPrefsDataSaver);
        _loadProcessor.Initialize(_playerPrefsDataSaver);
        _initialized = true;
    }

    public void Save(GameSnapshotBase save, IGameSaverOnSave onSaveFinished)
    {
        _saveProcessor.Save(save, onSaveFinished);
    }

    public void Load(GameSnapshotBase loadTarget, IGameSaverOnLoad onLoadFinished)
    {
        _loadProcessor.Load(loadTarget, onLoadFinished);
    }
    
}
