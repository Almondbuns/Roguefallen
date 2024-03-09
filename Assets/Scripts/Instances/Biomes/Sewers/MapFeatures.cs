using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MFTombSarcophagusRoom4 : MapFeatureData
{
    public MFTombSarcophagusRoom4(MapData map) : base(map)
    {
        dimensions = (9, 9);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("tomb_alt_wall_1"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_candles_1") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        collection.Add(new MapObjectData("tomb_candles_2") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        objects["light"] = collection;
    }

    public override void Generate()
    {        
        for (int x = 0; x < dimensions.x; ++x)
        {
            for (int y = 0; y < dimensions.y; ++y)
            {
                map.tiles[position.x + x, position.y + y].objects.Clear();
            }
        }

        map.tiles[position.x + 1, position.y].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 7, position.y].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 1, position.y + 1].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 2, position.y + 1].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 6, position.y + 1].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 7, position.y + 1].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 1, position.y + 2].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 2, position.y + 2].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 2].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 5, position.y + 2].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 6, position.y + 2].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 7, position.y + 2].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 2, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 5, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 6, position.y + 3].objects.Add(objects["wall"].Random());

        map.tiles[position.x + 1, position.y + 8].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 7, position.y + 8].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 1, position.y + 7].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 2, position.y + 7].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 6, position.y + 7].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 7, position.y + 7].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 1, position.y + 6].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 2, position.y + 6].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 6].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 5, position.y + 6].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 6, position.y + 6].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 7, position.y + 6].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 2, position.y + 5].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 5].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 5, position.y + 5].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 6, position.y + 5].objects.Add(objects["wall"].Random());

        DynamicObjectData s = new DynamicObjectData(0,0, new TombSarcophagus(difficulty_level));
        s.MoveTo(position.x, position.y + 4);
        map.Add(s);

        s = new DynamicObjectData(0,0, new TombSarcophagus(difficulty_level));
        s.MoveTo(position.x + 8, position.y + 4);
        map.Add(s);

        s = new DynamicObjectData(0,0, new TombSarcophagus(difficulty_level));
        s.MoveTo(position.x + 4,position.y);
        map.Add(s);

        s = new DynamicObjectData(0,0, new TombSarcophagus(difficulty_level));
        s.MoveTo(position.x + 4,position.y + 8);
        map.Add(s);    

        map.tiles[position.x + 4,position.y + 4].objects.Add(objects["light"].Random());
    }
}

public class MFTombSarcophagusRoom2 : MapFeatureData
{
    public MFTombSarcophagusRoom2(MapData map) : base(map)
    {
        dimensions = (12, 6);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("tomb_alt_wall_1"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_candles_1") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        collection.Add(new MapObjectData("tomb_candles_2") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        objects["light"] = collection;
    }

    public override void Generate()
    {        
        for (int x = 0; x < dimensions.x; ++x)
        {
            for (int y = 0; y < dimensions.y; ++y)
            {
                map.tiles[position.x + x, position.y + y].objects.Clear();
            }
        }

        map.tiles[position.x + 1, position.y + 1].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 10, position.y + 1].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 1, position.y + 2].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 2, position.y + 2].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 9, position.y + 2].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 10, position.y + 2].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 2, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 4, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 7, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 8, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 9, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 4, position.y + 4].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 5, position.y + 4].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 6, position.y + 4].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 7, position.y + 4].objects.Add(objects["wall"].Random());

        DynamicObjectData s = new DynamicObjectData(0,0, new TombSarcophagus(difficulty_level));
        s.MoveTo(position.x + 4, position.y + 2);
        map.Add(s);

        s = new DynamicObjectData(0,0, new TombSarcophagus(difficulty_level));
        s.MoveTo(position.x + 7, position.y + 2);
        map.Add(s);

        map.tiles[position.x + 5,position.y + 3].objects.Add(objects["light"].Random());
        map.tiles[position.x + 6,position.y + 3].objects.Add(objects["light"].Random());
    }
}

