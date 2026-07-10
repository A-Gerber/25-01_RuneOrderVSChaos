using UnityEngine;
using YG;

public class LeaderBoard : MonoBehaviour, ILeaderBoard
{
    private void Awake()
    {
        if (YG2.isFirstGameSession)
            SaveResult(0);
    }

    public void SaveResult(int score)
    {
        YG2.SetLeaderboard("LB1", score);
    }

    public void SetActive(bool isEnable)
    {
        gameObject.SetActive(isEnable);
    }
}