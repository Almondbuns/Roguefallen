using System;
using System.Collections.Generic;
using System.Linq;

public class DialogueChoice
{
    public string text;
    public string next_id;

    public GameConditionPrototype condition;
    public GameEffectPrototype effect;

    public DialogueChoice(string text, string nextId, GameConditionPrototype condition = null, GameEffectPrototype effect = null)
    {
        this.text = text;
        next_id = nextId;
        this.condition = condition;
        this.effect = effect;
    }

    public bool IsAvailable()
    {
        if (condition == null) return true;
        return condition.Evaluate();
    }
    public void ApplyEffect()
    {
        if (effect == null) return;
        effect.ApplyEffect();
    }
}

public class DialogueNode
{
    public string id;
    public string actor;
    public string text;
    public List<DialogueChoice> choices;

    public DialogueNode(string id, string actor, string text, List<DialogueChoice> choices)
    {
        this.id = id;
        this.actor = actor;
        this.text = text;
        this.choices = choices;
    }
}

public class DialogueTree
{
    public Dictionary<string, DialogueNode> nodes;

    public string start_node_id;
    public string revisited_start_node_id;

    public DialogueTree(string startNodeId, string revisitedStartNodeId, IEnumerable<DialogueNode> nodes)
    {
        start_node_id = startNodeId;
        revisited_start_node_id = revisitedStartNodeId;
        this.nodes = nodes.ToDictionary(n => n.id, n => n);
    }

    public DialogueNode GetNode(string id)
    {
        return nodes.TryGetValue(id, out var node) ? node : null;
    }
}
