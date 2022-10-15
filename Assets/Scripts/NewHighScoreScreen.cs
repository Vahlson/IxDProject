using Unity;
using UnityEngine;
using TMPro;
class NewHighScoreScreen : MonoBehaviour
{
    public TMP_Text scoreText;
    public GameObject leaderboardScreen;
    private int placement;
    void Start()
    {

        leaderboardScreen.GetComponent<LeaderboardScreen>().OnPlacementFound += setPositionText;

        leaderboardScreen.GetComponent<LeaderboardScreen>().CreateLeaderboardEntries();
    }

    void setPositionText(string position)
    {

        scoreText.text = "You placed " + position + " with " + GameManager.Instance.latestScore + " points!";
    }
    void OnDestroy()
    {
        leaderboardScreen.GetComponent<LeaderboardScreen>().OnPlacementFound -= setPositionText;

    }

}