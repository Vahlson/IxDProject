using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _entries;
    [SerializeField]
    private GameObject _entryPrefab;
    private List<GameObject> _children = new List<GameObject>();
    private GameObject _newHighScore;
    void Start()
    {
        CreateLeaderboardEntries();
    }

    public void rotateNewHighScore()
    {
        if (_newHighScore != null)
        {
            _newHighScore.transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.Self);

        }
    }
    public void UpdateEntryName(string name)
    {
        print(_newHighScore);
        print(name);
        if (_newHighScore != null)
        {
            _newHighScore.GetComponent<LeaderboardEntry>().playerName.text = name;
            GameManager.Instance.UpdateNewLeaderboardScoreName(name);
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
            }
            g.transform.SetParent(_entries.transform);
            g.transform.localScale = Vector3.one;
            g.transform.localPosition = Vector3.zero;
            g.transform.rotation = Quaternion.identity;
            LeaderboardEntry l = g.GetComponent<LeaderboardEntry>();
            l.leaderboardScore = items[i];
            l.playerName.text = items[i].name;
            l.score.text = items[i].score.ToString();
            l.position.text = (i + 1).ToString();
            _children.Add(g);
        }
    }
}
