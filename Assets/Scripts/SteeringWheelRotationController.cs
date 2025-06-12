using UnityEngine;

public class SteeringWheelRotationController : ForceRotationControl
{
    [SerializeField]
    GameObject ForkliftParent;

    [SerializeField]
    EngageablePart Gear;
    public override void LateUpdate()
    {
        base.LateUpdate();

        if (Gear.IsPartEngaged())
        {
            float rotationSpeed = 50f;

            float maxStep = rotationSpeed * Time.deltaTime;

            // Apply smooth rotation
            ForkliftParent.transform.localRotation = Quaternion.RotateTowards(
                ForkliftParent.transform.localRotation,
                transform.localRotation,
                maxStep
            );
        }
    }

}
