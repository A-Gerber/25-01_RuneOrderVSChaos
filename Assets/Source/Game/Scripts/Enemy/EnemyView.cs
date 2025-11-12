using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RuneOrderVSChaos
{
    public class EnemyView : MonoBehaviour
    {
        private const float Delay = 0.01f;

        private SimpleEnemyModel _enemyModel;
        private Slider _slider;
        private float _smoothEffectTime;
        private TextMeshProUGUI _text;
        private Coroutine _coroutine;
        private WaitForSeconds _wait;

        private void Awake()
        {
            _wait = new WaitForSeconds(Delay);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out WizardProjectile bullet))
            {
                bullet.Release();
            }
        }

        internal void Initialize(EnemyViewRoot enemyViewRoot)
        {
            if (enemyViewRoot == null)
                throw new InvalidOperationException("enemyViewRoot is null");

            _text = enemyViewRoot.Text;
            _slider = enemyViewRoot.Slider;
            _smoothEffectTime = enemyViewRoot.SmoothEffectTime;
        }

        internal void SetEnemy(SimpleEnemyModel simpleEnemyModel)
        {
            if (_enemyModel != null)
                _enemyModel.ChangedHealth -= Show;

            _enemyModel = simpleEnemyModel ?? throw new InvalidOperationException("simpleEnemyModel is null");
            _enemyModel.SetMaxHealth();
            _enemyModel.ChangedHealth += Show;
            Show(_enemyModel.MaxHealth);
        }

        internal void Unsubscribe()
        {
            _enemyModel.ChangedHealth -= Show;
        }

        private void Show(int value)
        {
            Debug.Log("Show");

            _text.text = $"{value} / {_enemyModel.MaxHealth}";
            float sliderValue = (float)value / _enemyModel.MaxHealth;

            if(_coroutine != null)
                StopCoroutine( _coroutine );

            _coroutine = StartCoroutine(ChangeValueOfSlider(sliderValue));
        }

        private IEnumerator ChangeValueOfSlider(float targetValue)
        {
            float step = Mathf.Abs((targetValue - _slider.value) / _smoothEffectTime);

            while (Mathf.Approximately(_slider.value, targetValue) == false)
            {
                yield return _wait;
                _slider.value = Mathf.MoveTowards(_slider.value, targetValue, step * Time.deltaTime);
            }
        }
    }
}