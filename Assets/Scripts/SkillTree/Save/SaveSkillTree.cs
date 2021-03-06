﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace skilltree
{
	[Serializable]
	public class SaveSkillTree
	{
		public int _skillPoints;
		public List<SaveSkill> _skills;
		public List<SaveSkillCategory> _categories;
		public List<SaveSkillCollection> _collections;
	}
}
