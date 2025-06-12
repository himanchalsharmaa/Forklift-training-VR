using UnityEngine;

public class ForceRotationControl : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Constraints are relative parameter is ignored and permanently set to true.")]
    protected Oculus.Interaction.TransformerUtils.PositionConstraints _PositionConstraints;
    [SerializeField]
    protected Oculus.Interaction.TransformerUtils.RotationConstraints _RotationConstraints;

    protected Vector3 _NewLocalPosition;
    protected Vector3 _NewLocalRotation;

    public virtual void LateUpdate()
    {
        _NewLocalPosition = transform.localPosition;

        if (_PositionConstraints.XAxis.ConstrainAxis)
            _NewLocalPosition.x = Mathf.Clamp(_NewLocalPosition.x, _PositionConstraints.XAxis.AxisRange.Min, _PositionConstraints.XAxis.AxisRange.Max);
        if (_PositionConstraints.YAxis.ConstrainAxis)
            _NewLocalPosition.y = Mathf.Clamp(_NewLocalPosition.y, _PositionConstraints.YAxis.AxisRange.Min, _PositionConstraints.YAxis.AxisRange.Max);
        if (_PositionConstraints.ZAxis.ConstrainAxis)
            _NewLocalPosition.z = Mathf.Clamp(_NewLocalPosition.z, _PositionConstraints.ZAxis.AxisRange.Min, _PositionConstraints.ZAxis.AxisRange.Max);

        _NewLocalRotation = transform.localEulerAngles;

        if (_RotationConstraints.XAxis.ConstrainAxis)
            _NewLocalRotation.x = ClampEulerAngle(_NewLocalRotation.x, _RotationConstraints.XAxis.AxisRange.Min, _RotationConstraints.XAxis.AxisRange.Max);
        if (_RotationConstraints.YAxis.ConstrainAxis)
            _NewLocalRotation.y = ClampEulerAngle(_NewLocalRotation.y, _RotationConstraints.YAxis.AxisRange.Min, _RotationConstraints.YAxis.AxisRange.Max);
        if (_RotationConstraints.ZAxis.ConstrainAxis)
            _NewLocalRotation.z = ClampEulerAngle(_NewLocalRotation.z, _RotationConstraints.ZAxis.AxisRange.Min, _RotationConstraints.ZAxis.AxisRange.Max);

        transform.SetLocalPositionAndRotation(_NewLocalPosition, Quaternion.Euler(_NewLocalRotation));
    }
    float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        if (angle < -180f) angle += 360f;
        return angle;
    }
    float ClampEulerAngle(float eulerAngle, float min, float max)
    {
        float angle = NormalizeAngle(eulerAngle);
        float minNorm = NormalizeAngle(min);
        float maxNorm = NormalizeAngle(max);

        float clamped = Mathf.Clamp(angle, minNorm, maxNorm);
        return (clamped + 360f) % 360f; 
    }
}
