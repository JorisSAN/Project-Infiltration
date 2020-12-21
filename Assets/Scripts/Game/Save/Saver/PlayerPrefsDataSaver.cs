using Newtonsoft.Json;
using System;
using UnityEngine;

namespace game.save.saver
{
    /// <summary>
    /// Saver using player prefs system
    /// </summary>
    public class PlayerPrefsDataSaver : FormattedSaveDataSaver<string>
    {
        private const string METADATA_SUFFIX = "_METADATA";
        private string _saveMetadataName;

        public PlayerPrefsDataSaver(string saveName) : base(saveName)
        {
            _saveMetadataName = saveName + METADATA_SUFFIX;
            _loadedData.GameSave = string.Empty;
            _isInitialized = true;
        }

        public void RemoveSave()
        {
            PlayerPrefs.DeleteKey(_saveName);
            PlayerPrefs.DeleteKey(_saveMetadataName);
        }

        public override void Save(IGameSave gameSave, SavedObject[] systemsToBeSaved, Action<bool> OnSaveDone)
        {
            FormattedSave<string> save = BuildFormattedSave(gameSave, systemsToBeSaved);
            string jsonObject = Serialize(save);
            _saveMetadata.SaveTime = DateTime.UtcNow;
            _saveMetadata.SaveVersion = gameSave.SaveVersion;
            string jsonMetadata = SerializeSaveMetadata();

            bool saveCanSucceed = !(string.IsNullOrEmpty(jsonObject) || jsonObject.Equals("{}"));
            if (saveCanSucceed)
            {
                PlayerPrefs.SetString(_saveName, jsonObject);
                PlayerPrefs.SetString(_saveMetadataName, jsonMetadata);
            }
            OnSaveDone(saveCanSucceed);
        }

        private string SerializeSaveMetadata()
        {
            return JsonConvert.SerializeObject(_saveMetadata);
        }

        public override void Load(Action<bool> OnLoadDataDone)
        {
            string savedObjectAsJSON = PlayerPrefs.GetString(_saveName);
            Debug.Log(savedObjectAsJSON);
            string metadataAsJSON = PlayerPrefs.GetString(_saveMetadataName);

            bool isSaveValid = !string.IsNullOrEmpty(savedObjectAsJSON) && !savedObjectAsJSON.Equals("{}")
                && !string.IsNullOrEmpty(metadataAsJSON) && !metadataAsJSON.Equals("{}");

            if (!isSaveValid)
            {
                Debug.Log("Save is not valid");
                _loadedData.GameSave = "";
                _loadedData.Systems = Array.Empty<SystemSave<string>>();
            }
            else
            {
                _loadedData = this.Deserialize<FormattedSave<string>>(savedObjectAsJSON);
                Debug.Log(_loadedData.GameSave);
                DeserializeMetadata(metadataAsJSON);
            }
            OnLoadDataDone(isSaveValid);
        }

        protected override T Deserialize<T>(string dataToDeserialize)
        {
            T savedObject;
            try
            {
                savedObject = JsonConvert.DeserializeObject<T>(dataToDeserialize);
            }
            catch (Exception exception)
            {
                throw new SavedDataLoadException($"The {_saveName} save file isn't of type {typeof(T)}.\n{exception.ToString()}");
            }
            return savedObject;
        }

        protected override string Serialize<T>(T dataToSerialize)
        {
            return JsonConvert.SerializeObject(dataToSerialize);
        }

        public override bool ContainsSave()
        {
            return PlayerPrefs.HasKey(_saveName);
        }

        public override DateTime LastSaveTime()
        {
            if (ContainsSave() && _saveMetadata.SaveTime.Equals(DateTime.MinValue)
                && PlayerPrefs.HasKey(_saveMetadataName))
            {
                string metadata = PlayerPrefs.GetString(_saveMetadataName);
                DeserializeMetadata(metadata);
            }
            return base.LastSaveTime();
        }

        /// <summary>
        ///     We need to deserialize the metadata as this
        ///     because il2cpp doesn't want to aot compiles
        ///     the call Deserialize<SaveMetadata>("string")
        /// </summary>
        /// <param name="metadata"></param>
        private void DeserializeMetadata(string metadata)
        {
            try
            {
                _saveMetadata = JsonConvert.DeserializeObject<SaveMetadata>(metadata);
            }
            catch (Exception exception)
            {
                throw new SavedDataLoadException($"The {_saveMetadataName} save file isn't of type {typeof(SaveMetadata)}.\n{exception.ToString()}");
            }
        }

        private class PlayerPrefsDataSaverAOT<T> : PlayerPrefsDataSaver
        {
            public PlayerPrefsDataSaverAOT() : base("dummy") { }

            public T DeserializeGeneric(string content) { return this.Deserialize<T>(content); }
        }
        private class PlayerPrefsSaveMetadataDeserialize : PlayerPrefsDataSaverAOT<SaveMetadata>
        {
            public SaveMetadata DeserializeSaveMetadata(string toDeserialize)
            {
                RetryToDeserializeGeneric();
                return this.DeserializeGeneric(toDeserialize);
            }

            private void RetryToDeserializeGeneric()
            {
                this.Deserialize<SaveMetadata>("");
            }
        }

#pragma warning disable 0219, 0168, 0612
        public static bool AOTCompiles()
        {
            PlayerPrefsDataSaver dataSaver = new PlayerPrefsDataSaver("aot_compiles");
            dataSaver.LastSaveTime();
            SaveMetadata metadata = dataSaver.Deserialize<SaveMetadata>("");

            FormattedSave<byte[]> formattedSave = dataSaver.Deserialize<FormattedSave<byte[]>>("");

            string answer = dataSaver.Serialize<SaveMetadata>(metadata);
            answer = dataSaver.Serialize<string>(answer);
            answer = dataSaver.Serialize<byte[]>(formattedSave.GameSave);

            PlayerPrefsSaveMetadataDeserialize aotCompilesPlease = new PlayerPrefsSaveMetadataDeserialize();
            SaveMetadata metadataGeneric = aotCompilesPlease.DeserializeGeneric("my content");
            SaveMetadata metadataNotGeneric = aotCompilesPlease.DeserializeSaveMetadata("my content");
            SaveMetadata directCall = JsonConvert.DeserializeObject<SaveMetadata>("another content");

            return !string.IsNullOrEmpty(answer) && answer.Length >= 0 && !dataSaver._saveName.Equals("")
                && !string.IsNullOrEmpty(metadataGeneric.ToString()) && !string.IsNullOrEmpty(metadataNotGeneric.ToString())
                && !string.IsNullOrEmpty(directCall.ToString());
        }
#pragma warning restore 0219, 0168, 0612
    }
}
