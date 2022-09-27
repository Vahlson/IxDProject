using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardScreen : MonoBehaviour
{
    private Leaderboard leaderboard;
    public GameObject entries;
    public GameObject entryPrefab;
    void Start()
    {
        leaderboard = DataSaver.loadData<Leaderboard>("Leaderboard");
        if (leaderboard != null)
        {
            leaderboard.scores.Sort();
            for (int i = 0; i < leaderboard.scores.Count; i++)
            {
                GameObject g = Instantiate(entryPrefab, Vector3.zero, Quaternion.identity);
                g.transform.SetParent(entries.transform);
                g.transform.localScale = Vector3.one;
                LeaderboardEntry l = g.GetComponent<LeaderboardEntry>();
                l.playerName.text = leaderboard.scores[i].name;
                l.score.text = leaderboard.scores[i].score.ToString();
                l.position.text = (i + 1).ToString();

            }
        }
    }
}
