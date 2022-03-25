using UnityEngine;
using UnityEngine.UI;

public class DrowningDetection : MonoBehaviour
{
    [SerializeField] private Text _sliderValue;
    [SerializeField] private Slider _slider;
    [SerializeField] private Text _totalDrownSwimmers;
    [SerializeField] private Group _swimmers;

    public void OnSliderChanged() => _sliderValue.text = _slider.value.ToString();

    public void SendDrowningValue()
    {
        _swimmers.UpdateNbToDrown((int) _slider.value);
        _swimmers.Drown();
        UpdateDrowningValue();
    }

    private void UpdateDrowningValue() => _totalDrownSwimmers.text = _swimmers.agents.Count.ToString();
}