using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player
{
    public class PlayerDiscretion
    {
        public int DiscretionMax { get; private set; }
        public int Discretion { get; private set; }
        public float BaseDiscretionTimer { get; private set; }
        public float ActualDiscretionTimer { get; set; }

        private const int AMOUNT_TO_REMOVE_WHEN_WALKING = 4;
        private const int AMOUNT_TO_REMOVE_WHEN_RUNNING = 7;
        private const int AMOUNT_TO_ADD_WHEN_CROUCHING = 1;
        private const int AMOUNT_TO_ADD_WHEN_STANDING = 1;

        // METHODS
        public void Initialize()
        {
            DiscretionMax = 100;
            Discretion = DiscretionMax;

            BaseDiscretionTimer = 2.0f;
            ActualDiscretionTimer = BaseDiscretionTimer;
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

        public void UpdateDiscretionWithMovement(bool isWalking, bool isRunning, bool isCrouching)
        {
            if (isRunning)
            {
                RemoveDiscretion(AMOUNT_TO_REMOVE_WHEN_RUNNING);
            }
            else if (isCrouching)
            {
                AddDiscretion(AMOUNT_TO_ADD_WHEN_CROUCHING);
            }
            else if (isWalking)
            {
                RemoveDiscretion(AMOUNT_TO_REMOVE_WHEN_WALKING);
            }
            else
            {
                AddDiscretion(AMOUNT_TO_ADD_WHEN_STANDING);
            }
        }
    }
}
