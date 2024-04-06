using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float radius = 0f;
    public int segments = 360;
    public LineRenderer lineRenderer;

    void Start()
    {
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        Draw();
    }

    void Draw()
    {
        lineRenderer.startWidth = 0.05f; // Set the width of the line
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = segments + 1; // +1 to close the circle
        lineRenderer.useWorldSpace = true;

        float deltaTheta = (2f * Mathf.PI) / segments;
        float theta = 0f;

        for (int i = 0; i <= segments; i++)
        {
            float x = radius * Mathf.Cos(theta);
            float y = radius * Mathf.Sin(theta);
            Vector3 pos = new Vector3(x, y, 0);
            lineRenderer.SetPosition(i, pos);
            theta += deltaTheta;
        }

        // Optional: Make the line renderer loop
        lineRenderer.loop = true;

        // Set the color of the line (optional)
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }
}
