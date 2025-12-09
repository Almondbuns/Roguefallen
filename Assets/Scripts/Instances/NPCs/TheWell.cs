using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheMysticWell : ActorPrototype
{
    public TheMysticWell(int level) : base(level)
    {
        name = "The Well";
        icon = "images/npc/mystic_well";
        prefab_index = 50;
        tile_width = 2;
        tile_height = 2;

        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.Passive,
            }
        };

        has_dialogue = true;

        stats.health_max = 1000;
        stats.stamina_max = 20;
        stats.mana_max = 20;
        stats.body_armor.Add(new ActorArmorStats { body_part = "Body", percentage = 100, armor = (10, 10, 10) });
        stats.movement_time = 100;
        stats.to_hit = 15;
        stats.dodge = 10;

        can_move = false;

        dialogue_tree = new DialogueTree(
            startNodeId: "ulrich_intro",
            revisitedStartNodeId: "ulrich_revisited",
            nodes: new[] {
                new DialogueNode(
                    id: "well_intro",
                    actor: "The Well",
                    text: "  *Sighs* Wonderful, a visitor. You followed the shiny mushrooms, didn’t you? Fine. Formalities first. I am Ulrich, yes, the so-called ‘last unicorn’. \n\n  This forest is cursed, moody, and generally unpleasant. A bit like me before breakfast. Nobody takes a stroll here anymore. So tell me: are you actually here to help or ... did you just get hopelessly lost?",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("Help? What seems to be the problem?", "end"),
                        new DialogueChoice("Are you really the last unicorn?", "last_unicorn"),
                        new DialogueChoice("Uh, I guess I took a wrong turn. Bye.", "end"),
                    }
                ),
                new DialogueNode(
                    id: "ulrich_revisited",
                    actor: "The Well",
                    text: "'Ah well, ...'",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("Seriously?", "end"),
                        new DialogueChoice("Who or what exactly are you?", "last_unicorn"),
                        new DialogueChoice("Well put. Bye.", "end"),
                    }
                ),
                new DialogueNode(
                    id: "last_unicorn",
                    actor: "The Well",
                    text: "  Oh stars, must we? Fine. No, I’m not the last of my kind. The name comes from … an incident. Or several.\n\n  Let’s just say I was not gifted in the art of running, and every race in my youth ended with me trailing behind everyone else. They started calling me ‘the last unicorn’ and it stuck. Unfortunately.\n\n  So yes, that’s the grand, humiliating truth. Can we please move on?",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("Sorry I asked. Jeez.", "ulrich_revisited"),
                    }
                ),
            }
        );
    }
}