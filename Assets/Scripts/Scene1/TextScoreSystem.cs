using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextScoreSystem : Singleton<TextScoreSystem> {
	private Text _myText;
	private float _originScale;
	private void Start()
	{
		_originScale = transform.localScale.x;
		_myText = GetComponent<Text>();
	}
	public void SetText(string text){
		_myText.text = text;
	}
	public void AddScore(int score, bool isBonus){
		_myText.text = score.ToString();
		int burst = isBonus ? 2 : 1;
		StartCoroutine(AnimationText(0.3f, transform.localScale.x + 1, burst));
	}
	private IEnumerator AnimationText(float duration, float scale, int burst = 1){
		float currentScale;
		float timeRun = 0;
		while(timeRun <= duration){
			timeRun += Time.deltaTime*1.5f;
			currentScale = Mathf.Lerp(_originScale, scale * burst, timeRun / duration);
			transform.localScale = Vector3.one * currentScale;
			yield return null;
		}
		timeRun = 0;
		while(timeRun <= duration){
			timeRun += Time.deltaTime / 2f;
			currentScale = Mathf.Lerp(scale * burst, _originScale, timeRun / duration);
			transform.localScale = Vector3.one * currentScale;
			yield return null;
		}
		transform.localScale = Vector3.one * _originScale;
	}
}
