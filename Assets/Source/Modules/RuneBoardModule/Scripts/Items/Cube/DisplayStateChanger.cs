using UnityEngine;

public class DisplayStateChanger : MonoBehaviour
{
    [SerializeField] private Transform _iceContainer;
    [SerializeField] private Transform _transparentSnow;
    [SerializeField] private Transform _opaqueSnow;
    [SerializeField] private Transform _transparentRune;
    [SerializeField] private Transform _opaqueRune;

    internal void ChangeFreeze(bool isFreezing)
    {
        _iceContainer.gameObject.SetActive(isFreezing);
    }

    internal void ChangeTransparent(bool isTransparent)
    {
        if (isTransparent)
        {
            _transparentRune.gameObject.SetActive(true);
            _opaqueRune.gameObject.SetActive(false);
            _transparentSnow.gameObject.SetActive(true);
            _opaqueSnow.gameObject.SetActive(false);
        }
        else
        {
            _transparentRune.gameObject.SetActive(false);
            _opaqueRune.gameObject.SetActive(true);
            _transparentSnow.gameObject.SetActive(false);
            _opaqueSnow.gameObject.SetActive(true);
        }
    }
}  