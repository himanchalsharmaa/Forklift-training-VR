using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class AccelerationController : MonoBehaviour
{
    [SerializeField]
    private GameObject ForkliftParent;
    [SerializeField]
    private Pedal AcceleratorPedal;
    [SerializeField]
    private Pedal BreakPedal;
    [SerializeField]
    private TMP_Text GearText;
    [SerializeField]
    private Transform GearTransform;
    [SerializeField]
    private float Acceleration = 2f;
    [SerializeField]
    private float AutoDeceleration = 0.5f;
    [SerializeField]
    private float BrakeDeceleration = 2f;
    [SerializeField]
    private float MaxSpeed = 10f;
    [SerializeField]
    private float CurrentSpeed = 0f;

    private GearPosition Gear = GearPosition.Neutral;
    private EngageablePart _EngageablePart;

    private void Start()
    {
        _EngageablePart = GetComponent<EngageablePart>();
    }
    private void Update()
    {
        if (_EngageablePart.IsPartEngaged())
        {
            // To move the lift
            float _ThumbYSecondary = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).y;

            float _Tolerance = 0.075f;

            bool _Accelerating = _ThumbYSecondary > _Tolerance;
            bool _Braking = _ThumbYSecondary < -_Tolerance;

            if (_Accelerating)
                CurrentSpeed += Acceleration * Time.deltaTime;
            else if (_Braking)
                CurrentSpeed -= BrakeDeceleration * Time.deltaTime;
            else
                CurrentSpeed -= AutoDeceleration * Time.deltaTime;

            CurrentSpeed = Mathf.Clamp(CurrentSpeed, 0f, MaxSpeed);

            Vector3 _MoveDirection = ForkliftParent.transform.right;        // By Default due to model pivoting, this is reverse

            if (Gear.Equals(GearPosition.Forward))
                _MoveDirection *= -1;
            else if (Gear.Equals(GearPosition.Neutral))
                _MoveDirection = Vector3.zero;

            ForkliftParent.transform.position += CurrentSpeed * Time.deltaTime * _MoveDirection;

            AcceleratorPedal.SetPressed(_Accelerating);
            BreakPedal.SetPressed(_Braking);
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CheckGear()
    {
        if (CompareVector3(GearTransform.localEulerAngles, new Vector3(0, 0, 10)))
        {
            Gear = GearPosition.Forward;
            GearText.text = "F";
        }
        else if (CompareVector3(GearTransform.localEulerAngles, new Vector3(0, 0, -10)) || CompareVector3(GearTransform.localEulerAngles, new Vector3(0, 0, 350)))
        {
            Gear = GearPosition.Reverse;
            GearText.text = "R";
        }
        else if (CompareVector3(GearTransform.localEulerAngles, new Vector3(0, 0, 0)))
        {
            GearText.text = "N";
            Gear = GearPosition.Neutral;
        }
    }

    private bool CompareVector3(Vector3 a, Vector3 b, float tolerance = 0.0001f)
    {
        return Vector3.Distance(a, b) < tolerance;
    }

}
[System.Serializable]
public enum GearPosition
{
    Forward,
    Neutral,
    Reverse
}
