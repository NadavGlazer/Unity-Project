using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class ScrollViewAdapter : MonoBehaviour
{
    public RectTransform Prefab;
    public ScrollRect ScrollView;
    public RectTransform content;
    public static bool HasChanged;
    List<ItemView> viewList = new List<ItemView>();
    // Start is called before the first frame update
    void Start()
    {
        HasChanged = false;
        FetchItemModelsDataFromDataBase(results => OnReciveNewModels(results));
    }
    void Update()
    {
        if (HasChanged)
        {
            FetchItemModelsDataFromDataBase(results => OnReciveNewModels(results));
            HasChanged = false;
        }
    }
    void OnReciveNewModels(ItemModel[] models)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
        viewList.Clear();
        int index = 0;
        foreach (var model in models)
        {
            var instance = GameObject.Instantiate(Prefab.gameObject) as GameObject;
            instance.transform.SetParent(content, false);
            var view = InitializeItemView(instance, model);
            viewList.Add(view);
            index++;
        }
    }
    ItemView InitializeItemView(GameObject viewGameObject, ItemModel model)
    {
        ItemView view = new ItemView(viewGameObject.transform);
        view.Place.text = model.Place;
        view.Name.text = model.Name;
        view.Score.text = model.Score;
        view.ScoreID = model.IDScore;
        view.CheckBold();
        return view;
    }
    void FetchItemModelsDataFromDataBase(Action<ItemModel[]> onDone)
    {
        var results = new ItemModel[10];
        for (int i = 0; i < 10; i++)
        {
            results[i] = new ItemModel();
            results[i].Place = (i + 1).ToString();
            results[i].Name = AuthScript.LeaderBoards[i].GetName().ToString();
            results[i].Score = AuthScript.LeaderBoards[i].GetScore().ToString();
            results[i].IDScore = AuthScript.LeaderBoards[i].GetId();
        }
        onDone(results);
    }

}

