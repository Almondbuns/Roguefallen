using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MousePointer: MonoBehaviour
{
    public List<GameObject> info_panels;

    //TODO: Auto-Remove panels if mouse is moved long enough

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<RectTransform>().position = new Vector3(Mouse.current.position.x.ReadValue(), Mouse.current.position.y.ReadValue(), 0);
    }

    public void AddInfoPanel(ItemData item_data)
    {
        GameObject info_panel;

        info_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().item_info_prefab, transform, false);

        info_panel.GetComponent<ItemInfo>().item_data = item_data;
        info_panel.GetComponent<ItemInfo>().Create();

        info_panels.Add(info_panel);

        
    }

    public void RemoveInfoPanel(ItemData item_data)
    {
        GameObject found_object = null;
        foreach(GameObject go in info_panels)
        {
            ItemInfo info = go.GetComponent<ItemInfo>();

            if (info == null || info.item_data != item_data)
                continue;

            found_object = go;
            break;
        }

        GameObject.Destroy(found_object);
        info_panels.Remove(found_object);

    }

    public void AddInfoPanel(string text)
    {
        GameObject info_panel;

        info_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().text_info_prefab, transform, false);

        info_panel.GetComponent<TextInfo>().text = text;
        info_panel.GetComponent<TextInfo>().Create();

        info_panels.Add(info_panel);


    }

    public void RemoveInfoPanel(string text)
    {
        GameObject found_object = null;
        foreach (GameObject go in info_panels)
        {
            TextInfo info = go.GetComponent<TextInfo>();

            if (info == null || info.text != text)
                continue;

            found_object = go;
            break;
        }

        GameObject.Destroy(found_object);
        info_panels.Remove(found_object);

    }

    public void AddInfoPanel(ActorData actor_data)
    {
        GameObject info_panel;

        info_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().actor_panel_prefab, transform, false);

        info_panel.GetComponent<ActorPanel>().actor_data = actor_data;
        info_panel.GetComponent<ActorPanel>().Refresh();

        info_panels.Add(info_panel);


    }

    public void RemoveInfoPanel(ActorData actor_data)
    {
        GameObject found_object = null;
        foreach (GameObject go in info_panels)
        {
            ActorPanel info = go.GetComponent<ActorPanel>();

            if (info == null || info.actor_data != actor_data)
                continue;

            found_object = go;
            break;
        }

        GameObject.Destroy(found_object);
        info_panels.Remove(found_object);

    }

    public void AddInfoPanel(TalentData talent)
    {
        GameObject info_panel;

        info_panel = GameObject.Instantiate(GameObject.Find("UI").GetComponent<UI>().talent_info_prefab, transform, false);

        info_panel.GetComponent<TalentInfo>().talent_data = talent;
        //info_panel.GetComponent<TalentInfo>().Create();

        info_panel.GetComponent<RectTransform>().localPosition = new Vector3(170, -20, 0);

        info_panels.Add(info_panel);
    }

    public void RemoveInfoPanel(TalentData talent)
    {
        GameObject found_object = null;
        foreach (GameObject go in info_panels)
        {
            TalentInfo info = go.GetComponent<TalentInfo>();

            if (info == null || info.talent_data != talent)
                continue;

            found_object = go;
            break;
        }

        GameObject.Destroy(found_object);
        info_panels.Remove(found_object);

    }
}
