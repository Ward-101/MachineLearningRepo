using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Scr_Manager : MonoBehaviour
{
    public int populationSize = 100;
    public float trainingDuration = 30;
    public float mutationRate = 5;

    public Scr_Agent agentPrefab;
    public Transform agentGroup;

    Scr_Agent agent;
    List<Scr_Agent> agents = new List<Scr_Agent>();

    public Scr_CameraController cameraController;

    private void Start()
    {
        StartCoroutine(InitCoroutine());
    }

    private IEnumerator InitCoroutine()
    {
        NewGeneration();
        Focus();

        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        NewGeneration();
        Focus();
        
        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(Loop());
    }

    void NewGeneration()
    {
        agents.Sort();
        AddOrRemoveAgent();
        Mutate();
    }

    void AddOrRemoveAgent()
    {
        if (agents.Count != populationSize)
        {
            int dif = populationSize - agents.Count;

            if (dif > 0)
            {
                for (int i = 0; i < dif, i++)
                {
                    AddAgent();
                }
            }
            else
            {
                for (int i = 0; i < -dif; i++)
                {
                    RemoveAgent();
                }
            }
        }
    }

    void AddAgent()
    {
        agent = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentGroup);
        agent.net = new Scr_NeuralNetwork(agent.net.layers);

        agents.Add(agent);
    }

    void RemoveAgent()
    {
        Destroy(agents[agents.Count - 1].transform);
        agents.RemoveAt(agents.Count - 1);
    }

    void Focus()
    {
        
        cameraController.target = (agents[0].transform);
    }

    void Mutate()
    {
        for (int i = agents.Count/2; i < agents.Count; i++)
        {
            agents[i].net.CopyNet(agents[i - (agents.Count / 2)].net);
        }
    }
}
