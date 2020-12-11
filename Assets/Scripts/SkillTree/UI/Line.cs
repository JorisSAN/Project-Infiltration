using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Line : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform = default;
    [SerializeField] private Image _image = default;

    public RectTransform RectTransform
    {
        get
        {
            return _rectTransform;
        }
    }

    public void ChangeImageColor(Color color)
    {
        _image.color = color;
    }
}
