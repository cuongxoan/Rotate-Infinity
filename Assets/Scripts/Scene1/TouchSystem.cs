using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchSystem : MonoBehaviour {
[SerializeField] private Transform _target;
[SerializeField] private float _deltaYTarget;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID
		if(Input.touchCount > 0){
			Touch touch = Input.GetTouch(0);
			switch(touch.phase){
				case TouchPhase.Began:

				break;
				case TouchPhase.Moved:
				_target.position = (Vector2)Camera.main.ScreenToWorldPoint(touch.position) + Vector2.up * _deltaYTarget;
				break;
				case TouchPhase.Ended:

				break;
			}
		}
		#endif
		#if UNITY_EDITOR
		if(Input.GetMouseButton(0)){
			_target.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) + Vector2.up * _deltaYTarget;
		}
		#endif
	}
}
