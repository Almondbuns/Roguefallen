using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public UI ui;
    public GameData game_data;
    public GameEngine game_engine;

    public bool ui_state_active = true; // if ui_state was active we need to wait one update cycle before accepting new input in case keys were used to close ui_state
    // Start is called before the first frame update
    void Start()
    {
        ui = GameObject.Find("UI").GetComponent<UI>();
        game_data = GameObject.Find("GameData").GetComponent<GameData>();
        game_engine = GameObject.Find("GameEngine").GetComponent<GameEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ui.current_ui_states.Count > 0)
        {
            ui_state_active = true;
            return;
        }
        else
        {
            if (ui_state_active == true)
            {
                ui_state_active = false; // wait one frame
                return;
            }
        }

        if (ui.is_player_turn == true)
        {
            int? move_destination_x = null;
            int? move_destination_y = null;

            if (Keyboard.current.qKey.wasPressedThisFrame || Keyboard.current.numpad7Key.wasPressedThisFrame)
            {
                move_destination_x = game_data.player_data.X - 1;
                move_destination_y = game_data.player_data.Y + 1;
            }
            else if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.numpad8Key.wasPressedThisFrame)
            {
                move_destination_x = game_data.player_data.X;
                move_destination_y = game_data.player_data.Y + 1;
            }
            else if (Keyboard.current.eKey.wasPressedThisFrame || Keyboard.current.numpad9Key.wasPressedThisFrame)
            {
                move_destination_x = game_data.player_data.X + 1;
                move_destination_y = game_data.player_data.Y + 1;
            }
            else if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.numpad4Key.wasPressedThisFrame)
            {
                move_destination_x = game_data.player_data.X - 1;
                move_destination_y = game_data.player_data.Y;
            }
            else if (Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.numpad5Key.wasPressedThisFrame)
            {
                game_data.player_data.current_action = new WaitAction(50);
                game_engine.StartCoroutine("ContinueTurns");
                return;
            }
            else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.numpad6Key.wasPressedThisFrame)
            {
                move_destination_x = game_data.player_data.X + 1;
                move_destination_y = game_data.player_data.Y;
            }
            else if (Keyboard.current.zKey.wasPressedThisFrame || Keyboard.current.yKey.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame)
            {
                move_destination_x = game_data.player_data.X - 1;
                move_destination_y = game_data.player_data.Y - 1;
            }
            else if (Keyboard.current.xKey.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame)
            {
                move_destination_x = game_data.player_data.X;
                move_destination_y = game_data.player_data.Y - 1;
            }
            else if (Keyboard.current.cKey.wasPressedThisFrame || Keyboard.current.numpad3Key.wasPressedThisFrame)
            {
                move_destination_x = game_data.player_data.X + 1;
                move_destination_y = game_data.player_data.Y - 1;
            }

            if (move_destination_x.HasValue && move_destination_y.HasValue)
            {
                //Check if map features catch movement (for example: store uses movement to buy things)
                bool stop_movement = false;

                foreach (MapFeatureData feature in game_data.current_map.features)
                {
                    if (feature.OnPlayerMovement(move_destination_x.Value, move_destination_y.Value) == false)
                        stop_movement = true;
                }
                if (stop_movement == true)
                    return;


                if (game_data.current_map.IsTileBlockedByActor(move_destination_x.Value, move_destination_y.Value) == true)
                {
                    ActorData actor_data = game_data.current_map.GetActorOnTile(move_destination_x.Value, move_destination_y.Value);
                    if (actor_data != null && actor_data.OnPlayerMovementHit() == true)
                    {
                        game_data.player_data.ActivateTalent(0,
                            new TalentInputData
                            {
                                local_data = game_data.current_map,
                                source_actor = game_data.player_data,
                                target_tiles = new List<(int, int)> { (move_destination_x.Value, move_destination_y.Value) }
                            });
                        game_engine.StartCoroutine("ContinueTurns");
                        return;
                    }
                }
            }

            if (move_destination_x.HasValue && move_destination_y.HasValue)
            {
                if (game_data.current_map.IsAccessableTile(move_destination_x.Value, move_destination_y.Value) == true)
                {
                    game_data.player_data.current_action =
                        new MoveAction(game_data.player_data, move_destination_x.Value, move_destination_y.Value, game_data.player_data.GetMovementTime());
                    game_engine.StartCoroutine("ContinueTurns");
                    return;
                }
            }

            if (Keyboard.current.gKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame) // pick up item
            {
                foreach (ItemData item in game_data.current_map.items)
                {
                    if (game_data.player_data.X == item.x && game_data.player_data.Y == item.y)
                    {
                        game_data.player_data.current_action =
                    new CollectItemAction(item, 100);
                        game_engine.StartCoroutine("ContinueTurns");
                        return;
                    }
                }
            }

            if (Keyboard.current.iKey.wasPressedThisFrame) // Inventory
            {
                ui.ActivateInventoryScreen();
                return;
            }

            if (Keyboard.current.kKey.wasPressedThisFrame) // Skills
            {
                ui.ActivateSkillScreen();
                return;
            }

            if (Keyboard.current.pKey.wasPressedThisFrame) // Character
            {
                ui.ActivateCharacterScreen();
                return;
            }

            if (Keyboard.current.mKey.wasPressedThisFrame) 
            {
                ui.ToggleCameraMode();
                return;
            }

            if ((Keyboard.current.nKey.wasPressedThisFrame && Keyboard.current.shiftKey.isPressed) || Keyboard.current.numpadMinusKey.wasPressedThisFrame) 
            {
                if (ui.current_ui_states.Count > 0) return;
                ui.DecreaseCameraZoom();
                return;
            }

            if (Keyboard.current.nKey.wasPressedThisFrame || Keyboard.current.numpadPlusKey.wasPressedThisFrame) 
            {
                if (ui.current_ui_states.Count > 0) return;
                ui.IncreaseCameraZoom();
                return;
            }

            if (Keyboard.current.escapeKey.wasPressedThisFrame) 
            {
                ui.ActivateMenuScreen();
                return;
            }

            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return;
                GameObject.Find("TalentPanel").GetComponent<TalentPanel>().TalentButtonClick(0);
                return;
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return;
                GameObject.Find("TalentPanel").GetComponent<TalentPanel>().TalentButtonClick(1);
                return;
            }

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return;
                GameObject.Find("TalentPanel").GetComponent<TalentPanel>().TalentButtonClick(2);
                return;
            }

            if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return;
                GameObject.Find("TalentPanel").GetComponent<TalentPanel>().TalentButtonClick(3);
                return;
            }

            if (Keyboard.current.digit5Key.wasPressedThisFrame)
            {
                if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return;
                GameObject.Find("TalentPanel").GetComponent<TalentPanel>().TalentButtonClick(4);
                return;
            }

            if (Keyboard.current.digit6Key.wasPressedThisFrame)
            {
                if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return;
                GameObject.Find("TalentPanel").GetComponent<TalentPanel>().TalentButtonClick(5);
                return;
            }

            if (Keyboard.current.digit7Key.wasPressedThisFrame)
            {
                if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return;
                GameObject.Find("TalentPanel").GetComponent<TalentPanel>().TalentButtonClick(6);
                return;
            }

            if (Keyboard.current.digit8Key.wasPressedThisFrame)
            {
                if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return;
                GameObject.Find("TalentPanel").GetComponent<TalentPanel>().TalentButtonClick(7);
                return;
            }

            if (Keyboard.current.digit9Key.wasPressedThisFrame)
            {
                if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return;
                GameObject.Find("TalentPanel").GetComponent<TalentPanel>().TalentButtonClick(8);
                return;
            }

            if (Keyboard.current.digit0Key.wasPressedThisFrame)
            {
                if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return;
                GameObject.Find("TalentPanel").GetComponent<TalentPanel>().TalentButtonClick(9);
                return;
            }
        }
    }
}
