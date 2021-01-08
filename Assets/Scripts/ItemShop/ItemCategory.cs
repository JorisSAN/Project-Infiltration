using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace itemshop
{
    public class ItemCategory : MonoBehaviour
    {
        [DisplayName("Category")] public string displayName = "Category";

        [SerializeField] private string _id = default;
        [SerializeField] private string _uuid = default;

        [TextArea(3, 5)]
        [SerializeField] private string _description = default;

        [SerializeField] private int _itemLevel = 0;

        // GETTERS

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
        }

        public int ItemLevel
        {
            get
            {
                return _itemLevel;
            }
            set
            {
                _itemLevel = value;
            }
        }

        public string Uuid
        {
            get
            {
                if (string.IsNullOrEmpty(_uuid))
                {
                    _uuid = System.Guid.NewGuid().ToString();
                }

                return _uuid;
            }

            set
            {
                _uuid = value;
            }
        }

        // METHODS

        /// <summary>
        /// Retreives item collections without any parents
        /// </summary>
        /// <returns>The root skill collections.</returns>
        public List<ItemCollection> GetRootItemCollections()
        {
            List<ItemCollection> items = new List<ItemCollection>();

            // Loop through and find all collection that are a child of something
            Dictionary<Transform, bool> blacklist = new Dictionary<Transform, bool>();
            foreach (Transform child in transform)
            {
                foreach (ItemCollection childNode in child.GetComponent<ItemCollection>().ChildSkills)
                {
                    blacklist[childNode.transform] = true;
                }
            }

            // Anything not blacklisted as a child node is a root, return those
            foreach (Transform child in transform)
            {
                if (!blacklist.ContainsKey(child))
                {
                    ItemCollection item = child.GetComponent<ItemCollection>();
                    items.Add(item);
                }
            }

            return items;
        }
    }
}
