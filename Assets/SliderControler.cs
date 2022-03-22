using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderControler : MonoBehaviour
{
    public Text valueText;
    public Slider slider;

    public void OnSliderChanged()
    {
        valueText.text = slider.value.ToString();
        
    }
}
