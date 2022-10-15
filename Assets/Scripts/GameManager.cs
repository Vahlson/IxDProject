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
    public int selectedCharacter = 0;
    public int latestScore;
    public int latestPlacement { get; private set; }
    private static GameManager _instance;
    private Leaderboard _leaderboard;
    private LeaderboardScore newLeaderboardScore;

    public bool useArduinoInput = false;

    private int nSpawnedTiles = 0;

    //origin point For generating perlin noise.
    private float perlinXOrg;
    private float perlinYOrg;
    public GameState gameState = GameState.menu;

    void Awake()
    {
        //We need to generate new center for perlin noise between each game.

        perlinXOrg = Random.Range(float.MinValue, float.MaxValue);
        perlinYOrg = Random.Range(float.MinValue, float.MaxValue);
        //print("Reload perlin center x: " + perlinXOrg + " ,y: " + perlinYOrg);

        if (_instance == null)
        {
            _instance = this;

        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _leaderboard = DataSaver.loadData<Leaderboard>("Leaderboard") == null ? new Leaderboard() : DataSaver.loadData<Leaderboard>("Leaderboard");
        DontDestroyOnLoad(this);
    }

    public Vector2 getPerlinCenter()
    {
        return new Vector2(perlinXOrg, perlinYOrg);
    }

    public void IncreaseNTilesSpawned()
    {
        nSpawnedTiles++;
    }
    public int getNSpawnedTiles()
    {
        return nSpawnedTiles;
    }


    public bool IsNewLeaderboardScore(LeaderboardScore leaderboardScore)
    {
        if (newLeaderboardScore == null)
        {
            return false;
        }
        else
        {
            return newLeaderboardScore.Equals(leaderboardScore);
        }
    }
    public bool IsNewHighScore()
    {
        foreach (var item in _leaderboard.scores)
        {
            if (latestScore >= item.score || _leaderboard.scores.Count < 5)
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
    public void SaveHighScore()
    {
        DataSaver.saveData<Leaderboard>(_leaderboard, "Leaderboard");
        newLeaderboardScore = null;
    }

    public List<LeaderboardScore> getScores()
    {
        return _leaderboard.scores;
    }

    public void CheckForHighScoreUpdates()
    {
        if (_leaderboard.scores.Count <= 0)
        {
            newLeaderboardScore = new LeaderboardScore(latestScore, selectedCharacter);
            _leaderboard.scores.Add(newLeaderboardScore);

        }
        else
        {
            foreach (var item in _leaderboard.scores)
            {
                if (latestScore >= item.score)
                {
                    newLeaderboardScore = new LeaderboardScore(latestScore, selectedCharacter);
                    _leaderboard.scores.Add(newLeaderboardScore);
                    break;
                }
            }
            _leaderboard.scores.Sort();
            _leaderboard.scores.Reverse();
            while (_leaderboard.scores.Count > 5)
            {
                _leaderboard.scores.RemoveAt(_leaderboard.scores.Count - 1);
            }
        }


    }
}
enum GameState
{
    ongoing, over, menu
}