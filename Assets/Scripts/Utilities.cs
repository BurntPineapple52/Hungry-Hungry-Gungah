using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    //shuffle list
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void Shuffle<T>(this T[] list)
    {
        int n = list.Length;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}

[System.Serializable]
public class ArrayLayout
{
    static private int _gridSize = 5; //Grid 5x5

    public ArrayLayout()
    {
        rows = new rowData[_gridSize];

        for (int i = 0; i < _gridSize; i++)
        {
            rows[i].row = new bool[_gridSize];
        }
    }

    [System.Serializable]
    public struct rowData
    {
        public bool[] row;
    }

    public rowData[] rows = new rowData[_gridSize];
}
