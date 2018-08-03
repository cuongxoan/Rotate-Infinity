using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindshieldSystem : MonoBehaviour {
	public bool IsShake;
	private Transform[] children;
	[SerializeField] private float _rotateSpeed;
	private int _directionTouch;
	private float _touchAmount;
	private Vector2 _touchPosition;
	// Use this for initialization
	void Start () {
		children = new Transform[transform.childCount];
		for(int i = 0; i < children.Length; i++)
			children[i] = transform.GetChild(i);
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID
		TouchFunction();
		#endif
		#if UNITY_EDITOR
		MouseFunction();
		#endif
			
		for(int i = 0; i < children.Length; i++){
			children[i].RotateAround(children[i].transform.position , transform.forward, _rotateSpeed * _touchAmount);
		}
	}
	private void TouchFunction(){
		if(Input.touchCount > 0){
			Touch touch = Input.GetTouch(0);
			switch(touch.phase){
				case TouchPhase.Began:
					_touchPosition = touch.position;
				break;
				case TouchPhase.Moved:
					float deltaTouchPosX = touch.position.x - _touchPosition.x;
					if(deltaTouchPosX > 0){
						_directionTouch = 1;
					}
					else if(deltaTouchPosX < 0){
						_directionTouch = -1;
					}					
					_touchAmount = deltaTouchPosX;
					_touchPosition = touch.position;
				break;
			}
		}
	}
	private void MouseFunction(){
		if(Input.GetMouseButtonDown(0)){
			_touchPosition = Input.mousePosition;
		}
		else if(Input.GetMouseButton(0)){
			float deltaTouchPosX = Input.mousePosition.x - _touchPosition.x;
			if(deltaTouchPosX > 0){
				_directionTouch = 1;
			}
			else if(deltaTouchPosX < 0){
				_directionTouch = -1;
			}					
			_touchAmount = deltaTouchPosX;
			_touchPosition = Input.mousePosition;
		}
	}
}
