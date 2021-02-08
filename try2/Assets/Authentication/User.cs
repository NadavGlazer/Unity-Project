using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string Name;
    public int Coins;
    public int BestScore;
    public List<int> OwnColors;
    public List<int> CurrentColor;
    public List<int> OptionalColors;
    public User()
    {

    }
    public User(string name)
    {
        Coins = 200;
        CurrentColor = new List<int> { 25, 108, 133, 255, 1 };
        OwnColors = new List<int> { -1, 25, 108, 133, 255 };
        OptionalColors = new List<int> { -1, 25, 108, 133, 255 };
        BestScore = 0;
        this.Name = name;
    }
    public User(int co, List<int> ow, List<int> cu, List<int> op, int best, string name)
    {
        Coins = co;
        BestScore = best;
        this.Name = name;
        OwnColors = new List<int>();
        CurrentColor = new List<int>();
        OptionalColors = new List<int>();

        OwnColors.AddRange(ow);
        CurrentColor.AddRange(cu);
        OptionalColors.AddRange(op);
    }
    public User(User temp)
    {
        Coins = temp.Coins;
        Name = temp.Name;
        BestScore = temp.BestScore;
        OwnColors = new List<int>();
        CurrentColor = new List<int>();
        OptionalColors = new List<int>();

        OwnColors.AddRange(temp.OwnColors);
        CurrentColor.AddRange(temp.CurrentColor);
        OptionalColors.AddRange(temp.OptionalColors);
    }
    public void AddColor(List<int> temp)
    {
        OptionalColors.AddRange(temp);
    }
    public void AddExColor(Color color)
    {
        OptionalColors.AddRange(new List<int> { (int)color.r, (int)color.g, (int)color.b, (int)color.a });
    }
    public void AddToOwn(List<int> temp)
    {
        OwnColors.AddRange(temp);
    }
    public void ChangeCurrent(List<int> temp)
    {
        CurrentColor = new List<int>(temp);
    }
    public List<int> GetCurrent()
    {
        return CurrentColor;
    }
    public List<int> GetOptional()
    {
        return OptionalColors;
    }
    public int GetCoins()
    {
        return Coins;
    }
    public void ChangeCoins(int coins)
    {
        this.Coins += coins;
    }
    public void SetCurrentPlace(int place)
    {
        CurrentColor[4] = place;
    }
    public List<int> GetOwned()
    {
        return OwnColors;
    }
    public void SetCoins(int coins)
    {
        this.Coins = coins;
    }
    public string GetName()
    {
        return Name;
    }
    public int GetBestScore()
    {
        return BestScore;
    }
    public void SetBestScore(int score)
    {
        BestScore = score;
    }
}
