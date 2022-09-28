using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScreen : MonoBehaviour
{
    private Leaderboard leaderboard;
    public GameObject entries;
    public GameObject entryPrefab;
    [SerializeField]
    private List<GameObject> _children = new List<GameObject>();
    void Start()
    {
        leaderboard = DataSaver.loadData<Leaderboard>("Leaderboard");
        if (leaderboard != null)
        {
            leaderboard.scores.Sort();
            leaderboard.scores.Reverse();
            CreateLeaderboardEntries(leaderboard.scores);
        }
    }

    public void rotateNewHighScore(LeaderboardScore score)
    {
        GameObject g = _children.Find((x) => x.GetComponent<LeaderboardEntry>().leaderboardScore == score);
        g.transform.Rotate(Vector3.up * 100 * Time.deltaTime, Space.Self);
    }
    public void UpdateEntryName(string name, LeaderboardScore score)
    {
        LeaderboardEntry g = _children.Find((x) => x.GetComponent<LeaderboardEntry>().leaderboardScore == score).GetComponent<LeaderboardEntry>();
        g.playerName.text = name;
        score.name = name;
    }
    public void CreateLeaderboardEntries(List<LeaderboardScore> items)
    {
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
