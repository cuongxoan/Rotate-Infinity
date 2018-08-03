using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBallSystem : MonoBehaviour {
	[SerializeField] private GameObject _ball;
	[SerializeField] private int _randomCountPosition;
	[SerializeField] private float _timeSpawn;
	private float _timeRunSpawn;
	private float _distanceSpawn;
	private Vector2 _topScreenPosition;
	private Vector2[] _PositionSpawns;
	
	// Use this for initialization
	void Start () {
		_distanceSpawn = - Camera.main.ScreenToWorldPoint(Vector3.zero).x + Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
		_distanceSpawn /= _randomCountPosition;
		_topScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
		_PositionSpawns = new Vector2[_randomCountPosition];
		for(int i = 0; i < _randomCountPosition; i++){
			_PositionSpawns[i] = new Vector2(_distanceSpawn * i - _randomCountPosition/2, _topScreenPosition.y);
			Debug.Log(_distanceSpawn * i);
		}
	}
	
	// Update is called once per frame
	void Update () {
		_timeRunSpawn += Time.deltaTime;
		if(_timeRunSpawn >= _timeSpawn){
			_timeRunSpawn = 0;
			GameObject ball = Instantiate(_ball, _PositionSpawns[Random.Range(0, _randomCountPosition)], Quaternion.identity);
			ball.tag = "ball";
			Destroy(ball, 10);
		}
	}
}
