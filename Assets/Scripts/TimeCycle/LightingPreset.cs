using UnityEngine;

namespace TimeCycle
{
    [CreateAssetMenu(fileName = "Lighting Preset", menuName = "TimeCycle/Lighting Preset", order = 1)]
    public class LightingPreset : ScriptableObject
    {
        public Gradient ambientColor;
        public Gradient directionalColor;
        public Gradient fogColor;
    }
}