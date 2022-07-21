using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab;
    
    private List<ScoreboardItem> Items;
    
    private void Awake()
    {
        Items = new List<ScoreboardItem>();
        
        for (int i = 0; i < 10; i++) {
            AddObject();
        }
    }

    private void Update()
    {
        int itemsCounter = 0;
        foreach (var score in RoomScore.Instanse.Scoreboard)
        {
            if (itemsCounter == Items.Count)
                AddObject();
            
            Items[itemsCounter].Initialize(score.Player, score.Deaths, score.Kills);
            Items[itemsCounter].gameObject.SetActive(true);

            itemsCounter++;
        }

        for (int i = itemsCounter; i < Items.Count; i++)
        {
            if (!Items[itemsCounter].gameObject.active)
                return;
            
            Items[itemsCounter].ReturnToPool();
        }
    }
    
    void AddObject() {
        GameObject temp = Instantiate(scoreboardItemPrefab, container);
        Items.Add(temp.GetComponent<ScoreboardItem>());
        temp.SetActive(false);
    }
}
