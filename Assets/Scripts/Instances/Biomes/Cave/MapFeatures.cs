using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class MFCaveExit : MFChangeDungeon
{
    public MFCaveExit(MapData map) : base(map)
    {
    }

    public MFCaveExit(MapData map, DungeonChangeData dcd) : base(map, dcd)
    {
        distribute_general_actors = false;
        distribute_general_items = false;

        dimensions = (5,5);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("cave_wall_1"));
        collection.Add(new MapObjectData("cave_wall_2"));
        collection.Add(new MapObjectData("cave_wall_3"));
        collection.Add(new MapObjectData("cave_wall_4"));
        objects["wall"] = collection;

        collection = new();
        if (dcd.target_entrance_parameter == "Down")
            collection.Add(new MapObjectData("cave_entrance_down"));
        else
            collection.Add(new MapObjectData("cave_entrance_up"));
        
        objects["entrance"] = collection;
    }

    public override void Generate()
    {
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y + 1; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();                
            }
        }

        map.tiles[position.x + dimensions.x / 2, position.y + dimensions.y / 2].objects.Add(objects["entrance"].Random());
        
        enter_tiles.Add((position.x + dimensions.x / 2, position.y + dimensions.y / 2));
        exit_tile = (position.x + dimensions.x / 2, position.y + dimensions.y / 2 - 1);
    }

}

public class MFCaveMonsterLair : MapFeatureData
{
    public MFCaveMonsterLair(MapData map) : base(map)
    {
        distribute_general_actors = false;
        distribute_general_items = false;

        dimensions = (10, 10);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("bones", false, false));
        objects["bones"] = collection;
    }

    public override void Generate()
    {
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();
            }
        }

        map.tiles[position.x + 3, position.y + 3].objects.Add(objects["bones"].Random());
        map.tiles[position.x + 3, position.y + 6].objects.Add(objects["bones"].Random());
        map.tiles[position.x + 6, position.y + 3].objects.Add(objects["bones"].Random());
        map.tiles[position.x + 6, position.y + 6].objects.Add(objects["bones"].Random());

        map.Add(new MonsterData(position.x + 2, position.y + 2, new Bear(9)));

        map.Add(ItemData.GetRandomItem(position.x + 3, position.y + 4, this.difficulty_level+1));
        map.Add(ItemData.GetRandomItem(position.x + 3, position.y + 5, this.difficulty_level+2));
        map.Add(ItemData.GetRandomItem(position.x + 4, position.y + 4, this.difficulty_level+1));
        map.Add(ItemData.GetRandomItem(position.x + 4, position.y + 5, this.difficulty_level+2));
        map.Add(ItemData.GetRandomItem(position.x + 5, position.y + 4, this.difficulty_level+1));
        map.Add(ItemData.GetRandomItem(position.x + 5, position.y + 5, this.difficulty_level+2));
        map.Add(ItemData.GetRandomItem(position.x + 6, position.y + 4, this.difficulty_level+1));
        map.Add(ItemData.GetRandomItem(position.x + 6, position.y + 5, this.difficulty_level+2));


    }
}

public class MFCaveStorageRoom : MapFeatureData
{
    public MFCaveStorageRoom(MapData map) : base(map)
    {
        dimensions = (10, 10);
    }

    public override void Generate()
    {
        for (int i = 0; i < UnityEngine.Random.Range(5,12); ++i)
        {
            int x = UnityEngine.Random.Range(position.x, position.x + dimensions.x);
            int y = UnityEngine.Random.Range(position.y, position.y + dimensions.y);
            (int x, int y)? tile = map.FindRandomEmptyNeighborTile(x, y);
            if (tile == null) continue;
            if (UnityEngine.Random.value <= 0.5)
                map.Add(new DynamicObjectData(tile.Value.x,tile.Value.y, new Crate(this.difficulty_level)));
            else
                map.Add(new DynamicObjectData(tile.Value.x,tile.Value.y, new Jar(this.difficulty_level)));
        }
    }
}

public class MFCaveTreasureRoom : MapFeatureData
{
    public MFCaveTreasureRoom(MapData map) : base(map)
    {
        dimensions = (6, 6);
    }

