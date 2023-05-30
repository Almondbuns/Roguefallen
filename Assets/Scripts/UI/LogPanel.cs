using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogPanel : MonoBehaviour, IGlobalLogListener
{
    long player_turn_tick = 0;

    // Start is called before the first frame update
    TMPro.TextMeshProUGUI text_log;
    void Start()
    {
        text_log = transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>();
        GameLogger.AddListener(this);
    }

    public void GetNewLog()
    {
        string log = "";
        bool after_player_tick = false;

        for (int i = Mathf.Max(0, GameLogger.log.Count - 41); i < GameLogger.log.Count; ++i)
        {
            if (after_player_tick == false && GameLogger.log[i].tick > player_turn_tick)
            {
                after_player_tick = true;
                //log += "------------------------------------------\n";
            }

            if (after_player_tick == true)
                log += GameLogger.log[i].message + "\n";
        }
        text_log.text = log;
        
        Vector2 sizes = text_log.GetPreferredValues();
        text_log.GetComponent<RectTransform>().sizeDelta = new Vector2(385.0f, sizes.y);
        GetComponent<RectTransform>().sizeDelta = new Vector2(385.0f, sizes.y);
    }

    public void MarkPlayerTurn(long tick)
    {
        player_turn_tick = tick;
    }
}
