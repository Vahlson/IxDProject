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
    public int latestScore;
    public int latestPlacement = 6;

    public LeaderboardScore _newLeaderboardEntry;
    public bool IsNewHighScore()
    {
        foreach (var item in _leaderboard.scores)
        {
            if (latestScore >= item.score)
            {
                return true;
            }

        }
        if (_leaderboard.scores.Count <= 0)
        {
            return true;
        }
        return false;
    }
    void Awake()
    {
        _instance = this;
        _leaderboard = DataSaver.loadData<Leaderboard>("Leaderboard") == null ? new Leaderboard() : DataSaver.loadData<Leaderboard>("Leaderboard");
        _leaderboard.scores.Add(_newLeaderboardEntry);

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
    public List<LeaderboardScore> getScores()
    {
        return _leaderboard.scores;
    }

    public void CheckForHighScoreUpdates()
    {
        foreach (var item in _leaderboard.scores)
        {
            print(item.score);
            if (latestScore >= item.score)
            {
                _newLeaderboardEntry = new LeaderboardScore(latestScore, "AAA");
                _leaderboard.scores.Add(_newLeaderboardEntry);
                break;
            }
        }
        if (_leaderboard.scores.Count > 5)
        {
            _leaderboard.scores.Sort();
            _leaderboard.scores.Reverse();
            _leaderboard.scores.RemoveAt(_leaderboard.scores.Count - 1);
            latestPlacement = _leaderboard.scores.FindIndex((x) => x == _newLeaderboardEntry);
        }
    }


}