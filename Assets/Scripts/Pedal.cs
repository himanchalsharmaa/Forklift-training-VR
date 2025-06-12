using UnityEngine;

public class Pedal : MonoBehaviour
{
    [SerializeField] private float PressDistance = 0.2f;
    [SerializeField] private float MoveSpeed = 5f;

    private Vector3 InitialLocalPosition;
    private Vector3 PressedLocalPosition;
    private bool IsPressed = false;

    void Start()
    {
        InitialLocalPosition = transform.localPosition;
        PressedLocalPosition = InitialLocalPosition + new Vector3(0f, -PressDistance, 0f);
    }
    public void SetPressed(bool pressed) => IsPressed = pressed;
    void Update()
    {
        Vector3 _Target = IsPressed ? PressedLocalPosition : InitialLocalPosition;
        transform.localPosition = Vector3.Lerp(transform.localPosition, _Target, Time.deltaTime * MoveSpeed);
    }
}
