using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.U2D;

public enum CameraMode
{
    Player,
    Map
}

public class UI : MonoBehaviour
{
    public bool is_player_turn = true;
    public GameObject state_text_prefab;
    public GameObject item_info_prefab;
    public GameObject actor_panel_prefab;
    public GameObject weapon_item_info_prefab;
    public GameObject shield_item_info_prefab;
    public GameObject armor_item_info_prefab;
    public GameObject inventory_panel_prefab;
    public GameObject skill_panel_prefab;
    public GameObject character_panel_prefab;
    public GameObject buy_panel_prefab;
    public GameObject escape_panel_prefab;
    public GameObject quest_start_panel_prefab;
    public GameObject quest_end_panel_prefab;
    public GameObject text_info_prefab;
    public GameObject talent_info_prefab;
    public GameObject death_screen_prefab;
    public GameObject boss_panel_prefab;
    public GameObject boss_panel;
    public GameObject controls_panel_prefab;
    public GameObject concepts_panel_prefab;
    public GameObject quest_panel_prefab;

    public List<UIState> current_ui_states;

    public CameraMode camera_mode;
    public float standard_camera_zoom_multiplier = 1.0f;

    void Start()
    {
        current_ui_states = new();
        transform.Find("PlayerPanel").GetComponent<PlayerPanel>().player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
    }

    internal void Refresh()
    {
        transform.Find("PlayerPanel").GetComponent<PlayerPanel>().Refresh();
        transform.Find("TalentPanel").GetComponent<TalentPanel>().Refresh();
        if (GameObject.Find("Map"))
            GameObject.Find("Map").GetComponent<Map>().Refresh();
        if (GameObject.Find("Inventory Panel") != null) // TODO: Move to Refresh in state ui
            GameObject.Find("Inventory Panel").GetComponent<InventoryPanel>().Refresh();
        if (boss_panel != null)
            boss_panel.GetComponent<BossPanel>().Refresh();

        if (current_ui_states.Count > 0)
            current_ui_states[0].Refresh();
    }

    public void ContinueTurns()
    {
        StartCoroutine(GameObject.Find("GameEngine").GetComponent<GameEngine>().ContinueTurns());
    }

    public void Update()
    {
        if (current_ui_states.Count > 0) 
            current_ui_states[0].Update();
    }

    public void ToggleCameraMode()
    {
        if (current_ui_states.Count > 0) return;
        if (camera_mode == CameraMode.Player)
        {
            camera_mode = CameraMode.Map;
            GameObject go = GameObject.Find("Camera");
            MapData map = GameObject.Find("GameData").GetComponent<GameData>().current_map;
            go.GetComponent<PixelPerfectCamera>().assetsPPU = (int) Mathf.Floor(1 / (1/1024.0f * map.tiles.GetLength(1)));
            go.transform.SetParent(null);
            go.transform.position = new Vector3(map.tiles.GetLength(0) / 2.0f, map.tiles.GetLength(1) / 2.0f, -10);
        }
        else
        {
            camera_mode = CameraMode.Player;
            GameObject go = GameObject.Find("Camera");
            go.GetComponent<PixelPerfectCamera>().assetsPPU = Mathf.RoundToInt(64 * standard_camera_zoom_multiplier);
            go.transform.SetParent(GameObject.Find("Player").transform);
            go.transform.localPosition = new Vector3(0, 0, -10);
        }
    }

    internal void IncreaseCameraZoom()
    {
        if (camera_mode == CameraMode.Player)
        {
            standard_camera_zoom_multiplier += standard_camera_zoom_multiplier * 0.1f;
            GameObject go = GameObject.Find("Camera");
            go.GetComponent<PixelPerfectCamera>().assetsPPU = Mathf.RoundToInt(64 * standard_camera_zoom_multiplier);
        }
    }

    internal void DecreaseCameraZoom()
    {
        if (camera_mode == CameraMode.Player)
        {
            standard_camera_zoom_multiplier -= standard_camera_zoom_multiplier * 0.1f;
            GameObject go = GameObject.Find("Camera");
            go.GetComponent<PixelPerfectCamera>().assetsPPU = Mathf.RoundToInt(64 * standard_camera_zoom_multiplier);
        }
    }

    internal void ClearUIState(UIState state)
    {
        current_ui_states.Remove(state);
        Refresh();
    }

    internal void AddUIState(UIState state)
    {
        current_ui_states.Add(state);
    }

    public void ActivateBossPanel(ActorData monster)
    {
        if (boss_panel == null)
        {
            boss_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().boss_panel_prefab, GameObject.Find("UI").transform);
            boss_panel.GetComponent<BossPanel>().Activate(monster);
        }
    }
    public void DeactivateBossPanel()
    {
        if (boss_panel == null) return;
        Destroy(boss_panel);
        boss_panel = null;
    }

    public void ActivateCharacterScreen()
    {
        if(current_ui_states.Count > 0) return;
            AddUIState(new UIStateCharacter());
    }

    public void ActivateInventoryScreen()
    {
        if (current_ui_states.Count > 0) return;
            AddUIState(new UIStateInventory());
    }

    public void ActivateSkillScreen()
    {
        if (current_ui_states.Count > 0) return;
            AddUIState(new UIStateSkills());
    }

    public void ActivateMenuScreen()
    {
        if (current_ui_states.Count > 0) return;
                AddUIState(new UIStateEscapePanel());
    }

    public void ActivateQuestScreen()
    {
        if (current_ui_states.Count > 0) return;
                AddUIState(new UIStateQuestJournal());
    }
}
