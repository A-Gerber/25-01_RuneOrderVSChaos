using UnityEngine;
using UnityEngine.UI;

namespace RuneOrderVSChaos
{
    internal abstract class Window : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _windowGroup;
        [SerializeField] private Button _actionButton;

        protected CanvasGroup WindowGroup => _windowGroup;
        protected Button ActionButton => _actionButton;

        private void OnEnable()
        {
            _actionButton.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            _actionButton.onClick.RemoveListener(OnButtonClick);
        }

        internal abstract void Open();
        internal abstract void Close();

        protected abstract void OnButtonClick();
    }
}
