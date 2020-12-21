using System.IO;

namespace game.save.snapshot
{
    [UnityEngine.SerializeField]
    public abstract class GameSnapshotBase
    {
        public abstract void ClearSnapshot();
    }
}
