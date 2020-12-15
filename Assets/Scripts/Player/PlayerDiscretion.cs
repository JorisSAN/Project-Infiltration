using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerDiscretion
    {
        private int _discretionMax;
        public int Discretion { get; private set; }

        // METHODS
        public void Initialize()
        {
            _discretionMax = 100;
            Discretion = _discretionMax;
        }

        public bool IsDiscover()
        {
            return Discretion == 0;
        }

        public bool IsFullyHide()
        {
            return Discretion == _discretionMax;
        }

        public void AddDiscretion(int discretionToAdd)
        {
            Discretion += discretionToAdd;
            if (Discretion > _discretionMax)
            {
                Discretion = _discretionMax;
            }
        }

        public void RemoveDiscretion(int discretionToRemove)
        {
            Discretion -= discretionToRemove;
            if (Discretion < 0)
            {
                Discretion = 0;
            }
        }
    }
}
