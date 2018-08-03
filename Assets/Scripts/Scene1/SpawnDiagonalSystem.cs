using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnDiagonalSystem : MonoBehaviour {
	private int _circleCount;
	[SerializeField] private Sprite _iCircle;
	[SerializeField] private float _radiusCircleChild;
	[SerializeField] private float _radiusCircleParent;
	[SerializeField] private int _directionSpawn = -1;
	private List<Transform> _circleList = new List<Transform>();
	private Vector2 _centerSpawn;
	// Use this for initialization
	void Start () {
		Spawn();
	}
	void Spawn () {
		int directionRotate = 1;
		_centerSpawn = transform.position;
		Debug.Log(_centerSpawn);
		var colorTable = ColorTableComponent.Instance.ColorTable;
		_circleCount = colorTable.Length;
		for(int i = 0; i < _circleCount; i++){
			GameObject circle = new GameObject("Circle", typeof(SpriteRenderer), typeof(Rigidbody2D));
			circle.GetComponent<Rigidbody2D>().gravityScale = 0;
			circle.GetComponent<SpriteRenderer>().color = colorTable[i];
			circle.GetComponent<SpriteRenderer>().sprite = _iCircle;
			circle.transform.SetParent(transform);
			circle.transform.localScale = Vector3.one * _radiusCircleChild;
			circle.name = (i%(_circleCount/2)).ToString();
			_circleList.Add(circle.transform);
			Vector3 position = circle.transform.position ;
   			position.x = Mathf.Sin(i * 1) * _radiusCircleParent ;
			position.y = position.x = Mathf.Sin(i * 1) * _radiusCircleParent;
   			circle.transform.position = position ;
		}
	}
}
