using System.Collections.Generic;
using UnityEngine;

public class RotateArray : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int[] arr = { 1,2,3,4,5,6,7};
        PrintArray(arr);

        int k = 3;
        k %= arr.Length;

        RevSub(arr,0,arr.Length - 1);
        RevSub(arr, 0, k - 1);
        RevSub(arr, k, arr.Length - 1);

        PrintArray(arr);
    }
    private void PrintArray(int[] arr)
    {
        string output = "";
        foreach(int i in arr)
            output += i + " ";
        Debug.LogError(output);
    }
    private void RevSub(int[] arr, int ind1, int ind2)
    {
        while(ind1 < ind2)
        {
            int temp = arr[ind1];
            arr[ind1] = arr[ind2];
            arr[ind2] = temp;
            ind1 += 1;
            ind2 -= 1;
        }
    }
}
