using UnityEngine;
using UnityEngine.Events;
using Oculus.Interaction;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EngageablePart : MonoBehaviour
{
    [SerializeField]
    public string PartName = "";
    [SerializeField]
    protected UnityEvent[] WhenEngaged;
    [SerializeField]
    protected UnityEvent[] WhenDisengaged;

    [SerializeField]
    protected bool IsEngaged = false;
    private InteractableUnityEventWrapper _Wrapper;

    private void Start()
    {
        _Wrapper = GetComponent<InteractableUnityEventWrapper>();
        if(_Wrapper != null)
            _Wrapper.WhenSelect.AddListener(ToggleEngagement);
    }
    public bool IsPartEngaged()
    {
        return IsEngaged;
    }
    
    public virtual void ToggleEngagement() 
    {
        if (IsEngaged)
            IsEngaged = false;
        else
            IsEngaged = true;

        CallRespectiveEvents();
    }
    public virtual void EngagePart()
    {
        IsEngaged = true;
        CallRespectiveEvents();
    }
    public virtual void DisengagePart()
    {
        IsEngaged = false;
        CallRespectiveEvents();
    }
    public void CallRespectiveEvents()
    {
        if (IsEngaged)
            foreach (UnityEvent _Event in WhenEngaged)
                _Event?.Invoke();
        else
            foreach (UnityEvent _Event in WhenDisengaged)
                _Event?.Invoke();
    }
    private void OnDestroy()
    {
        if(_Wrapper != null)
            _Wrapper.WhenSelect.RemoveListener(ToggleEngagement);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(EngageablePart))]
public class EngageablePartEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get a reference to the target script
        EngageablePart part = (EngageablePart)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Engage Part"))
        {
            part.EngagePart();
        }

        if (GUILayout.Button("Disengage Part"))
        {
            part.DisengagePart();
        }
    }
}
#endif