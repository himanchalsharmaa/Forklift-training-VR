using UnityEngine;
using UnityEngine.Events;
using System.Collections;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class DependentEngageablePart : EngageablePart
{
    [Header("Requirements for engagement")]
    [SerializeField]
    [Tooltip("Parts that should be pre-engaged in order to engage this part.")]
    EngageablePart[] RequiredEngagedParts;
    [SerializeField]
    [Tooltip("Parts that should be pre-disengaged in order to engage this part.")]
    EngageablePart[] RequiredDisengagedParts;

    public override void ToggleEngagement()
    {
        if (IsEngaged)
            IsEngaged = false;
        else
        {
            foreach (EngageablePart _EngagedPart in RequiredEngagedParts)
            {
                if (!_EngagedPart.IsPartEngaged())
                {
                    HandlUserInstructions.ShowGlobalMessage?.Invoke(new GlobalMessage($"{_EngagedPart.PartName} is not engaged. It is required to be engaged to engage with {PartName}.",
    GlobalMessageType.EngagementFailure));
                    Debug.LogError($"{_EngagedPart.PartName} is not engaged. It is required to be engaged to engage with {PartName}.");

                    IsEngaged = false;
                    CallRespectiveEvents();
                    return;
                }
            }
            foreach (EngageablePart _DisengagedPart in RequiredDisengagedParts)
            {
                if (_DisengagedPart.IsPartEngaged())
                {
                    HandlUserInstructions.ShowGlobalMessage?.Invoke(new GlobalMessage($"{_DisengagedPart.PartName} is engaged. It is required to be disengaged to engage with {PartName}.",
GlobalMessageType.EngagementFailure));
                    Debug.LogError($"{_DisengagedPart.PartName} is not engaged. It is required to be engaged to engage with {PartName}.");

                    IsEngaged = false;
                    CallRespectiveEvents();
                    return;
                }
            }
            IsEngaged = true;
        }
        CallRespectiveEvents();
    }
    private bool InCooldown = false;
    private float ErrorCooldown = 2.5f;
    private IEnumerator DisableErrorCooldown()
    {
        InCooldown = true;
        yield return new WaitForSeconds(ErrorCooldown);
        InCooldown = false;
    }
    public override void EngagePart()
    {
        foreach (EngageablePart _EngagedPart in RequiredEngagedParts)
            if (!_EngagedPart.IsPartEngaged())
            {
                if (!InCooldown)
                {
                    StartCoroutine(DisableErrorCooldown());

                    HandlUserInstructions.ShowGlobalMessage?.Invoke(new GlobalMessage($"{_EngagedPart.PartName} is not engaged. It is required to be engaged to engage with {PartName}.",
                        GlobalMessageType.EngagementFailure));
                    Debug.LogError($"{_EngagedPart.PartName} is not engaged. It is required to be engaged to engage with {PartName}.", this);

                    ToggleEmission _TE = _EngagedPart.GetComponent<ToggleEmission>();
                    if (_TE == null)
                        _TE = _EngagedPart.GetComponentInChildren<ToggleEmission>(true);
                    if (_TE != null)
                        _TE.DrawAttention();
                }
                return;
            }
        #region test
        foreach (EngageablePart _DisengagedPart in RequiredDisengagedParts)
            if (_DisengagedPart.IsPartEngaged())
            {
                if (!InCooldown)
                {
                    StartCoroutine(DisableErrorCooldown());

                    HandlUserInstructions.ShowGlobalMessage?.Invoke(new GlobalMessage($"{_DisengagedPart.PartName} is engaged. It is required to be disengaged to engage with {PartName}.",
          GlobalMessageType.EngagementFailure));
                    Debug.LogError($"{_DisengagedPart.PartName} is not engaged. It is required to be engaged to engage with {PartName}.", this);

                    ToggleEmission _TE = _DisengagedPart.GetComponentInChildren<ToggleEmission>(true);
                    if (_TE != null)
                        _TE.DrawAttention();
                }
                return;
            }
        #endregion
        IsEngaged = true;
        CallRespectiveEvents();
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(DependentEngageablePart))]
public class DependentEngageablePartEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get the target component
        DependentEngageablePart part = (DependentEngageablePart)target;

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
