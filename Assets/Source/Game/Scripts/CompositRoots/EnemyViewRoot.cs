using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RuneOrderVSChaos
{
    public class EnemyViewRoot : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Slider _slider;
        [SerializeField] private float _smoothEffectTime = 0.25f;

        internal TextMeshProUGUI Text => _text;
        internal Slider Slider => _slider;
        internal float SmoothEffectTime => _smoothEffectTime;

    }
}