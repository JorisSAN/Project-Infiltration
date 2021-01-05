using game.save.snapshot;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace game.save
{
    public class TypeConstraintAttribute : PropertyAttribute
    {
        private Type type;

        public TypeConstraintAttribute(Type type)
        {
            this.type = type;
        }

        public Type Type
        {
            get { return type; }
        }
    }

    /// <summary>
    /// Our Manager for the saves of the game.
    /// </summary>
    public class GameSaveManager : MonoBehaviour
    {
        [SerializeField] private string _saveName = "Project_infiltration_save";
        [SerializeField, TypeConstraint(typeof(IGameSaveDataHolder))] private List<GameObject> _dataHolders = new List<GameObject>(0);
        
        private SaverSystem _activePersistantSaver = default;

        private bool _initialized;
        private bool _saveInPending;
        private bool _saving;
        private bool _resetSave;
        private List<IGameSaveDataHolder> gameSaveDataHolders;

        private Type _gameSnapshotClass;

        public static GameSaveManager Instance { get; private set; } // Singleton for the moment : to change

        private void Awake()
        {
            _initialized = false;
            Instance = this;
        }

        private void Update()
        {
            ProcessSave();
        }

        public bool Initialized
        {
            get
            {
                return _initialized && _activePersistantSaver.Initialized;
            }
        }

        public bool ContainSave()
        {
            return _activePersistantSaver.ContainSave;
        }

        public void Initialize(Type gameSnapshotClass)
        {
            _gameSnapshotClass = gameSnapshotClass; // typeof(GameSnapshot)
            _resetSave = false;

            gameSaveDataHolders = new List<IGameSaveDataHolder>();
            foreach (GameObject dataHolder in _dataHolders)
            {
                gameSaveDataHolders.Add(dataHolder.GetComponent<IGameSaveDataHolder>());
            }

            _activePersistantSaver = new SaverSystem();
            _activePersistantSaver.Initialize(_saveName);

            _initialized = true;
        }

        public void Load(GameSnapshotBase loadTarget, UnityAction<bool> callback)
        {
            if (!_initialized)
                throw new Exception("Not initialized");

            SaverSystem saverToUse = _activePersistantSaver;
            new LoadJob().InitializeDataHolders(gameSaveDataHolders).Execute(loadTarget, saverToUse, callback);
        }

        public void Save()
        {
            _saveInPending = true;
        }

        public void ClearSaves()
        {
            _resetSave = true;
            _saveInPending = true;
        }

        private void ProcessSave()
        {
            if (!_initialized || !_saveInPending || _saving)
            {
                return;
            }

            _saveInPending = false;
            _saving = true;

            GameSnapshotBase snapshot = (GameSnapshotBase)Activator.CreateInstance(_gameSnapshotClass);

            if (!_resetSave)
            {
                foreach (IGameSaveDataHolder dataHolder in gameSaveDataHolders)
                    dataHolder.Save(snapshot);
            }
            else
            {
                snapshot.ClearSnapshot();
            }

            new SaveJob().Execute(_activePersistantSaver, snapshot, (bool isSuccess) =>
            {
                Debug.Log("saveSucessState : " + isSuccess);
                _saving = false;
            });

        }

        private class SaveJob
        {
            private SaverSystem currentSaver;
            private GameSnapshotBase save;
            private bool run;
            private UnityAction<bool> callback;

            public void Execute(SaverSystem saver, GameSnapshotBase save, UnityAction<bool> callback)
            {
                if (run)
                    throw new Exception("Illegal state");

                run = true;

                currentSaver = saver;
                this.save = save;
                this.callback = callback;

                if (currentSaver != null)
                    currentSaver.Save(save, OnSaveFinished);
                else if (callback != null)
                    callback(false);
            }

            private void OnSaveFinished(bool success)
            {
                if (callback != null)
                    callback(success);
            }
        }

        private class LoadJob
        {
            private SaverSystem currentSaver;
            private GameSnapshotBase currentLoadTarget;
            private UnityAction<bool> callback;
            private List<IGameSaveDataHolder> dataHolders = new List<IGameSaveDataHolder>();

            public LoadJob InitializeDataHolders(List<IGameSaveDataHolder> gameSaveDataHolders)
            {
                this.dataHolders = gameSaveDataHolders;
                return this;
            }

            public void Execute(GameSnapshotBase loadTarget, SaverSystem saver, UnityAction<bool> callback)
            {
                Debug.Log("Load execute...");
                currentSaver = saver;
                currentLoadTarget = loadTarget;
                this.callback = callback;
                currentSaver.Load(currentLoadTarget, OnLoadFinished);
            }

            private void OnLoadFinished(bool success, GameSnapshotBase save)
            {
                Debug.Log("Load success = " + success);
                if (success)
                {
                    foreach (IGameSaveDataHolder dataHolder in dataHolders)
                        dataHolder.Load(save);
                }
                if (callback != null)
                {
                    callback(success);
                }
            }
        }
    }
}
