using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard
{
    public int Score;
    public string Name;
    public string ID;
    public LeaderBoard()
    {

    }
    public LeaderBoard(int score, string name, string ID)
    {
        this.Score = score;
        this.Name = name;
        this.ID = ID;
    }
    public LeaderBoard(LeaderBoard temp)
    {
        this.Score = temp.Score;
        this.Name = temp.Name;
        this.ID = temp.ID;
    }
    public int GetScore()
    {
        return Score;
    }
    public string GetName()
    {
        return Name;
    }
    public void SetName(string name)
    {
        this.Name = name;
    }
    public void SetScore(int score)
    {
        this.Score = score;
    }
    public void SetId(string ID)
    {
        this.ID = ID;
    }
    public string GetId()
    {
        return ID;
    }
}
