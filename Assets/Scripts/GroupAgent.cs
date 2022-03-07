using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class GroupAgent : MonoBehaviour
{
    Group agentGroup;
    public Group AgentGroup { get { return agentGroup; } }

    Animator agentAnimator;
    public Animator AgentAnimator { get { return agentAnimator; } }

    // Start is called before the first frame update
    void Start()
    {
        agentAnimator = GetComponent<Animator>();
    }

    public void Initialize(Group group)
    {
        agentGroup = group;
    }

    public void Move()
    {
        // move
    }
}
