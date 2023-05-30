using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnCounter : MonoBehaviour
{
    GameData game_data;
    long current_ticks = 0;
    // Start is called before the first frame update
    void Start()
    {
        game_data = GameObject.Find("GameData").GetComponent<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (game_data.global_ticks != current_ticks)
        {
            transform.Find("Turns").GetComponent<TMPro.TextMeshProUGUI>().text = (game_data.global_ticks / 100).ToString() + ":" + (game_data.global_ticks % 100).ToString();
        }
    }
}
