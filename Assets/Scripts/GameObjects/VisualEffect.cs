using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VisualEffect
{
    public abstract void ActivateOnTile(int x, int y);
}

public class VisualEffectHit : VisualEffect
{
    public override void ActivateOnTile(int x, int y)
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        Map map = GameObject.Find("Map").GetComponent<Map>();
        PrefabFactory prefabs = GameObject.Find("PrefabFactory").GetComponent<PrefabFactory>();

        if (x < 0 || y < 0 || x >= map_data.tiles.GetLength(0) || y >= map_data.tiles.GetLength(1)) return;
        if (map_data.tiles[x, y].visibility != Visibility.Active) return;

        GameObject visual_effect = GameObject.Instantiate(prefabs.visual_effect_prefabs[0], map.transform);
        visual_effect.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, -1);
        GameObject.Destroy(visual_effect, 5);
    }
}

public class VisualEffectFire : VisualEffect
{
    public override void ActivateOnTile(int x, int y)
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        Map map = GameObject.Find("Map").GetComponent<Map>();
        PrefabFactory prefabs = GameObject.Find("PrefabFactory").GetComponent<PrefabFactory>();

        if (x < 0 || y < 0 || x >= map_data.tiles.GetLength(0) || y >= map_data.tiles.GetLength(1)) return;
        if (map_data.tiles[x, y].visibility != Visibility.Active) return;

        GameObject visual_effect = GameObject.Instantiate(prefabs.visual_effect_prefabs[1], map.transform);
        visual_effect.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, -1);
        GameObject.Destroy(visual_effect, 5);
    }
}

public class VisualEffectTeleport : VisualEffect
{
    public override void ActivateOnTile(int x, int y)
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        Map map = GameObject.Find("Map").GetComponent<Map>();
        PrefabFactory prefabs = GameObject.Find("PrefabFactory").GetComponent<PrefabFactory>();

        if (x < 0 || y < 0 || x >= map_data.tiles.GetLength(0) || y >= map_data.tiles.GetLength(1)) return;
        if (map_data.tiles[x, y].visibility != Visibility.Active) return;

        GameObject visual_effect = GameObject.Instantiate(prefabs.visual_effect_prefabs[2], map.transform);
        visual_effect.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, -1);
        GameObject.Destroy(visual_effect, 5);
    }
}

public class VisualEffectText : VisualEffect
{
    string text;
    Color color;
    public void SetText(string text, Color color)
    {
        this.text = text;
        this.color = color;
    }

    public override void ActivateOnTile(int x, int y)
    {
        MapData map_data = GameObject.Find("GameData").GetComponent<GameData>().current_map;
        Map map = GameObject.Find("Map").GetComponent<Map>();
        PrefabFactory prefabs = GameObject.Find("PrefabFactory").GetComponent<PrefabFactory>();

        if (x < 0 || y < 0 || x >= map_data.tiles.GetLength(0) || y >= map_data.tiles.GetLength(1)) return;
        if (map_data.tiles[x, y].visibility != Visibility.Active) return;

        GameObject visual_effect = GameObject.Instantiate(prefabs.visual_effect_prefabs[3], map.transform);
        visual_effect.transform.Find("Canvas").Find("FloatingNumber").Find("Number").GetComponent<TMPro.TextMeshProUGUI>().text = text;
        visual_effect.transform.Find("Canvas").Find("FloatingNumber").Find("Number").GetComponent<TMPro.TextMeshProUGUI>().color = color;
        visual_effect.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, -1);
        GameObject.Destroy(visual_effect, 10);
    }
}