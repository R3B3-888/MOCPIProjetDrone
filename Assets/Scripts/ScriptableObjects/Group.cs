using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public GroupAgent agentPrefab;
    List<GroupAgent> agents = new List<GroupAgent>();

    [Range(5, 150)]
    public int startingCount = 20;
    const float AgentDensity = 0.9f;

    [Range(0, 5)]
    public int willDrownAgent = 0;

    List<GroupAgent> nonDrowningAgents;
    private int drowningAgent = 0;


    GroupAgent spawnInSemiCircle()
    {
        GroupAgent newAgent;
        Vector3 pos = Random.insideUnitCircle * startingCount * AgentDensity;
        pos.Set(-Mathf.Abs(pos.x), 0, pos.y);
        if (startingCount < 10) pos *= 3;
        newAgent = Instantiate(
            agentPrefab,
            pos + transform.position,
            Quaternion.LookRotation(new Vector3(-1f, 0, 0), Vector3.up),
            transform
            );
        return newAgent;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < startingCount; i++)
        {
            GroupAgent newAgent = spawnInSemiCircle();

            newAgent.name = "Agent " + i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
        nonDrowningAgents = new List<GroupAgent>(agents);
    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        int nonDrowningSwimmers = startingCount - drowningAgent;
        if( nonDrowningSwimmers > 0)
        {
            if(willDrownAgent > 0 && willDrownAgent <= nonDrowningSwimmers)
            {
                int rand = 0;
                for(int i = 0; i < willDrownAgent; i++)
                {
                    rand = (int) Random.Range(0f, nonDrowningSwimmers * 1f);
                    Drown(rand);
                }
                willDrownAgent = 0;
            }
        }
    }

    public void Drown(int val)
    {
        if(val < nonDrowningAgents.Count)
        {
            nonDrowningAgents[val].drown();
            nonDrowningAgents.RemoveAt(val);
        }
    }

    public void Rescue(GroupAgent agent)
    {
        agent.rescue();
        nonDrowningAgents.Add(agent);
    }
}
