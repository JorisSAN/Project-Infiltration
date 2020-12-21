using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace game.save.saver
{
    [Serializable]
    public struct SavedObject
    {
        public string Identifier;
        public SavableObject SavedData;
    }
}
