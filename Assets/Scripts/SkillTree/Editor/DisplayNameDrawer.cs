using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Globalization;

namespace skilltree
{
	[CustomPropertyDrawer(typeof(DisplayNameAttribute))]
	public class DisplayNameDrawer : PropertyDrawer
	{
		private DisplayNameAttribute _nameAttribute { get { return ((DisplayNameAttribute)attribute); } }

		static string GetHiearchySafeName(string val, string defaultValue)
		{
			if (string.IsNullOrEmpty(val))
			{
				return defaultValue;
			}

			// Capitalize each first letter
			TextInfo txt = new CultureInfo("fr-FR", false).TextInfo;
			val = txt.ToTitleCase(val); // Uppercase letters
			val = val.Replace(" ", ""); // Strip spaces
			val = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 -]").Replace(val, ""); // Strip non-alphanumeric
			return val;

		}

		public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
		{
			EditorGUI.BeginChangeCheck();
			string value = EditorGUI.TextField(position, label, prop.stringValue);
			if (EditorGUI.EndChangeCheck())
			{
				prop.stringValue = value;
				prop.serializedObject.targetObject.name = GetHiearchySafeName(value, _nameAttribute._defaultName);
			}
		}
	}
}