public class MFTombSarcophagusRoom2Small : MapFeatureData
{
    public MFTombSarcophagusRoom2Small(MapData map) : base(map)
    {
        dimensions = (5, 3);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("tomb_alt_wall_1"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_candles_1") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        collection.Add(new MapObjectData("tomb_candles_2") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        objects["light"] = collection;
    }

    public override void Generate()
    {        
        for (int x = 0; x < dimensions.x; ++x)
        {
            for (int y = 0; y < dimensions.y; ++y)
            {
                map.tiles[position.x + x, position.y + y].objects.Clear();
            }
        }

        map.tiles[position.x + 1, position.y + 0].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 0].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 2, position.y + 1].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 1, position.y + 2].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 2].objects.Add(objects["wall"].Random());
       
        DynamicObjectData s = new DynamicObjectData(0,0, new TombSarcophagus(difficulty_level));
        s.MoveTo(position.x + 0, position.y + 1);
        map.Add(s);

        s = new DynamicObjectData(0,0, new TombSarcophagus(difficulty_level));
        s.MoveTo(position.x + 4, position.y + 1);
        map.Add(s);

        map.tiles[position.x + 2,position.y + 2].objects.Add(objects["light"].Random());
    }
}

public class MFTombSarcophagusRoom1 : MapFeatureData
{
    public MFTombSarcophagusRoom1(MapData map) : base(map)
    {
        dimensions = (7, 7);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("tomb_alt_wall_1"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_candles_1") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        collection.Add(new MapObjectData("tomb_candles_2") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        objects["light"] = collection;
    }

    public override void Generate()
    {        
        for (int x = 0; x < dimensions.x; ++x)
        {
            for (int y = 0; y < dimensions.y; ++y)
            {
                map.tiles[position.x + x, position.y + y].objects.Clear();
            }
        }

        map.tiles[position.x + 3, position.y + 0].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 1].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 0, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 1, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 5, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 6, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 5].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 6].objects.Add(objects["wall"].Random());
       
        DynamicObjectData s = new DynamicObjectData(0,0, new TombSarcophagus(difficulty_level));
        s.MoveTo(position.x + 3, position.y + 3);
        map.Add(s);

        map.tiles[position.x + 2,position.y + 2].objects.Add(objects["light"].Random());
        map.tiles[position.x + 4,position.y + 2].objects.Add(objects["light"].Random());
        map.tiles[position.x + 2,position.y + 4].objects.Add(objects["light"].Random());
        map.tiles[position.x + 4,position.y + 4].objects.Add(objects["light"].Random());
    }
}

public class MFTombPillars : MapFeatureData
{
    public MFTombPillars(MapData map) : base(map)
    {
        dimensions = (Random.Range(5,11), Random.Range(5,11));

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("tomb_alt_wall_1"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_candles_1") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        collection.Add(new MapObjectData("tomb_candles_2") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        objects["light"] = collection;
    }

    public override void Generate()
    {        
        map.Add(new DynamicObjectData(position.x + 1, position.y + 1 , new TombPillar(1)));
        map.Add(new DynamicObjectData(position.x + dimensions.x -2, position.y + 1, new TombPillar(1)));
        map.Add(new DynamicObjectData(position.x + 1, position.y + dimensions.y - 2, new TombPillar(1)));
        map.Add(new DynamicObjectData(position.x + dimensions.x -2, position.y + dimensions.y - 2, new TombPillar(1)));
    }
}

public class MFTombJars : MapFeatureData
{
    public MFTombJars(MapData map) : base(map)
    {
        dimensions = (7, 7);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("tomb_alt_wall_1"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObjectData("tomb_candles_1") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        collection.Add(new MapObjectData("tomb_candles_2") { emits_light = true, light_color = new Color(1.0f,1.0f,1.0f), movement_blocked = false, sight_blocked = false });
        objects["light"] = collection;
    }

    public override void Generate()
    {        
        for (int x = 0; x < dimensions.x; ++x)
        {
            for (int y = 0; y < dimensions.y; ++y)
            {
                map.tiles[position.x + x, position.y + y].objects.Clear();
            }
        }

        map.tiles[position.x + 3, position.y + 0].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 1].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 0, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 1, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 5, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 6, position.y + 3].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 5].objects.Add(objects["wall"].Random());
        map.tiles[position.x + 3, position.y + 6].objects.Add(objects["wall"].Random());
       
