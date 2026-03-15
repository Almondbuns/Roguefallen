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
            startNodeId: "well_intro",
            revisitedStartNodeId: "well_revisited",
            nodes: new[] {
                new DialogueNode(
                    id: "well_intro",
                    actor: "The Well",
                    text: "  'Hello? Was that a footstep? I think it was a footstep. It sounded like a footstep. I wonder what feet must feel like.'",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("... hello?", "well_topics"),
                        new DialogueChoice("[Ignore it]", "end"),
                    }
                ),
                new DialogueNode(
                    id: "well_revisited",
                    actor: "The Well",
                    text: "  'Ah, you have returned. Did you pick some flowers? I think they would be lovely on my little wall.'",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("I have some more questions.", "well_topics"),
                        new DialogueChoice("[Ignore it]", "end"),
                    }
                ),
                new DialogueNode(
                    id: "exit",
                    actor: "The Well",
                    text: "  '... Sigh ... I guess it was nice while it lasted. If you find out what you are ... please come back and tell me.'",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("Sure...", "end"),
                    }
                ),
                 new DialogueNode(
                    id: "well_topics",
                    actor: "The Well",
                    text: "  'You seem to be puzzled. It is a good state of mind.'",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("Who is down there?", "down_there", null, new NPCSetFlagEffect("The Well","test")),
                        new DialogueChoice("Are you stuck in the well?", "stuck_inside", new NPCFlagCheck("The Well","test")),
                        new DialogueChoice("Sorry, I have to go.", "exit"),
                    }
                ),
                new DialogueNode(
                    id: "down_there",
                    actor: "The Well",
                    text: "  'Down there? ... I am not sure. You mean: inside of me? I don't think somebody is inside of me. It feels pretty empty.'",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("I see.", "well_topics"),
                    }
                ),
                new DialogueNode(
                    id: "stuck_inside",
                    actor: "The Well",
                    text: "  'Stuck? I don't think so. I am the well ... or the well is me. I don't really remember much. I just know that it is better this way.'",
                    choices: new List<DialogueChoice> {
                        new DialogueChoice("I see.", "well_topics"),
                    }
                ),
            }
        );
    }
}