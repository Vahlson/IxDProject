using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LeaderboardScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _entries;
    [SerializeField]
    private GameObject _entryPrefab;
    private GameObject _newHighScore;
    public string placement = "";
    public event Action<string> OnPlacementFound;
    [SerializeField]
    private GameObject[] _placements;

    void Start()
    {
        CreateLeaderboardEntries();
    }
    void Update()
    {
        rotateNewHighScore();
    }
    public void rotateNewHighScore()
    {
        if (_newHighScore != null)
        {
            LeaderboardEntry nhs = _newHighScore.GetComponent<LeaderboardEntry>();
            nhs.position.transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.Self);
            nhs.position.transform.RotateAround(nhs.position.transform.position, new Vector3(0, 1, 0), 5 * Time.deltaTime);
            nhs.position.color = Color.yellow;
            nhs.score.transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.Self);
            nhs.score.transform.RotateAround(nhs.score.transform.position, new Vector3(0, 1, 0), 5 * Time.deltaTime);
            nhs.score.color = Color.yellow;
        }
    }

    public void CreateLeaderboardEntries()
    {
        List<LeaderboardScore> items = GameManager.Instance.getScores();
        items.Sort();
        items.Reverse();
        for (int i = 0; i < items.Count; i++)
        {
            _placements[i].SetActive(true);
            LeaderboardEntry l = _placements[i].GetComponent<LeaderboardEntry>();
            l.leaderboardScore = items[i];
            print("Character is:" + items[i].character);
            l.score.text = items[i].score.ToString() + " pts";
            l.position.text = getPositionText(i + 1);

            if (GameManager.Instance.IsNewLeaderboardScore(items[i]))
            {
                _newHighScore = _placements[i];
                placement = getPositionText(i + 1);
                OnPlacementFound?.Invoke(placement);
            }
            l.characterSelect.activateCharacter(items[i].character);

        }
    }

    string getPositionText(int position)
    {
        string pos;
        switch (position)
        {
            case 1:
                pos = position + "st";
                return pos;
            case 2:
                pos = position + "nd";
                return pos;
            case 3:
                pos = position + "rd";
                return pos;
            default:
                pos = position + "th";
                return pos;

        }
    }
}
