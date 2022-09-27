using System.Collections.Generic;
using System;
using UnityEngine;
[Serializable]
public class Leaderboard
{
    public List<LeaderboardScore> scores = new List<LeaderboardScore>();
    public Leaderboard()
    {
        this.scores.Sort();
        this.scores.Reverse();
    }
}
[Serializable]
public class LeaderboardScore : IComparable<LeaderboardScore>
{
    public LeaderboardScore(int score, string name)
    {
        this.score = score;
        this.name = name;
    }
    public int score;
    public string name;
    public int CompareTo(LeaderboardScore obj)
    {
        return score.CompareTo(obj.score);
    }
}