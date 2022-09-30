using Unity;
using UnityEngine;
using TMPro;
class ScoreScreen : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject leaderboardScreen;

    void OnStart()
    {
        scoreText.text = "Your Score is: " + GameManager.Instance.latestScore;
        leaderboardScreen.GetComponent<LeaderboardScreen>().CreateLeaderboardEntries();
    }
}