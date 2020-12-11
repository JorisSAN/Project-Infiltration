using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCollectionGridItem
{
	private SkillCollection _collection;
	private int _x;
	private int _y;

	public SkillCollection Collection
    {
		get
        {
			return _collection;
        }
		set
        {
			_collection = value;
        }
    }

	public int X
    {
		get
        {
            return _x;
        }
        set
        {
            _x = value;
        }
    }

    public int Y
    {
        get
        {
            return _y;
        }
        set
        {
            _y = value;
        }
    }
}
