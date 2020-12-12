using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace skilltree
{
	[CustomEditor(typeof(SkillTree), true)]
	public class SkillTreeEditor : Editor
	{
		private EditorWindow _window;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Edit Skill Tree"))
			{
				_window = EditorWindow.GetWindow<GraphController>();
				_window.Show();
			}

			// Set dirty to save changes
			if (GUI.changed) EditorUtility.SetDirty(target);
		}
	}
}
