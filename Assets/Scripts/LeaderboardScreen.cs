using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScreen : MonoBehaviour
{
    private Leaderboard leaderboard;
    public GameObject entries;
    public GameObject entryPrefab;
    public GameObject newHighScore;

    [SerializeField]
    private List<GameObject> _children = new List<GameObject>();
    void Start()
    {
        CreateLeaderboardEntries();
    }

    public void rotateNewHighScore()
    {
        if (newHighScore != null)
        {
            newHighScore.transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.Self);

        }
    }
    public void UpdateEntryName(string name)
    {
        if (newHighScore != null)
        {
            newHighScore.GetComponent<LeaderboardEntry>().playerName.text = name;
            GameManager.Instance._newLeaderboardEntry.name = name;
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

            GameObject g = Instantiate(entryPrefab, Vector3.zero, Quaternion.identity);
            if (GameManager.Instance._newLeaderboardEntry != null && GameManager.Instance._newLeaderboardEntry == items[i])
            {
                newHighScore = g;
            }
            g.transform.SetParent(entries.transform);
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
