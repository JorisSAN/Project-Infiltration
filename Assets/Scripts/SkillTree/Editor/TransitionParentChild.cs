using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to represent a parent to child relationship for transitions in a single class
/// </summary>
public class TransitionParentChild
{
	private SkillCollection _parent;
	private SkillCollection _child;

	public SkillCollection Parent
    {
		get
        {
            return _parent;
        }
        set
        {
            _parent = value;
        }
    }

    public SkillCollection Child
    {
        get
        {
            return _child;
        }
        set
        {
            _child = value;
        }
    }
}
