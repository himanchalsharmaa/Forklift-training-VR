using System;
using System.Collections;
using UnityEngine;

public class HandleForkliftColiision : MonoBehaviour
{
    [SerializeField]
    Transform ForkliftParent;
    [SerializeField]
    HandleForkliftColiision[] ForkliftColidors;
    public bool IsAnimating = false;

    public bool FlipThrowDirection = false;
    private void OnTriggerEnter(Collider other)
    {
        foreach (HandleForkliftColiision _Collider in ForkliftColidors)
            if (_Collider.IsAnimating)
                return;
        if (!IsAnimating && other.CompareTag("Wall"))
            StartCoroutine(PerformWallResponse());
    }

    private IEnumerator PerformWallResponse()
    {
        IsAnimating = true;

        Vector3 startPos = ForkliftParent.position;
        Vector3 targetPos1 = Vector3.one;
        if (FlipThrowDirection)
            targetPos1 = startPos + ForkliftParent.right + Vector3.up;
        else
            targetPos1 = startPos - ForkliftParent.right + Vector3.up;

        yield return MoveOverTime(startPos, targetPos1, 1f);

        Vector3 targetPos2 = targetPos1 - Vector3.up;
        yield return MoveOverTime(targetPos1, targetPos2, 1f);

        IsAnimating = false;
    }

    private IEnumerator MoveOverTime(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            ForkliftParent.position = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        ForkliftParent.position = to;
    }
}
