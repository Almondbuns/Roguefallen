using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TileSelector : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public (int x, int y) position;

    // Start is called before the first frame update
    void Start()
    {    
    }

    public void SetPosition((int x, int y) position)
    {
        this.position = position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UI ui = GameObject.Find("UI").GetComponent<UI>();
        
        if (ui.current_ui_states.Count == 0) return;
        if (!(ui.current_ui_states[0] is UIStateSelectTile)) return;

        UIStateSelectTile select_tile_state = (UIStateSelectTile)ui.current_ui_states[0];

        select_tile_state.SetTarget(position);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UI ui = GameObject.Find("UI").GetComponent<UI>();

        if (ui.current_ui_states.Count == 0) return;
        if (!(ui.current_ui_states[0] is UIStateSelectTile)) return;

        UIStateSelectTile select_tile_state = (UIStateSelectTile)ui.current_ui_states[0];

        select_tile_state.SetClickedTarget(position);
    }

    internal void CheckSelected(List<(int x, int y)> path)
    {
        bool hit = false;
        foreach((int x, int y) p in path)
        {
            if (position.x == p.x && position.y == p.y)
                hit = true;
        }

        if (hit == true)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, .8f);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, .2f);
        }
    }
}
