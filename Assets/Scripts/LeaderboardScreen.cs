using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScreen : MonoBehaviour
{
    private Leaderboard leaderboard;
    public GameObject entries;
    public GameObject entryPrefab;
    [SerializeField]
    private bool _gameOver = false;
    private Stack<GameObject> _children = new Stack<GameObject>();
    void Start()
    {
        CreateLeaderboardEntries();
    }

    private void CreateLeaderboardEntries()
    {
        leaderboard = DataSaver.loadData<Leaderboard>("Leaderboard");
        if (leaderboard != null)
        {
            leaderboard.scores.Sort();
            leaderboard.scores.Reverse();
            for (int i = 0; i < leaderboard.scores.Count; i++)
            {
                GameObject g = Instantiate(entryPrefab, Vector3.zero, Quaternion.identity);
                g.transform.SetParent(entries.transform);
                g.transform.localScale = Vector3.one;
                g.transform.localPosition = Vector3.zero;
                LeaderboardEntry l = g.GetComponent<LeaderboardEntry>();
                l.playerName.text = leaderboard.scores[i].name;
                l.score.text = leaderboard.scores[i].score.ToString();
                l.position.text = (i + 1).ToString();
                _children.Push(g);
                if (i == 9)
                {
                    return;
                }
            }
        }
    }
    public void CreateLeaderboardEntries(List<LeaderboardScore> list)
    {
        while (_children.Count > 0)
        {
            Destroy(_children.Pop());
        }
        leaderboard = DataSaver.loadData<Leaderboard>("Leaderboard");
        if (leaderboard != null)
        {
            leaderboard.scores.Sort();
            leaderboard.scores.Reverse();
            for (int i = 0; i < leaderboard.scores.Count; i++)
            {
                GameObject g = Instantiate(entryPrefab, Vector3.zero, Quaternion.identity);
                g.transform.SetParent(entries.transform);
                g.transform.localScale = Vector3.one;
                g.transform.localPosition = Vector3.zero;
                LeaderboardEntry l = g.GetComponent<LeaderboardEntry>();
                l.playerName.text = leaderboard.scores[i].name;
                l.score.text = leaderboard.scores[i].score.ToString();
                l.position.text = (i + 1).ToString();
                _children.Push(g);
            }
        }
    }
}
