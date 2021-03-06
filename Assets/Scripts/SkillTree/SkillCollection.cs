﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace skilltree
{
	public class SkillCollection : MonoBehaviour
	{
		[DisplayName("SkillCollection")] public string displayName = "Skill Collection";

		[SerializeField] private string _id = default;
		[SerializeField] private string _uuid = default;

		[TextArea(3, 5)]
		[SerializeField] private string _notes = default;

		[SerializeField] private List<SkillCollection> _childSkills = new List<SkillCollection>();
		[SerializeField] private Rect _windowRect = default;

		private int _currentSkill;

		// GETTERS

		public string Id
		{
			get
			{
				return _id;
			}
		}

		public int SkillIndex
		{
			get
			{
				return _currentSkill;
			}

			set
			{
				if (value >= 0 && value < SkillCount)
				{
					_currentSkill = value;
				}
			}
		}

		public List<SkillCollection> ChildSkills
		{
			get
			{
				return _childSkills;
			}
		}

		public Rect WindowRect
		{
			get
			{
				return _windowRect;
			}
			set
			{
				_windowRect = value;
			}
		}

		// How many skills does this collection contain
		public int SkillCount
		{
			get
			{
				return transform.childCount;
			}
		}

		// Returns the current active skill
		public Skill Skill
		{
			get
			{
				return transform.GetChild(_currentSkill).GetComponent<Skill>();
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
		/// Set the x and y coords of the windows rect
		/// </summary>
		/// <param name="x">coord x</param>
		/// <param name="y">coord y</param>
		public void SetWindowRectPos(float x, float y)
		{
			_windowRect.x = x;
			_windowRect.y = y;
		}

		/// <summary>
		/// Get the skill at a specific index
		/// </summary>
		/// <returns>The skill.</returns>
		/// <param name="index">Index.</param>
		public Skill GetSkill(int index)
		{
			return transform.GetChild(index).GetComponent<Skill>();
		}

		/// <summary>
		/// Add a skill collection to the skillChilds list
		/// </summary>
		/// <param name="skillCol">Skill collection</param>
		public void AddSkillCollection(SkillCollection skillCol)
		{
			_childSkills.Add(skillCol);

		}

		/// <summary>
		/// Unlock the currently active skill and set the pointer to the next unlocked if available
		/// </summary>
		public void UnlockSkill()
		{
			SkillTree skillTree = transform.parent.parent.GetComponent<SkillTree>();

			if (skillTree.SkillPoints <= 0) return;
			//skillTree.SkillPoints -= 1;
			skillTree.RemoveSkillPoints(1);

			Skill.Unlocked = true;
			SkillIndex += 1;
		}
	}
}
