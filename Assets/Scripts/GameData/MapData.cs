using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapObjectData
{
    public string name;
    public string texture_name;
    public bool movement_blocked = true;
    public bool sight_blocked = true;
    public bool emits_light = false;
    public Color light_color;

    internal void Save(BinaryWriter save)
    {
        save.Write(name);
        save.Write(texture_name);
        save.Write(movement_blocked);
        save.Write(sight_blocked);
        save.Write(emits_light);
        save.Write(light_color.r);
        save.Write(light_color.g);
        save.Write(light_color.b);
        save.Write(light_color.a);
    }

    internal void Load(BinaryReader save)
    {
        name = save.ReadString();
        texture_name = save.ReadString();
        movement_blocked = save.ReadBoolean();
        sight_blocked = save.ReadBoolean();
        emits_light = save.ReadBoolean();
        light_color = new Color(save.ReadSingle(), save.ReadSingle(), save.ReadSingle(), save.ReadSingle());
    }

    public MapObjectData(string name, bool path_blocked = true, bool sight_blocked = true)
    {
        this.name = name;
        this.texture_name = name;
        this.movement_blocked = path_blocked;
        this.sight_blocked = sight_blocked;
    }

    public MapObjectData()
    {
        this.name = "";
        this.texture_name = "";
    }
}

public enum Visibility
{
    None,
    Once,
    Active
}

public class TileData
{
    public MapObjectData floor;
    public List<MapObjectData> objects;
    public Color light;
    public Visibility visibility;

    public TileData()
    {
        objects = new List<MapObjectData>();
    }

    internal void Save(BinaryWriter save)
    {
        floor.Save(save);

        save.Write(objects.Count);
        foreach (var o in objects)
            o.Save(save);

        save.Write(light.r);
        save.Write(light.g);
        save.Write(light.b);
        save.Write(light.a);

        save.Write((int) visibility);
    }

    internal void Load(BinaryReader save)
    {
        floor = new MapObjectData();
        floor.Load(save);

        int size = save.ReadInt32();
        objects = new List<MapObjectData>(size);
        for (int i = 0; i < size; ++ i)
        {
            MapObjectData o = new MapObjectData();
            o.Load(save);
            objects.Add(o);
        }

        light = new Color(save.ReadSingle(), save.ReadSingle(), save.ReadSingle(), save.ReadSingle());
        visibility = (Visibility) save.ReadInt32();
    }
}

public class MapData
{
    public int w;
    public int h;

    public bool visited = false;
    public bool tick_entered = false;
    public bool cleared = false;
    public bool is_always_visible = false;

    public TileData[,] tiles;
    public List<ActorData> actors;
    public List<ItemData> items;
    public List<MapFeatureData> features;

    public List<(int x, int y)> important_connect_tiles;
    public List<(int x, int y)> visibility_check_list;
    
    public delegate void ActorHandler(ActorData actor);
    public delegate void ItemHandler(ItemData actor);
    public delegate void VoidHandler();

    public event ActorHandler CreateActor;
    public event ItemHandler CreateItem;
    public event VoidHandler RefreshVisibility;

    internal void Save(BinaryWriter save)
    {
        save.Write(w);
        save.Write(h);
        save.Write(visited);
        save.Write(tick_entered);
        save.Write(cleared);
        save.Write(is_always_visible);

        save.Write(tiles.GetLength(0));
        save.Write(tiles.GetLength(1));
        for (int i = 0; i < tiles.GetLength(0); ++i)
            for (int j = 0; j < tiles.GetLength(1); ++j)
                tiles[i, j].Save(save);

        save.Write(actors.FindAll(x=> !(x is PlayerData)).Count);
        foreach(ActorData v in actors)
        {
            if (v is PlayerData) // player data is saved in game data
                continue;

            save.Write(v.GetType().Name);

            v.Save(save);
        }

        save.Write(items.Count);
        foreach (ItemData v in items)
        {
              v.Save(save);
        }

        save.Write(features.Count);
        foreach (MapFeatureData v in features)
        {
            save.Write(v.GetType().Name);
            v.Save(save);
        }

        save.Write(important_connect_tiles.Count);
        foreach (var v in important_connect_tiles)
        {
            save.Write(v.x);
            save.Write(v.y);
        }

        save.Write(visibility_check_list.Count);
        foreach (var v in visibility_check_list)
        {
            save.Write(v.x);
            save.Write(v.y);
        }

    }

