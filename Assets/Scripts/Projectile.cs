using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    LineRenderer lineRenderer;

    Vector3 end;
    Vector3 start;

    public void SetLine(Vector3 start, Vector3 end)
    {
        lineRenderer.SetPositions(new Vector3[] { start, end });
        this.end = end;
        this.start = start;
        StartCoroutine(lineDissolve());
    }

    IEnumerator lineDissolve()
    {
        float timePast = 0f;
        float timeTarget = .2f;
        WaitForEndOfFrame waiter = new WaitForEndOfFrame();

        while (timePast < timeTarget)
        {
            timePast += Time.deltaTime;
            Vector3 currentStart = Vector3.Lerp(start, end, timePast / timeTarget);
            lineRenderer.SetPositions(new Vector3[] { currentStart, end });
            yield return waiter;
        }

        Destroy(gameObject);
    }
}
