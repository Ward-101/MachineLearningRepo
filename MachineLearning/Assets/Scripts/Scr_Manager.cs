using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scr_Manager : MonoBehaviour
{
    public static Scr_Manager instance = null;

    public int populationSize = 100;
    public float trainingDuration = 30;
    public float mutation = 5;

    public Scr_Agent agentPrefab;
    public Transform agentGroup;
    private int currentGeneration = 0;
    public Text currentGenText;
    public Text trainingTimeText;

    private Scr_Agent agent;
    List<Scr_Agent> agents = new List<Scr_Agent>();

    public Scr_CameraController cameraController;

    public GameObject playerCar; // ADDED FOR VERSUS MODE //

    [HideInInspector] public bool isVersusModeOn; // ADDED FOR VERSUS MODE //

    private void Awake()
    {
        if (instance == null) instance = this;
    }

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

        LoadLoadout();

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

    public void LoadLoadout()
    {
        Scr_LoadoutData loadoutData = Scr_DataManager.instance.LoadLoadout();

        if (loadoutData != null)
        {
            for (int i = 0; i < agents.Count; i++)
            {
                agents[i].rb.mass = loadoutData.loadout.slidersValues[0];
                agents[i].carController.motorForce = loadoutData.loadout.slidersValues[1];

                agents[i].carController.wheelFrontRightCollider.mass = loadoutData.loadout.slidersValues[2];
                agents[i].carController.wheelFrontRightCollider.wheelDampingRate = loadoutData.loadout.slidersValues[3];
                agents[i].carController.wheelFrontRightCollider.forceAppPointDistance = loadoutData.loadout.slidersValues[4];

                agents[i].carController.wheelFrontLeftCollider.mass = loadoutData.loadout.slidersValues[2];
                agents[i].carController.wheelFrontLeftCollider.wheelDampingRate = loadoutData.loadout.slidersValues[3];
                agents[i].carController.wheelFrontLeftCollider.forceAppPointDistance = loadoutData.loadout.slidersValues[4];

                agents[i].carController.wheelBackRightCollider.mass = loadoutData.loadout.slidersValues[2];
                agents[i].carController.wheelBackRightCollider.wheelDampingRate = loadoutData.loadout.slidersValues[3];
                agents[i].carController.wheelBackRightCollider.forceAppPointDistance = loadoutData.loadout.slidersValues[4];

                agents[i].carController.wheelBackLeftCollider.mass = loadoutData.loadout.slidersValues[2];
                agents[i].carController.wheelBackLeftCollider.wheelDampingRate = loadoutData.loadout.slidersValues[3];
                agents[i].carController.wheelBackLeftCollider.forceAppPointDistance = loadoutData.loadout.slidersValues[4];

                agents[i].carController.driveType = loadoutData.loadout.driveTypeIndex;
            }

            
        }
    }

    private void InitNeuralNetworkViewer()
    {
        NeuralNetworkViewer.instance.Init(agents[0]);
    }

    private void SetUIText()
    {
        trainingTimeText.text = "Training Duration : " + trainingDuration.ToString();

        currentGeneration++;
        currentGenText.text = "Generation : " + currentGeneration;
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

        PlayerLoadLoadout();

        
    }

    public void PlayerLoadLoadout()
    {
        Scr_LoadoutData loadoutData = Scr_DataManager.instance.LoadLoadout();

        if (loadoutData != null)
        {
            playerCar.GetComponent<Rigidbody>().mass = loadoutData.loadout.slidersValues[0];
            playerCar.GetComponent<Scr_CarController>().motorForce = loadoutData.loadout.slidersValues[1];

            playerCar.transform.GetChild(4).GetComponent<WheelCollider>().mass = loadoutData.loadout.slidersValues[2];
            playerCar.transform.GetChild(4).GetComponent<WheelCollider>().wheelDampingRate = loadoutData.loadout.slidersValues[3];
            playerCar.transform.GetChild(4).GetComponent<WheelCollider>().forceAppPointDistance = loadoutData.loadout.slidersValues[4];

            playerCar.transform.GetChild(5).GetComponent<WheelCollider>().mass = loadoutData.loadout.slidersValues[2];
            playerCar.transform.GetChild(5).GetComponent<WheelCollider>().wheelDampingRate = loadoutData.loadout.slidersValues[3];
            playerCar.transform.GetChild(5).GetComponent<WheelCollider>().forceAppPointDistance = loadoutData.loadout.slidersValues[4];

            playerCar.transform.GetChild(6).GetComponent<WheelCollider>().mass = loadoutData.loadout.slidersValues[2];
            playerCar.transform.GetChild(6).GetComponent<WheelCollider>().wheelDampingRate = loadoutData.loadout.slidersValues[3];
            playerCar.transform.GetChild(6).GetComponent<WheelCollider>().forceAppPointDistance = loadoutData.loadout.slidersValues[4];

            playerCar.transform.GetChild(7).GetComponent<WheelCollider>().mass = loadoutData.loadout.slidersValues[2];
            playerCar.transform.GetChild(7).GetComponent<WheelCollider>().wheelDampingRate = loadoutData.loadout.slidersValues[3];
            playerCar.transform.GetChild(7).GetComponent<WheelCollider>().forceAppPointDistance = loadoutData.loadout.slidersValues[4];

            playerCar.GetComponent<Scr_CarController>().driveType = loadoutData.loadout.driveTypeIndex;


        }
    }

    private void VersusFocus()
    {
        NeuralNetworkViewer.instance.agent = agents[0];
        NeuralNetworkViewer.instance.RefreshAxon();
        cameraController.target = playerCar.transform;
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
        List<Scr_NeuralNetwork> nets = new List<Scr_NeuralNetwork>();

        nets.Add(agents[0].net);

        Scr_DataManager.instance.SaveChampion(nets);
    }

    public void championLoad()
    {
        Scr_Data championData = Scr_DataManager.instance.LoadChampion();

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

    #region Simon

    public void AddTrainingTime()
    {
        trainingDuration += 5;
    }

    public void RemoveTrainingTime()
    {
        if (trainingDuration > 5)
            trainingDuration -= 5;
    }

    #endregion
}

