using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrowningDetection : MonoBehaviour
{
    public Text sliderValueText;
    public Slider slider;

    public Text valueText;
    public Group swimmers;

    private int drowningValue;

    void Start()
    {
        drowningValue = swimmers.agents.Count;
    }

    void Update()
    {
        drowningValue = swimmers.agents.Count;
    }

    public void OnSliderChanged()
    {
        sliderValueText.text = slider.value.ToString();
    }

    public void SendDrowningValue()
    {
        swimmers.UpdateNbToDrown((int) slider.value);
        swimmers.Drown();
    }

    public void UpdateDrowningValue()
    {
        valueText.text = drowningValue.ToString();
    }
}
