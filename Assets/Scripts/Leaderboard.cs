using System.Collections.Generic;
using System;
[Serializable]
public class Leaderboard
{
    public List<LeaderboardScore> scores = new List<LeaderboardScore>();
}
[Serializable]
public class LeaderboardScore
{
    public LeaderboardScore(int score, string name)
    {
        this.score = score;
        this.name = name;
    }
    public int score;
    public string name;
}