using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnicornUlrich : ActorPrototype
{
    public UnicornUlrich(int level) : base(level)
    {
        name = "Ulrich, the last unicorn";
        icon = "images/npc/ulrich";
        prefab_index = 45;
        monster = new MonsterPrototype
        {
            ai_prototype = new AIPrototype
            {
                personality = AIPersonality.Passive,
            }
        };

        has_dialogue = true;

        stats.health_max = 50;
        stats.stamina_max = 20;
        stats.mana_max = 20;
        stats.body_armor.Add(new ActorArmorStats { body_part = "Body", percentage = 100, armor = (1, 1, 1) });
        stats.movement_time = 100;
        stats.to_hit = 15;
        stats.dodge = 10;

        dialogue_tree = new DialogueTree(
            startNodeId: "ulrich_intro",
            revisitedStartNodeId: "ulrich_revisited",
            nodes: new[] {
                new DialogueNode(
                    id: "ulrich_intro",
                    actor: "Ulrich, The Last Unicorn",
                    text: "  *Sighs* Wonderful, a visitor. You followed the shiny mushrooms, didn’t you? Fine. Formalities first. I am Ulrich, yes, the so-called ‘last unicorn’. \n\n  This forest is cursed, moody, and generally unpleasant. A bit like me before breakfast. Nobody takes a stroll here anymore. So tell me: are you actually here to help or ... did you just get hopelessly lost?",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("Help? What seems to be the problem?", "end"),
                        new DialogueChoice("Are you really the last unicorn?", "last_unicorn"),
                        new DialogueChoice("Uh, I guess I took a wrong turn. Bye.", "end"),
                    }
                ),
                new DialogueNode(
                    id: "ulrich_revisited",
                    actor: "Ulrich, The Last Unicorn",
                    text: "  *Sighs* Anything else?",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("What was this help thing all about?", "end"),
                        new DialogueChoice("Are you really the last unicorn?", "last_unicorn"),
                        new DialogueChoice("Uh, I guess I took a wrong turn. Bye.", "end"),
                    }
                ),
                new DialogueNode(
                    id: "last_unicorn",
                    actor: "Ulrich, The Last Unicorn",
                    text: "  Oh stars, must we? Fine. No, I’m not the last of my kind. The name comes from … an incident. Or several.\n\n  Let’s just say I was not gifted in the art of running, and every race in my youth ended with me trailing behind everyone else. They started calling me ‘the last unicorn’ and it stuck. Unfortunately.\n\n  So yes, that’s the grand, humiliating truth. Can we please move on?",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("Sorry I asked. Jeez.", "ulrich_revisited"),
                    }
                ),
            }
        );
    }
}