    public override void Generate()
    {
        for (int i = 0; i < UnityEngine.Random.Range(5,11); ++i)
        {
            int x = UnityEngine.Random.Range(position.x, position.x + dimensions.x);
            int y = UnityEngine.Random.Range(position.y, position.y + dimensions.y);
            (int x, int y)? tile = map.FindRandomEmptyNeighborTile(x, y);
            if (tile == null) continue;
            map.Add(new ItemData(new ItemGold(this.difficulty_level), tile.Value.x, tile.Value.y));
        }
    }
}

public class MFCaveSpiderRoom : MapFeatureData
{
    public MFCaveSpiderRoom(MapData map) : base(map)
    {
        distribute_general_actors = false;

        dimensions = (8, 8);

    }

    public override void Generate()
    {
        for (int i = 0; i < UnityEngine.Random.Range(10, 21); ++i)
        {
            int x = UnityEngine.Random.Range(position.x, position.x + dimensions.x);
            int y = UnityEngine.Random.Range(position.y, position.y + dimensions.y);
            (int x, int y)? tile = map.FindRandomEmptyNeighborTile(x, y);
            if (tile == null) continue;
            map.Add(new DynamicObjectData(tile.Value.x, tile.Value.y, new SpiderWebTrap(this.difficulty_level)));
        }

        for (int i = 0; i < UnityEngine.Random.Range(2, 5); ++i)
        {
            int x = UnityEngine.Random.Range(position.x, position.x + dimensions.x);
            int y = UnityEngine.Random.Range(position.y, position.y + dimensions.y);
            (int x, int y)? tile = map.FindRandomEmptyNeighborTile(x, y);
            if (tile == null) continue;
            map.Add(new MonsterData(tile.Value.x, tile.Value.y, new CaveSpider(this.difficulty_level)));
        }
    }
}

public class MFCaveTrollBossRoom : MapFeatureData
{
    public MFCaveTrollBossRoom(MapData map) : base(map)
    {
        distribute_general_actors = false;
        distribute_general_items = false;

        dimensions = (20, 20);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("bones", false, false));
        objects["bones"] = collection;
    }

    public override void Generate()
    {
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();
            }
        }

        for (int i = 0; i < 20; ++ i)
        {
            int random_x = UnityEngine.Random.Range(position.x, position.x + dimensions.x);
            int random_y = UnityEngine.Random.Range(position.y, position.y + dimensions.y);
            map.tiles[random_x, random_y].objects.Add(objects["bones"].Random());
        }

        for (int i = 0; i < 20; ++ i)
        {
            int random_x = UnityEngine.Random.Range(position.x, position.x + dimensions.x);
            int random_y = UnityEngine.Random.Range(position.y, position.y + dimensions.y);
            ItemData item = ItemData.GetRandomItem(random_x, random_y, difficulty_level);
            
            int r = UnityEngine.Random.Range(1, 101);
            if (r <= 10)
                item.SetQuality(ItemQuality.Unique);
            else if (r <= 40)
                item.SetQuality(ItemQuality.Magical2);
            else if (r <= 70)
                item.SetQuality(ItemQuality.Magical1);

            map.Add(item);
        }
       
        MonsterData boss = new MonsterData(position.x + 10, position.y + 10, new Troll(10));
        boss.is_boss = true;
        map.Add(boss);
    }
}

public class MFCaveIrchBossRoom : MapFeatureData
{
    public MFCaveIrchBossRoom(MapData map) : base(map)
    {
        distribute_general_actors = false;
        distribute_general_items = false;

        dimensions = (10, 10);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("bones", false, false));
        objects["bones"] = collection;
    }

    public override void Generate()
    {
        for (int x = position.x; x < position.x + dimensions.x; ++x)
        {
            for (int y = position.y; y < position.y + dimensions.y; ++y)
            {
                map.tiles[x, y].objects.Clear();
            }
        }

        for (int i = 0; i < 10; ++ i)
        {
            int random_x = UnityEngine.Random.Range(position.x, position.x + dimensions.x);
            int random_y = UnityEngine.Random.Range(position.y, position.y + dimensions.y);
            map.tiles[random_x, random_y].objects.Add(objects["bones"].Random());
        }

        for (int i = 0; i < 10; ++ i)
        {
            int random_x = UnityEngine.Random.Range(position.x, position.x + dimensions.x);
            int random_y = UnityEngine.Random.Range(position.y, position.y + dimensions.y);
            ItemData item = ItemData.GetRandomItem(random_x, random_y, difficulty_level);
            
            int r = UnityEngine.Random.Range(1, 101);
            if (r <= 10)
                item.SetQuality(ItemQuality.Unique);
            else if (r <= 40)
                item.SetQuality(ItemQuality.Magical2);
            else if (r <= 70)
                item.SetQuality(ItemQuality.Magical1);

            map.Add(item);
        }
       
        MonsterData boss = new MonsterData(position.x + 5, position.y + 5, new Irch(5));
        boss.is_boss = true;
        map.Add(boss);
    }
}

