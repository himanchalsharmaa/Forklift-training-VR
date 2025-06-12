using UnityEngine;

public class WelcomeInstruction : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HandlUserInstructions.ShowGlobalMessage?.Invoke(new GlobalMessage("Turn the key on to begin."));   
    }

}
