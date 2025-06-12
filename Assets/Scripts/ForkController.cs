using UnityEngine;

public class ForkController : MonoBehaviour
{
    [SerializeField]
    private float ForkMoveSpeed = 1.5f;
    [SerializeField]
    private Transform ForkGear;

    private float CurrentY;
    private bool ToggleForks = false;

    void Start()
    {
        CurrentY = transform.localPosition.y;
    }
    void Update()
    {
        if (ToggleForks)
        {
            float _RotZ = NormalizeAngle(ForkGear.localEulerAngles.z);

            CurrentY = Mathf.MoveTowards(CurrentY, Mathf.Lerp(0f, 1.7f, Mathf.InverseLerp(0f, 10f, _RotZ)), ForkMoveSpeed * Time.deltaTime);

            Vector3 _CurrPos = transform.localPosition;
            _CurrPos.y = CurrentY;
            transform.localPosition = _CurrPos;
        }
    }
    float NormalizeAngle(float _Angle)
    {
        _Angle %= 360f;
        if (_Angle > 180f) _Angle -= 360f;
        return _Angle;
    }
    public void ToggleForkMovement(bool _EnableForks) => ToggleForks = _EnableForks;
}
