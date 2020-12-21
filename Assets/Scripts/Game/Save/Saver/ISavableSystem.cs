namespace game.save.saver
{
    public interface ISavableSystem
    {
        string GetSaveIdentifier();

        /// <summary>
        ///     Retrieve the current savable of the object
        /// </summary>
        /// <returns>the current state of the object</returns>
        SavableObject RetrieveDataToSave();

        /// <summary>
        ///     Callback when the save has finished.
        /// </summary>
        /// <param name="isSuccess">Tells if the save succeeded or not</param>
        void OnSaveDone(bool isSuccess);

        /// <summary>
        ///     Ask the system to load its data
        ///     from the specified ILoader
        /// </summary>
        /// <param name="systemLoader">The loader that will serve the system to load back its data</param>
        void LoadFrom(ILoader systemLoader);
    }
}
