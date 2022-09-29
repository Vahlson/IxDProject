
using UnityEngine;

public class HighscoreScreen : MonoBehaviour
{
    private int _score;
    private int _highScore;

    void Start()
    {
        _score = PlayerPrefs.GetInt("Score");

        _highScore = PlayerPrefs.GetInt("HighScore");

        Leaderboard leaderboard = DataSaver.loadData<Leaderboard>("Leaderboard");
        if (leaderboard == null)
        {
            leaderboard = new Leaderboard();
        }
        leaderboard.scores.Add(new LeaderboardScore((int)_score, "coolkid"));
        DataSaver.saveData<Leaderboard>(leaderboard, "Leaderboard");


    }

    // Update is called once per frame
    void Update()
    {

    }
}
