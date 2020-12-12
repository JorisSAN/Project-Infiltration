using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace skilltree
{
	public class DisplayNameAttribute : PropertyAttribute
	{
		public readonly string _defaultName;

		public DisplayNameAttribute(string defaultName)
		{
			this._defaultName = defaultName;
		}
	}
}