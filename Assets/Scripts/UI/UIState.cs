using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public abstract class UIState
{
    public abstract void Update();
    public abstract void DestroyState();

    public virtual void Refresh()
    { }
}

public class UIStateQuestStartDialog : UIState
{
    public MFTavern tavern;
    public QuestData quest_data;
    public GameObject quest_panel;
    public ActorData questgiver;

    public UIStateQuestStartDialog(MFTavern tavern, QuestData quest_data, ActorData questgiver)
    {
        this.tavern = tavern;
        this.quest_data = quest_data;
        this.questgiver = questgiver;

        quest_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().quest_start_panel_prefab,
            GameObject.Find("WindowCanvas").transform, false);
        quest_panel.GetComponent<QuestStartPanel>().quest_data = this.quest_data;
        quest_panel.GetComponent<QuestStartPanel>().questgiver = this.questgiver;

        quest_panel.transform.Find("AcceptButton").GetComponent<Button>().onClick.AddListener(AcceptQuest);
        quest_panel.transform.Find("RejectButton").GetComponent<Button>().onClick.AddListener(DestroyState);
    }

    public override void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            DestroyState();
        }
    }

    public override void DestroyState()
    {
        GameObject.Destroy(quest_panel);
        GameObject.Find("UI").GetComponent<UI>().ClearUIState(this);
    }

    private void AcceptQuest()
    {
        GameObject.Find("GameData").GetComponent<GameData>().player_data.AddQuest(quest_data);
        tavern.RemoveQuestgiver(questgiver);   
        DestroyState();
    }
}

public class UIStateQuestEndDialog : UIState
{
    public QuestData quest_data;
    public GameObject quest_panel;

    public UIStateQuestEndDialog(QuestData quest_data)
    {
        this.quest_data = quest_data;
     
        quest_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().quest_end_panel_prefab,
            GameObject.Find("WindowCanvas").transform, false);
        quest_panel.GetComponent<QuestEndPanel>().quest_data = this.quest_data;
  
        quest_panel.transform.Find("CollectRewardButton").GetComponent<Button>().onClick.AddListener(CollectReward);
    }

    public override void Update()
    {
    }

    public override void DestroyState()
    {
        GameObject.Destroy(quest_panel);
        GameObject.Find("UI").GetComponent<UI>().ClearUIState(this);
    }

    private void CollectReward()
    {
        GameData game_data = GameObject.Find("GameData").GetComponent<GameData>();
        if (quest_data.reward.gold > 0)
            game_data.player_data.gold_amount += quest_data.reward.gold;
        if (quest_data.reward.xp > 0)
            game_data.player_data.GainExperience(quest_data.reward.xp);
        if (quest_data.reward.items.Count > 0)
        {
            foreach(ItemData item in quest_data.reward.items)
            {
                game_data.player_data.AddItem(item);
            }
        }

        game_data.player_data.active_quests.Remove(quest_data);

        DestroyState();
    }
}


public class UIStateShopBuy : UIState
{
    public MFStore store;
    public ItemData item_data;
    public GameObject buy_panel;

    public UIStateShopBuy(MFStore store, ItemData item_data)
    {
        this.store = store;
        this.item_data = item_data;
        buy_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().buy_panel_prefab,
            GameObject.Find("WindowCanvas").transform, false);
        buy_panel.GetComponent<BuyPanel>().item_data = item_data;

        buy_panel.transform.Find("BuyButton").GetComponent<Button>().onClick.AddListener(BuyItem);
        buy_panel.transform.Find("CancelButton").GetComponent<Button>().onClick.AddListener(DestroyState);
    }

    public override void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            DestroyState();
        }
    }

    public override void DestroyState()
    {
        GameObject.Destroy(buy_panel);
        GameObject.Find("UI").GetComponent<UI>().ClearUIState(this);
    }

    private void BuyItem()
    {
        if (GameObject.Find("GameData").GetComponent<GameData>().player_data.gold_amount < item_data.GetGoldValue())
        {
            GameLogger.Log("The Player has not enough gold to buy " + item_data.GetName() + ".");
            return;
        }
        
        bool success = GameObject.Find("GameData").GetComponent<GameData>().player_data.AddItem(item_data);
        if (success == true)
        {
            GameObject.Find("GameData").GetComponent<GameData>().player_data.gold_amount -= item_data.GetGoldValue();
            GameLogger.Log("The Player buys " + item_data.GetName() + ".");
            store.ReplaceItem(item_data);
        }
        DestroyState();
    }
}

public class UIStateInventory : UIState
{
    public GameObject inventory_panel;

    public UIStateInventory()
    {
        inventory_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().inventory_panel_prefab, 
            GameObject.Find("WindowCanvas").transform, false);
        inventory_panel.GetComponent<InventoryPanel>().left_side = new PanelSidePlayerEquipment(inventory_panel);
        inventory_panel.GetComponent<InventoryPanel>().right_side = new PanelSidePlayerInventory(inventory_panel);

