using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace player.obj
{
    public class PlayerObjectCollection
    {
        private List<PlayerObject> _objects = new List<PlayerObject>();

        public PlayerObject CurrentObject
        {
            get; set;
        }

        public void SelectObject(string objectUuid)
        {
            if (ContainObject(objectUuid) && IsObjectUnlocked(objectUuid))
            {
                Debug.Log("Object " + objectUuid + " selected !");
                CurrentObject = GetObject(objectUuid);
            }
        }

        public bool ContainObject(string objectUuid)
        {
            foreach (PlayerObject obj in _objects)
            {
                if (obj.Uuid.Equals(objectUuid))
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsObjectUnlocked(string objectUuid)
        {
            return GetObject(objectUuid).Unlocked;
        }

        public PlayerObject GetObject(string objectUuid)
        {
            foreach (PlayerObject obj in _objects)
            {
                if (obj.Uuid.Equals(objectUuid))
                {
                    return obj;
                }
            }
            return null;
        }

        public void Load()
        {
            // Fake load for the moment
            Debug.Log("Load objects !");
            _objects.Add(new PlayerObject("Object_1", true, "pistol-gun"));
            _objects.Add(new PlayerObject("Object_2", false, "crossbow"));
        }
    }
}