        DynamicObjectData s = new DynamicObjectData(0,0, new TombPillar(difficulty_level));
        s.MoveTo(position.x + 3, position.y + 3);
        map.Add(s);

        s = new DynamicObjectData(0,0, new Jar(difficulty_level));
        s.MoveTo(position.x + 0, position.y + 0);
        map.Add(s);

        s = new DynamicObjectData(0,0, new Jar(difficulty_level));
        s.MoveTo(position.x + 0, position.y + 6);
        map.Add(s);

        s = new DynamicObjectData(0,0, new Jar(difficulty_level));
        s.MoveTo(position.x + 6, position.y + 0);
        map.Add(s);

        s = new DynamicObjectData(0,0, new Jar(difficulty_level));
        s.MoveTo(position.x + 6, position.y + 6);
        map.Add(s);

        map.tiles[position.x + 2,position.y + 2].objects.Add(objects["light"].Random());
        map.tiles[position.x + 4,position.y + 2].objects.Add(objects["light"].Random());
        map.tiles[position.x + 2,position.y + 4].objects.Add(objects["light"].Random());
        map.tiles[position.x + 4,position.y + 4].objects.Add(objects["light"].Random());
    }
}

/*
public class MFOctopusBoss : MapFeatureData
{
    ActorData octopus;
    List<ActorData> tentacles;

    public MFOctopusBoss(MapData map) : base(map)
    {
        distribute_general_actors = false;
        distribute_general_items = false;

        tentacles = new();

        MapObjectCollection collection = new();
        collection.Add(new MapObject("sewers_water_1"));
        collection.Add(new MapObject("sewers_water_2"));
        collection.Add(new MapObject("sewers_water_3"));
        floors["water"] = collection;

        collection = new();
        collection.Add(new MapObject("stone_mosaic_1"));
        collection.Add(new MapObject("stone_mosaic_2"));
        collection.Add(new MapObject("stone_mosaic_3"));
        floors["stones"] = collection;
    }

    public override void Generate()

    {
        position = (1, 1, 13, 13);

        for (int x = 1; x < 15; ++x)
        {
            for (int y = 1; y < 15; ++y)
            {
                map.tiles[x, y].objects.Clear();

                if (x == 1 || x == 14 || y == 1 || y == 14)
                    map.tiles[x, y].floor = floors["stones"].Random();
                else
                    map.tiles[x, y].floor = floors["water"].Random();

            }
        }


        tentacles.Add(new OctopusTentacle(3, 12, 1));
        tentacles.Add(new OctopusTentacle(12, 12, 1));
        tentacles.Add(new OctopusTentacle(12, 3, 1));
        tentacles.Add(new OctopusTentacle(3, 3, 1));
        tentacles.Add(new OctopusTentacle(7, 12, 1));
        tentacles.Add(new OctopusTentacle(12, 7, 1));
        tentacles.Add(new OctopusTentacle(7, 3, 1));
        tentacles.Add(new OctopusTentacle(3, 7, 1));

        foreach (ActorData tentacle in tentacles)
            map.Add(tentacle);

        octopus = new Octopus(8, 8, 5);
        map.Add(octopus);
    }
}

public class MFSewersArena : MapFeatureData
{
    public MFSewersArena(MapData map) : base(map)
    {
        MapObjectCollection collection = new();
        collection.Add(new MapObject("stone_mosaic_1"));
        collection.Add(new MapObject("stone_mosaic_2"));
        collection.Add(new MapObject("stone_mosaic_3"));
        floors["stones"] = collection;

        collection = new();
        collection.Add(new MapObject("sewers_wall"));
        objects["wall"] = collection;
    }

    public override void Generate()
    {
        position = (2, 2, 11, 11);

        for (int x = 2; x < 14; ++x)
        {
            for (int y = 2; y < 14; ++y)
            {
                map.tiles[x, y].objects.Clear();

                map.tiles[x, y].floor = floors["stones"].Random();
            }
        }

        map.tiles[4, 4].objects.Add(objects["wall"].Random());
        map.tiles[4, 5].objects.Add(objects["wall"].Random());
        map.tiles[5, 4].objects.Add(objects["wall"].Random());
        map.tiles[5, 5].objects.Add(objects["wall"].Random());

        map.tiles[10, 4].objects.Add(objects["wall"].Random());
        map.tiles[10, 5].objects.Add(objects["wall"].Random());
        map.tiles[11, 4].objects.Add(objects["wall"].Random());
        map.tiles[11, 5].objects.Add(objects["wall"].Random());

        map.tiles[4, 10].objects.Add(objects["wall"].Random());
        map.tiles[4, 11].objects.Add(objects["wall"].Random());
        map.tiles[5, 10].objects.Add(objects["wall"].Random());
        map.tiles[5, 11].objects.Add(objects["wall"].Random());

        map.tiles[10, 10].objects.Add(objects["wall"].Random());
        map.tiles[10, 11].objects.Add(objects["wall"].Random());
        map.tiles[11, 10].objects.Add(objects["wall"].Random());
        map.tiles[11, 11].objects.Add(objects["wall"].Random());
    }
}

public class MFSewersMonsterLair: MapFeatureData
{
    public MFSewersMonsterLair(MapData map) : base(map)
    {
        distribute_general_actors = false;
        distribute_general_items = false;

        MapObjectCollection collection = new();
        collection.Add(new MapObject("stone_mosaic_1"));
        collection.Add(new MapObject("stone_mosaic_2"));
        collection.Add(new MapObject("stone_mosaic_3"));
        floors["stones"] = collection;

        collection = new();
        collection.Add(new MapObject("sewers_earth_1"));
        collection.Add(new MapObject("sewers_earth_2"));
        collection.Add(new MapObject("sewers_earth_3"));
        floors["earth"] = collection;

        collection = new();
        collection.Add(new MapObject("sewers_wall"));
        objects["wall"] = collection;

        collection = new();
        collection.Add(new MapObject("bones", false));
        objects["bones"] = collection;
    }

    public override void Generate()
    {
        position = (5, 5, 5, 5);

        for (int x = 5; x < 11; ++x)
        {
            for (int y = 5; y < 11; ++y)
            {
                map.tiles[x, y].objects.Clear();

                map.tiles[x, y].floor = floors["earth"].Random();
            }
        }

        map.tiles[6, 6].objects.Add(objects["bones"].Random());
        map.tiles[6, 9].objects.Add(objects["bones"].Random());
        map.tiles[9, 6].objects.Add(objects["bones"].Random());
        map.tiles[9, 9].objects.Add(objects["bones"].Random());

        map.Add(new Bear(7, 7, 5));

        map.Add(new ItemBootsHeavy(6, 7, 2));
        map.Add(new ItemChestHeavy(6, 8, 2));
        map.Add(new ItemHealthPotion(7, 7, 2));
        map.Add(new ItemMeatHorn(7, 8, 2));
        map.Add(new ItemAxe1H(8, 7, 2));
        map.Add(new ItemSword2H(8, 8, 2));
        map.Add(new ItemSpear1H(9, 7, 2));
        map.Add(new ItemHealthPotion(9, 8, 2));


    }
}
*/