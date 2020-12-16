using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerDiscretion
    {
        public int DiscretionMax { get; private set; }
        public int Discretion { get; private set; }

        // METHODS
        public void Initialize()
        {
            DiscretionMax = 100;
            Discretion = DiscretionMax;
        }

        public bool IsDiscover()
        {
            return Discretion == 0;
        }

        public bool IsFullyHide()
        {
            return Discretion == DiscretionMax;
        }

        public void AddDiscretion(int discretionToAdd)
        {
            Discretion += discretionToAdd;
            if (Discretion > DiscretionMax)
            {
                Discretion = DiscretionMax;
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
