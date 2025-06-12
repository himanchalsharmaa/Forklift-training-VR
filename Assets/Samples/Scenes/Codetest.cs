using UnityEngine;

public class Codetest : MonoBehaviour
{

    private void Start()
    {
        fib(5);
    }
    private void fib(int n)
    {
        int count = 0;
        int fib = 1;
        int prev = fib;
        Debug.Log(0);
        Debug.Log(1);
        while(count < n - 1)
        {
            prev = fib;
            fib += prev;
            count++;
            Debug.Log(fib + " ");
        }
    }
}
