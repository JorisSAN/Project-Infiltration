namespace game.save.saver
{
    public class SystemSaveWrapper<T>
    {
        public int SaveVersion = -1;
        public T SavedSystem = default(T);

        public SystemSaveWrapper() : this(-1, default(T))
        {
        }

        public SystemSaveWrapper(int saveVersion, T savedSystem)
        {
            SaveVersion = saveVersion;
            SavedSystem = savedSystem;
        }
    }
}
