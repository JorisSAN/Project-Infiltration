using System;
using UnityEngine;

namespace game.save.saver
{
    /// <summary>
    /// A saver for the data of the game, witch is formatted correctly in json
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FormattedSaveDataSaver<T> : IDataSaver
    {
        protected string _saveName;
        protected FormattedSave<T> _loadedData;
        protected SaveMetadata _saveMetadata;
        protected Action _onSystemReady;
        protected bool _isInitialized;

        public FormattedSaveDataSaver(string saveName)
        {
            if (string.IsNullOrEmpty(saveName))
            {
                throw new NullReferenceException("The passed saveName is empty!");
            }
            _saveName = saveName;
            _loadedData = new FormattedSave<T>();
            _saveMetadata = new SaveMetadata()
            {
                SaveTime = DateTime.MinValue,
                SaveVersion = -1
            };
            _isInitialized = false;
        }

        public bool IsInitialized()
        {
            return _isInitialized;
        }

        public void SubscribeToSystemReady(Action setReady)
        {
            if (_isInitialized)
            {
                setReady();
                return;
            }
            _onSystemReady += setReady;
        }

        protected FormattedSave<T> BuildFormattedSave(IGameSave gameSave, SavedObject[] systemsToBeSaved)
        {
            FormattedSave<T> formattedSave = new FormattedSave<T>(systemsToBeSaved.Length);
            formattedSave.GameSave = Serialize(gameSave);

            for (int i = 0; i < systemsToBeSaved.Length; i++)
            {
                formattedSave.Systems[i] = new SystemSave<T>();
                formattedSave.Systems[i].Identifier = systemsToBeSaved[i].Identifier;
                formattedSave.Systems[i].SavedData = Serialize(systemsToBeSaved[i].SavedData);
            }

            return formattedSave;
        }

        public U LoadGameSave<U>() where U : IGameSave
        {
            if (!IsSaveLoadable(_loadedData.GameSave))
            {
                Debug.Log("Save not loadable");
                return default(U);
            }
            U gameSave = Deserialize<U>(_loadedData.GameSave);
            _saveMetadata.SaveVersion = gameSave.SaveVersion;
            return gameSave;
        }

        protected virtual bool IsSaveLoadable(T saveToLoad)
        {
            return !_loadedData.GameSave.Equals(default(T));
        }

        public SystemSaveWrapper<U> LoadSystem<U>(string identifier)
        {
            int systemPosition = FindSystemPositionInSave(identifier);
            bool isSystemSaveCanBeLoaded = systemPosition >= 0 && IsSaveLoadable(_loadedData.Systems[systemPosition].SavedData);
            if (!isSystemSaveCanBeLoaded)
            {
                return new SystemSaveWrapper<U>();
            }
            U systemSave = Deserialize<U>(_loadedData.Systems[systemPosition].SavedData);
            return new SystemSaveWrapper<U>(_saveMetadata.SaveVersion, systemSave);
        }

        private int FindSystemPositionInSave(string systemIdentifier)
        {
            int systemPosition;
            for (systemPosition = _loadedData.Systems.Length - 1;
                systemPosition >= 0
                && !_loadedData.Systems[systemPosition].Identifier.Equals(systemIdentifier);
                systemPosition--)
            { }

            return systemPosition;
        }

        public abstract bool ContainsSave();

        public abstract void Save(IGameSave gameSave, SavedObject[] systemsToBeSaved, Action<bool> OnSaveDone);

        public abstract void Load(Action<bool> OnLoadDataDone);

        protected abstract U Deserialize<U>(T dataToDeserialize);

        protected abstract T Serialize<U>(U dataToSerialize);

        public virtual DateTime LastSaveTime()
        {
            return _saveMetadata.SaveTime;
        }
    }
}