public class MFCaveStoreConsumables : MFStore
{
    public MFCaveStoreConsumables(MapData map) : base(map)
    {
        dimensions = (11, 5);
        capacity = 8;
        use_walls = false;
        use_small_version = true;

        item_types.Add(typeof(ItemMeatHorn));
        item_types.Add(typeof(ItemStaminaPotion));
        item_types.Add(typeof(ItemAlmondBun));
        item_types.Add(typeof(ItemHealthPotion));
        item_types.Add(typeof(ItemFirebomb));
        item_types.Add(typeof(ItemThrowingKnife));
        item_types.Add(typeof(ItemBlueberries));        
        item_types.Add(typeof(ItemManaPotion));
        item_types.Add(typeof(ItemAcidFlask));
    }
}

public class MFCaveStoreUsables : MFStore
{
    public MFCaveStoreUsables(MapData map) : base(map)
    {
        dimensions = (11, 5);
        capacity = 8;
        use_walls = false;
        use_small_version = true;

        item_types.Add(typeof(ItemPoemOfReturn));
        item_types.Add(typeof(ItemPoemOfAJourney));
        item_types.Add(typeof(ItemPoemOfAWalk));
        item_types.Add(typeof(ItemPeppermintTea));
        item_types.Add(typeof(ItemStrawberryTea));
        item_types.Add(typeof(ItemCamomileTea));
        item_types.Add(typeof(ItemFluteOfHealing));
        item_types.Add(typeof(ItemFluteOfBraveness));
        item_types.Add(typeof(ItemFluteOfEndurance));
        item_types.Add(typeof(ItemRepairPowder));
    }
}

public class MFCaveOilRoom : MapFeatureData
{
    public MFCaveOilRoom(MapData map) : base(map)
    {
        dimensions = (8, 8);
    }

    public override void Generate()
    {
        for (int i = 0; i < UnityEngine.Random.Range(10,31); ++i)
        {
            int x = UnityEngine.Random.Range(position.x, position.x + dimensions.x);
            int y = UnityEngine.Random.Range(position.y, position.y + dimensions.y);
            (int x, int y)? tile = map.FindRandomEmptyNeighborTile(x, y);
            if (tile == null) continue;
            map.Add(new DynamicObjectData(tile.Value.x, tile.Value.y, new OilPuddle(difficulty_level)));
        }
    }
}

public class MFCavePoisonFlowerRoom : MapFeatureData
{
    public MFCavePoisonFlowerRoom(MapData map) : base(map)
    {
        dimensions = (8, 8);

        MapObjectCollectionData collection = new();
        collection.Add(new MapObjectData("invisible") { emits_light = true, light_color = new Color(0.0f,0.6f,0.0f), movement_blocked = false, sight_blocked = false });
        objects["light"] = collection;
    }

    public override void Generate()
    {
        for (int i = 0; i < UnityEngine.Random.Range(3,6); ++i)
        {
            int tries = 0;
            bool found = false;
            int x = 0;
            int y = 0;
            MonsterData flower = new MonsterData(0,0, new Flower(difficulty_level));
            while (found == false && tries < 1000)
            {
                x = UnityEngine.Random.Range(position.x, position.x + dimensions.x);
                y = UnityEngine.Random.Range(position.y, position.y + dimensions.y);
                
                if (map.CanBeMovedInByActor(x,y, flower) == true)
                    found = true;
                    
                ++tries;
            }

            if (found == true)
            {
                flower.MoveTo(x,y);
                map.Add(flower);
                map.tiles[x, y].objects.Add(objects["light"].Random());
            }
        }
    }
}