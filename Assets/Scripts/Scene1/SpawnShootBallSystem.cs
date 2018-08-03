using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShootBallSystem : MonoBehaviour, ILevel {
	[SerializeField] private float _circleGravity = 0.5f;
	[SerializeField] private Sprite _iCircle;
	[SerializeField] private float _radiusCircleChild = 1;
	[SerializeField] private float _timeShowCircle = 1;
	[RangeAttribute(0f, 1f)]
	[SerializeField] private float _circleBounciness = 0.5f;
	private PhysicsMaterial2D _circlePhysicMat2D;
	private List<Transform> _circleList = new List<Transform>();
	private Vector2 _topRightScreen;
	private Vector2 _bottomLeftScreen;
	private int _circleCount;
	void Awake () {
		_topRightScreen = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		_bottomLeftScreen = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
		_timeShowRun = _timeShowCircle;
		_circlePhysicMat2D = new PhysicsMaterial2D("Bounce");
		_circlePhysicMat2D.bounciness = _circleBounciness;
		//Spawn();
	}
	public void ResetLevel(){
		foreach(var circle in _circleList){
			if(circle != null)
				Destroy(circle.gameObject);
		}
		_indexCircleList = 0;
		_circleList.Clear();
		Spawn();
	}
	void Spawn () {
		var colorTable = ColorTableComponent.Instance.ColorTable;
		_circleCount = colorTable.Length;
		for(int i = 0; i < _circleCount; i++){
			Vector2 circleposition = Vector2.zero;
			circleposition.x = (_topRightScreen.x - _bottomLeftScreen.x) * (i - _circleCount/2)/ _circleCount;//(i / (float)_circleCount);
			circleposition.y = transform.position.y;
			GameObject circle = CircleController.Instance.SpawnCircle(circleposition, _radiusCircleChild, _iCircle, colorTable[i], "circle", transform);
			var circleRigidbody = circle.GetComponent<Rigidbody2D>();
			circleRigidbody.mass = 1f;
			circleRigidbody.gravityScale = _circleGravity;
			var circleColldier = circle.GetComponent<CircleCollider2D>();
			circleColldier.sharedMaterial = _circlePhysicMat2D;
			circle.SetActive(false);
			_circleList.Add(circle.transform);
		}
	}
	
	private float _timeShowRun;
	private int _indexCircleList;
	void Update () {
		if(_indexCircleList == _circleList.Count) return;
		_timeShowRun += Time.deltaTime;
		if(_timeShowRun >= _timeShowCircle){
			_timeShowRun = 0;
			_circleList[_indexCircleList].gameObject.SetActive(true);
			_indexCircleList++;
		}
	}
}
