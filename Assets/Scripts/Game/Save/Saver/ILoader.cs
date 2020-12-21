namespace game.save.saver
{
    public interface ILoader
    {
        /// <summary>
        ///     Load a game save of a specific type T from the system
        /// </summary>
        /// <typeparam name="T">the type of the gamesave we want to load</typeparam>
        /// <returns>the game save as a T object</returns>
        T LoadGameSave<T>() where T : IGameSave;

        /// <summary>
        ///     Load a system save of a specific type T from the system
        /// </summary>
        /// <typeparam name="T">the type of the system save we want</typeparam>
        /// <param name="identifier">the identifier associated to the system save</param>
        /// <returns>the system save</returns>
        SystemSaveWrapper<T> LoadSystem<T>(string identifier);
    }
}