using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class MapObjectCollectionData
{
    public List<MapObjectData> map_objects;

    internal void Save(BinaryWriter save)
    {
        save.Write(map_objects.Count);
        foreach(var v in map_objects)
        {
            v.Save(save);
        }
    }

    internal void Load(BinaryReader save)
    {
        int size = save.ReadInt32();
        map_objects = new(size);
        for (int i = 0; i < size; ++i)
        {
            MapObjectData v = new();
            v.Load(save);
            map_objects.Add(v);
        }
    }

    public MapObjectCollectionData()
    {
        map_objects = new();
    }

    public void Add(MapObjectData mo)
    {
        map_objects.Add(mo);
    }

    public MapObjectData Random()
    {
        int index = UnityEngine.Random.Range(0, map_objects.Count);
        return map_objects[index];
    }

}
public abstract class BiomeData
{
    public string name;
    public float connectivity_probability = 0.5f;
    public Color ambience_light = new Color(1,1,1);

    public Dictionary<string, MapObjectCollectionData> floors;
    public Dictionary<string, MapObjectCollectionData> objects;

  

    internal virtual void Save(BinaryWriter save)
    {
        save.Write(name);
        save.Write(connectivity_probability);
        save.Write(ambience_light.r);
        save.Write(ambience_light.g);
        save.Write(ambience_light.b);
        save.Write(ambience_light.a);

        save.Write(floors.Count);
        foreach (var v in floors)
        {
            save.Write(v.Key);
            v.Value.Save(save);
        }

        save.Write(objects.Count);
        foreach (var v in objects)
        {
            save.Write(v.Key);
            v.Value.Save(save);
        }

        
    }

    internal virtual void Load(BinaryReader save)
    {
        name = save.ReadString();
        connectivity_probability = save.ReadSingle();
        ambience_light = new Color(save.ReadSingle(), save.ReadSingle(), save.ReadSingle(), save.ReadSingle());

        int size = save.ReadInt32();
        floors = new(size);
        for (int i = 0; i < size; ++i)
        {
            string key = save.ReadString();
            MapObjectCollectionData v = new();
            v.Load(save);
            floors.Add(key, v);
        }

        size = save.ReadInt32();
        objects = new(size);
        for (int i = 0; i < size; ++i)
        {
            string key = save.ReadString();
            MapObjectCollectionData v = new();
            v.Load(save);
            objects.Add(key, v);
        }

        
    }

    public BiomeData()
    {
        floors = new ();
        objects = new ();
    }

    //public abstract MapData CreateMap();
    public abstract MapData CreateMapLevel(int level, int max_x, int max_y, int number_of_rooms, List<(Type type, int amount_min, int amount_max)> map_features, List<DungeonChangeData> dungeon_change_data, List<(int x, int y, int w, int h)> room_list);

    /*protected void GuaranteeMapLevelConnectivity(MapData map)
    {
        (int x, int y) random_map = (UnityEngine.Random.Range(0, map.GetLength(0)), UnityEngine.Random.Range(0, map.GetLength(1)));

        int max_cost = map.GetLength(0) * map.GetLength(1) + 1;
        for (int x = 0; x < map.GetLength(0); ++x)
        {
            for (int y = 0; y < map.GetLength(1); ++y)
            {
                //string debug = "";
                Path path = Algorithms.AStar(map, (x, y), (random_map.x, random_map.y));
                (int x, int y, int cost) old_node = (x, y, 0);
                foreach ((int xx, int yy, int cost) in path.path)
                {
                    //debug += "(" + xx +"," + yy +" : " + cost + ") ";
                    if (cost >= max_cost)
                    {
                        if (xx == old_node.x - 1 && yy == old_node.y)
                        {
                            map[old_node.x, old_node.y].exit_left = true;
                            map[xx, yy].exit_right = true;
                        }
                        if (xx == old_node.x + 1 && yy == old_node.y)
                        {
                            map[old_node.x, old_node.y].exit_right = true;
                            map[xx, yy].exit_left = true;
                        }
                        if (xx == old_node.x && yy == old_node.y - 1)
                        {
                            map[old_node.x, old_node.y].exit_down = true;
                            map[xx, yy].exit_up = true;
                        }
                        if (xx == old_node.x && yy == old_node.y + 1)
                        {
                            map[old_node.x, old_node.y].exit_up = true;
                            map[xx, yy].exit_down = true;
                        }
                    }
                    old_node = (xx, yy, cost);
                }
                //Debug.Log(debug);
            }
        }
    }*/
}

