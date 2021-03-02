using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scr_Manager : MonoBehaviour
{
    public int populationSize = 100;
    public float trainingDuration = 30;
    public float mutation = 5;

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
        NewGenation();
        Focus();

        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(LoopCoroutine());
    }

    private IEnumerator LoopCoroutine()
    {
        NewGenation();
        Focus();

        yield return new WaitForSeconds(trainingDuration);
    }

    private void NewGenation()
    {
        agents.Sort();

        AddOrRemoveAgent();

        Mutate();

        ResetAgents();

        SetColors();
    }

    private void AddOrRemoveAgent()
    {
        if (agents.Count != populationSize)
        {
            int dif = populationSize - agents.Count;

            if (dif > 0)
            {
                for (int i = 0; i < dif; i++)
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

    private void AddAgent()
    {
        agent = Instantiate(agentPrefab, Vector3.zero, Quaternion.identity, agentGroup);
        agent.net = new Scr_NeuralNetwork(agent.net.layers);

        agents.Add(agent);
    }

    private void RemoveAgent()
    {
        Destroy(agents[agents.Count - 1].transform);

        agents.RemoveAt(agents.Count - 1);
    }

    private void Focus()
    {
        //pour mercredi

        cameraController.target = (agents[0].transform);
    }

    private void Mutate()
    {
        for (int i = agents.Count / 2; i < agents.Count; i++)
        {
            agents[i].net.CopyNet(agents[i - agents.Count / 2].net);
            agents[i].net.Mutate(mutation);
            agents[i].SetMutatedColor();
        }
    }

    private void ResetAgents()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].ResetAgent();
        }
    }

    private void SetColors()
    {
        for (int i = 0; i < agents.Count / 2; i++)
        {
            agents[i].SetDefaultColor();
        }

        agents[0].SetFirstColor();
    }

    public void End()
    {
        StopAllCoroutines();
        StartCoroutine(LoopCoroutine());
    }

    public void ResetNets()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].net = new Scr_NeuralNetwork(agent.net.layers);
        }

        End();
    }
}

