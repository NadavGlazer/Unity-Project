using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CurrentUser
{
    public User User;
    public string UserID;
    public CurrentUser()
    {

    }
    public CurrentUser(List<int> current, List<int> optional, List<int> own, int coins, string id, int best, string name)
    {
        User = new User(coins, own, current, optional, best, name);
        UserID = id;
    }
    public CurrentUser(User user, string id)
    {
        this.User = new User(user);
        UserID = id;
    }
    public void UpdateCurrent(List<int> current)
    {
        User.GetCurrent().Clear();
        User.GetCurrent().AddRange(current);
    }
    public void UpdateOwned(List<int> owned)
    {
        User.GetOwned().Clear();
        User.GetOwned().AddRange(owned);
    }
    public void UpdateOptional(List<int> optional)
    {
        User.GetOptional().Clear();
        User.GetOptional().AddRange(optional);
    }
    public void UpdateCoins(int coins)
    {
        User.SetCoins(coins);
    }
    public User GetUser()
    {
        return User;
    }
    public string GetUserId()
    {
        return UserID;
    }
}
