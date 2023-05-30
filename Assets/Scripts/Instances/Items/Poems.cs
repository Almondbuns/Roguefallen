using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPoemOfReturn : ItemPrototype
{
    public ItemPoemOfReturn(int level) : base(level)
    {
        name = "Poem Of Return";
        icon = "images/objects/poem";
        type = ItemType.CONSUMABLE;
        gold_value = 50;
        is_stackable = true;
        stack_max = 5;
        weight = 1;
        talents_when_consumed.Add(new ItemTalentReturn());
        start_amount_min = 1;
        start_amount_max = 2;
    }
}

public class ItemPoemOfAWalk : ItemPrototype
{
    public ItemPoemOfAWalk(int level) : base(level)
    {
        name = "Poem Of A Walk";
        icon = "images/objects/poem";
        type = ItemType.CONSUMABLE;
        gold_value = 50;
        is_stackable = true;
        stack_max = 5;
        weight = 1;
        talents_when_consumed.Add
        (
            new ItemTalentTeleportRandom()
            {
                name = name,
                icon = "images/objects/poem",
                cost_stamina = 5,
                prepare_time = 50,
                recover_time = 100,

                target_range = 8,

                prepare_message ="The <name> starts to read a poem.",
                action_message = "The <name> finished reading " + name + ".",

                description = "Teleports the reader to a random location within 8 tiles.",
            }
        );
        start_amount_min = 1;
        start_amount_max = 2;
    }
}

public class ItemPoemOfAJourney : ItemPrototype
{
    public ItemPoemOfAJourney(int level) : base(level)
    {
        name = "Poem Of A Journey";
        icon = "images/objects/poem";
        type = ItemType.CONSUMABLE;
        gold_value = 100;
        is_stackable = true;
        stack_max = 5;
        weight = 1;
        talents_when_consumed.Add
        (
            new ItemTalentTeleportRandom()
            {
                name = name,
                icon = "images/objects/poem",
                cost_stamina = 5,
                prepare_time = 50,
                recover_time = 100,

                target_range = 100,

                prepare_message ="The <name> starts to read a poem.",
                action_message = "The <name> finished reading " + name + ".",

                description = "Teleports the reader to a random location within 100 tiles.",
            }
        );
        start_amount_min = 1;
        start_amount_max = 2;
    }
}