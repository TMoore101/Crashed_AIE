using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]

public class GradientUI : MonoBehaviour
{

    [SerializeField] private Gradient _gradient = null;
    [SerializeField] private Image _image = null;
    [SerializeField] private Slider _slider = null;

    private void Update()
    {
        if (_slider.value > 0)
        {
            _image.color = _gradient.Evaluate(_slider.value / _slider.maxValue);
        }
        _image.fillAmount = _slider.value / _slider.maxValue;
    }
}
