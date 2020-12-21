using game.save.snapshot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.save
{
    /// <summary>
    /// Common interface for object that have to be save
    /// </summary>
    public interface IGameSaveDataHolder
    {
        void Load(GameSnapshotBase save);
        void Save(GameSnapshotBase snapshot);
    }
}
