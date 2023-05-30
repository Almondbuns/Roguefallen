using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class MapFeatureData
{
    public (int x, int y) position;
    public (int x, int y) dimensions;
    public int difficulty_level = 0;

    public bool distribute_general_actors = true;
    public bool distribute_general_items = true;

    protected MapData map;

    public Dictionary<string, MapObjectCollectionData> floors;
    public Dictionary<string, MapObjectCollectionData> objects;

    internal virtual void Save(BinaryWriter save)
    {
        save.Write(position.x);
        save.Write(position.y);
        save.Write(dimensions.x);
        save.Write(dimensions.y);
        save.Write(difficulty_level);

        save.Write(floors.Count);
        foreach(var v in floors)
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
        position.x = save.ReadInt32();
        position.y = save.ReadInt32();
        dimensions.x = save.ReadInt32();
        dimensions.y = save.ReadInt32();
        difficulty_level = save.ReadInt32();

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

    public MapFeatureData(MapData map)
    {
        this.map = map;

        floors = new ();
        objects = new ();
    }

    public virtual void Generate(){}
    public virtual void EnterMap(){}
    public virtual void Tick(){}
    public virtual void ExitMap(){}

    public virtual bool OnPlayerMovement(int move_destination_x, int move_destination_y)
    {
        return true;
    }
}

public abstract class MFChangeDungeon : MapFeatureData
{
    public DungeonChangeData dungeon_change_data;
    public List<(int x, int y)> enter_tiles;
    public (int x, int y) exit_tile;

    internal override void Save(BinaryWriter save)
    {
        base.Save(save);
        dungeon_change_data.Save(save);

        save.Write(enter_tiles.Count);
        foreach (var v in enter_tiles)
        {
            save.Write(v.x);
            save.Write(v.y);
        }

        save.Write(exit_tile.x);
        save.Write(exit_tile.y);
    }

    internal override void Load(BinaryReader save)
    {
        base.Load(save);
        dungeon_change_data = new();
        dungeon_change_data.Load(save);

        int size = save.ReadInt32();
        enter_tiles = new(size);
        for (int i = 0; i < size; ++i)
        {
            enter_tiles.Add((save.ReadInt32(), save.ReadInt32()));
        }

        exit_tile = (save.ReadInt32(), save.ReadInt32());
    }

    public MFChangeDungeon(MapData map, DungeonChangeData dcd = null) : base(map)
    {
        this.dungeon_change_data = dcd;
        enter_tiles = new();
    }

    public override bool OnPlayerMovement(int move_destination_x, int move_destination_y)
    {
        foreach ((int x, int y) tile in enter_tiles)
        {
            if (move_destination_x == tile.x && move_destination_y == tile.y)
            {
                PlayerData player_data = GameObject.Find("GameData").GetComponent<GameData>().player_data;
                player_data.current_action = new ChangeDungeonAction(dungeon_change_data, player_data, 100);
                GameObject.Find("GameEngine").GetComponent<GameEngine>().StartCoroutine("ContinueTurns");
                return false;
            }
        }

        return true;
    }
}