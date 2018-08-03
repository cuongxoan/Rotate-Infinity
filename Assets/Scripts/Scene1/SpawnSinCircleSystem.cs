using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSinCircleSystem : MonoBehaviour
{
    [SerializeField] private Sprite _imgCircle;
    [SerializeField] private int _inflectionCount = 2;
    [SerializeField] private float _inflectionAmount = 0.7f;

    [SerializeField] private float _radiusParent = 1;
    [SerializeField] private float _radiusChild = 1;

    [SerializeField] private int _circleCount = 20;

    private Vector2 _centerSpawn;

    private int _directionSpawn = 1;
	// Use this for initialization
	void Start () {
		SpawnSin();
	}
    [ContextMenu("Spawn")]
    private void SpawnSin()
    {
        int index;
        int inflectionPosition = 0;
        _centerSpawn = transform.position;
        int step = (_circleCount / _inflectionCount);
        new GameObject("center").transform.position = _centerSpawn;
        for (int i = 0; i < _circleCount; i++)
        {
            if (i % step == 0)
            {
                inflectionPosition = (i + step + i) / 2;
                _centerSpawn += Vector2.right * _radiusParent * 2;
                new GameObject("center").transform.position = _centerSpawn;
                _directionSpawn *= -1;
            }
            index = i % (_circleCount / _inflectionCount);
            GameObject circle = new GameObject(i.ToString(), typeof(SpriteRenderer));
            circle.GetComponent<SpriteRenderer>().sprite = _imgCircle;
            circle.transform.SetParent(transform);
            float diff = (step - Mathf.Abs(i - inflectionPosition)) * _inflectionAmount;
            
            Debug.Log(diff +":"+ i);
            Vector2 circleposition = circle.transform.position;
            circleposition = _centerSpawn + Vector2.left * _radiusParent;
            circle.transform.position = circleposition;
            circle.transform.RotateAround(_centerSpawn, Vector3.back, 180f * index *_directionSpawn / (float)step);
            circleposition = circle.transform.position;
            circleposition.y *= diff * _directionSpawn / 10f;
            circle.transform.position = circleposition;
        }
    }
}
