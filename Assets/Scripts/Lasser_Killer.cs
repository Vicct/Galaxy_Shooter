using UnityEngine;

public class Lasser_Killer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public int numberOfSegments = 20;
    public float coneAngle = 0f;
    public float coneHeight = 5f;

    void Start()
    {
        lineRenderer.positionCount = numberOfSegments + 1;

        for (int i = 0; i <= numberOfSegments; i++)
        {
            float t = (float)i / numberOfSegments;
            float angle = Mathf.Lerp(-coneAngle / 2f, coneAngle / 2f, t);
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * (coneHeight - t * coneHeight);
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * (coneHeight - t * coneHeight);
            lineRenderer.SetPosition(i, new Vector3(x, t * coneHeight, z));
        }
    }
}
