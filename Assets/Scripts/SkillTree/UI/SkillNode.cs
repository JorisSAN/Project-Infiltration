using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace skilltree
{
	public class SkillNode : MonoBehaviour
	{
		private Button _button;

		[HideInInspector] public SkillMenu _menu;

		public SkillCollection _skillCollection;

		private NodeStatus status;

		public List<SkillNode> _parents = new List<SkillNode>();
		public List<SkillNode> _children = new List<SkillNode>();

		[SerializeField] private RectTransform _rectTransform = default;
		[SerializeField] private Text _content = default;
		[SerializeField] private Image _image = default;

		private Sprite _icon;

		public RectTransform RectTransform
		{
			get
			{
				return _rectTransform;
			}
		}

		void Awake()
		{
			_button = GetComponent<Button>();
		}

		public void ShowDetails()
		{
			_menu.ShowNodeDetails(this);
		}

		public void SetStatus(NodeStatus status, Color color)
		{
			this.status = status;

			ColorBlock colorBlock = _button.colors;
			colorBlock.normalColor = color;
			colorBlock.highlightedColor = color;

			_button.colors = colorBlock;
		}

		public NodeStatus GetStatus()
		{
			return status;
		}

		public void ChangeRectTransformAnchoredPosition(Vector2 newAnchoredPosition)
		{
			_rectTransform.anchoredPosition = newAnchoredPosition;
		}

		public void ChangeContent(string newContent)
		{
			_content.text = newContent;
		}

		public void ResetPosition()
		{
			float actualLocalX = gameObject.transform.localPosition.x;
			float actualLocalY = gameObject.transform.localPosition.y;
			gameObject.transform.localPosition = new Vector3(actualLocalX, actualLocalY, 0);
		}

		public void AddIcon(Sprite icon)
        {
			//_button.image.sprite = icon;
			_icon = icon;
			_image.sprite = _icon;

			if (_icon != null)
            {
				HideText();
            }
			else
            {
				ShowText();
            }
        }

		public void ShowText()
        {
			_content.gameObject.SetActive(true);
        }

		public void HideText()
		{
			_content.gameObject.SetActive(false);
		}
	}
}
