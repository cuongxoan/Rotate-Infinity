using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : Singleton<CircleController> {
	[SerializeField] private Sprite _imgCircle;
	[SerializeField] private GameObject _particleCircle;
	[SerializeField] private int _circlePoolCount;
	
	public GameObject CIRCLE;
	public GameObject SpawnCircle(Vector2 position, float radiusCircle, Sprite imgCircle, Color circleColor, string name = "circle", Transform parent = null, bool isTrigger = false, string tag = "obtacle"){
		GameObject circle = new GameObject(name, typeof(CircleCollider2D), typeof(SpriteRenderer), typeof(Rigidbody2D));
		var circleCollier = circle.GetComponent<CircleCollider2D>();
		var spriteCircle = circle.GetComponent<SpriteRenderer>();
		var circleRigid = circle.GetComponent<Rigidbody2D>();
		circle.transform.SetParent(parent);
		circle.transform.localScale = Vector3.one * radiusCircle;
		circle.transform.position = position;
		circle.tag = tag;
		circleCollier.isTrigger = isTrigger;
		if(imgCircle == null) spriteCircle.sprite = _imgCircle;
		else spriteCircle.sprite = imgCircle;
		spriteCircle.color = circleColor;
		Transform par = Instantiate(_particleCircle).transform;
		par.SetParent(circle.transform);
		par.transform.localPosition = Vector3.zero;
		par.localScale = Vector3.one;
		return circle;
	}
	[HideInInspector]
	public Vector2 TOPRIGHTSCREEN;
	[HideInInspector]
	public Vector2 BOTTOMLEFTSCREEN;
	private void Awake(){
		TOPRIGHTSCREEN = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
		BOTTOMLEFTSCREEN = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
		SpawnPool();
	}
	private void Start(){
	}
	private void SpawnPool(){
		Color[] colorTable = ColorTableComponent.Instance.ColorTable;
		GameObject circle = CircleController.Instance.CIRCLE;
		for(int j = 0; j < _circlePoolCount; j++){
			Pool pool = ObjectPoolSystem.Instance.CreatePool(circle, colorTable.Length);	
			for(int i = 0; i < pool.Circles.Length; i++){
				pool.Circles[i].GetComponent<SpriteRenderer>().color = colorTable[i];
			}		
		}
	}
}
