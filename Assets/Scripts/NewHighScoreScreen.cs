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

    void setPositionText(int position)
    {
        string pos;
        switch (position)
        {
            case 1:
                pos = position + "st";
                break;
            case 2:
                pos = position + "nd";
                break;
            case 3:
                pos = position + "rd";
                break;
            default:
                pos = position + "th";
                break;

        }
        scoreText.text = "You placed " + pos + " with " + GameManager.Instance.latestScore + " points!";

    }


}