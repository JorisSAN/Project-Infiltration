using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace skilltree
{
	public class SkillMenu : MonoBehaviour
	{
		private Dictionary<SkillCollection, SkillNode> _nodeRef;
		private List<SkillNode> _skillNodes;

		public SkillTree _skillTree;

		[Header("Header")]
		[SerializeField] private Transform _categoryContainer = default;
		[SerializeField] private ButtonSkillCategory _categoryButtonPrefab = default;
		[SerializeField] private Text _skillOutput = default;
		[SerializeField] private Text _categoryName = default;

		[Header("Nodes")]
		[SerializeField] private Transform _nodeContainer = default;
		[SerializeField] private RectTransform _nodeInnerContainer = default;
		//[SerializeField] private GameObject _nodeRowPrefab = default;
		[SerializeField] private SkillNode _nodePrefab = default;
		[SerializeField] private Color _colorUnlock = default;
		[SerializeField] private Color _colorPurchase = default;
		[SerializeField] private Color _colorLock = default;

		[SerializeField] private Vector2 _cellSize = default;

		[Header("Node Lines")]
		[SerializeField] private Transform _lineContainer = default;
		[SerializeField] private Line _linePrefab = default;
		[SerializeField] private Color _lineColor = default;

		[Header("Context Sidebar")]
		[SerializeField] private RectTransform _sidebarContainer = default;
		[SerializeField] private Text _sidebarTitle = default;
		[SerializeField] private Text _sidebarBody = default;
		[SerializeField] private Text _sidebarRequirements = default;
		[SerializeField] private Text _sidebarPurchasedMessage = default;
		[SerializeField] private Button _sidebarPurchase = default;
		[SerializeField] private Image _sidebarSkillIcon = default;

		private void Start()
		{
			// Clear out test categories
			foreach (Transform child in _categoryContainer)
			{
				Destroy(child.gameObject);
			}

			// Populate categories
			SkillCategory[] skillCategories = _skillTree.GetCategories();
			foreach (SkillCategory category in skillCategories)
			{
				ButtonSkillCategory buttonSkillCat = Instantiate<ButtonSkillCategory>(_categoryButtonPrefab);
				buttonSkillCat.transform.SetParent(_categoryContainer);
				buttonSkillCat.transform.localScale = Vector3.one;

				//buttonSkillCat.ResetPosition();
				buttonSkillCat.ChangeContent(category.displayName);

				// Dump in a tmp variable to force capture the variable by the event
				SkillCategory tmpCat = category;
				buttonSkillCat.RemoveAllListeners();
				buttonSkillCat.AddListener(() =>
				{
					ShowCategory(tmpCat);
				});
			}

			if (skillCategories.Length > 0)
			{
				ShowCategory(skillCategories[0]);
			}
		}

		public void ShowCategory(SkillCategory category)
		{
			_skillNodes = new List<SkillNode>();
			_nodeRef = new Dictionary<SkillCollection, SkillNode>();
			_categoryName.text = string.Format("{0}: Level {1}", category.displayName, category.SkillLevel);
			ClearDetails();

			CreateGrid(category, _cellSize);

			StartCoroutine(ConnectNodes());
		}

		private void CreateGrid(SkillCategory category, Vector2 cellSize)
		{
			// Clean up pre-existing data
			foreach (Transform child in _nodeContainer)
			{
				Destroy(child.gameObject);
			}

			foreach (Transform child in _lineContainer)
			{
				Destroy(child.gameObject);
			}

			SkillCollectionGrid grid = category.GetComponentInParent<SkillTree>().GetGrid(category);

			// Generate container with width and height based on cellSize
			_nodeInnerContainer.sizeDelta = new Vector2(grid.Width * cellSize.x, grid.Height * cellSize.y);

			// Adjust the container position based on padding, resulting in perfectly aligned grid items
			RectTransform nodeRect = _nodePrefab.RectTransform;
			Vector2 cellPadding = new Vector2((cellSize.x - nodeRect.sizeDelta.x) / 2f, (cellSize.y - nodeRect.sizeDelta.y) / 2f);

			_nodeRef = new Dictionary<SkillCollection, SkillNode>();

			// Place all grid items
			foreach (SkillCollectionGridItem gridItem in grid.GetAllCollections())
			{
				SkillNode node = Instantiate<SkillNode>(_nodePrefab);
				node.transform.SetParent(_nodeContainer);
				node.transform.localScale = Vector3.one;
				//node.ResetPosition();
				node.ChangeContent(gridItem.Collection.displayName);
				node.ChangeRectTransformAnchoredPosition(new Vector2((gridItem.X * cellSize.x) + cellPadding.x, (gridItem.Y * cellSize.y * -1f) - cellPadding.y));
				node.AddIcon(gridItem.Collection.Skill.Icon);

				node._menu = this;
				node._skillCollection = gridItem.Collection;
				_skillNodes.Add(node);

				_nodeRef.Add(gridItem.Collection, node);
			}
		}

		private void UpdateNodes()
		{
			foreach (SkillNode node in _skillNodes)
			{
				node.SetStatus(NodeStatus.Locked, _colorLock);

				if (node._skillCollection.Skill.Unlocked)
				{
					node.SetStatus(NodeStatus.Unlocked, _colorUnlock); // Fully purchased
				}
				else if (_skillTree.SkillPoints > 0 && node._skillCollection.Skill.IsRequirements())
				{
					node.SetStatus(NodeStatus.Purchasable, _colorPurchase); // Avaialable for purchase
				}
			}
		}

		private IEnumerator ConnectNodes()
		{
			// We have to skip a frame so the Unity GUI can recalculate all the positioning
			yield return null;

			foreach (SkillNode node in _nodeContainer.GetComponentsInChildren<SkillNode>())
			{
				foreach (SkillCollection child in node._skillCollection.ChildSkills)
				{
					// @NOTE We must translate a center point on the node into a transform position for accurary of the line
					Vector3 lineStart = node.transform.GetChild(0).position;
					Vector3 lineEnd = _nodeRef[child].transform.GetChild(0).position;
					DrawLine(_lineContainer, lineStart, lineEnd, _lineColor);
				}
			}

			Repaint();
		}

		private void DrawLine(Transform container, Vector3 start, Vector3 end, Color color)
		{
			Line line = Instantiate<Line>(_linePrefab);
			line.ChangeImageColor(color);
			//line.ResetPosition();

			// Adjust the layering so it appears underneath
			line.transform.SetParent(container);
			line.transform.localScale = Vector3.one;
			line.transform.SetSiblingIndex(0);

			// Adjust height to proper sizing
			RectTransform rectTrans = line.RectTransform;
			Rect rect = rectTrans.rect;
			rect.height = Vector3.Distance(start, end);
			rectTrans.sizeDelta = new Vector2(rect.width, rect.height);

			// Adjust rotation and placement
			line.transform.rotation = Rotate2D(start, end);
			line.transform.position = start;
		}

		public Quaternion Rotate2D(Vector3 start, Vector3 end)
		{
			Vector3 diff = start - end;
			diff.Normalize();
			float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
			return Quaternion.Euler(0f, 0f, rot_z - 90f);
		}

		/// <summary>
		/// Recursively adds rows.
		/// </summary>
		/// <param name="rows">Rows.</param>
		/// <param name="history">History of all added items so we don't accidentally add two of the same thing</param>
		private void RecursiveRowAdd(List<List<SkillCollection>> rows, Dictionary<SkillCollection, bool> history)
		{
			List<SkillCollection> row = new List<SkillCollection>();
			foreach (SkillCollection collection in rows[rows.Count - 1])
			{
				foreach (SkillCollection child in collection.ChildSkills)
				{
					if (!row.Contains(child) && !history.ContainsKey(child))
					{
						row.Add(child);
						history[child] = true;
					}
				}
			}

			if (row.Count > 0)
			{
				rows.Add(row);
				RecursiveRowAdd(rows, history);
			}
		}

		public void ShowNodeDetails(SkillNode node)
		{
			SkillCollection skillCollection = node._skillCollection;
			NodeStatus status = node.GetStatus();

			_sidebarTitle.text = string.Format("{0}: Lvl {1}", skillCollection.displayName, skillCollection.SkillIndex + 1);
			_sidebarBody.text = skillCollection.Skill.Description;

			if (skillCollection.Skill.Icon != null)
            {
				_sidebarSkillIcon.sprite = skillCollection.Skill.Icon;
				_sidebarSkillIcon.gameObject.SetActive(true);
			}
			else
            {
				_sidebarSkillIcon.gameObject.SetActive(false);
			}

			_sidebarSkillIcon.preserveAspect = true;
			

			string requirements = skillCollection.Skill.GetRequirements();
			if (string.IsNullOrEmpty(requirements))
			{
				_sidebarRequirements.gameObject.SetActive(false);
			}
			else
			{
				_sidebarRequirements.text = "<b>Requirements:</b> \n" + skillCollection.Skill.GetRequirements();
				_sidebarRequirements.gameObject.SetActive(true);
			}

			if (status == NodeStatus.Purchasable)
			{
				_sidebarPurchasedMessage.gameObject.SetActive(false);
				_sidebarPurchase.gameObject.SetActive(true);
				_sidebarPurchase.onClick.RemoveAllListeners();
				_sidebarPurchase.onClick.AddListener(() =>
				{
					skillCollection.UnlockSkill();
					UpdateNodes();
					ShowNodeDetails(node);
					UpdateSkillPoints();
					UpdateSkillIcon(node);
				});
			}
			else if (status == NodeStatus.Unlocked)
			{
				_sidebarPurchasedMessage.gameObject.SetActive(true);
				_sidebarPurchase.gameObject.SetActive(false);
			}
			else
			{
				_sidebarPurchasedMessage.gameObject.SetActive(false);
				_sidebarPurchase.gameObject.SetActive(false);
			}

			_sidebarContainer.gameObject.SetActive(true);
		}

		private void ClearDetails()
		{
			_sidebarContainer.gameObject.SetActive(false);
		}

		private void UpdateSkillPoints()
		{
			_skillOutput.text = "Skill Points: " + _skillTree.SkillPoints;
		}

		private void UpdateSkillIcon(SkillNode node)
        {
			node.AddIcon(node._skillCollection.Skill.Icon);
        }

		private void Repaint()
		{
			UpdateSkillPoints();
			UpdateNodes();
		}
	}
}
