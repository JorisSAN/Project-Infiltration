using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace skilltree
{
	public class Skill : MonoBehaviour
	{
		[DisplayName("Skill")] public string displayName = "Skill";

		[SerializeField] private string _id = default;
		[SerializeField] private string _uuid = default;
		[SerializeField] private bool _unlocked = default;
		[SerializeField] private bool _usable = default;
		[SerializeField] private float _cooldown = default;

		[TextArea(3, 5)]
		[SerializeField] private string _description = default;

		[SerializeField] private int _levelRequirement = default;
		[SerializeField] private Skill[] _extraRequirements = default;

		[SerializeField] private Sprite _icon = default;

		private SkillCategory _category;
		private SkillCollection _collection;
		private SkillTree _tree;

		// GETTERS
		public string Id
		{
			get
			{
				return _id;
			}
		}

		public bool Unlocked
		{
			get
			{
				return _unlocked;
			}
			set
			{
				_unlocked = value;
			}
		}

		public bool Usable
		{
			get
			{
				return _usable;
			}
			set
			{
				_usable = value;
			}
		}

		public float Cooldown
        {
			get
            {
				return _cooldown;
            }
			set
            {
				_cooldown = value;
            }
        }

		public string Description
		{
			get
			{
				return _description;
			}
		}

		public int LevelRequirement
		{
			get
			{
				return _levelRequirement;
			}
		}

		public Skill[] ExtraRequirements
		{
			get
			{
				return _extraRequirements;
			}
		}

		public Sprite Icon
        {
			get
            {
				return _icon;
            }
        }

		public SkillCategory Category
		{
			get
			{
				if (_category == null) _category = transform.parent.parent.GetComponent<SkillCategory>();
				return _category;
			}
		}

		public SkillCollection Collection
		{
			get
			{
				if (_collection == null) _collection = GetComponentInParent<SkillCollection>();
				return _collection;
			}
		}

		public SkillTree Tree
		{
			get
			{
				if (_tree == null) _tree = Category.GetComponentInParent<SkillTree>();
				return _tree;
			}
		}

		public string Uuid
		{
			get
			{
				if (string.IsNullOrEmpty(_uuid))
				{
					//_uuid = System.Guid.NewGuid().ToString();
					_uuid = _id;
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
		/// Visual print out of requirements
		/// </summary>
		/// <returns>The requirements.</returns>
		public string GetRequirements()
		{
			string requirements = "";
			SkillCategory category = transform.parent.parent.GetComponent<SkillCategory>();

			if (_levelRequirement > 0)
				requirements += string.Format("* {0} Skill Lv {1} \n", category.displayName, _levelRequirement);

			foreach (Skill skill in _extraRequirements)
			{
				SkillCollection collection = skill.transform.parent.GetComponent<SkillCollection>();
				requirements += string.Format("* {0} Lv {1} \n", collection.displayName, skill.transform.GetSiblingIndex() + 1);
			}

			return requirements;
		}

		/// <summary>
		/// Loops through all requirements to check if this skill is available for purchase
		/// </summary>
		/// <returns><c>true</c> if this instance is requirements; otherwise, <c>false</c>.</returns>
		public bool IsRequirements()
		{
			if (!Tree.IsParentUnlocked(Collection))
			{
				return false;
			}

			if (Category.SkillLevel < LevelRequirement)
			{
				return false;
			}

			foreach (Skill skill in ExtraRequirements)
			{
				if (!skill.Unlocked)
				{
					return false;
				}
			}

			return true;
		}
	}
}
