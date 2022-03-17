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

    private bool drowning;
    private bool rescued;

    // Start is called before the first frame update
    void Start()
    {
        agentAnimator = GetComponent<Animator>();
        agentAnimator.SetBool("isDrowning", false);
        agentAnimator.SetBool("isRescued", false);
    }

    public void Initialize(Group group)
    {
        agentGroup = group;
    }

    public void drown()
    {
        agentAnimator.SetBool("isDrowning", true);
        drowning = true;
    }

    public void rescue()
    {
        agentAnimator.SetBool("isRescued", true);
        agentAnimator.SetBool("isDrowning", false);
        rescued = true;
        drowning = false;
    }

    public bool isDrowning()
    {
        return drowning;
    }

    public bool isRescued()
    {
        return rescued;
    }
}
