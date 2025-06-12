using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
[RequireComponent(typeof(EngageablePart))]
public class RotationAndEngagement : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The rotation(in euler angles) at which this part will be considered engaged. Every other angle will be considered disengaged.")]
    private List<Vector3> EngageRotationEuler;
    private EngageablePart _EngageablePart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _EngageablePart = GetComponent<EngageablePart>();
    }

    private void LateUpdate()
    {
            foreach (Vector3 _Rotation in EngageRotationEuler)
            {
                if (CompareVector3(_Rotation, transform.localEulerAngles))
                {
                    _EngageablePart.EngagePart();
                    return;
                }
            }
            _EngageablePart.DisengagePart();
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool CompareVector3(Vector3 a, Vector3 b, float tolerance = 0.0001f)
    {
        return Vector3.Distance(a, b) < tolerance;
    }
}
