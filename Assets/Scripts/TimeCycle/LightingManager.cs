using UnityEngine;

namespace TimeCycle
{
    [ExecuteAlways]
    public class LightingManager : MonoBehaviour
    {
        #region Variables

        [Header("Logger")] [SerializeField] private Utilities.Logger _logger;

        [Header("Lights")] [SerializeField] private Light _directionalLight;

        [SerializeField] private LightingPreset _preset;

        [SerializeField, Range(0, 24)] private float _timeOfDay;

        [Header("Time")] [SerializeField, Range(1, 10)]
        private int _timeMultiplier; // 1 : Time is fast, 10: Time is slow

        private const int HoursInADay = 24;

        #endregion

        #region Main Methods

        private void Update()
        {
            if (_preset == null)
                return;

            if (Application.isPlaying)
            {
                _timeOfDay += Time.deltaTime / _timeMultiplier;
                _logger.Log(Time.timeScale, this);
                _timeOfDay %= HoursInADay;
                UpdateLighting(_timeOfDay / HoursInADay);
            }
            else
            {
                UpdateLighting(_timeOfDay / HoursInADay);
            }
        }

        #endregion

        private void UpdateLighting(float timePercent)
        {
            //Set ambient and fog
            RenderSettings.ambientLight = _preset.ambientColor.Evaluate(timePercent);
            RenderSettings.fogColor = _preset.fogColor.Evaluate(timePercent);

            //If the directional light is set then rotate and set it's color, I actually rarely use the rotation because it casts tall shadows unless you clamp the value
            if (_directionalLight == null) return;
            _directionalLight.color = _preset.directionalColor.Evaluate(timePercent);

            _directionalLight.transform.localRotation =
                Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }
}