using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCircleSystem : MonoBehaviour, ILevel {
	[SerializeField] private float _maxLimitMoveSpeed = 5;
	[SerializeField] private Sprite _iCircle;
	[SerializeField] private float _rotateGravity = 1f;
	private int _circleCount;
	[SerializeField] private float _radiusCircleParent;
	[SerializeField] private float _radiusCircleChild;
	[SerializeField] private float _rotateSpeed;
	private List<Transform> _circleList = new List<Transform>();
	// Use this for initialization
	private void Start(){
		Spawn();
	}
	public void ResetLevel(){
		foreach(var circle in _circleList){
			if(circle != null)
				Destroy(circle.gameObject);
		}
		_circleList.Clear();
		Spawn();
	}
	void Spawn () {
		var colorTable = ColorTableComponent.Instance.ColorTable;
		_circleCount = colorTable.Length;
		for(int i = 0; i < _circleCount; i++){
			var circleposition = (Vector2)transform.position + Vector2.up * _radiusCircleParent;
			GameObject circle = CircleController.Instance.SpawnCircle(circleposition, _radiusCircleChild, _iCircle, colorTable[i], "circle", transform, true);
			circle.GetComponent<Rigidbody2D>().gravityScale = 0;
			circle.transform.RotateAround(transform.position, Vector3.back, 360f*i/_circleCount);
			_circleList.Add(circle.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(transform.forward * _rotateSpeed * Time.deltaTime);
		if(_rotateSpeed <= _maxLimitMoveSpeed)
			_rotateSpeed += Time.deltaTime * _rotateGravity;
	}
}
