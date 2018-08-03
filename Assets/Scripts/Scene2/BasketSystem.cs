using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasketSystem : MonoBehaviour {
	[SerializeField] private Text _tPointCount;
	private int _pointCount;
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "ball"){
			_pointCount ++;
			_tPointCount.text = _pointCount.ToString();
		}
	}
}
