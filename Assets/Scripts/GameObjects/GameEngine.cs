using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEngine : MonoBehaviour
{
    GameData game_data;
    public GameObject map_prefab;
 
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    public void StartNewGame()
    {
        game_data = GameObject.Find("GameData").GetComponent<GameData>();
        GameObject.Instantiate(map_prefab);

        GameLogger.Log("Starting game.");
        StartCoroutine("ContinueTurns");
    }

    public void LoadGame()
    {
        game_data = GameObject.Find("GameData").GetComponent<GameData>();
        game_data.Load("EscapeQuitTest.sav");
        GameObject.Instantiate(map_prefab);

        GameLogger.Log("Loading game.");
        StartCoroutine("ContinueTurns");
    }

    public IEnumerator ContinueTurns()
    {
        UI ui = GameObject.Find("UI").GetComponent<UI>();
        ui.is_player_turn = false;
        if (GameObject.Find("Map") != null)
            GameObject.Find("Map").GetComponent<Map>().SpeedUpActorVisualActions();

        while (true)
        {
            if (game_data.player_data.current_action == null)
                break;

            if (game_data.player_data.current_action.HasFinished() == true)
                break;

            game_data.current_dungeon.Tick();
            game_data.current_map.Tick();
            
            float wait_time = game_data.player_data.Tick();
            if (wait_time > 0)
            {
                ui.Refresh();
                yield return new WaitForSeconds(wait_time);
            }

            for (int i = game_data.current_map.actors.Count -1; i >= 0; --i)
            {
                //Do not handle player within actor list because she might leave the map and destroy the list!
                if (game_data.current_map.actors[i].GetType() == typeof(PlayerData)) continue;

                wait_time = game_data.current_map.actors[i].Tick();

                if (wait_time > 0)
                {
                    ui.Refresh();
                    yield return new WaitForSeconds(wait_time);
                }
            }
        
            ++game_data.global_ticks;
            //if (game_data.global_ticks % 1 == 0) yield return new WaitForSeconds(0.01f);

            game_data.current_map.RemoveAllDeadActors();

        }
        yield return null; // At least one frame must past until ui objects are generated

        ui.is_player_turn = true;
        game_data.current_map.UpdateVisibility();
        GameObject.Find("LogPanel").GetComponent<LogPanel>().MarkPlayerTurn(game_data.global_ticks - 1);

        ui.Refresh();
        yield return null;
    }
}
