using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.save.saver
{
    /// <summary>
    /// Representation of a savable object for the memory system.
    /// </summary>
    [Serializable]
    public class SavableObject
    {
        /// <summary>
        ///     Methods that tells us if the savable
        ///     object is valid after a loading
        ///     I.E. if it is coherent according to its data
        /// </summary>
        /// <returns>true if the object is valid, false otherwise</returns>
        public virtual bool IsValid()
        {
            throw new NotImplementedException("Not implemented !");
        }
    }
}
