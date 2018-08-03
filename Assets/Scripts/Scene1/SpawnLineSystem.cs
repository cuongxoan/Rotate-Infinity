using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLineSystem : MonoBehaviour, ILevel {
	private int _circleCount;
	[SerializeField] private Sprite _iCircle;
	[SerializeField] private float _radiusCircleChild;
	[SerializeField] private float _distanceSpawnChild;
	[SerializeField] private int _directionSpawn = 1;
	[SerializeField] private float _moveSpeed = 1;
	[SerializeField] private int _directionMove = 1;
	private List<Transform> _circleList = new List<Transform>();

	// Use this for initialization
	void Start () {	
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
			var circleposition = (Vector2)transform.position + Vector2.up * _distanceSpawnChild * _directionSpawn * i;
			GameObject circle = CircleController.Instance.SpawnCircle(circleposition, _radiusCircleChild, _iCircle, colorTable[i], "circle", transform, true);
			circle.GetComponent<Rigidbody2D>().gravityScale = 0;
			_circleList.Add(circle.transform);
		}
	}
	int i = 0;
	private void Update(){
		for(i = 0; i < _circleList.Count; i++){
			var circle = _circleList[i];
			if(circle == null) {
				_circleList.Remove(circle);
				continue;
			}
			circle.Translate(Vector3.up * _directionMove * _moveSpeed * Time.deltaTime);
			if(_directionMove == -1 && circle.position.y <= CircleController.Instance.BOTTOMLEFTSCREEN.y){
				circle.position = new Vector2(circle.position.x, circle.position.y + _circleCount * _distanceSpawnChild);
			}
			else if(_directionMove == 1 && circle.position.y >= CircleController.Instance.TOPRIGHTSCREEN.y){
				circle.position = new Vector2(circle.position.x, circle.position.y - _circleCount * _distanceSpawnChild);
			}
		}
	}
}
