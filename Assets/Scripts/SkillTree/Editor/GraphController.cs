using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace skilltree
{
	public class GraphController : EditorWindow
	{
		public static GraphController _current;
		private SkillTree _target;
		private GraphSidebar _sidebar;
		public GraphCamera _camera;

		private GUIStyle _textStyle; // Style used for title in upper left
		private Vector2 _mousePos; // Local screen mouse position
		private Vector2 _mousePosGlobal; // Global mouse position based on camera offsets
		private bool _isTransition; // Are we in transistion mode
		private SkillCollection[] _collect = new SkillCollection[0];

		private float SIDE_BAR_WIDTH = 240f; // Size of the sidebar
		private int _selectIndex = -1; // Currently selected window
		private SkillCollection _selectNode; // Actively selected node for a transition

		private SkillCollection _snapNode; // We need to refresh this node and force snap it
		public bool _forceSnapping = true;

		private void OnEnable()
		{
			titleContent.text = "Skill Tree";

			_textStyle = new GUIStyle();
			_textStyle.fontSize = 20;

			if (_sidebar == null) _sidebar = new GraphSidebar();

			if (_camera == null) _camera = new GraphCamera();

			_current = this;

			UpdateTarget(Selection.activeGameObject);
		}

		[MenuItem("Window/Skill Tree")]
		private static void ShowEditor()
		{
			EditorWindow.GetWindow<GraphController>();
		}

		private void OnSelectionChange()
		{
			UpdateTarget(Selection.activeGameObject);
		}

		private void UpdateTarget(GameObject go)
		{
			if (go != null)
			{
				SkillTree skillTree = go.GetComponent<SkillTree>();
				if (skillTree)
				{
					// Verify inheritance on SkillTree
					if (IsValidInheritance(skillTree))
					{
						// Assign the new skill tree
						_target = skillTree;
						_sidebar._target = _target;
						_camera.Reset();
					}
					else
					{
						Debug.LogError("Invalid inheritance for skill classes. Please verify they inherit from proper base classes before proceeding.");
					}
				}
			}

			Repaint();
		}

		// Checks if the current skill tree classes properly inherit from a base class
		private bool IsValidInheritance(SkillTree target)
		{
			/*
			if (!target.SkillCategory.IsSubclassOf(typeof(SkillCategory)))
			{
				return false;
			}
			else if (!target.SkillCollection.IsSubclassOf(typeof(SkillCollection)))
			{
				return false;
			}
			else if (!target.Skill.IsSubclassOf(typeof(Skill)))
			{
				return false;
			}
			*/
			return true;
		}

		private void OnGUI()
		{
			if (_target == null) return;
			DrawTitle();

			Event e = Event.current;
			_mousePos = e.mousePosition; // Mouse position local to the viewing window
			_mousePosGlobal = _camera.GetMouseGlobal(e.mousePosition); // Mouse position local to the scroll window
			bool clickedNode = false;

			// Poll the current skill collection
			if (_target._currentCategory != null)
			{
				_collect = _target._currentCategory.GetComponentsInChildren<SkillCollection>();
			}
			else
			{
				_collect = new SkillCollection[0];
			}

			// Main content area
			if (_target._currentCategory != null)
			{
				// Clicked outside of sidebar and scrollbar GUI
				if (_mousePos.x < position.width - SIDE_BAR_WIDTH - GUI.skin.verticalScrollbar.fixedWidth &&
					_mousePos.y < position.height - GUI.skin.horizontalScrollbar.fixedHeight)
				{

					// Context menu
					if (e.button == 1 && !_isTransition) // RIGHT CLIC
					{
						if (e.type == EventType.MouseDown)
						{
							for (int i = 0; i < _collect.Length; i++)
							{
								if (_collect[i].WindowRect.Contains(_mousePosGlobal))
								{
									_selectIndex = i;
									clickedNode = true;
									break;
								}
							}

							if (clickedNode)
							{
								GenericMenu menu = new GenericMenu();
								menu.AddItem(new GUIContent("Add Skill"), false, AddSkill, _collect[_selectIndex]);
								menu.AddItem(new GUIContent("Add Child Transition"), false, BeginSkillGroupTransition);
								menu.AddSeparator("");
								menu.AddItem(new GUIContent("Delete Skill Group"), false, DeleteSkillGroup);
								menu.AddSeparator("");

								foreach (SkillCollection child in _collect[_selectIndex].ChildSkills)
								{
									menu.AddItem(new GUIContent("Delete Transition " + child.displayName), false, DeleteSkillGroupTransition, new TransitionParentChild
									{
										Parent = _collect[_selectIndex],
										Child = child
									});
								}

								menu.ShowAsContext();
								e.Use();
							}
							else
							{
								GenericMenu menu = new GenericMenu();
								menu.AddItem(new GUIContent("Add Skill Group"), false, CreateSkillGroup);
								menu.ShowAsContext();
								e.Use();
							}
						}
					}
					else if (e.button == 0) // LEFT CLIC
					{
						if (e.type == EventType.MouseDown)
						{
							for (int i = 0; i < _collect.Length; i++)
							{
								if (_collect[i].WindowRect.Contains(_mousePosGlobal))
								{
									_selectIndex = i;
									clickedNode = true;
									_snapNode = _collect[i];
									break;
								}
							}

							if (_isTransition)
							{
								_selectNode.ChildSkills.Remove(_collect[_selectIndex]);
								_selectNode.ChildSkills.Add(_collect[_selectIndex]);
								EndSkillGroupTransition();
							}
							else
							{
								if (clickedNode)
								{
									Selection.activeGameObject = _target._currentCategory.transform.GetChild(_selectIndex).gameObject;
								}
								else
								{
									_camera.BeginMove(_mousePos);
								}
							}
						}
						else if (e.type == EventType.MouseUp && _snapNode != null)
						{
							if (_forceSnapping) SnapNodeToGrid(_snapNode);
							_snapNode = null;
						}
					}
				}

				_camera._offset = GUI.BeginScrollView(new Rect(0f, 0f, position.width - SIDE_BAR_WIDTH, position.height), _camera._offset, new Rect(_camera.VIEWPORT_SIZE / -2f, _camera.VIEWPORT_SIZE / -2f, _camera.VIEWPORT_SIZE, _camera.VIEWPORT_SIZE));

				BeginWindows();
				for (int i = 0; i < _collect.Length; i++)
				{
					_collect[i].WindowRect = GUI.Window(i, _collect[i].WindowRect, DrawNodeWindow, _collect[i].displayName);

					foreach (SkillCollection child in _collect[i].ChildSkills)
					{
						DrawLineBottomToTop(_collect[i].WindowRect, child.WindowRect);
					}
				}
				EndWindows();

				GUI.EndScrollView(); // Camera scroll for windows
			}

			// Draw a transistion if in transistion mode
			if (_isTransition && _selectNode != null)
			{
				Vector2 globalOffset = _camera.GetOffsetGlobal();
				Rect beginRect = _selectNode.WindowRect;
				beginRect.x -= globalOffset.x;
				beginRect.y -= globalOffset.y;
				Rect mouseRect = new Rect(_mousePos.x, _mousePos.y, 10f, 10f);

				DrawNodeCurve(beginRect, mouseRect);

				Repaint();
			}

			_sidebar.DrawSidebar(new Rect(position.width - SIDE_BAR_WIDTH, 0, SIDE_BAR_WIDTH, position.height), 10f, Color.gray);

			// Always stop the camera on mouse up (even if not in the window)
			if (Event.current.rawType == EventType.MouseUp)
			{
				_camera.EndMove();
			}

			// Poll and update the viewport if the camera has moved
			if (_camera.PollCamera(_mousePos))
			{
				Repaint();
			}
		}

		public void SnapNodeToGrid(SkillCollection node)
		{
			Vector2 snapRatio = _target.GridCellSize;
			float x = Mathf.Round(node.WindowRect.x / snapRatio.x) * snapRatio.x;
			float y = Mathf.Round(node.WindowRect.y / snapRatio.y) * snapRatio.y;

			node.SetWindowRectPos(x, y);
		}

		public void SnapAllNodesToGrid()
		{
			foreach (SkillCollection col in _collect)
			{
				SnapNodeToGrid(col);
			}
		}

		private void BeginSkillGroupTransition()
		{
			_isTransition = true;
			SkillCollection[] collect = _target._currentCategory.GetComponentsInChildren<SkillCollection>();
			_selectNode = collect[_selectIndex];
		}

		private void DeleteSkillGroupTransition(object obj)
		{
			TransitionParentChild parentChild = obj as TransitionParentChild;
			parentChild.Parent.ChildSkills.Remove(parentChild.Child);
		}

		private void EndSkillGroupTransition()
		{
			_isTransition = false;
			_selectNode = null;
		}

		public static void DrawLineBottomToTop(Rect start, Rect end)
		{
			Vector3 startPos = new Vector3(start.x + (start.width / 2f), start.y + start.height, 0f);
			Vector3 endPos = new Vector3(end.x + (end.width / 2f), end.y, 0f);
			Vector3 startTan = startPos + Vector3.up * 50f;
			Vector3 endTan = endPos - Vector3.up * 50f;
			Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 3f);
		}

		public static void DrawNodeCurve(Rect start, Rect end)
		{
			Vector3 startPos = new Vector3(start.x + start.width / 2, start.y + start.height / 2, 0);
			Vector3 endPos = new Vector3(end.x + end.width / 2, end.y + end.height / 2, 0);
			Vector3 startTan = startPos + Vector3.down * 50;
			Vector3 endTan = endPos + Vector3.up * 50;
			Color shadowCol = new Color(0, 0, 0, 0.06f);

			for (int i = 0; i < 3; i++)
			{
				Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (i + 1) * 5);
			}

			Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 1);
		}

		private void DrawNodeWindow(int id)
		{
			Skill deleteSkill = null;
			foreach (Skill skill in _collect[id].GetComponentsInChildren<Skill>())
			{
				GUILayout.BeginHorizontal();
				skill.Unlocked = GUILayout.Toggle(skill.Unlocked, "");

				if (GUILayout.Button(skill.displayName, GUILayout.Width(100f)))
				{
					Selection.activeGameObject = skill.gameObject;
				}

				if (GUILayout.Button("UP"))
				{
					skill.transform.SetSiblingIndex(skill.transform.GetSiblingIndex() - 1);
				}

				if (GUILayout.Button("DN"))
				{
					skill.transform.SetSiblingIndex(skill.transform.GetSiblingIndex() + 1);
				}

				if (GUILayout.Button("X"))
				{
					if (EditorUtility.DisplayDialog("Delete Skill?",
													"Are you sure you want to delete this skill? This cannot be undone.",
													"Delete Skill",
													"Cancel"))
					{
						deleteSkill = skill;
					}
				}

				GUILayout.EndHorizontal();
			}

			if (deleteSkill != null) DestroyImmediate(deleteSkill.gameObject);

			GUI.DragWindow();
		}

		private void CreateSkillGroup()
		{
			GameObject go = new GameObject();
			go.name = "SkillCollection";
			SkillCollection skill = go.AddComponent(_target.SkillCollection) as SkillCollection;
			skill.WindowRect = new Rect(_mousePosGlobal.x, _mousePosGlobal.y, 220, 150);
			go.transform.SetParent(_target._currentCategory.transform);

			AddSkill(skill);
		}

		private void DeleteSkillGroup()
		{
			if (EditorUtility.DisplayDialog("Delete Skill Collection?",
											"Are you sure you want to delete this skill collection? It will delete this collection plus all skills it contains.",
											"Delete Skill Collection",
											"Cancel"))
			{
				SkillCollection[] collect = _target._currentCategory.GetComponentsInChildren<SkillCollection>();
				SkillCollection t = collect[_selectIndex];

				// Clean out all references to our skill collection
				foreach (SkillCollection node in collect)
				{
					node.ChildSkills.Remove(t);
				}

				DestroyImmediate(t.gameObject);
			}
		}

		private void AddSkill(object obj)
		{
			SkillCollection col = obj as SkillCollection;
			AddSkill(col);
		}

		private void AddSkill(SkillCollection col)
		{
			GameObject go = new GameObject();
			go.name = "Skill";
			Skill s = go.AddComponent(_target.Skill) as Skill;
			//s.Uuid = Guid.NewGuid().ToString();
			go.transform.SetParent(col.transform);
		}

		void DrawTitle()
		{
			if (_target != null)
			{
				string title = _target.displayName;
				if (_target._currentCategory != null) title += ": " + _target._currentCategory.displayName;

				GUI.Label(new Rect(10, 10, 100, 20), title, _textStyle);
			}
		}
	}
}
