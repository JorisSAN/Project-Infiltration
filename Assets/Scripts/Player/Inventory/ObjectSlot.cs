using player.inventory;
using player.obj;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.inventory
{
    public class ObjectSlot : IInventorySlot
    {
        private PlayerObject _object;

        public void LoadObject(PlayerObject obj)
        {
            if (_object == null)
            {
                _object = obj;
                return;
            }

            if (!_object.Uuid.Equals(obj.Uuid))
            {
                _object = obj;
            }
        }

        public void Use()
        {
            if (!(_object is null))
            {
                ObjectManager.Instance.UseObject(_object.Uuid);
            }
        }
    }
}
