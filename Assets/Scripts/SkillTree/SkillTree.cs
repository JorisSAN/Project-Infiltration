using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace skilltree
{
	public class SkillTree : MonoBehaviour
	{
		[DisplayName("SkillTreeData")] public string displayName = "Skill Tree";

		[TextArea(3, 5)]
		[SerializeField] private string _description;

		// Number of available skill points
		[SerializeField] private int _skillPoints = 0;

		[SerializeField] private Vector2 _gridCellSize = new Vector2(250f, 200f);

		[HideInInspector] public SkillCategory _currentCategory;

		/* TEMPORAIRE */
		public System.Type SkillCategory { get { return typeof(SkillCategory); } }
		public System.Type SkillCollection { get { return typeof(SkillCollection); } }
		public System.Type Skill { get { return typeof(Skill); } }
		/* TEMPORAIRE */

		private Dictionary<SkillCollection, List<SkillCollection>> _childParents;

		private Dictionary<string, SkillCategory> _categoryLib = new Dictionary<string, SkillCategory>();
		private Dictionary<string, SkillCollection> _skillCollectionLib = new Dictionary<string, SkillCollection>();
		private Dictionary<string, Skill> _skillLib = new Dictionary<string, Skill>();

		private Dictionary<string, SkillCategory> _categoryUuidLib = new Dictionary<string, SkillCategory>();
		private Dictionary<string, SkillCollection> _collectionUuidLib = new Dictionary<string, SkillCollection>();
		private Dictionary<string, Skill> _skillUuidLib = new Dictionary<string, Skill>();

		private bool _initialized = false;

		public int SkillPoints
		{
			get
			{
				return _skillPoints;
			}
			set
			{
				_skillPoints = value;
			}
		}

		public Vector2 GridCellSize
		{
			get
			{
				return _gridCellSize;
			}
		}

		public bool Initialized
        {
			get
            {
				return _initialized;
            }
        }

        // METHODS

        public void Awake()
        {
            if (!_initialized)
            {
				Initialize();
			}
        }

        public void Initialize()
        {
			foreach (SkillCategory cat in GetCategories())
			{
				if (!string.IsNullOrEmpty(cat.Id)) _categoryLib[cat.Id] = cat;
				_categoryUuidLib[cat.Uuid] = cat;
			}

			foreach (SkillCollection col in GetSkillCollections())
			{
				if (!string.IsNullOrEmpty(col.Id)) _skillCollectionLib[col.Id] = col;
				_collectionUuidLib[col.Uuid] = col;
			}

			foreach (Skill skill in GetSkills())
			{
				if (!string.IsNullOrEmpty(skill.Id)) _skillLib[skill.Id] = skill;
				_skillUuidLib[skill.Uuid] = skill;
			}

			_childParents = GetParentData();

			_initialized = true;
		}

		/// <summary>
		/// Loops through all collections to determine parent elements. Warning, quite expensive.
		/// </summary>
		/// <returns>The parent data.</returns>
		private Dictionary<SkillCollection, List<SkillCollection>> GetParentData()
		{
			Dictionary<SkillCollection, List<SkillCollection>> childParents = new Dictionary<SkillCollection, List<SkillCollection>>();

			foreach (SkillCategory category in GetComponentsInChildren<SkillCategory>())
			{
				foreach (SkillCollection parent in category.GetComponentsInChildren<SkillCollection>())
				{
					foreach (SkillCollection child in parent.ChildSkills)
					{
						if (!childParents.ContainsKey(child))
						{
							childParents[child] = new List<SkillCollection>();
						}

						childParents[child].Add(parent);
					}
				}
			}

			return childParents;
		}

		/// <summary>
		/// Retrieve all active categories. Warning, expensive
		/// </summary>
		/// <returns>The categories.</returns>
		public SkillCategory[] GetCategories()
		{
			return GetComponentsInChildren<SkillCategory>();
		}

		/// <summary>
		/// Returns all skill collecitons. Warning, expensive
		/// </summary>
		/// <returns>The skill collections.</returns>
		public SkillCollection[] GetSkillCollections()
		{
			return GetComponentsInChildren<SkillCollection>();
		}

		/// <summary>
		/// Returns all skils. Warning expensive
		/// </summary>
		/// <returns>The skills.</returns>
		public Skill[] GetSkills()
		{
			return GetComponentsInChildren<Skill>();
		}

		/// <summary>
		/// Returns a category from the user assigned ID
		/// </summary>
		/// <returns>The category.</returns>
		/// <param name="categoryId">Category identifier.</param>
		public SkillCategory GetCategory(string categoryId)
		{
			return _categoryLib[categoryId];
		}

		/// <summary>
		/// Returns a collection from the user assigned ID
		/// </summary>
		/// <returns>The collection.</returns>
		/// <param name="collectionId">Collection identifier.</param>
		public SkillCollection GetCollection(string collectionId)
		{
			return _skillCollectionLib[collectionId];
		}

		/// <summary>
		/// Returns a skill from the user assigned ID
		/// </summary>
		/// <returns>The skill.</returns>
		/// <param name="skillId">Skill identifier.</param>
		public Skill GetSkill(string skillId)
		{
			return _skillLib[skillId];
		}

		/// <summary>
		/// Return the current skill from a collection
		/// </summary>
		/// <returns>The skill from category.</returns>
		/// <param name="collectionId">Collection identifier.</param>
		public Skill GetSkillFromCategory(string collectionId)
		{
			return GetCollection(collectionId).Skill;
		}

		/// <summary>
		/// Declare the current level of a specific category
		/// </summary>
		/// <param name="categoryId">Category identifier.</param>
		/// <param name="lvl">Lv.</param>
		public void SetCategoryLvl(string categoryId, int lvl)
		{
			SkillCategory cat = GetCategory(categoryId);
			cat.SkillLevel = lvl;
		}

		/// <summary>
		/// Add points to actual skillPoints
		/// </summary>
		/// <param name="pointsToAdd"></param>
		public void AddSkillPoints(int pointsToAdd)
		{
			_skillPoints += pointsToAdd;
		}

		/// <summary>
		/// Remove points to actual skillPoints
		/// </summary>
		/// <param name="pointsToRemove"></param>
		public void RemoveSkillPoints(int pointsToRemove)
		{
			_skillPoints -= pointsToRemove;
			if (_skillPoints < 0)
			{
				_skillPoints = 0;
			}
		}

		/// <summary>
		/// Check if a specific skill has been unlocked
		/// </summary>
		/// <returns><c>true</c> if this instance is unlocked the specified skillId; otherwise, <c>false</c>.</returns>
		/// <param name="skillId">Skill identifier.</param>
		public bool IsUnlocked(string skillId)
		{
			return GetSkill(skillId).Unlocked;
		}

		/// <summary>
		/// Determines if a parent collection is properly unlocked
		/// </summary>
		/// <returns><c>true</c> if this instance is parent unlocked the specified collection; otherwise, <c>false</c>.</returns>
		/// <param name="collection">Collection.</param>
		public virtual bool IsParentUnlocked(SkillCollection collection)
		{
			// Check to see if it has no parent
			if (!_childParents.ContainsKey(collection))
			{
				return true;
			}

			// Check all parents to see if any are unlocked
			foreach (SkillCollection parent in _childParents[collection])
			{
				if (parent.GetSkill(0).Unlocked) return true;
			}

			// No matches
			return false;
		}

		public SkillCollectionGrid GetGrid(SkillCategory category)
		{
			SkillCollection[] collect = category.GetComponentsInChildren<SkillCollection>();
			Vector2 min = new Vector2(Mathf.Infinity, Mathf.Infinity);
			Vector2 max = new Vector2(Mathf.NegativeInfinity, Mathf.NegativeInfinity);

			// Find the min x, min y, max x, and max y
			foreach (SkillCollection col in collect)
			{
				if (col.WindowRect.x < min.x) min.x = col.WindowRect.x;
				if (col.WindowRect.x > max.x) max.x = col.WindowRect.x;
				if (col.WindowRect.y < min.y) min.y = col.WindowRect.y;
				if (col.WindowRect.y > max.y) max.y = col.WindowRect.y;
			}

			int x, y;
			int width = Mathf.CeilToInt(Mathf.Abs(min.x - max.x) / GridCellSize.x) + 1;
			int height = Mathf.CeilToInt(Mathf.Abs(min.y - max.y) / GridCellSize.y) + 1;
			SkillCollection[,] grid = new SkillCollection[width, height];
			foreach (SkillCollection col in collect)
			{
				x = Mathf.RoundToInt((col.WindowRect.x - min.x) / GridCellSize.x);
				y = Mathf.RoundToInt((col.WindowRect.y - min.y) / GridCellSize.y);
				grid[x, y] = col;
			}

			return new SkillCollectionGrid(grid);
		}

		/* SAVE */

		/// <summary>
		/// Returns a snapshot of this skill tree's current state
		/// </summary>
		/// <returns>The snapshot.</returns>
		virtual public SaveSkillTree GetSnapshot()
		{
			List<SaveSkill> skills = new List<SaveSkill>();
			List<SaveSkillCollection> skillCollections = new List<SaveSkillCollection>();
			List<SaveSkillCategory> skillCategories = new List<SaveSkillCategory>();

			foreach (Skill s in GetSkills())
			{
				string skillName = "";
				if (s.Icon != null)
				{
					skillName = s.Icon.name;

				}
				skills.Add(new SaveSkill
				{
					_uuid = s.Uuid,
					_unlocked = s.Unlocked,
					_usable = s.Usable,
					_icon = skillName,
					_cooldown = s.Cooldown
				});
			}

			foreach (SkillCollection s in GetSkillCollections())
			{
				skillCollections.Add(new SaveSkillCollection
				{
					_uuid = s.Uuid,
					_skillIndex = s.SkillIndex
				});
			}

			foreach (SkillCategory s in GetCategories())
			{
				skillCategories.Add(new SaveSkillCategory
				{
					_uuid = s.Uuid,
					_skillLevel = s.SkillLevel
				});
			}

			return new SaveSkillTree
			{
				_skillPoints = this._skillPoints,
				_skills = skills,
				_collections = skillCollections,
				_categories = skillCategories
			};
		}

		/// <summary>
		/// Restores a snapshot and overwrites the current skill tree values with it
		/// </summary>
		/// <param name="snapshot">Snapshot.</param>
		virtual public void LoadSnapshot(SaveSkillTree snapshot)
		{
			Initialize();

			this._skillPoints = snapshot._skillPoints;

			foreach (SaveSkill s in snapshot._skills)
			{
				_skillLib[s._uuid].Unlocked = s._unlocked;
			}

			foreach (SaveSkillCollection c in snapshot._collections)
			{
				_skillCollectionLib[c._uuid].SkillIndex = c._skillIndex;
			}

			foreach (SaveSkillCategory c in snapshot._categories)
			{
				_categoryLib[c._uuid].SkillLevel = c._skillLevel;
			}
		}

		/* SAVE */
	}
}
