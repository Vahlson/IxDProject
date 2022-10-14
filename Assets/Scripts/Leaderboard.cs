using System.Collections.Generic;
using System;
using UnityEngine;
using Unity;
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
    public LeaderboardScore(int score, int character)
    {
        this.score = score;
        this.character = character;
    }
    public int score;
    public int character;
    public int CompareTo(LeaderboardScore obj)
    {
        return score.CompareTo(obj.score);
    }
}