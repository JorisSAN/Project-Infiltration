﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}