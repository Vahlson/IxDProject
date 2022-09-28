using Unity;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }
    private static GameManager _instance;
    private Leaderboard _leaderboard;
    private int _latestScore = 0;
    private int _latestPlacement = 6;

    public LeaderboardScore _newLeaderboardEntry;

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }
    public void SaveHighScore()
    {
        DataSaver.saveData<Leaderboard>(_leaderboard, "Leaderboard");

    }
    public void UpdateName(string name)
    {
        _newLeaderboardEntry.name = name;

    }
    void Start()
    {
        _leaderboard = DataSaver.loadData<Leaderboard>("Leaderboard");

    }

    void Update()
    {


    }
    public List<LeaderboardScore> getScores()
    {
        return _leaderboard.scores;
    }

    public void CheckForHighScoreUpdates()
    {
        foreach (var item in _leaderboard.scores)
        {
            print(item.score);
            if (_latestScore >= item.score)
            {
                _newLeaderboardEntry = new LeaderboardScore(_latestScore, "AAA");
                _leaderboard.scores.Add(_newLeaderboardEntry);
                break;
            }
        }
        if (_leaderboard.scores.Count > 5)
        {
            _leaderboard.scores.Sort();
            _leaderboard.scores.Reverse();
            _leaderboard.scores.RemoveAt(_leaderboard.scores.Count - 1);
            _latestPlacement = _leaderboard.scores.FindIndex((x) => x == _newLeaderboardEntry);
        }
    }


}