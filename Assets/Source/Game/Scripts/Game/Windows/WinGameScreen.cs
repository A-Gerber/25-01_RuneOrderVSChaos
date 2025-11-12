using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RuneOrderVSChaos
{
    internal class WinGameScreen : Window
    {
        internal event Action ContinueButtonClicked;

        internal override void Close()
        {
            WindowGroup.alpha = 0f;
            WindowGroup.blocksRaycasts = false;
            ActionButton.interactable = false;
        }

        internal override void Open()
        {
            WindowGroup.alpha = 1f;
            WindowGroup.blocksRaycasts = true;
            ActionButton.interactable = true;
        }

        protected override void OnButtonClick()
        {
            ContinueButtonClicked?.Invoke();
        }
    }
}