using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scr_Agent : MonoBehaviour , IComparable<Scr_Agent>
{
    public float fitness;
    public float distanceTraveled;

    public Scr_NeuralNetwork net;
    public Scr_CarController carController;

    public Transform nextCheckpoint;
    public float nextCheckpointDist;
    private float timer;

    public float[] inputs;

    public Rigidbody rb;

    private RaycastHit hit;
    public float rayRange = 5;

    public LayerMask layerMask;

    public Material firstMat;
    public Material defaultMat;
    public Material mutatedMat;
    public Material championMat;

    public MeshRenderer mapFeedbackRenderer;

    public void ResetAgent()
    {
        fitness = 0f;
        distanceTraveled = 0f;
        timer = 0f;
        tempSpeed = 0f;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        inputs = new float[net.layers[0]];

        carController.ResetInput();

        nextCheckpoint = Scr_CheckpointManager.instance.firstCheckpoint;
        nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;
    }

    public void FixedUpdate()
    {
        timer += Time.deltaTime;

        InputUpdate();
        OutUpdate();
        FitnessUpdate();
    }

    private void InputUpdate()
    {
        inputs[0] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward, 4f);
        inputs[1] = RaySensor(transform.position + Vector3.up * 0.2f, transform.right, 1.5f);
        inputs[2] = RaySensor(transform.position + Vector3.up * 0.2f, -transform.right, 1.5f);
        inputs[3] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward + transform.right, 2f);
        inputs[4] = RaySensor(transform.position + Vector3.up * 0.2f, transform.forward - transform.right, 2f);

        inputs[5] = (float)Math.Tanh(rb.velocity.magnitude * 0.05f);
        inputs[6] = (float)Math.Tanh(rb.angularVelocity.y * 0.1f);

        inputs[7] = 1;
    }

    float RaySensor(Vector3 pos, Vector3 dir, float lengh)
    {
        if (Physics.Raycast(pos, dir, out hit, lengh * rayRange, layerMask))
        {
            Debug.DrawRay(pos, dir * hit.distance, Color.Lerp(Color.red, Color.green, (rayRange * lengh - hit.distance) / (rayRange * lengh)));

            return (rayRange * lengh - hit.distance)/(rayRange * lengh);
        }
        else
        {
            Debug.DrawRay(pos, dir * rayRange * lengh, Color.red);
        }
        return 0;
    }

    private void OutUpdate()
    {
        net.FeedForward(inputs);

        carController.horizontalInput = net.neurons[net.layers.Length - 1][0];
        carController.verticalInput = net.neurons[net.layers.Length - 1][1];
    }

    private float tempDistance;
    private float tempSpeed;

    private void FitnessUpdate()
    {
        tempDistance = distanceTraveled + (nextCheckpointDist - (transform.position - nextCheckpoint.position).magnitude);

        if (fitness < tempDistance + tempSpeed)
        {
            fitness = tempDistance + tempSpeed;
        }

    }

    public void CheckpointReached(Transform checkpoint)
    {
        tempSpeed += (nextCheckpointDist / timer) * 0.8f;
        timer = 0;
        distanceTraveled += nextCheckpointDist;
        nextCheckpoint = checkpoint;
        nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;

        //distanceTraveled += nextCheckpointDist;
        //nextCheckpoint = checkpoint;
        //nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;
    }

    public void SetFirstColor()
    {
        GetComponent<MeshRenderer>().material = firstMat;
        mapFeedbackRenderer.material = firstMat;
    }

    public void SetDefaultColor()
    {
        GetComponent<MeshRenderer>().material = defaultMat;
        mapFeedbackRenderer.material = defaultMat;
    }

    public void SetMutatedColor()
    {
        GetComponent<MeshRenderer>().material = mutatedMat;
        mapFeedbackRenderer.material = mutatedMat;
    }

    public void SetChampionColor()
    {
        GetComponent<MeshRenderer>().material = championMat;
        mapFeedbackRenderer.material = championMat;
    }

    public int CompareTo(Scr_Agent other)
    {
        if (fitness < other.fitness)
        {
            return 1;
        }

        if(fitness > other.fitness)
        {
            return -1;
        }

        return 0;
    }
}
