using System.Collections;
using UnityEngine;

public class ToggleEmission : MonoBehaviour
{
    [SerializeField]
    GameObject[] AlsoToggleThese;

    Material ButtonMaterial;
    private bool Active = false;

    private void OnEnable()
    {
        ButtonMaterial = GetComponent<Renderer>().material;
    }

    public void ToggleEmissionMode(bool _Activate)
    {
        if (ButtonMaterial != null)
        {
            if (_Activate)
            {
                Active = true;
                ButtonMaterial.EnableKeyword("_EMISSION");
            }
            else
            {
                Active = false;
                ButtonMaterial.DisableKeyword("_EMISSION");
            }
        }
        foreach (GameObject _GO in AlsoToggleThese)
            _GO.SetActive(Active);
    }
    private bool CoroutineRunning;
    public void DrawAttention()
    {
        if(!CoroutineRunning)
            StartCoroutine(TempEnableEmission());
    }
    private IEnumerator TempEnableEmission()
    {
        CoroutineRunning = true;
        ToggleEmissionMode(true);
        
        yield return new WaitForSeconds(2.5f);

        ToggleEmissionMode(false);
        CoroutineRunning = false;
    }
}
