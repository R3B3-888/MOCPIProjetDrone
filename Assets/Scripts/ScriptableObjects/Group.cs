using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    public GroupAgent agentPrefab;
    public List<GroupAgent> agents = new List<GroupAgent>();

    [Range(5, 150)]
    public int startingCount = 20;
    const float AgentDensity = 0.9f;

    [Range(0, 5)]
    public int willDrownAgent = 0;

    List<GroupAgent> nonDrowningAgents = new List<GroupAgent>();
    private int nonDrowningSwimmers;

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
            nonDrowningAgents.Add(newAgent);
        }
        agents = new List<GroupAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateNbToDrown(int nb){
        willDrownAgent = nb;
        nonDrowningSwimmers = nonDrowningAgents.Count;
    }

    public void Drown()
    {
        if(nonDrowningSwimmers > 0)
        {
            if(willDrownAgent > 0 && willDrownAgent <= nonDrowningSwimmers)
            {
                int rand = 0;
                GroupAgent agent;
                for(int i = 0; i < willDrownAgent; i++)
                {
                    rand = (int) Random.Range(0f, nonDrowningSwimmers * 1f);
                    agent = nonDrowningAgents[rand];
                    agent.drown();
                    agents.Add(agent);
                    nonDrowningAgents.RemoveAt(rand);
                }
                willDrownAgent = 0;
            }
        }
        
    }

    public void Rescue(GroupAgent agent)
    {
        agent.rescue();
        nonDrowningAgents.Add(agent);
    }
}
