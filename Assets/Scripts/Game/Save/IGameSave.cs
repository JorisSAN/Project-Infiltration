using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.save
{
    /// <summary>
    /// Interface to have a common parent for all game saves.    
    /// </summary>
    public interface IGameSave
    {
        int SaveVersion { get; }
    }
}
