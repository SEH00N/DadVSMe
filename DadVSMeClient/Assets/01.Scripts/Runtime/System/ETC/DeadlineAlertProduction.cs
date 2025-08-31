using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace DadVSMe.Production
{
    public class DeadlineAlertProduction : MonoBehaviour
    {
        [Header("Production")]
        [SerializeField] TMP_Text _remainDistanceText;
        [SerializeField] Transform _remainDistanceTransform;
        [SerializeField] Volume _volume;

        [Header("Target")]
        [SerializeField] Transform _deadlineTransform;
        [SerializeField] Transform _playerTransform;

        private const float ALERT_MIN_DISTANCE = 20;
        private const float PANEL_MIN_SCALE = 0.5f;
        private const float PANEL_MAX_SCALE = 1.7f;

        private bool _alertFlag = false;

        private void Start()
        {
            _alertFlag = false;
        }

        private void Update()
        {
            float distance = Vector2.Distance(_deadlineTransform.position, _playerTransform.position);
            bool alertFlag = distance <= ALERT_MIN_DISTANCE;

            if(alertFlag != _alertFlag)
            {
                if(_volume.profile.TryGet(out Vignette vignette))
                {
                    vignette.active = alertFlag;
                }

                _remainDistanceText.color = alertFlag ? Color.red : Color.white;
                _alertFlag = alertFlag;
            }

            _remainDistanceText.text = $"{Mathf.Floor(distance)}m";

            float t = Mathf.InverseLerp(ALERT_MIN_DISTANCE, 0f, distance);
            float value = Mathf.Lerp(PANEL_MIN_SCALE, PANEL_MAX_SCALE, t);

            _remainDistanceTransform.localScale = Vector2.one * value;
        }
    }
}
