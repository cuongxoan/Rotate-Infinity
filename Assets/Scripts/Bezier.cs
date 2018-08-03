using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bezier : MonoBehaviour {

    public Transform[] controlPoints;
    [SerializeField] private float _radiusCircle = 1;
    [SerializeField] private Sprite _iCircle;
    public LineRenderer lineRenderer;

    private int curveCount = 0;
    private int layerOrder = 0;
    private int SEGMENT_COUNT;
    private int _circleCount;
    private List<Transform> _circlesCurve = new List<Transform>();

    void Start()
    {
        if (!lineRenderer)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerID = layerOrder;
        curveCount = (int)controlPoints.Length / 3;
        SpawnCircle();
    }
    private void SpawnCircle(){
        var colorTable = ColorTableComponent.Instance.ColorTable;
        _circleCount = colorTable.Length;
        SEGMENT_COUNT = colorTable.Length;
		for(int i = 0; i < _circleCount; i++){
			GameObject circle = new GameObject("Circle", typeof(CircleCollider2D), typeof(SpriteRenderer), typeof(Rigidbody2D));
			circle.GetComponent<Rigidbody2D>().gravityScale = 0;
			circle.GetComponent<SpriteRenderer>().color = colorTable[i];
			circle.GetComponent<SpriteRenderer>().sprite = _iCircle;
			circle.transform.SetParent(transform);
            circle.transform.localScale = Vector3.one * _radiusCircle;
			_circlesCurve.Add(circle.transform);
		}
    }
    void Update()
    {

        DrawCurve();
    }

    void DrawCurve()
    {
        for (int j = 0; j < curveCount; j++)
        {
            for (int i = 1; i <= SEGMENT_COUNT; i++)
            {
                float t = i / (float)SEGMENT_COUNT;
                int nodeIndex = j * 3;
                Vector3 pixel = CalculateCubicBezierPoint(t, controlPoints[nodeIndex].position, controlPoints[nodeIndex + 1].position, controlPoints[nodeIndex + 2].position, controlPoints[nodeIndex + 3].position);
                // lineRenderer.SetVertexCount(((j * SEGMENT_COUNT) + i));
                // lineRenderer.SetPosition((j * SEGMENT_COUNT) + (i - 1), pixel);
                _circlesCurve[(j * SEGMENT_COUNT) + (i - 1)].transform.position = pixel;
            }

        }
    }

    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
