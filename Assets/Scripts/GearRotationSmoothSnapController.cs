using UnityEngine;

public class GearRotationSmoothSnapController : ForceRotationControl
{
    [SerializeField]
    float SnapSpeed = 180f;
    public override void LateUpdate()
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
            _NewLocalRotation.x = Mathf.Clamp(_NewLocalRotation.x, _RotationConstraints.XAxis.AxisRange.Min, _RotationConstraints.XAxis.AxisRange.Max);
        if (_RotationConstraints.YAxis.ConstrainAxis)
            _NewLocalRotation.y = Mathf.Clamp(_NewLocalRotation.y, _RotationConstraints.YAxis.AxisRange.Min, _RotationConstraints.YAxis.AxisRange.Max);
        if (_RotationConstraints.ZAxis.ConstrainAxis)
        {
            float _CurrZ = _NewLocalRotation.z;
            _CurrZ = NormalizeAngle(_CurrZ);

            float _Min = _RotationConstraints.ZAxis.AxisRange.Min;
            float _Max = _RotationConstraints.ZAxis.AxisRange.Max;

            if (_CurrZ > _Max)
            {
                _NewLocalRotation.z = _Max;
                transform.SetLocalPositionAndRotation(_NewLocalPosition, Quaternion.Euler(_NewLocalRotation));
                return;
            }
            else if (_CurrZ < _Min)
            {
                _NewLocalRotation.z = _Min;
                transform.SetLocalPositionAndRotation(_NewLocalPosition, Quaternion.Euler(_NewLocalRotation));
                return;
            }

            float[] _SnapTargets = new float[]
            {
                NormalizeAngle(_Min),
                NormalizeAngle(_Max),
                0f
            };

            float _NextZ = SnapToClosestAngle(_CurrZ, _SnapTargets);

            _NewLocalRotation.z = Mathf.MoveTowardsAngle(_CurrZ, _NextZ, Time.deltaTime * SnapSpeed); 
        }

        transform.SetLocalPositionAndRotation(_NewLocalPosition, Quaternion.Euler(_NewLocalRotation));

    }
    float NormalizeAngle(float _Angle)
    {
        _Angle %= 360f;
        if (_Angle > 180f) _Angle -= 360f;
        return _Angle;
    }

    float SnapToClosestAngle(float value, float[] targets)
    {
        float closest = targets[0];
        float minDistance = Mathf.Abs(Mathf.DeltaAngle(value, closest));

        for (int i = 1; i < targets.Length; i++)
        {
            float dist = Mathf.Abs(Mathf.DeltaAngle(value, targets[i]));
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = targets[i];
            }
        }

        return closest;
    }
    public void ResetRotation()
    {
        transform.localEulerAngles = Vector3.zero;
    }
}
