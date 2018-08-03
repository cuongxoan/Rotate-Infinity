using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationDevice : MonoBehaviour {
	public Text _rotate;
	public Text _rotatex;
	public Text _rotatey;
	public Text _rotatez;
	private int _count;
	private float _xValueAcceleration;
	private int _xValueInt;
	private bool[] _fourDireciton = new bool[4];
	private bool[] _movedPosition = new bool[10];
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		_xValueAcceleration = Input.acceleration.x;
		_rotate.text =  _count.ToString();
		_rotatex.text = Input.acceleration.x.ToString();
		_rotatey.text = Input.acceleration.y.ToString();
		_rotatez.text = Input.acceleration.z.ToString();

		_xValueInt = (int)(_xValueAcceleration*10);
		_movedPosition[_xValueInt] = true;

		if(CheckFinishRound()) _count++;
	}
	private bool CheckFinishRound(){
		for(int i = 0; i < _movedPosition.Length; i++){
			if(!_movedPosition[i]) return false;
		}
		for(int i = 0; i < _movedPosition.Length; i++){
			_movedPosition[i] = false;
		}
		return true;
	}
}
