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
    private List<GameObject> _children = new List<GameObject>();
    private GameObject _newHighScore;
    public int placement = 0;
    public event Action<int> OnPlacementFound;

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
        foreach (var item in _children)
        {
            Destroy(item);
        }
        _children.Clear();
        items.Sort();
        items.Reverse();
        for (int i = 0; i < items.Count; i++)
        {
            GameObject g = Instantiate(_entryPrefab, Vector3.zero, Quaternion.identity);
            if (GameManager.Instance.IsNewLeaderboardScore(items[i]))
            {
                _newHighScore = g;
                placement = i;
                OnPlacementFound?.Invoke(placement + 1);
            }
            g.transform.SetParent(_entries.transform);
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.identity;
            LeaderboardEntry l = g.GetComponent<LeaderboardEntry>();
            l.leaderboardScore = items[i];
            l.characterSelect.activateCharacter(items[i].character);
            // l.characterSelect.transform.localPosition += new Vector3(0, -25, 0);
            l.score.text = items[i].score.ToString();
            l.position.text = (i + 1).ToString();
            _children.Add(g);
        }
    }
}
