using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace AdapterNameSpace
{
    public class ScrollViewAdapter : MonoBehaviour
    {
        public RectTransform Prefab;
        public ScrollRect ScrollView;
        public RectTransform content;
        List<ItemView> viewList = new List<ItemView>();
        // Start is called before the first frame update
        void Start()
        {
            FetchItemModelsDataFromDataBase(results => OnReciveNewModels(results));
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
            view.NamePlace.text = model.NamePlace;
            view.Score.text = model.Score;
            view.ScoreID = model.ScoreID;
            view.CheckBold();
            return view;
        }
        void FetchItemModelsDataFromDataBase(Action<ItemModel[]> onDone)
        {
            var results = new ItemModel[10];
            for (int i = 0; i < 10; i++)
            {
                results[i] = new ItemModel();
                results[i].NamePlace = (i + 1).ToString() + " " + AuthScript.LeaderBoards[i].GetName().ToString();
                results[i].Score = AuthScript.LeaderBoards[i].GetScore().ToString();
                results[i].ScoreID = AuthScript.LeaderBoards[i].GetId();
            }
            onDone(results);
        }

    }
}

