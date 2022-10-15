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


    //public float mainMenuSteps = 0f;
    //public Queue<float> steps = new Queue<float>();
    //[SerializeField] private float keepBpmTime = 1f;

    [HideInInspector] public Queue<float> mainMenuAchievedSteps;

    [HideInInspector] public float timeBeforeSceneSwitch;
    [HideInInspector] public bool shouldDequeSteps = true;

    [SerializeField] private float currentStepDecreaseSpeed = 20f;
    public float maxTramplingSpeed = 125f;
    public float stepSpeedIncrease = 10f;
    public float mainMenuTramplingSpeed;
    public float tramplingSpeed = 0f;
    public float onOnDamageBPMAccelerationMultiplier = 0.5f;
    [Range(0f, 1f)] public float increaseSpeedFromTramplingThreshold = 0.8f;


    public bool useArduinoInput = false;

    private int nSpawnedTiles = 0;

    //origin point For generating perlin noise.
    private float perlinXOrg;
    private float perlinYOrg;
    public GameState gameState = GameState.menu;


    void Update()
    {
        print(tramplingSpeed);
        tramplingSpeed -= Time.deltaTime * currentStepDecreaseSpeed;
        tramplingSpeed = Mathf.Clamp(tramplingSpeed, 0f, maxTramplingSpeed);
    }

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


    void Start()
    {

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
            print("in isnewleaderboardscore");
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
            while (_leaderboard.scores.Count > 5)
            {
                _leaderboard.scores.Sort();
                _leaderboard.scores.Reverse();
                _leaderboard.scores.RemoveAt(_leaderboard.scores.Count - 1);
            }
        }
        _leaderboard.scores.Sort();
        _leaderboard.scores.Reverse();
        // latestPlacement = _leaderboard.scores.FindIndex((x) => x.Equals(newLeaderboardScore)) + 1;
        // print("index: " + latestPlacement);

    }

    public void slowDownTrampling()
    {
        tramplingSpeed *= onOnDamageBPMAccelerationMultiplier;
    }


    public void step()
    {
        tramplingSpeed += stepSpeedIncrease;
        print(tramplingSpeed);
        //steps.Enqueue(Time.realtimeSinceStartup);
    }/* 
    public void recountBPM()
    {
        print(getBPM());

        while (steps.Count > 0 && shouldDequeSteps && Time.realtimeSinceStartup - steps.Peek() >= keepBpmTime)
        {
            steps.Dequeue();
        }
        //bpmAcceleration = getBPM() * bpmFactor;
    } */
    /* public int getBPM()
    {
        return steps.Count * (int)(60 / keepBpmTime);
    } */
}
enum GameState
{
    ongoing, over, menu
}