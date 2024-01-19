using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class GameData : MonoBehaviour
{
    public static bool GODMODE = true;
    public int starting_level = 1;

    public string game_version = "0.2 Alpha 1";
    public int world_map_index;
    public long global_ticks = 0;
    
    public List<DungeonData> dungeons;
    public List<BiomeData> biomes;
    public PlayerData player_data;

    //References
    public DungeonData current_dungeon;
    public DungeonLevelData current_map_level;
    public MapData current_map;

    public void Save(string filename)
    {
        BinaryWriter save = new BinaryWriter(File.Open(Application.persistentDataPath + "\\" + filename, System.IO.FileMode.Create));
       
        save.Write(game_version);
        save.Write(world_map_index);
        save.Write(global_ticks);

        save.Write(dungeons.Count);

        foreach (DungeonData d in dungeons)
            d.Save(save);

        
        save.Write(biomes.Count);
        foreach (var v in biomes)
        {
            save.Write(v.GetType().Name);
            v.Save(save);
        }

        player_data.Save(save);

        int dungeon_index = dungeons.IndexOf(current_dungeon);
        Debug.Log(dungeon_index);
        save.Write(dungeon_index);

        int dungeon_level_index = current_dungeon.dungeon_levels.IndexOf(current_map_level);
        save.Write(dungeon_level_index);

        save.Write(ActorData.id_counter);
        save.Write(ItemData.id_counter);
        save.Write(QuestData.id_counter);
        save.Write(TalentData.id_counter);
        
        save.Close();
    }

    public void Load(string filename)
    {
        BinaryReader save = new BinaryReader(File.Open(Application.persistentDataPath + "\\" + filename, System.IO.FileMode.Open));

        game_version = save.ReadString();
        world_map_index = save.ReadInt32();
        global_ticks = save.ReadInt64();

        int size = save.ReadInt32();
        dungeons = new List<DungeonData>(size);
        for (int i = 0; i < size; ++ i)
        {
            DungeonData d = new DungeonData();
            d.Load(save);
            dungeons.Add(d);
        }
        
        size = save.ReadInt32();
        biomes = new (size);
        for (int i = 0; i < size; ++i)
        {
            Type type = Type.GetType(save.ReadString());
            BiomeData v = (BiomeData) Activator.CreateInstance(type);
            v.Load(save);
            biomes.Add(v);
        }

        player_data.Load(save);

        int dungeon_index = save.ReadInt32();
        current_dungeon = dungeons[dungeon_index];

        int dungeon_level_index = save.ReadInt32();
        current_map_level = current_dungeon.dungeon_levels[dungeon_level_index];
        current_map = current_map_level.map;

        current_map.Add(player_data);

        ActorData.id_counter = save.ReadInt64();
        ItemData.id_counter = save.ReadInt64();
        QuestData.id_counter = save.ReadInt64();
        TalentData.id_counter = save.ReadInt64();

        GameObject.Find("DungeonInfo").transform.Find("DungeonName").GetComponent<TMPro.TextMeshProUGUI>().text = current_dungeon.name;
        GameObject.Find("DungeonInfo").transform.Find("DungeonLevel").GetComponent<TMPro.TextMeshProUGUI>().text = "Level " + (current_map_level.dungeon_level + 1);        
        save.Close();
    }

    void Awake()
    {
        biomes = new List<BiomeData>
        {
            new BiomeWorldMap(),
            new BiomeVillage(),
            new BiomeCave(),
            new BiomeSewers(),
            new BiomeCastle(),
            new BiomeFrozenCave(),
        };

        player_data = new PlayerData(starting_level);

        dungeons = new List<DungeonData>();

        int number_of_dungeons_per_type = 4;
        
        dungeons.Add(new DungeonData()        
        {
            name = "World Map",
            is_persistent = true,

            dungeon_levels =
            {
                new DungeonLevelData
                {
                    dimensions = (64, 32),
                    number_of_rooms = (0,0),
                    difficulty_level = 1,
                    has_enemies = false,
                    has_items = false,
                    is_always_visible = true,

                    biome_index = 0,

                    dungeon_changes =
                    {
                        
                    },

                    map_features =
                    {                       
                    },
                }
            },
        });

        for (int i = 0; i < number_of_dungeons_per_type; ++ i)
        {
            dungeons[0].dungeon_levels[0].dungeon_changes.Add(new DungeonChangeData
            {          
                name = "Village Entrance " + i,
                dungeon_change_type = typeof(MFStandardDungeonEntrance),
                dungeon_change_image = "images/objects/village_1",
                target_dungeon_name = "High Meadow " + i,
                target_entrance_name = "World Map Exit",
            });
        }

        for (int i = 0; i < number_of_dungeons_per_type / 2; ++ i)
        {
            dungeons[0].dungeon_levels[0].dungeon_changes.Add(new DungeonChangeData
            {          
                name = "Cave Entrance " + i,
                dungeon_change_type = typeof(MFStandardDungeonEntrance),
                dungeon_change_image = "images/objects/cave_1",
                target_dungeon_name = "The Frosty Cave",
                target_entrance_name = "World Map Exit",
            });

            dungeons[0].dungeon_levels[0].dungeon_changes.Add(new DungeonChangeData
            {          
                name = "Cave Entrance " + i,
                dungeon_change_type = typeof(MFStandardDungeonEntrance),
                dungeon_change_image = "images/objects/cave_1",
                target_dungeon_name = "The Mountain Cave",
                target_entrance_name = "World Map Exit",
            });
        }

        for (int i = 0; i < number_of_dungeons_per_type; ++ i)
        {
            dungeons[0].dungeon_levels[0].dungeon_changes.Add(new DungeonChangeData
            {          
                name = "Sewers Entrance " + i,
                dungeon_change_type = typeof(MFStandardDungeonEntrance),
                dungeon_change_image = "images/objects/sewers_1",
                target_dungeon_name = "The Sewers " + i,
                target_entrance_name = "World Map Exit",
            });
        }

        for (int i = 0; i < number_of_dungeons_per_type; ++ i)
        {
            dungeons[0].dungeon_levels[0].dungeon_changes.Add(new DungeonChangeData
            {          
                name = "Castle Entrance " + i,
                dungeon_change_type = typeof(MFStandardDungeonEntrance),
                dungeon_change_image = "images/objects/castle",
                target_dungeon_name = "The Castle " + i,
                target_entrance_name = "World Map Exit",
            });
        }

        for (int i = 0; i < number_of_dungeons_per_type; ++i)
        {
            dungeons.Add(new DungeonData()        
            {
                name = "High Meadow " + i,
                is_persistent = true,

                dungeon_levels =
                {
                    new DungeonLevelData
                    {
                        dimensions = (80, 50),
                        number_of_rooms = (0,0),
                        difficulty_level = 1 + starting_level,
                        has_enemies = false,
                        has_items = false,
                        is_always_visible = true,

                        biome_index = 1,

                        dungeon_changes =
                        {
                            new DungeonChangeData
                            {
                                name = "World Map Exit",
                                dungeon_change_type = typeof(MFStandardDungeonEntrance),
                                dungeon_change_image = "images/objects/village_1",
                                target_dungeon_name = "World Map",
                                target_entrance_name = "Village Entrance " + i,
                            }
                        },

                        map_features =
                        {
                            (typeof(MFStoreWeapons), 2,2),
                            (typeof(MFStoreArmor), 2,2),
                            (typeof(MFStoreConsumables), 1,1),
                            (typeof(MFStoreUsables), 1,1),
                            (typeof(MFStoreJewelry), 1,1),
                            (typeof(MFTavern), 1,1),
                            (typeof(MFVillageSunflowers), 4,4),
                            (typeof(MFVillageField), 4,4),
                        },
                    }
                },
            });
        }

        for (int i = 0; i < number_of_dungeons_per_type / 2; ++i)
        {

            DungeonData cave = new FrostyCave();
            dungeons.Add(cave);   

            cave = new MountainCave();
            dungeons.Add(cave);            
        }

        for (int i = 0; i < number_of_dungeons_per_type; ++i)
        {
            DungeonData sewers = new DungeonData()        
            {
                name = "The Sewers " + i,
            };
            dungeons.Add(sewers);

            for (int level = 0; level < 10; ++level)
            {
                DungeonLevelData level_data = new DungeonLevelData
                {
                    biome_index = 3,
                    //is_always_visible = true,

                    map_features =
                    {                    
                        
                        //(typeof(MFCaveTreasureRoom), 0, 1), 
                        //(typeof(MFCaveOilRoom), 0, 2),                                           
                    },                
                };

                if (level >= 2)
                    level_data.map_features.Add((typeof(MFCavePoisonFlowerRoom), 0, 1));

                if (UnityEngine.Random.value < 0.1f)
                    level_data.map_features.Add((typeof(MFCaveStoreConsumables), 1,1));

                if (UnityEngine.Random.value < 0.2f)
                    level_data.map_features.Add((typeof(MFCaveStorageRoom), 1, 1));

                if (UnityEngine.Random.value < 0.1f)
                    level_data.map_features.Add((typeof(MFCaveStoreUsables), 1,1));

                if (UnityEngine.Random.value < 0.1f)
                    level_data.map_features.Add((typeof(MFCaveSpiderRoom), 0, 1));


                if (level > 0 && level <= 6 && UnityEngine.Random.value < 0.1f)
                    level_data.map_features.Add((typeof(MFCaveMonsterLair), 1, 1));

                //Add bosses
                if (level == 4)
                    level_data.map_features.Add((typeof(MFCaveIrchBossRoom),1,1));
                if (level == 9)
                    level_data.map_features.Add((typeof(MFCaveTrollBossRoom),1,1));

                if (level <= 4)
                {
                    level_data.dimensions = (128, 64);
                    level_data.difficulty_level = level + 1;
                    level_data.number_of_rooms = (40, 50);
                    level_data.number_of_encounters = (10, 15);
                    level_data.number_of_gold_items = (5, 10);

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHealthPotion), prob_amount = {(1.0f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAlmondBun), prob_amount = {(1.0f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemStaminaPotion), prob_amount = {(1.0f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemMeatHorn), prob_amount = {(1.0f, 1)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemRepairPowder), prob_amount = {(.8f, 0),(.2f, 1)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHammer1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemMace1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFlail1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemWarHammer2H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHandAxe1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemDoubleAxe1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPickaxe1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemBattleAxe2H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemBootsHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemChestHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHandsHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                    
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHeadHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemShieldHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemShieldMedium), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemRing), prob_amount = {(.70f, 0),(.25f, 1),(.05f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAmulet), prob_amount = {(.70f, 0),(.25f, 1),(.05f, 2)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFirebomb), prob_amount = {(.25f, 0), (.5f, 1), (.25f, 2)}});                
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemThrowingKnife), prob_amount = {(.25f, 0), (.5f, 1), (.25f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAcidFlask), prob_amount = {(.25f, 0), (.5f, 1), (.25f, 2)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfReturn), prob_amount = {(.7f, 0), (.2f, 1), (.1f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfAWalk), prob_amount = {(.7f, 0), (.2f, 1), (.1f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfAJourney), prob_amount = {(.7f, 0), (.2f, 1), (.1f, 2)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemCamomileTea), prob_amount = {(.95f, 0), (.05f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPeppermintTea), prob_amount = {(.95f, 0), (.05f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemStrawberryTea), prob_amount = {(.95f, 0), (.05f, 1)}});

                    level_data.dynamic_objects = new()
                    {
                        (typeof(Crate), 5, 10),
                        (typeof(Jar), 5, 10),
                        (typeof(BrokenCrate), 5, 10),
                        (typeof(Chest), 0, 1),
                        (typeof(BearTrap), 5, 10),
                        (typeof(SpiderWebTrap), 10, 20),
                    };
                }
                else 
                {
                    level_data.dimensions = (200, 100);
                    level_data.difficulty_level = level + 1;
                    level_data.number_of_rooms = (10, 15);
                    level_data.number_of_encounters = (20, 30);
                    level_data.number_of_gold_items = (10, 15);

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHealthPotion), prob_amount = {(0.3f, 1), (0.4f, 2), (0.3f, 3)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAlmondBun), prob_amount = {(0.3f, 1), (0.4f, 2), (0.3f, 3)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemStaminaPotion), prob_amount = {(0.3f, 1), (0.4f, 2), (0.3f, 3)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemMeatHorn), prob_amount = {(0.3f, 1), (0.4f, 2), (0.3f, 3)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemRepairPowder), prob_amount = {(.8f, 0),(.2f, 1)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHammer1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemMace1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFlail1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemWarHammer2H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHandAxe1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemDoubleAxe1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPickaxe1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemBattleAxe2H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemBootsHeavy), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemChestHeavy), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHandsHeavy), prob_amount ={(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHeadHeavy), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemShieldHeavy), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemShieldMedium), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});


                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemRing), prob_amount = {(.25f, 0),(.5f, 1),(.25f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAmulet), prob_amount = {(.25f, 0),(.5f, 1),(.25f, 2)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFirebomb), prob_amount = {(.1f, 0), (.2f, 1), (.5f, 2), (.2f, 3)}});                
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemThrowingKnife), prob_amount = {(.1f, 0), (.2f, 1), (.5f, 2), (.2f, 3)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAcidFlask), prob_amount = {(.1f, 0), (.2f, 1), (.5f, 2), (.2f, 3)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfReturn), prob_amount = {(.5f, 0), (.4f, 1), (.1f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfAWalk), prob_amount = {(.5f, 0), (.4f, 1), (.1f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfAJourney), prob_amount = {(.5f, 0), (.4f, 1), (.1f, 2)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFluteOfHealing), prob_amount = {(.9f, 0), (.1f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFluteOfEndurance), prob_amount = {(.9f, 0), (.1f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFluteOfBraveness), prob_amount = {(.9f, 0), (.1f, 1)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemCamomileTea), prob_amount = {(.95f, 0), (.05f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPeppermintTea), prob_amount = {(.95f, 0), (.05f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemStrawberryTea), prob_amount = {(.95f, 0), (.05f, 1)}});

                    level_data.dynamic_objects = new()
                    {
                        (typeof(Crate), 10, 20),
                        (typeof(Jar), 10, 20),
                        (typeof(BrokenCrate), 10, 20),
                        (typeof(Chest), 1, 2),
                        (typeof(BearTrap), 10, 20),
                        (typeof(SpiderWebTrap), 20, 40),
                    };
                }

                if (level == 0)
                {
                    level_data.dungeon_changes.Add
                    (
                        new DungeonChangeData
                        {
                            name = "World Map Exit",
                            dungeon_change_type = typeof(MFCaveExit),
                            target_dungeon_name = "World Map",
                            target_entrance_name = "Sewers Entrance " + i,
                            target_entrance_parameter = "Up",
                        }
                    );
                }

                if (level < 9)
                    level_data.dungeon_changes.Add
                    (
                        new DungeonChangeData
                        {
                            name = "Level " + (level + 2) + " Enter",
                            dungeon_change_type = typeof(MFCaveExit),
                            target_dungeon_name = "The Sewers " + i,
                            target_entrance_name = "Level " + (level + 2) + " Exit",
                            target_entrance_parameter = "Down",
                        }
                    );

                if (level > 0)
                    level_data.dungeon_changes.Add
                    (
                        new DungeonChangeData
                        {
                            name = "Level " + (level + 1) + " Exit",
                            dungeon_change_type = typeof(MFCaveExit),
                            target_dungeon_name = "The Sewers " + i,
                            target_entrance_name = "Level " + (level + 1) + " Enter",
                            target_entrance_parameter = "Up",
                        }
                    );

                sewers.dungeon_levels.Add(level_data);
            }
        }

        for (int i = 0; i < number_of_dungeons_per_type; ++i)
        {
            DungeonData castle = new DungeonData()        
            {
                name = "The Castle " + i,
            };
            dungeons.Add(castle);

            for (int level = 0; level < 10; ++level)
            {
                DungeonLevelData level_data = new DungeonLevelData
                {
                    biome_index = 4,
                    //is_always_visible = true,

                    map_features =
                    {                    
                        
                        //(typeof(MFCaveTreasureRoom), 0, 1), 
                        //(typeof(MFCaveOilRoom), 0, 2),                                           
                    },                
                };

                if (level >= 2)
                    level_data.map_features.Add((typeof(MFCavePoisonFlowerRoom), 0, 1));

                if (UnityEngine.Random.value < 0.1f)
                    level_data.map_features.Add((typeof(MFCaveStoreConsumables), 1,1));

                if (UnityEngine.Random.value < 0.2f)
                    level_data.map_features.Add((typeof(MFCaveStorageRoom), 1, 1));

                if (UnityEngine.Random.value < 0.1f)
                    level_data.map_features.Add((typeof(MFCaveStoreUsables), 1,1));

                if (UnityEngine.Random.value < 0.1f)
                    level_data.map_features.Add((typeof(MFCaveSpiderRoom), 0, 1));


                if (level > 0 && level <= 6 && UnityEngine.Random.value < 0.1f)
                    level_data.map_features.Add((typeof(MFCaveMonsterLair), 1, 1));

                //Add bosses
                if (level == 4)
                    level_data.map_features.Add((typeof(MFCaveIrchBossRoom),1,1));
                if (level == 9)
                    level_data.map_features.Add((typeof(MFCaveTrollBossRoom),1,1));

                if (level <= 4)
                {
                    level_data.dimensions = (128, 64);
                    level_data.difficulty_level = level + 1;
                    level_data.number_of_rooms = (40, 50);
                    level_data.number_of_encounters = (10, 15);
                    level_data.number_of_gold_items = (5, 10);

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHealthPotion), prob_amount = {(1.0f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAlmondBun), prob_amount = {(1.0f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemStaminaPotion), prob_amount = {(1.0f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemMeatHorn), prob_amount = {(1.0f, 1)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemRepairPowder), prob_amount = {(.8f, 0),(.2f, 1)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHammer1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemMace1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFlail1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemWarHammer2H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHandAxe1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemDoubleAxe1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPickaxe1H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemBattleAxe2H), prob_amount = {(.2f, 0),(.3f, 1),(.2f, 2), (.2f, 3), (.1f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemBootsHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemChestHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHandsHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                    
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHeadHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemShieldHeavy), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemShieldMedium), prob_amount = {(.1f, 0),(.2f, 1),(.3f, 2), (.2f, 3), (.2f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemRing), prob_amount = {(.70f, 0),(.25f, 1),(.05f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAmulet), prob_amount = {(.70f, 0),(.25f, 1),(.05f, 2)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFirebomb), prob_amount = {(.25f, 0), (.5f, 1), (.25f, 2)}});                
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemThrowingKnife), prob_amount = {(.25f, 0), (.5f, 1), (.25f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAcidFlask), prob_amount = {(.25f, 0), (.5f, 1), (.25f, 2)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfReturn), prob_amount = {(.7f, 0), (.2f, 1), (.1f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfAWalk), prob_amount = {(.7f, 0), (.2f, 1), (.1f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfAJourney), prob_amount = {(.7f, 0), (.2f, 1), (.1f, 2)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemCamomileTea), prob_amount = {(.95f, 0), (.05f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPeppermintTea), prob_amount = {(.95f, 0), (.05f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemStrawberryTea), prob_amount = {(.95f, 0), (.05f, 1)}});

                    level_data.dynamic_objects = new()
                    {
                        (typeof(Crate), 5, 10),
                        (typeof(Jar), 5, 10),
                        (typeof(BrokenCrate), 5, 10),
                        (typeof(Chest), 0, 1),
                        (typeof(BearTrap), 5, 10),
                        (typeof(SpiderWebTrap), 10, 20),
                    };
                }
                else 
                {
                    level_data.dimensions = (200, 100);
                    level_data.difficulty_level = level + 1;
                    level_data.number_of_rooms = (10, 15);
                    level_data.number_of_encounters = (20, 30);
                    level_data.number_of_gold_items = (10, 15);

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHealthPotion), prob_amount = {(0.3f, 1), (0.4f, 2), (0.3f, 3)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAlmondBun), prob_amount = {(0.3f, 1), (0.4f, 2), (0.3f, 3)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemStaminaPotion), prob_amount = {(0.3f, 1), (0.4f, 2), (0.3f, 3)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemMeatHorn), prob_amount = {(0.3f, 1), (0.4f, 2), (0.3f, 3)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemRepairPowder), prob_amount = {(.8f, 0),(.2f, 1)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHammer1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemMace1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFlail1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemWarHammer2H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHandAxe1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemDoubleAxe1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPickaxe1H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemBattleAxe2H), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemBootsHeavy), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemChestHeavy), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHandsHeavy), prob_amount ={(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemHeadHeavy), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemShieldHeavy), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemShieldMedium), prob_amount = {(.1f, 0),(.1f, 1),(.2f, 2), (.3f, 3), (.3f, 4)}});


                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemRing), prob_amount = {(.25f, 0),(.5f, 1),(.25f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAmulet), prob_amount = {(.25f, 0),(.5f, 1),(.25f, 2)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFirebomb), prob_amount = {(.1f, 0), (.2f, 1), (.5f, 2), (.2f, 3)}});                
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemThrowingKnife), prob_amount = {(.1f, 0), (.2f, 1), (.5f, 2), (.2f, 3)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemAcidFlask), prob_amount = {(.1f, 0), (.2f, 1), (.5f, 2), (.2f, 3)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfReturn), prob_amount = {(.5f, 0), (.4f, 1), (.1f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfAWalk), prob_amount = {(.5f, 0), (.4f, 1), (.1f, 2)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPoemOfAJourney), prob_amount = {(.5f, 0), (.4f, 1), (.1f, 2)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFluteOfHealing), prob_amount = {(.9f, 0), (.1f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFluteOfEndurance), prob_amount = {(.9f, 0), (.1f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemFluteOfBraveness), prob_amount = {(.9f, 0), (.1f, 1)}});

                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemCamomileTea), prob_amount = {(.95f, 0), (.05f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemPeppermintTea), prob_amount = {(.95f, 0), (.05f, 1)}});
                    level_data.items.Add(new ItemPlacementData(){type = typeof(ItemStrawberryTea), prob_amount = {(.95f, 0), (.05f, 1)}});

                    level_data.dynamic_objects = new()
                    {
                        (typeof(Crate), 10, 20),
                        (typeof(Jar), 10, 20),
                        (typeof(BrokenCrate), 10, 20),
                        (typeof(Chest), 1, 2),
                        (typeof(BearTrap), 10, 20),
                        (typeof(SpiderWebTrap), 20, 40),
                    };
                }

                if (level == 0)
                {
                    level_data.dungeon_changes.Add
                    (
                        new DungeonChangeData
                        {
                            name = "World Map Exit",
                            dungeon_change_type = typeof(MFCaveExit),
                            target_dungeon_name = "World Map",
                            target_entrance_name = "Castle Entrance " + i,
                            target_entrance_parameter = "Up",
                        }
                    );
                }

                if (level < 9)
                    level_data.dungeon_changes.Add
                    (
                        new DungeonChangeData
                        {
                            name = "Level " + (level + 2) + " Enter",
                            dungeon_change_type = typeof(MFCaveExit),
                            target_dungeon_name = "The Castle " + i,
                            target_entrance_name = "Level " + (level + 2) + " Exit",
                            target_entrance_parameter = "Down",
                        }
                    );

                if (level > 0)
                    level_data.dungeon_changes.Add
                    (
                        new DungeonChangeData
                        {
                            name = "Level " + (level + 1) + " Exit",
                            dungeon_change_type = typeof(MFCaveExit),
                            target_dungeon_name = "The Castle " + i,
                            target_entrance_name = "Level " + (level + 1) + " Enter",
                            target_entrance_parameter = "Up",
                        }
                    );

                castle.dungeon_levels.Add(level_data);
            }
        }
    
        world_map_index = 0;
        
        QuestData main_quest = new QDMain();
        main_quest.GenerateQuest(1, QuestComplexity.Long);
        player_data.AddQuest(main_quest);

        current_dungeon = dungeons[1];
        current_dungeon.SetRegenerationNeeded();
        current_map_level = dungeons[1].GetMapLevelData(0);
        current_dungeon.RegenerateLevel(current_map_level);

        current_map = dungeons[1].GetMapData(0);

        current_map.actors.Insert(1,player_data);
        current_map.visited = true;
        current_map.tick_entered = true;

        foreach(MapFeatureData feature in current_map.features)
        {
            if (feature is MFChangeDungeon)
            {
                MFChangeDungeon change_feature = (MFChangeDungeon)feature;
                if (change_feature.dungeon_change_data.name == "Cave Entrance")
                {
                    player_data.MoveTo(change_feature.exit_tile.x, change_feature.exit_tile.y);                    
                    break;
                }
            }
        }


    }
}

