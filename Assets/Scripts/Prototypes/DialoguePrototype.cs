using System;
using System.Collections.Generic;
using System.Linq;

public class DialogueChoice
{
    public string text;
    public string next_id;

    public DialogueChoice(string text, string nextId)
    {
        this.text = text;
        next_id = nextId;
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

    public DialogueTree(string startNodeId, IEnumerable<DialogueNode> nodes)
    {
        start_node_id = startNodeId;
        this.nodes = nodes.ToDictionary(n => n.id, n => n);
    }

    public DialogueNode GetNode(string id)
    {
        return nodes.TryGetValue(id, out var node) ? node : null;
    }
}


public class ActorDialogueRunner
{
    private readonly DialogueTree _tree;
   
}

