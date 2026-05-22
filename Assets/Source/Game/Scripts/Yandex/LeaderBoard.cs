using UnityEngine;
using YG;

public class LeaderBoard : MonoBehaviour
{
    private void Awake()
    {
        if (YG2.isFirstGameSession)
            SaveResult(0);
    }

    internal void SaveResult(int score)
    {
        YG2.SetLeaderboard("LB1", score);
    }

}