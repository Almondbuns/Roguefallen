using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestJournalPanel : MonoBehaviour
{
    PlayerData player_data;
    public GameObject quest_summary_button_prefab;

    // Start is called before the first frame update
    void Start()
    {
        player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        
        Refresh();  
        if (player_data.active_quests.Count > 0)
            SelectQuest(player_data.active_quests[0].id);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {
        int counter = 0;

        //Always start with the main quest(s)
        foreach (var quest in player_data.active_quests.FindAll(x => x.type == QuestType.MainQuest))
        {
            GameObject quest_button = GameObject.Instantiate(quest_summary_button_prefab, transform, false);
            
            quest_button.GetComponent<RectTransform>().localPosition = new Vector3(-411, 290 - counter * 100);
            quest_button.transform.Find("QuestName").GetComponent<TMPro.TextMeshProUGUI>().text = quest.name;
            if (quest.type == QuestType.MainQuest)
                quest_button.transform.Find("QuestType").GetComponent<TMPro.TextMeshProUGUI>().text = "Main Quest";
            else
                quest_button.transform.Find("QuestType").GetComponent<TMPro.TextMeshProUGUI>().text = "Side Quest, Level " + quest.difficulty_level;

            long index_closure = quest.id;
            quest_button.GetComponent<Button>().onClick.AddListener(() => SelectQuest(index_closure));                
            ++counter;
        }

        foreach (var quest in player_data.active_quests.FindAll(x => x.type == QuestType.SideQuest))
        {
            GameObject quest_button = GameObject.Instantiate(quest_summary_button_prefab, transform, false);
            
            quest_button.GetComponent<RectTransform>().localPosition = new Vector3(-411, 290 - counter * 100);
            quest_button.transform.Find("QuestName").GetComponent<TMPro.TextMeshProUGUI>().text = quest.name;
            if (quest.type == QuestType.MainQuest)
                quest_button.transform.Find("QuestType").GetComponent<TMPro.TextMeshProUGUI>().text = "Main Quest";
            else
                quest_button.transform.Find("QuestType").GetComponent<TMPro.TextMeshProUGUI>().text = "Side Quest, Level " + quest.difficulty_level;
            long index_closure = quest.id;
            quest_button.GetComponent<Button>().onClick.AddListener(() => SelectQuest(index_closure));                
            ++counter;
        }
        
        
    }
    
    public void SelectQuest(long id)
    {
        QuestData quest = player_data.active_quests.Find(x => x.id == id);
        transform.Find("QuestName").GetComponent<TMPro.TextMeshProUGUI>().text = quest.name;
        if (quest.type == QuestType.MainQuest)
                transform.Find("QuestType").GetComponent<TMPro.TextMeshProUGUI>().text = "Main Quest";
            else
                transform.Find("QuestType").GetComponent<TMPro.TextMeshProUGUI>().text = "Side Quest, Level " + quest.difficulty_level;

    }
}
