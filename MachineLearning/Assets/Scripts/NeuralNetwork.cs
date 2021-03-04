using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class NeuralNetwork
{
    public int[] layers;
    public float[][] neurons;
    public float[][][] axons;

    private int x;
    private int y;
    private int z;

    public NeuralNetwork()
    {
    }

    public NeuralNetwork(int[] _layers)
    {
        layers = new int[_layers.Length];

        for (x = 0; x < _layers.Length; x ++)
        {
            layers[x] = _layers[x];
        }

        InitNeurons();

        InitAxons();
    }

    public void CopyNet(NeuralNetwork netCopy)
    {
        for (int x = 0; x < netCopy.axons.Length; x++)
        {
            for (int y = 0; y < netCopy.axons[x].Length; y++)
            {
                for (int z = 0; z < netCopy.axons[x][y].Length; z++)
                {
                    axons[x][y][z] = netCopy.axons[x][y][z];
                }
            }
        }
    }


    private void InitNeurons()
    {
        neurons = new float[layers.Length][];

        for (int x = 0; x < layers.Length; x++)
        {
            neurons[x] = new float[layers[x]];
        }

    }

    private void InitAxons()
    {
        axons = new float[layers.Length - 1][][];

        for (int x = 0; x < layers.Length - 1; x++)
        {
            axons[x] = new float[layers[x + 1]][];

            for (int y = 0; y < layers[x + 1]; y++)
            {
                axons[x][y] = new float[layers[x]];

                for (int z = 0; z < layers[x]; z++)
                {
                    axons[x][y][z] = UnityEngine.Random.Range(-1f, 1f);
                }
            }
        }
    }

    private float value;

    public void FeedForward(float[] inputs)
    {
        neurons[0] = inputs;

        for (int x = 1; x < layers.Length; x++)
        {
            for (int y = 0; y < layers[x]; y++)
            {
                value = 0;

                for (int z = 0; z < layers[x - 1]; z++)
                {
                    value += neurons[x - 1][z] * axons[x - 1][y][z];
                }

                neurons[x][y]= (float)Math.Tanh(value);
            }
        }
    }

    private float randomFloat;

    public void Mutate(float probabilty)
    {
        for (int x = 0; x < axons.Length; x++)
        {
            for (int y = 0; y < axons[x].Length; y++)
            {
                for (int z = 0; z < axons[x][y].Length; z++)
                {
                    randomFloat = UnityEngine.Random.Range(0f, 100f);

                    if (randomFloat < 0.06f * probabilty)
                    {
                        axons[x][y][z] = UnityEngine.Random.Range(-1f, 1f);
                    }
                    else if (randomFloat < 0.07f * probabilty)
                    {
                        axons[x][y][z] *= -1f;
                    }
                    else if (randomFloat < 0.5f * probabilty)
                    {
                        axons[x][y][z] += 0.1f * UnityEngine.Random.Range(-1f, -1f);
                    }
                    else if (randomFloat < 0.75f * probabilty)
                    {
                        axons[x][y][z] *= UnityEngine.Random.Range(0f, 1f) + 1f;
                    }
                    else if (randomFloat < 1.0f * probabilty)
                    {
                        axons[x][y][z] *= UnityEngine.Random.Range(0f, 1f);
                    }
                }
            }
        }
    }

}
