using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserFence : MonoBehaviour
{
    [SerializeField] private float[] laserHeights;
    [SerializeField] private bool loop = true;

    private List<Vector3> positions;
    private LineRenderer lineRenderer;

    void Start()
    {
        positions = new List<Vector3>();
        lineRenderer = transform.GetChild(1).GetComponent<LineRenderer>();
        lineRenderer.positionCount = laserHeights.Length * (transform.GetChild(0).childCount + (loop?1:0));
        for(int h = 0; h < laserHeights.Length; h++)
        {
            if(h % 2 == 0)
            {
                for(int i = 0; i < transform.GetChild(0).childCount; i++)
                {
                    Vector3 position = transform.GetChild(0).GetChild(i).transform.position;
                    position.y += laserHeights[h];
                    positions.Add(position);
                }
                if(loop)
                {
                    Vector3 position = transform.GetChild(0).GetChild(0).transform.position;
                    position.y += laserHeights[h];
                    positions.Add(position);
                }
            }
            else
            {
                for(int i = transform.GetChild(0).childCount - 1; i >= 0; i--)
                {
                    Vector3 position = transform.GetChild(0).GetChild(i).transform.position;
                    position.y += laserHeights[h];
                    positions.Add(position);
                }
                if(loop)
                {
                    Vector3 position = transform.GetChild(0).GetChild(transform.GetChild(0).childCount - 1).transform.position;
                    position.y += laserHeights[h];
                    positions.Add(position);
                }
            }
        }
        lineRenderer.SetPositions(positions.ToArray());

        for(int i = 0; i < lineRenderer.positionCount - 1; i++)
        {
            AddLineCollider(lineRenderer, lineRenderer.GetPosition(i), lineRenderer.GetPosition(i+1));
        }
    }

    void AddLineCollider(LineRenderer lineRenderer, Vector3 startPos, Vector3 endPos)
    {
        BoxCollider lineCollider = new GameObject("LineCollider").AddComponent<BoxCollider>();
        lineCollider.transform.parent = lineRenderer.transform;

        float lineWidth = lineRenderer.endWidth;
        float lineLength = Vector3.Distance(startPos, endPos);
        lineCollider.size = new Vector3(lineLength, lineWidth, 1.0f);

        Vector3 midPoint = (startPos + endPos) / 2.0f;
        lineCollider.transform.position = midPoint;

        float angle = Mathf.Atan2((endPos.z - startPos.z), (endPos.x - startPos.x));
        angle *= Mathf.Rad2Deg;
        angle *= -1.0f;
        lineCollider.transform.Rotate(0.0f, angle, 0.0f);
    }
}