    internal void Load(BinaryReader save)
    {
        w = save.ReadInt32();
        h = save.ReadInt32();
        visited = save.ReadBoolean();
        tick_entered = save.ReadBoolean();
        cleared = save.ReadBoolean();
        is_always_visible = save.ReadBoolean();

        int tiles_x = save.ReadInt32();
        int tiles_y = save.ReadInt32();
        tiles = new TileData[tiles_x, tiles_y];
        for (int i = 0; i < tiles.GetLength(0); ++i)
        {
            for (int j = 0; j < tiles.GetLength(1); ++j)
            {
                tiles[i, j] = new TileData();
                tiles[i, j].Load(save);
            }
        }

        int size = save.ReadInt32();
        actors = new(size);
        for (int i = 0; i < size; ++ i)
        {         
            Type a_type = Type.GetType(save.ReadString());
            Debug.Log("Type " + a_type);

            ActorData a = (ActorData)Activator.CreateInstance(a_type, -1, -1, null);
            a.Load(save);
            actors.Add(a);
            Debug.Log(a.prototype.name);
        }

        size = save.ReadInt32();
        items = new(size);
        for (int i = 0; i < size; ++i)
        {
            ItemData a = new(null);
            a.Load(save);
            items.Add(a);
        }

        size = save.ReadInt32();
        features = new(size);
        for (int i = 0; i < size; ++i)
        {
            Type a_type = Type.GetType(save.ReadString());
            MapFeatureData a = (MapFeatureData)Activator.CreateInstance(a_type, this);
            a.Load(save);
            features.Add(a);
        }

        size = save.ReadInt32();
        important_connect_tiles = new(size);
        for (int i = 0; i < size; ++i)
        {
            important_connect_tiles.Add((save.ReadInt32(), save.ReadInt32()));
        }

        size = save.ReadInt32();
        visibility_check_list = new(size);
        for (int i = 0; i < size; ++i)
        {
            visibility_check_list.Add((save.ReadInt32(), save.ReadInt32()));
        }
    }

    public MapData(int max_x, int max_y)
    {
        w = max_x;
        h = max_y;

        tiles = new TileData[max_x, max_y];
        for (int x = 0; x < max_x; ++x)
            for (int y = 0; y < max_y; ++y)
                tiles[x, y] = new TileData();

        actors = new List<ActorData>();
        items = new List<ItemData>();
        features = new();
        important_connect_tiles = new();
        visibility_check_list = new();
    }

    internal (int x, int y)? FindRandomEmptyNeighborTile(int x, int y)
    {
        int tries = 0;
        while (tries < 1000)
        {
            int r_x = UnityEngine.Random.Range(-1, 2);
            int r_y = UnityEngine.Random.Range(-1, 2);

            if (IsAccessableTile(x + r_x, y + r_y))
                return (x + r_x, y + r_y);
        }

        return null;
    }

    public void Add(ActorData actor)
    {
        actors.Add(actor);
        CreateActor?.Invoke(actor);
    }

    public void Add(ItemData item)
    {
        items.Add(item);
        CreateItem?.Invoke(item);
    }

    public bool Remove(ItemData item)
    {
        bool success = items.Remove(item);
        if (success)
            item.OnRemoveFromMap();

        return success;
    }

    public bool Remove(ActorData actor)
    {
        bool success = actors.Remove(actor);
      
        return success;
    }

