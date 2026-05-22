using System;

internal interface IAdvertisementViewer
{
    event Action ClickedNextButton;
    event Action ClickedRewardButton;
}