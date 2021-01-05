using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace skilltree
{
    public class SkillCategory : MonoBehaviour
    {
        [DisplayName("Category")] public string displayName = "Category";

        [SerializeField] private string _id = default;
        [SerializeField] private string _uuid = default;

        [TextArea(3, 5)]
        [SerializeField] private string _description = default;

        [SerializeField] private int _skillLevel = 0;

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

        public int SkillLevel
        {
            get
            {
                return _skillLevel;
            }
            set
            {
                _skillLevel = value;
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
        /// Retreives skill collections without any parents
        /// </summary>
        /// <returns>The root skill collections.</returns>
        public List<SkillCollection> GetRootSkillCollections()
        {
            List<SkillCollection> skills = new List<SkillCollection>();

            // Loop through and find all collection that are a child of something
            Dictionary<Transform, bool> blacklist = new Dictionary<Transform, bool>();
            foreach (Transform child in transform)
            {
                foreach (SkillCollection childNode in child.GetComponent<SkillCollection>().ChildSkills)
                {
                    blacklist[childNode.transform] = true;
                }
            }

            // Anything not blacklisted as a child node is a root, return those
            foreach (Transform child in transform)
            {
                if (!blacklist.ContainsKey(child))
                {
                    SkillCollection skill = child.GetComponent<SkillCollection>();
                    skills.Add(skill);
                }
            }

            return skills;
        }
    }
}