        inventory_panel.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(DestroyState);
        inventory_panel.GetComponent<InventoryPanel>().ui_state = this;
    }

    public override void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.iKey.wasPressedThisFrame)
        {
            DestroyState();
        }
    }

    public override void DestroyState()
    {
        GameObject.Destroy(inventory_panel);
        GameObject.Find("UI").GetComponent<UI>().ClearUIState(this);
    }
}

public class UIStateInventoryChest : UIState
{
    public GameObject inventory_panel;

    public UIStateInventoryChest(ActorData chest)
    {
        inventory_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().inventory_panel_prefab,
            GameObject.Find("WindowCanvas").transform, false);
        inventory_panel.GetComponent<InventoryPanel>().left_side = new PanelSideChestInventory(inventory_panel, chest);
        inventory_panel.GetComponent<InventoryPanel>().right_side = new PanelSidePlayerInventory(inventory_panel);

        inventory_panel.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(DestroyState);
        inventory_panel.GetComponent<InventoryPanel>().ui_state = this;
    }

    public override void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.iKey.wasPressedThisFrame)
        {
            DestroyState();
        }
    }

    public override void DestroyState()
    {
        GameObject.Destroy(inventory_panel);
        GameObject.Find("UI").GetComponent<UI>().ClearUIState(this);
    }
}

public class UIStateCharacter : UIState
{
    public GameObject character_panel;

    public UIStateCharacter()
    {
        character_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().character_panel_prefab,
            GameObject.Find("WindowCanvas").transform, false);

        character_panel.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(DestroyState);
    }

    public override void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.pKey.wasPressedThisFrame)
        {
            DestroyState();
        }
    }

    public override void DestroyState()
    {
        GameObject.Destroy(character_panel);
        GameObject.Find("UI").GetComponent<UI>().ClearUIState(this);
    }
}
public class UIStateSelectTile : UIState
{
    public GameData game_data;
    public GameObject state_text;
    public TalentData talent_data;
    public int talent_index;
    public TalentInputData talent_input_data;
    public List<GameObject> tile_selectors;
    public UIStateSelectTile()
    {
        game_data = GameObject.Find("GameData").GetComponent<GameData>();
        state_text = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().state_text_prefab, GameObject.Find("UI").transform);
        tile_selectors = new List<GameObject>();

    }

    public void SetInput(int index, TalentData selected_talent, TalentInputData talent_input)
    {
        talent_index = index;
        talent_data = selected_talent;
        talent_input_data = talent_input;

        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;

        for (int i = -talent_data.prototype.target_range; i <= talent_data.prototype.target_range; ++i)
        {
            for (int j = -talent_data.prototype.target_range; j <= talent_data.prototype.target_range; ++j)
            {
                if (i == 0 && j == 0) continue;

                int x = player_data.X + i;
                int y = player_data.Y + j;
                if (x < 0 || x >= game_data.current_map.tiles.GetLength(0) || y < 0 || y >= game_data.current_map.tiles.GetLength(1)) continue;

                GameObject tile_selector = GameObject.Instantiate(GameObject.Find("Map").GetComponent<Map>().tile_selector_prefab, GameObject.Find("Map").transform);
                tile_selector.transform.position = new Vector3(x + 0.5f, y + 0.5f, 0);
                tile_selectors.Add(tile_selector);
                tile_selector.GetComponent<TileSelector>().SetPosition((x, y));
            }
        }
    }

    public override void Update()
    {
        int? x = null;
        int? y = null;

        if (Keyboard.current.qKey.wasPressedThisFrame || Keyboard.current.numpad7Key.wasPressedThisFrame)
        {
            x = game_data.player_data.X - 1;
            y = game_data.player_data.Y + 1;
        }
        else if (Keyboard.current.wKey.wasPressedThisFrame || Keyboard.current.numpad8Key.wasPressedThisFrame)
        {
            x = game_data.player_data.X;
            y = game_data.player_data.Y + 1;
        }
        else if (Keyboard.current.eKey.wasPressedThisFrame || Keyboard.current.numpad9Key.wasPressedThisFrame)
        {
            x = game_data.player_data.X + 1;
            y = game_data.player_data.Y + 1;
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame || Keyboard.current.numpad4Key.wasPressedThisFrame)
        {
            x = game_data.player_data.X - 1;
            y = game_data.player_data.Y;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame || Keyboard.current.numpad6Key.wasPressedThisFrame)
        {
            x = game_data.player_data.X + 1;
            y = game_data.player_data.Y;
        }
        else if (Keyboard.current.zKey.wasPressedThisFrame || Keyboard.current.yKey.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            x = game_data.player_data.X - 1;
            y = game_data.player_data.Y - 1;
        }
        else if (Keyboard.current.xKey.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame)
        {
            x = game_data.player_data.X;
            y = game_data.player_data.Y - 1;
        }
        else if (Keyboard.current.cKey.wasPressedThisFrame || Keyboard.current.numpad3Key.wasPressedThisFrame)
        {
            x = game_data.player_data.X + 1;
            y = game_data.player_data.Y - 1;
        }
        else if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            DestroyState();
        }

        if (x.HasValue && y.HasValue)
        {
            if (x < 0 || x > 15 || y < 0 || y > 15) return;

            talent_input_data.target_tiles = new List<(int, int)>();
            talent_input_data.target_tiles.Add((x.Value, y.Value));
            talent_input_data.local_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;

            PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
            player_data.ActivateTalent(talent_index, talent_input_data);
            DestroyState();
            GameObject.Find("UI").GetComponent<UI>().StartCoroutine("ContinueTurns");
        }
    }

    public override void DestroyState()
    {
        foreach (GameObject go in tile_selectors)
            GameObject.Destroy(go);
        GameObject.Destroy(state_text);
        GameObject.Find("UI").GetComponent<UI>().ClearUIState(this);
    }

    public void SetTarget((int x, int y ) position)
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        List<(int x, int y)> path = Algorithms.LineofSight((player_data.X, player_data.Y), position);
        foreach(GameObject o in tile_selectors)
        {
            o.GetComponent<TileSelector>().CheckSelected(path);
        }
    }

    public void SetClickedTarget((int x, int y) position)
    {
        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        List<(int x, int y)> path = Algorithms.LineofSight((player_data.X, player_data.Y), position);
        
        path.RemoveAt(0); // Ignore Player Tile

        talent_input_data.target_tiles = path;
        talent_input_data.local_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;

        player_data.ActivateTalent(talent_index, talent_input_data);
        DestroyState();
        GameObject.Find("UI").GetComponent<UI>().StartCoroutine("ContinueTurns");
    }
}

