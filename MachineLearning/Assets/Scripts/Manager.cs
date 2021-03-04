using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public int populationSize = 100;
    public float trainingDuration = 30;
    public float mutation = 5;

    public Agent agentPrefab;
    public Transform agentGroup;

    Agent agent;
    List<Agent> agents = new List<Agent>();

    public CameraController cameraController;

    public GameObject playerCar; // ADDED FOR VERSUS MODE //

    [HideInInspector]

    public bool isVersusModeOn; // ADDED FOR VERSUS MODE //

    public void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(InitCoroutine());
    }

    private IEnumerator InitCoroutine()
    {
        NewGenation();
        InitNeutralNetworkViewer();
        Focus();

        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        NewGenation();
        Focus();

        yield return new WaitForSeconds(trainingDuration);

        StartCoroutine(Loop());
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
        agent.net = new NeuralNetwork(agent.net.layers);

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

    private void VersusFocus()
    {
        NeuralNetworkViewer.instance.agent = agents[0];
        NeuralNetworkViewer.instance.RefreshAxon();
        cameraController.target = playerCar.transform;
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
        StartCoroutine(Loop());
        isVersusModeOn = false;
    }

    public void ResetNets()
    {
        for (int i = 0; i < agents.Count; i++)
        {
            agents[i].net = new NeuralNetwork(agent.net.layers);
        }

        End();
    }

    public void Save()
    {
        List<NeuralNetwork> nets = new List<NeuralNetwork>();

        for (int i = 0; i < agents.Count; i++)
        {
            nets.Add(agents[i].net);
        }

        DataManager.instance.Save(nets);
    }

    public void Load()
    {
        Data data = DataManager.instance.Load();

        if(data != null)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].net = data.nets[i];
            }
        }

        End();
    }

    void InitNeutralNetworkViewer()
    {
        NeuralNetworkViewer.instance.Init(agents[0]);
    }

    #region VERSUS MODE

    public void Versus()
    {
        isVersusModeOn = true;

        StopAllCoroutines();

        VersusGeneration();
        VersusFocus();
    }

    private void VersusGeneration()
    {
        agents.Sort();

        OnlyChampion();

        ResetAgents();

        VersusColor();

        playerCar.SetActive(true);
    }

    private void OnlyChampion()
    {
        for (int i = agents.Count - 1; i > 0; i--)
        {
            Destroy(agents[i].transform.gameObject);
            agents.RemoveAt(i);
        }
    }

    private void VersusColor()
    {
        agents[0].SetChampionColor();
    }

    public void championSave()
    {
        List<NeuralNetwork> nets = new List<NeuralNetwork>();

        nets.Add(agents[0].net);

        DataManager.instance.SaveChampion(nets);
    }

    public void championLoad()
    {
        Data championData = DataManager.instance.LoadChampion();

        if (championData != null)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].net = championData.nets[0];
            }
        }

        Versus();
    }

    #endregion
}

