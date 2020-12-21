using System;

namespace game.save.saver
{
    /// <summary>
    ///     Class that will contains the different objects we want to save for the game.
    ///     
    ///     It has two parts : 
    ///             - the game save that contains specific data for the game
    ///             - the system saves that contains generic systems saves (statistics, achievements ect..)
    ///     
    ///     We save the game save and systems save as T object
    ///     because it permits us to have a deep serialization without having problem
    ///     when deserializing. 
    ///     
    /// </summary>
    /// <typeparam name="T">The serializing type of the save on disk</typeparam>
    [Serializable]
    public class FormattedSave<T>
    {
        public T GameSave;
        public SaveSeparator SaveSeparator;
        public SystemSave<T>[] Systems;
        public SaveSeparator EndSave;

        public FormattedSave() : this(0) { }

        public FormattedSave(int numberOfSystem)
        {
            Systems = new SystemSave<T>[numberOfSystem];
        }
    }

    [Serializable]
    public class SystemSave<SerializeFormat>
    {
        public string Identifier;
        public SerializeFormat SavedData;
        public SaveSeparator EndOfSave;
    }

    [Serializable]
    public struct SaveSeparator
    {
    }
}
