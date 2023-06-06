using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalentPanel : MonoBehaviour
{
    public GameObject talent_prefab;
    public List<GameObject> talent_buttons;

    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    public void TalentButtonClick(int index)
    {
        if (GameObject.Find("UI").GetComponent<UI>().current_ui_states.Count > 0) return;

        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        TalentData selected_talent = player_data.usable_talents[index].talent;
        TalentInputData talent_input = new TalentInputData();
        talent_input.source_actor = player_data;
        switch (selected_talent.prototype.target)
        {
            case TalentTarget.Self:
                talent_input.target_actor = player_data;
                player_data.ActivateTalent(index, talent_input);
                GameObject.Find("UI").GetComponent<UI>().StartCoroutine("ContinueTurns");
                return;
                

            case TalentTarget.Tile:
            case TalentTarget.AdjacentTiles:
                UIStateSelectTile ui_state = new UIStateSelectTile();
                ui_state.SetInput(index, selected_talent, talent_input);
                GameObject.Find("UI").GetComponent<UI>().AddUIState(ui_state);
                return;
                
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh()
    {
        while (transform.childCount > 0)
            DestroyImmediate(transform.GetChild(0).gameObject);
            
        talent_buttons = new List<GameObject>();
        for (int i = 0; i < 24; ++ i)
        {
            GameObject talent_button = GameObject.Instantiate(talent_prefab, transform, false);
            int x = 10 + (i % 6) * 69;
            int y = 20 + 3*69 - (10 + (i / 6) * 69);
            talent_button.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
            talent_buttons.Add(talent_button);
        }

        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
        player_data.ReevaluateTalents();

        int counter = 0;
        foreach (var usable_talent in player_data.usable_talents)
        {
            Texture2D texture = Resources.Load<Texture2D>(usable_talent.talent.prototype.icon);
            talent_buttons[counter].GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f)); ;
            if (player_data.CanUseTalent(counter) == true)
            {
                if (player_data.current_substained_talents_id.Contains(usable_talent.talent.id) == true)
                {
                    talent_buttons[counter].GetComponent<Image>().color = new Color(1, 1, 1);
                    talent_buttons[counter].transform.Find("Substained").gameObject.SetActive(true);
                }
                else
                {
                    talent_buttons[counter].GetComponent<Image>().color = new Color(1, 1, 1);
                    talent_buttons[counter].transform.Find("Substained").gameObject.SetActive(false);
                }
            }
            else
            {
                talent_buttons[counter].GetComponent<Image>().color = new Color(.2f, .2f, .2f); 
                talent_buttons[counter].transform.Find("Substained").gameObject.SetActive(false);
            }

            

            int closure_index = counter;
            talent_buttons[counter].GetComponent<Button>().onClick.AddListener(() => TalentButtonClick(closure_index));
            if (counter <= 8)
                talent_buttons[counter].transform.Find("Counter").GetComponent<TMPro.TextMeshProUGUI>().text = (counter + 1).ToString();
            else if (counter == 9)
                talent_buttons[counter].transform.Find("Counter").GetComponent<TMPro.TextMeshProUGUI>().text = "0";
            else
                talent_buttons[counter].transform.Find("Counter").GetComponent<TMPro.TextMeshProUGUI>().text = "";

            talent_buttons[counter].GetComponent<TalentButton>().talent_data = usable_talent.talent;

            if (usable_talent.source.type == PlayerTalentSourceType.Item)
            {
                if (usable_talent.talent.prototype is TalentWeaponAttack || usable_talent.source.item.GetPrototype().shield != null)
                {
                    talent_buttons[counter].transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = "";  
                }
                else if (usable_talent.source.item.usable_item_data != null)
                {
                    talent_buttons[counter].transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = usable_talent.source.item.usable_item_data.number_of_uses.ToString();  
                }
                else
                {
                    talent_buttons[counter].transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = usable_talent.source.item.amount.ToString();  
                }  
            }
            else
            {
                talent_buttons[counter].transform.Find("Amount").GetComponent<TMPro.TextMeshProUGUI>().text = "";  
            }
            
            if (usable_talent.talent.cooldown_current > 0)
            {
                talent_buttons[counter].transform.Find("Cooldown").Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = usable_talent.talent.cooldown_current.ToString();
            }
            else
            {
                talent_buttons[counter].transform.Find("Cooldown").gameObject.SetActive(false);
            }

            counter += 1;
        }

        for (int i = counter; i < 24; ++ i)
        {
            talent_buttons[i].transform.Find("Cooldown").gameObject.SetActive(false);
            talent_buttons[i].transform.Find("Substained").gameObject.SetActive(false);
        }
    }
}
