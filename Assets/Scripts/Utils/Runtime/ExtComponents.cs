using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace utils.runtime
{
    public static class ExtComponents
    {
        /// <summary>
        /// Get a component of type T on any of the children recursivly
        /// use:
        /// Player player = gameObject.GetExtComponentInChildrens<Player>();
        /// Player player = gameObject.GetExtComponentInChildrens<Player>(4, true);
        /// Player[] player = gameObject.GetExtComponentInChildrens<Player>();
        /// Player[] player = gameObject.GetExtComponentInChildrens<Player>(4, true);
        /// </summary>
        public static T GetExtComponentInChildrens<T>(this Component component, int depth = 99, bool startWithOurSelf = false)
            where T : Component
        {
            return (GetExtComponentInChildrens<T>(component.gameObject, depth, startWithOurSelf));
        }
        public static T GetExtComponentInChildrens<T>(this GameObject gameObject, int depth = 99, bool startWithOurSelf = false)
            where T : Component
        {
            if (startWithOurSelf)
            {
                T result = gameObject.GetComponent<T>();
                if (result != null)
                    return (result);
            }

            foreach (Transform t in gameObject.transform)
            {
                if (depth - 1 <= 0)
                    return (null);
                return (t.gameObject.GetExtComponentInChildrens<T>(depth - 1, true));
            }
            return (null);
        }

        public static T[] GetExtComponentsInChildrens<T>(this Component component, int depth = 99, bool startWithOurSelf = false)
            where T : Component
        {
            return (GetExtComponentsInChildrens<T>(component.gameObject, depth, startWithOurSelf));
        }
        public static T[] GetExtComponentsInChildrens<T>(this GameObject gameObject, int depth = 99, bool startWithOurSelf = false)
            where T : Component
        {
            List<T> results = new List<T>();
            if (startWithOurSelf)
            {
                T[] result = gameObject.GetComponents<T>();
                for (int i = 0; i < result.Length; i++)
                {
                    results.Add(result[i]);
                }
            }

            foreach (Transform t in gameObject.transform)
            {
                if (depth - 1 <= 0)
                    break;
                results.AddRange(t.gameObject.GetExtComponentsInChildrens<T>(depth - 1, true));
            }

            return results.ToArray();
        }
    }
}