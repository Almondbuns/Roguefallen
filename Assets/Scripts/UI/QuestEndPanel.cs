using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestEndPanel : MonoBehaviour
{
    public QuestData quest_data;

    void Start()
    {
        if (quest_data == null)
            return;

        /*Texture2D texture = Resources.Load<Texture2D>(questgiver.prototype.icon);
        transform.Find("QuestGiverImage").GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));*/

        transform.Find("QuestText").GetComponent<TMPro.TextMeshProUGUI>().text = quest_data.end_quest_dialog;

        transform.Find("DifficultyValue").GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + quest_data.difficulty_level.ToString();
        transform.Find("ComplexityValue").GetComponent<TMPro.TextMeshProUGUI>().text = quest_data.complexity_level.ToString();

        transform.Find("GoldValue").GetComponent<TMPro.TextMeshProUGUI>().text = quest_data.reward.gold.ToString();
    }
}
