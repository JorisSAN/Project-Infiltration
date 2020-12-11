using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayNameAttribute : PropertyAttribute
{
	public readonly string _defaultName;

	public DisplayNameAttribute(string defaultName)
	{
		this._defaultName = defaultName;
	}
}