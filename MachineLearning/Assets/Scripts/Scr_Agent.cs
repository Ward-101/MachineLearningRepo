using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Scr_Agent : MonoBehaviour
{
    public Scr_CarController carController;
    
    public Scr_NeuralNetwork net;
    public float fitness;
    public float distanceTraveled;

    public float[] inputs;

    public Transform nextCheckpoint;
    public float nextCheckpointDist;

    public MeshRenderer mapFeedbackRenderer;
    public Material firstMat;
    public Material Default;
    public Material mutated;

    public Rigidbody rb;

    RaycastHit hit;
    public float rayRange = 5;

    public LayerMask layerMask;

    public void ResetAgent()
    {
        fitness = 0;
        distanceTraveled = 0;

        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        inputs = new float[net.layers[0]];

        //Scr_CarController.Reset();

        nextCheckpoint = Scr_CheckpointManager.instance.firstCheckpoint;
    }

    public void FixedUpdate()
    {
        InputUpdate();
        OutUpdate();
        FitnessUpdate();
    }

    void InputUpdate()
    {
        inputs[0] = RaySensor(transform.position + Vector3.up * 0.5f, transform.forward, 4f);
        inputs[1] = RaySensor(transform.position + Vector3.up * 0.5f, transform.right, 4f);
        inputs[2] = RaySensor(transform.position + Vector3.up * 0.5f, -transform.right, 4f);
        inputs[3] = RaySensor(transform.position + Vector3.up * 0.5f, transform.forward + transform.right, 4f);
        inputs[4] = RaySensor(transform.position + Vector3.up * 0.5f, transform.forward - transform.right, 4f);

        inputs[5] = (float)Math.Tanh(rb.velocity.magnitude * 0.05f);
        inputs[6] = (float)Math.Tanh(rb.angularVelocity.y * 0.01f);
    }

    float RaySensor(Vector3 pos, Vector3 direction, float length)
    {
        if (Physics.Raycast(pos, direction, out hit, length * rayRange, layerMask))
        {
            Debug.DrawRay(pos, direction * hit.distance, Color.Lerp(Color.red, Color.green, (rayRange * length - hit.distance) / (rayRange * length)));
            
            return (rayRange*length - hit.distance)/(rayRange*length);
        }
        else
        {
            Debug.DrawRay(pos, direction * rayRange * length, Color.red);
            return 0;
        }
    }
    
    public void OutUpdate()
    {
        net.FeedForward(inputs);

        carController.horizontalInput = net.neurons[net.layers.Length - 1][0];
        carController.verticalInput = net.neurons[net.layers.Length - 1][1];
    }

    float tempDistance;
    public void FitnessUpdate()
    {
        tempDistance = distanceTraveled + (nextCheckpointDist - (transform.position - nextCheckpoint.position).magnitude);

        if (fitness < tempDistance)
        {
            fitness = tempDistance;
        }
    }

    public void CheckpointReached(Transform checkpoint)
    {
        distanceTraveled += nextCheckpointDist;
        nextCheckpoint = checkpoint;
        nextCheckpointDist = (transform.position - nextCheckpoint.position).magnitude;
    }

    public void SetFirstColor()
    {
        GetComponent<MeshRenderer>().material = firstMat;
        mapFeedbackRenderer.material = firstMat;
    }

    public void SetDefaultColor()
    {
        GetComponent<MeshRenderer>().material = Default;
        mapFeedbackRenderer.material = Default;
    }

    public void SetMutatedColor()
    {
        GetComponent<MeshRenderer>().material = mutated;
        mapFeedbackRenderer.material = mutated;
    }

    public int CompareTo(Scr_Agent other)
    {
        if (fitness < other.fitness)
        {
            return 1;
        }
        
        if (fitness > other.fitness)
        {
            return -1;
        }
        return 0;
    }
}
