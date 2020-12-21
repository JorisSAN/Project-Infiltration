using System;

namespace game.save.saver
{
    [Serializable]
    public struct SaveMetadata
    {
        public DateTime SaveTime;
        public int SaveVersion;
    }
}
