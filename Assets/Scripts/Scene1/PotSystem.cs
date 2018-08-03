using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotSystem : MonoBehaviour {
	[SerializeField] private float _timeSpawn;
	private float _timeRunSpawn;
	private List<CircleInfo> _circles = new List<CircleInfo>();
	private CircleController _circleController;
	private Queue<GameObject> _poolCircle = new Queue<GameObject>();
	// Use this for initialization
	void Start () {
		_circleController = CircleController.Instance;
		_poolCircle = ObjectPoolSystem.Instance.GetQueuePool(0);
		for(int i = 0; i < _poolCircle.Count; i++){
			
			var circle = _poolCircle.Dequeue();
			_poolCircle.Enqueue(circle);
			var position = new Vector2(Random.Range(-1f,1f), transform.position.y);
			circle.transform.position = position;
			circle.transform.localScale = Vector3.one * Random.RandomRange(0.2f, 0.8f);
			circle.SetActive(true);
			circle.GetComponent<Rigidbody2D>().gravityScale = 0;
			var circleInfo = new CircleInfo(circle.transform, Time.time, Random.Range(1, 3) * 2 - 3, circle.transform.localScale.x * 10f, 2f);
			circleInfo.Active = false;
			_circles.Add(circleInfo);
		}
	}
	private void Update(){
		_timeRunSpawn += Time.deltaTime;
		if(_timeRunSpawn >= _timeSpawn){
			_timeRunSpawn = 0;
			var color = new Color(1f, 90f/255f, 208f/255f, 1);
			if(Random.Range(0,2) == 1 ? true : false)
				color = new Color(1, 23f/255f, 0, 1);
				if(_poolCircle.Count == 0) Debug.LogError("Count Queue = 0");
			
			var circle = _circles[0];
			circle.Active = true;
			circle.StartTime = Time.time;
			circle.DirectionMove = Random.Range(1, 3) * 2 - 3;
			_circles.Remove(circle);
			_circles.Add(circle);
			circle.ColorCircle = color;
		}
		StartCoroutine(MoveCircleSin());
	}
	WaitForSeconds wait = new WaitForSeconds(0.3f);
	private IEnumerator MoveCircleSin(){
		foreach(var circle in _circles){
			//if(circle == null) Debug.Log("Null i : " + _circles.IndexOf(circle));
			if(circle.Active == true){
				circle.CircleTr.position = new Vector3(Mathf.Sin(Time.time - circle.StartTime) * circle.DirectionMove * 0.5f + circle.OriginPosition.x, (Time.time - circle.StartTime) * 0.15f * circle.MoveSpeed + circle.OriginPosition.y, 0);
			
				if(Time.time - circle.StartTime >= circle.TimeLife){
					circle.CircleTr.GetComponentInChildren<ParticleSystem>().Play();
					yield return wait;
					circle.Active = false;
					circle.CircleTr.position = circle.OriginPosition;
					continue;
				}
			}
		}
	}
}
public class CircleInfo{
	public Transform CircleTr;
	public Vector2 OriginPosition;
	public float StartTime;
	public int DirectionMove;
	public float MoveSpeed;
	public float TimeLife;
	public CircleInfo(Transform circle, float timeStart, int direction, float moveSpeed, float timeLife){
		TimeLife = timeLife;
		CircleTr = circle;
		OriginPosition = circle.position;
		StartTime = timeStart;
		DirectionMove = direction;
		MoveSpeed = moveSpeed;
	}
	public Color ColorCircle{
		get {
			return CircleTr.GetComponent<SpriteRenderer>().color;
		}
		set{
			CircleTr.GetComponent<SpriteRenderer>().color = value;
		}
	}
	public bool Active{
		get {return CircleTr.gameObject.activeInHierarchy;}
		set{CircleTr.gameObject.SetActive(value);}
	}
}
