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
    }

    // Update is called once per frame
    // Update is called once per frame
    void Update()
    {
        foreach (GroupAgent agent in agents)
        {
            // chance to drown
        }
    }
}
