using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BigColorSystem : MonoBehaviour, ILevel {
	[SerializeField] private float _radiusCircleParent;
	[SerializeField] private float _radiusCircleChild;
	[SerializeField] private float _timeFlyUp;
	[SerializeField] private float _timeBurstRotate;
	[Range(0f, 1f)]
	[SerializeField] private float _heighFlyUp = 0.5f;
	private Transform _myTr;
	ColorTableComponent _colorTableComponent;
	CircleController _circleControl;
	private Rigidbody2D _myRb;
	private CircleCollider2D _myCol;
	private SpriteRenderer _mySprite;
	
	public List<GameObject> _circles = new List<GameObject>();
	private Vector2 _flyUpPosition;
	
	// Use this for initialization
	void Start () {
		_myRb = GetComponent<Rigidbody2D>();
		_myCol = GetComponent<CircleCollider2D>();
		_mySprite = GetComponent<SpriteRenderer>();
		_mySprite.color = RocketSystem.Instance.GetComponent<SpriteRenderer>().color;
		_myTr = transform;
		_circleControl = CircleController.Instance;
		_colorTableComponent = ColorTableComponent.Instance;
		SetStartPosition();
		FlyUp(_flyUpPosition.y);
		RecycleCircles();
	}
	private void RecycleCircles(){
		var circleCountBurst = _colorTableComponent.ColorTable.Length;
		Transform pool = ObjectPoolSystem.Instance.GetAPool(0);
		pool.SetParent(transform);
		for(int i = 0; i < pool.childCount; i++){
			GameObject circle = pool.GetChild(i).gameObject;
			circle.GetComponent<Rigidbody2D>().gravityScale = 0;
			circle.GetComponent<CircleCollider2D>().isTrigger = true;
			circle.transform.localScale = Vector3.one * _radiusCircleChild;
			_circles.Add(circle);
			circle.SetActive(false);
		}
	}
	private void SetStartPosition(){
		_flyUpPosition.x = (_circleControl.TOPRIGHTSCREEN.x + _circleControl.BOTTOMLEFTSCREEN.x )/2;
		_flyUpPosition.y = (_circleControl.TOPRIGHTSCREEN.y + _circleControl.BOTTOMLEFTSCREEN.y)/2;
		Vector2 startPosition;
		startPosition.x = _flyUpPosition.x;
		startPosition.y = _circleControl.BOTTOMLEFTSCREEN.y - _radiusCircleParent;
		_myTr.position = startPosition;		
	}
	private void RevivalMySelf(bool state){
		if(_mySprite == null || _myCol == null) return;
		
			Debug.Log("RevivalMySelf");
		_mySprite.enabled = state;
		_myCol.enabled = state;
	}
	private void FlyUp(float flyUpY){
		if(_myTr == null) return;
		_myTr.DOMoveY(flyUpY + 2, _timeFlyUp).OnComplete(()=>{
			_myTr.DOMoveY(flyUpY, _timeFlyUp/50);
		});
		_myTr.DOScale(0.8f * _radiusCircleParent, 1);
	}
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == TagComponent.TOMAHOK){
			_myTr.DOPause();
			Debug.Log("Collision Tomahok");
			for(int i = 0; i < _circles.Count; i++){
				_circles[i].SetActive(true);
			}
			StartCoroutine(BurstCircles(_timeBurstRotate));
			RevivalMySelf(false);
		}
	}
	WaitForSeconds wait = new WaitForSeconds(1);
	private IEnumerator BurstCircles(float time){
		wait = new WaitForSeconds(time/_circles.Count);
		for(int i = 0; i < _circles.Count; i++){
			var circle = _circles[i].transform;
			circle.eulerAngles = new Vector3(circle.eulerAngles.x, circle.eulerAngles.y * 0.5f, 360f * (float)i/_circles.Count);
			circle.DOMove(circle.position - circle.right * 1, 1);
			yield return wait;
		}
	}
    public void ResetLevel()
    {
		if(_circles != null && _circles.Count > 0){
			for(int i = 0; i < _circles.Count; i++){
				_circles[i].SetActive(false);
			}
		}
        RevivalMySelf(true);
		FlyUp(_flyUpPosition.y);
    }
}