    internal bool IsTileBlockedByActor(int x, int y)
    {
        foreach (ActorData actor_data in actors)
        {
            if (actor_data.prototype.blocks_tiles == true)
            {
                if (actor_data.x <= x && actor_data.x + actor_data.prototype.tile_width - 1 >= x 
                    && actor_data.y <= y && actor_data.y + actor_data.prototype.tile_height - 1 >= y )
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal ActorData GetActorOnTile(int x, int y)
    {
        foreach (ActorData actor_data in actors)
        {
            if (actor_data.x <= x && actor_data.x + actor_data.prototype.tile_width - 1 >= x 
                    && actor_data.y <= y && actor_data.y + actor_data.prototype.tile_height - 1 >= y )
            {
                return actor_data;
            }
        }

        return null;
    }

    internal void RemoveAllDeadActors()
    {
        actors.RemoveAll(item => item.is_dead == true);
    }

    internal void CalculateLight(Color ambient_light)
    {
        List<(int x, int y, Color color)> lights = new();

        //Find all light sources
        for (int x = 0; x < tiles.GetLength(0); ++x)
            for (int y = 0; y < tiles.GetLength(1); ++y)
            {
                foreach (MapObjectData mo in tiles[x, y].objects)
                {
                    if (mo.emits_light == true)
                        lights.Add((x, y, mo.light_color));
                }
            }

        int light_max_distance = 12;
        //Calculate light
        for (int x = 0; x < tiles.GetLength(0); ++x)
            for (int y = 0; y < tiles.GetLength(1); ++y)
            {
                tiles[x, y].light = ambient_light;
                foreach((int x, int y, Color color) light in lights)
                {
                    float distance = Mathf.Sqrt(Mathf.Pow(x - light.x,2) + Mathf.Pow(y - light.y, 2));
                    if (distance >= light_max_distance) continue;

                    List<(int x, int y)> light_path = Algorithms.LineofSight((light.x, light.y), (x, y));
                    int number_of_light_barriers = 0;
                    foreach( (int x, int y) path_position in light_path)
                    {
                        if (path_position == (light.x, light.y) || path_position == (x, y)) continue;

                        foreach (MapObjectData mo in tiles[path_position.x, path_position.y].objects)
                        {
                            if (mo.movement_blocked == true)
                                number_of_light_barriers += 1;
                        }
                    }


                    float light_factor = Mathf.Max(0, (light_max_distance * light_max_distance - distance * distance)) / (float) (light_max_distance* light_max_distance);

                    if (number_of_light_barriers > 0)
                        light_factor *= 1/ (float) ((number_of_light_barriers + 1));

                    tiles[x, y].light.r = Mathf.Max(tiles[x, y].light.r, light.color.r * light_factor);
                    tiles[x, y].light.g = Mathf.Max(tiles[x, y].light.g, light.color.g * light_factor);
                    tiles[x, y].light.b = Mathf.Max(tiles[x, y].light.b, light.color.b * light_factor);
                }

                tiles[x, y].light = new Color(Math.Min(1, tiles[x, y].light.r), Math.Min(1, tiles[x, y].light.g), Math.Min(1, tiles[x, y].light.b));
            }
    }

    public void UpdateVisibility()
    {
        if (is_always_visible == true)
            return;

        foreach(var tile in visibility_check_list)
        {
            tiles[tile.x, tile.y].visibility = Visibility.Once;
        }

        visibility_check_list.Clear();

        PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;

        int player_sight = 16;
        int player_light = 6;

        for (int x = player_data.x - player_sight; x <= player_data.x + player_sight; ++x)
        {
            for (int y = player_data.y - player_sight; y <= player_data.y + player_sight; ++y)
            {
                if (x < 0 || y < 0 || x >= tiles.GetLength(0) || y >= tiles.GetLength(1))
                    continue;

                List<(int x, int y)> path = Algorithms.LineofSight((player_data.x, player_data.y), (x, y));


                bool blocked = IsLineofSightBlocked(path);

                if (blocked == false)
                {
                    if ((x >= player_data.x - player_light && x <= player_data.x + player_light && y >= player_data.y - player_light && y <= player_data.y + player_light)
                       || tiles[x, y].light.maxColorComponent > 0.8)
                    {
                        tiles[x, y].visibility = Visibility.Active;
                        visibility_check_list.Add((x, y));
                    }
                }
            }
        }

        RefreshVisibility?.Invoke();
    }

    bool IsLineofSightBlocked(List<(int x, int y)> path)
    {
        bool blocked = false;
        int counter = 0;
        while (blocked == false && counter < path.Count - 1) //ignore target tile
        {
            foreach (MapObjectData o in tiles[path[counter].x, path[counter].y].objects)
            {
                if (o.sight_blocked == true)
                {
                    return true;
                }
            }
            ++counter;
        }
        return false;
    }

    public bool IsAccessableTile(int x, int y, bool ignore_player = false, ActorData ignore_actor = null)
    {
        if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1))
            return false;

        foreach(MapObjectData o in tiles[x, y].objects)
        {
            if (o.movement_blocked == true)
                return false;
        }

        ActorData actor_data = GetActorOnTile(x,y);
        
        if (actor_data != null && actor_data.prototype.blocks_tiles == true)
        {
            if (ignore_player == false && actor_data is PlayerData)
                return false;
            
            if (actor_data is not PlayerData)
            {
                if (ignore_actor == null || actor_data != ignore_actor)
                    return false;
            }
        }
     
        return true;
    }

    public bool CanBeMovedInByActor(int x, int y, ActorData actor = null)
    {
        if (x < 0 || x >= tiles.GetLength(0) || y < 0 || y >= tiles.GetLength(1))
            return false;

        for (int i = x; i < x + actor.prototype.tile_width; ++ i)
        {
            for (int j = y; j < y + actor.prototype.tile_height; ++ j)
            {
                foreach(MapObjectData o in tiles[i, j].objects)
                {
                    if (o.movement_blocked == true)
                        return false;
                }

                ActorData actor_data = GetActorOnTile(i,j);
        
                if (actor_data != null && actor_data.prototype.blocks_tiles == true)
                {
                    if (actor_data != actor)
                        return false;                    
                }
            }
        }
     
        return true;
    }

    public void DistributeDamage(ActorData src_actor, AttackedTileData tile)
    {
        GameObject.Find("Map").GetComponent<Map>().AddVisualEffectToTile(VisualEffect.Hit, (tile.x, tile.y));
        //Use copy because actors may spawn new actors on attack which changes the list while traversing => error
        List<ActorData> actors_copy = new List<ActorData>(actors);

        foreach(ActorData actor in actors_copy)
        {
            if (tile.x >= actor.x && tile.y >= actor.y
                && tile.x <= actor.x + actor.prototype.tile_width - 1&& tile.y <= actor.y + actor.prototype.tile_height - 1)
            {
                actor.TryToHit(src_actor.GetToHit(), tile.damage_on_hit, tile.effects_on_hit, tile.diseases_on_hit, tile.poisons_on_hit);
            }
        }
    }

    public void Tick()
    {
        tick_entered = false;
    }

    public void Move(long actor_id, int destination_x, int destination_y)
    {
        ActorData actor = actors.Find(x => x.id == actor_id);
        if (actor == null) return;

        actor.MoveTo(destination_x, destination_y);

        List<ActorData> actors_copy = new List<ActorData>(actors);

        foreach (ActorData a in actors_copy)
        {
            if (a.x >= actor.x && a.y >= actor.y && actor != a
                && a.x <= actor.x + actor.prototype.tile_width - 1 && a.y <= actor.y + actor.prototype.tile_height - 1)
            {
                a.OnEnterTile(actor);
            }
        }
    }

    internal ActorData GetActor(long actor_id)
    {
        ActorData actor = actors.Find(x => x.id == actor_id);
        return actor;
    }

    internal ItemData GetItem(long item_id)
    {
        ItemData item = items.Find(x => x.id == item_id);
        return item;
    }

    internal ItemData GetItem(int x, int y)
    {
        ItemData item = items.Find(item => (item.x == x && item.y == y));
        return item;
    }

}
