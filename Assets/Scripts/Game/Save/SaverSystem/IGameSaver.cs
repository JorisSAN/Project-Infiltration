using game.save.snapshot;
using System;

public interface IGameSaver
{
    DateTime LastSaveTime { get; }

    void Initialize(string saveName);
    bool Initialized { get; }
    bool ContainSave { get; }
    void Load(GameSnapshotBase loadTarget, IGameSaverOnLoad onLoadFinished);
    void Save(GameSnapshotBase save, IGameSaverOnSave onSaveFinished);
}

public delegate void IGameSaverOnLoad(bool success, GameSnapshotBase save);
public delegate void IGameSaverOnSave(bool success);