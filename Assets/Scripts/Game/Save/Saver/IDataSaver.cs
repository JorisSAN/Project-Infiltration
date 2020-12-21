using System;

namespace game.save.saver
{
    public interface IDataSaver : ILoader
    {
        /// <summary>
        ///     Load the data from the disk
        /// </summary>
        /// <param name="OnLoadDataDone">
        ///     Callback called when the loading ends. It has true in parameter 
        ///     if there the load succeed, false otherwise
        /// </param>
        void Load(Action<bool> OnLoadDataDone);

        /// <summary>
        ///     Save the data passed in parameter on disk
        /// </summary>
        /// <param name="gameSave">the save representing the game data</param>
        /// <param name="systemsToBeSaved">the systems data that we want to save</param>
        /// <param name="OnSaveDone">callback for when the save finished</param>
        void Save(IGameSave gameSave, SavedObject[] systemsToBeSaved, Action<bool> OnSaveDone);

        /// <summary>
        ///     Check the presence of a save in the system
        /// </summary>
        /// <returns>true if there is a loadable save, false otherwise</returns>
        bool ContainsSave();

        /// <summary>
        ///     Get the last time a save occured
        /// </summary>
        /// <returns>the date of the last save</returns>
        DateTime LastSaveTime();

        /// <summary>
        ///     Add a callback for when the system is ready to work
        /// </summary>
        /// <param name="onSystemReady">callback for when the system is ready</param>
        void SubscribeToSystemReady(Action onSystemReady);

        /// <summary>
        ///     Check if the system is initialized
        /// </summary>
        /// <returns>true if the system has been initialized, false otherwise</returns>
        bool IsInitialized();
    }
}