public class UIStateEscapePanel : UIState
{
    public GameObject escape_panel;
    public GameObject controls_panel;
    public GameObject concepts_panel;

    public UIStateEscapePanel()
    {
        escape_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().escape_panel_prefab,
            GameObject.Find("WindowCanvas").transform, false);

        escape_panel.transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(QuitButtonPressed);
        escape_panel.transform.Find("ControlsButton").GetComponent<Button>().onClick.AddListener(ShowControls);
        escape_panel.transform.Find("ConceptsButton").GetComponent<Button>().onClick.AddListener(ShowConcepts);
    }

    public void QuitButtonPressed()
    {
        GameObject.Find("GameData").GetComponent<GameData>().Save("EscapeQuitTest.sav");
        Application.Quit();
    }

    public override void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            DestroyState();
        }
    }

    public override void DestroyState()
    {
        GameObject.Destroy(escape_panel); 
        if (controls_panel)
            GameObject.Destroy(controls_panel);
        if (concepts_panel)
            GameObject.Destroy(concepts_panel);
        GameObject.Find("UI").GetComponent<UI>().ClearUIState(this);
    }

    public void ShowControls()
    {
        controls_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().controls_panel_prefab, GameObject.Find("WindowCanvas").transform);
        controls_panel.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(CloseControls);
    }

    public void CloseControls()
    {
        GameObject.Destroy(controls_panel);
    }

     public void ShowConcepts()
    {
        concepts_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().concepts_panel_prefab, GameObject.Find("WindowCanvas").transform);
        concepts_panel.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(CloseConcepts);
    }

    public void CloseConcepts()
    {
        GameObject.Destroy(concepts_panel);
    }
}

public class UIStateDeathPanel : UIState
{
    public GameObject death_panel;

    public UIStateDeathPanel()
    {
        death_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().death_screen_prefab,
            GameObject.Find("WindowCanvas").transform, false);

        death_panel.transform.Find("QuitButton").GetComponent<Button>().onClick.AddListener(QuitButtonPressed);
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }

    public override void Update()
    {
    }

    public override void DestroyState()
    {
        GameObject.Destroy(death_panel);
        GameObject.Find("UI").GetComponent<UI>().ClearUIState(this);
    }
}
public class UIStateSkills : UIState
{
    public GameObject skills_panel;

    public UIStateSkills()
    {
        skills_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().skill_panel_prefab,
            GameObject.Find("WindowCanvas").transform, false);
     
        skills_panel.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(DestroyState);
    }

    public override void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.kKey.wasPressedThisFrame)
        {
            DestroyState();
        }
    }

    public override void DestroyState()
    {
        GameObject.Destroy(skills_panel);
        GameObject.Find("UI").GetComponent<UI>().ClearUIState(this);
    }

    public override void Refresh()
    {
        skills_panel.GetComponent<SkillPanel>().Refresh();
    }
}

public class UIStateQuestJournal : UIState
{
    public GameObject journal_panel;

    public UIStateQuestJournal()
    {
        journal_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().quest_panel_prefab,
            GameObject.Find("WindowCanvas").transform, false);
     
        journal_panel.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(DestroyState);
    }

    public override void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame || Keyboard.current.kKey.wasPressedThisFrame)
        {
            DestroyState();
        }
    }

    public override void DestroyState()
    {
        GameObject.Destroy(journal_panel);
        GameObject.Find("UI").GetComponent<UI>().ClearUIState(this);
    }

    public override void Refresh()
    {
        journal_panel.GetComponent<SkillPanel>().Refresh();
    }
}