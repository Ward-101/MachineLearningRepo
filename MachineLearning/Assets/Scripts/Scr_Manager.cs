using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_Manager : MonoBehaviour
{
    public int populationSize = 100;
    public float trainingDuration = 30;
    public float mutation = 5;

    public Scr_Agent agentPrefab;
    public Transform agentGroup;
    private int currentGeneration = 0;
    public Text currentGenText;
    public Text currentTrainDurationText;
    public InputField trainingDurationField;

    private Scr_Agent agent;
    List<Scr_Agent> agents = new List<Scr_Agent>();

    public Scr_CameraController cameraController;

    private void Start()
    {
        StartCoroutine(InitCoroutine());
    }

    private IEnumerator InitCoroutine()
    {
        NewGenation();
        InitNeuralNetworkViewer();
        Focus();

        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(LoopCoroutine());
    }

    private IEnumerator LoopCoroutine()
    {
        NewGenation();
        Focus();

        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(LoopCoroutine());
    }

    private void NewGenation()
    {
        agents.Sort();

        AddOrRemoveAgent();

        Mutate();

        ResetAgents();

        SetColors();

        SetUIText();
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
        NeuralNetworkViewer.instance.agent = agents[0];
        NeuralNetworkViewer.instance.RefreshAxon();

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

    public void Save()
    {
        List<Scr_NeuralNetwork> nets = new List<Scr_NeuralNetwork>();

        for (int i = 0; i < agents.Count; i++)
        {
            nets.Add(agents[i].net);
        }

        Scr_DataManager.instance.Save(nets);
    }

    public void Load()
    {
        Scr_Data data = Scr_DataManager.instance.Load();

        if (data != null)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].net = data.nets[i];
            }
        }

        End();
    }

    private void InitNeuralNetworkViewer()
    {
        NeuralNetworkViewer.instance.Init(agents[0]);
    }

    private void SetUIText()
    {
        currentGeneration++;
        currentGenText.text = "Generation : " + currentGeneration;

        //if (trainingDurationField.text != null)
        //{
        //    trainingDuration = float.Parse(trainingDurationField.text);
        //    trainingDurationField.text = null;
        //}

        //currentTrainDurationText.text = "Current Training Duration : " + trainingDuration;
    }
}

