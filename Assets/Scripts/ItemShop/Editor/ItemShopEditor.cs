using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace itemshop.editor
{
	[CustomEditor(typeof(ItemShop), true)]
	public class ItemShopEditor : Editor
	{
		private EditorWindow _window;

		public override void OnInspectorGUI()
		{
			DrawDefaultInspector();

			if (GUILayout.Button("Edit Item Shop"))
			{
				_window = EditorWindow.GetWindow<GraphControllerItemShop>();
				_window.Show();
			}

			// Set dirty to save changes
			if (GUI.changed) EditorUtility.SetDirty(target);
		}
	}
}
