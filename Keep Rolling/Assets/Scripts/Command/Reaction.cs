using UnityEngine;

public class Reaction : Command
{
    public ReactionEnum reaction;
    public float timeReaction;

    public Reaction(ReactionEnum reaction, float time)
    {
        this.reaction = reaction;
        this.timeReaction = time;
    }
    public override void Execute()
    {
        ReactionManager.instance.nextReaction = reaction;
        ReactionManager.instance.timeLeft = timeReaction;
    }

    public override void Undo()
    {
        throw new System.NotImplementedException();
    }
}