using game.save;
using game.save.saver;
using game.save.snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveProcessor
{
    private IDataSaver _playerPrefsDataSaver;
    private SavableGameSnapshot _savableGameSnapshot;

    private const int INITIAL_NUMBER_OF_SYSTEM = 0;
    private List<string> _identifiers = new List<string>();
    private List<ISavableSystem> _objectsToSave = new List<ISavableSystem>();

    private IGameSave _gameSaveData;
    private SavedObject[] _systemsToSave;
    private IGameSaverOnSave _onSaveFinished;

    private const int MAX_SAVE_TRIES = 5;
    private int _saveTries;
    
    public void Initialize(IDataSaver playerPrefsDataSaver)
    {
        _playerPrefsDataSaver = playerPrefsDataSaver;
        _savableGameSnapshot = new SavableGameSnapshot();
    }

    public IGameSave RetrieveSaveData()
    {
        return _savableGameSnapshot;
    }

    public void Save(GameSnapshotBase save, IGameSaverOnSave onSaveFinished)
    {
        GameSnapshot gameSnapshoToBeSavable = (GameSnapshot)save;
        _onSaveFinished = onSaveFinished;
        _savableGameSnapshot.InitializeFromGameSnapshot(gameSnapshoToBeSavable);
        LaunchDataToSave();
    }

    public void LaunchDataToSave()
    {
        SavedObject[] systemsToSave = new SavedObject[_identifiers.Count];
        for (int i = 0; i < _identifiers.Count; i++)
        {
            systemsToSave[i].Identifier = _identifiers[i];
            SavableObject objectToSave = _objectsToSave[i].RetrieveDataToSave();
            systemsToSave[i].SavedData = objectToSave;
        }

        IGameSave gameSaveData = RetrieveSaveData();
        StartSaveProcessus(gameSaveData, systemsToSave);
    }

    public void StartSaveProcessus(IGameSave gameSaveData, SavedObject[] systemsToSave)
    {
        _gameSaveData = gameSaveData;
        _systemsToSave = systemsToSave;
        SaveProcessus(gameSaveData, systemsToSave);
    }

    private void SaveProcessus(IGameSave gameSaveData, SavedObject[] systemsToSave)
    {
        _playerPrefsDataSaver.Save(gameSaveData, systemsToSave, RetrySaveIfFailed);
    }

    private void RetrySaveIfFailed(bool isSaveASuccess)
    {
        _saveTries++;
        if (!isSaveASuccess)
        {
            RetrySaveOnFail();
            return;
        }

        CallOnSaveDoneWithSuccessSetTo(true);
    }

    private void RetrySaveOnFail()
    {
        if (_saveTries > MAX_SAVE_TRIES)
        {
            CallOnSaveDoneWithSuccessSetTo(false);
        }
    }

    private void CallOnSaveDoneWithSuccessSetTo(bool isSuccess)
    {
        _onSaveFinished(isSuccess);
        _gameSaveData = null;
        _systemsToSave = null;
    }
}
