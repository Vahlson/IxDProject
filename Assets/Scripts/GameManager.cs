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

    private AudioSource audioSource;


    void Update()
    {
        //print(tramplingSpeed);
        tramplingSpeed -= Time.deltaTime * currentStepDecreaseSpeed;
        tramplingSpeed = Mathf.Clamp(tramplingSpeed, 0f, maxTramplingSpeed);
    }

    void Awake()
    {
        if (TryGetComponent(out AudioSource aSource))
        {
            audioSource = aSource;
        }
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

    public void slowDownTrampling()
    {
        tramplingSpeed *= onOnDamageBPMAccelerationMultiplier;
    }


    public void step()
    {
        tramplingSpeed += stepSpeedIncrease;
        //print(tramplingSpeed);
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


    //Just a helper function for playing sounds sequentially. https://stackoverflow.com/questions/43715482/play-several-audio-clips-sequentially 
    public IEnumerator playAudioSequentially(AudioSource adSource, List<AudioClip> adClips, float delayAfter = 0f, float delayBefore = 0f)
    {

        //yield return null;

        yield return new WaitForSeconds(delayBefore);
        //1.Loop through each AudioClip
        for (int i = 0; i < adClips.Count; i++)
        {


            //2.Assign current AudioClip to audiosource
            adSource.clip = adClips[i];


            //3.Play Audio
            adSource.Play();

            //4.Wait for it to finish playing
            while (adSource.isPlaying)
            {
                yield return null;
            }

            yield return new WaitForSeconds(delayAfter);

            //5. Go back to #2 and play the next audio in the adClips array
        }
    }

    public IEnumerator playAudioOnGameManager(AudioClip adClip)
    {
        yield return null;

        if (audioSource != null)
        {
            audioSource.clip = adClip;
            audioSource.Play();
        }

    }

    public IEnumerator playAudioWithDelay(AudioSource adSource, AudioClip adClip, float waitTimeSeconds)
    {
        print("YOOOOOOOOO");
        yield return new WaitForSeconds(waitTimeSeconds);
        adSource.clip = adClip;
        adSource.Play();

    }
}
enum GameState
{
    ongoing, over, menu
}