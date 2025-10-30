using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class ShapeHandler : MonoBehaviour
    {
        private const int ClickButton = 0;

        [SerializeField] private Camera _camera;
        [SerializeField] private Ray _ray;

        private ShapeView _shape;
        private bool _isRaisedShape = false;

        private void Update()
        {
            if (Input.GetMouseButton(ClickButton))
            {
                _ray = _camera.ScreenPointToRay(Input.mousePosition);

                if (_isRaisedShape == false)
                {
                    if (Physics.Raycast(_ray, out RaycastHit hit, Mathf.Infinity) && hit.transform.TryGetComponent(out ShapeView shape))
                    {
                        _shape = shape;
                        _shape.Raise();
                        _isRaisedShape = shape.IsRaised;
                    }
                }
            }

            if (Input.GetMouseButtonUp(ClickButton) && _shape != null)
            {
                _isRaisedShape = false;
                _shape.Put();
            }
        }
    }
}