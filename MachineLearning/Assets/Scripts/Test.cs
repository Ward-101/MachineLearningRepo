using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Scr_Test : MonoBehaviour , IComparable<Scr_Test>
{
    private static Scr_Test instance;
    public List<int> listOfInt;

    public int[][] jaggedArray2DofInt;
    public int[][][] jaggedArray3DOfInt;

    public float value;

    public int CompareTo(Scr_Test other)
    {
        if (value < other.value)
        {
            return 1;
        }

        else if (value > other.value)
        {
            return -1;
        }

        return 0;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //TestList();
        //TestJagged2D();
        TestJagged3D();
    }


    private void TestJagged2D()
    {
        //Nbr de colonnes
        jaggedArray2DofInt = new int[4][];

        //Nbr de ligne par colonne
        jaggedArray2DofInt[0] = new int[2];
        jaggedArray2DofInt[1] = new int[3];
        jaggedArray2DofInt[2] = new int[3];
        jaggedArray2DofInt[3] = new int[2];

        jaggedArray2DofInt[0][0] = 1;

        for(int x = 0; x < jaggedArray2DofInt.Length; x++)
        {
            for (int y = 0; y < jaggedArray2DofInt[x].Length; y++)
            {
                Debug.Log(jaggedArray2DofInt[x][y]);
            }
        }

    }

    private void TestJagged3D()
    {
        //Nbr de colonnes
        jaggedArray3DOfInt = new int[4][][];

        //Nbr de ligne par colonne
        jaggedArray3DOfInt[0] = new int[2][];
        jaggedArray3DOfInt[1] = new int[3][];
        jaggedArray3DOfInt[2] = new int[3][];
        jaggedArray3DOfInt[3] = new int[2][];

        for (int x = 0; x < jaggedArray3DOfInt.Length; x++)
        {
            for (int y = 0; y < jaggedArray3DOfInt[x].Length; y++)
            {
                if(x < 3)
                {
                    jaggedArray3DOfInt[x][y] = new int[3];
                }
                else
                {
                    jaggedArray3DOfInt[x][y] = new int[2];
                }

                
            }
        }

        jaggedArray3DOfInt[0][0][0] = 1;

        for (int x = 0; x < jaggedArray3DOfInt.Length; x++)
        {
            for (int y = 0; y < jaggedArray3DOfInt[x].Length; y++)
            {
                for(int z = 0; z < jaggedArray3DOfInt[x][y].Length; z++)
                {
                    Debug.Log(jaggedArray3DOfInt[x][y][z]);
                }
            }
        }


    }

    private void TestList()
    {
        listOfInt = new List<int>();

        listOfInt.Add(item: 123);
        int myint = 321;

        listOfInt.Add(myint);

        Debug.Log(listOfInt[0] + " : " + listOfInt[1]);

        listOfInt.RemoveAt(0);

        listOfInt.Add(item: 2);
        listOfInt.Add(item: 1);

        listOfInt.Sort();

    }

    public LayerMask hitLayer;

    public void Raycast()
    {
        Vector3 pos = Vector3.zero;
        Vector3 dir = Vector3.forward;
        RaycastHit hit;
        float range = 1f;

        if (Physics.Raycast(pos, dir, out hit, range, hitLayer))
        {
            Debug.DrawRay(pos, dir, Color.red);
        }
        else
        {
            Debug.DrawRay(pos, dir, Color.red);
        }
    }
}
