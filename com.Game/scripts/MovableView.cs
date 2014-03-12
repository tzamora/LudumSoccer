using UnityEngine;
using System.Collections;

public class MovableView : MonoBehaviour
{
    public Vector3 OpenPosition;

    public Vector3 ClosedPosition;

    public bool IsHidden = true;

    public float Duration = 0.5f;

    public void Show()
    {
        if (IsHidden)
        {
            StartCoroutine(Move(ClosedPosition, OpenPosition, Duration));

            IsHidden = false;
        }
    }

    public void Hide()
    {
        if (!IsHidden)
        {
            StartCoroutine(Move(OpenPosition, ClosedPosition, Duration));

            IsHidden = true;
        }
    }

    IEnumerator Move(Vector3 pointA, Vector3 pointB, float time)
    {
        float i = 0.0f;

        float rate = 1.0f / time;

        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;

            this.transform.localPosition = Vector3.Lerp(pointA, pointB, i);

            yield return null;
        }
    }
}
