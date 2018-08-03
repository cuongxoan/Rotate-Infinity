using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeSystem : Singleton<ShakeSystem> {
	public bool debugMode = false;//Test-run/Call ShakeCamera() on start
 
	public float shakeAmount;//The amount to shake this frame.
	public float shakeDuration;//The duration this frame.
 
	//Readonly values...
	float shakePercentage;//A percentage (0-1) representing the amount of shake to be applied when setting rotation.
	float startAmount;//The initial shake amount (to determine percentage), set when ShakeCamera is called.
	float startDuration;//The initial shake duration, set when ShakeCamera is called.
 
	bool isRunning = false;	//Is the coroutine running right now?
 
	public bool smooth;//Smooth rotation?
	public float smoothAmount = 5f;//Amount to smooth
 
 
	public void ShakeCamera(GameObject gameObjectShake, float amount, float duration) {
 
		shakeAmount = amount;//Add to the current amount.
		startAmount = shakeAmount;//Reset the start amount, to determine percentage.
		shakeDuration = duration;//Add to the current time.
		startDuration = shakeDuration;//Reset the start time.
 
		if(!isRunning) StartCoroutine (Shake(gameObjectShake));//Only call the coroutine if it isn't currently running. Otherwise, just set the variables.
	}
 
 
	IEnumerator Shake(GameObject gameObjectShake) {
		isRunning = true;
		Debug.Log(gameObjectShake.name);
		while (shakeDuration > 0.01f) {
			if(gameObjectShake == null) break;
			Vector3 positionAmount = Random.insideUnitSphere * shakeAmount;//A Vector3 to add to the Local Rotation
			positionAmount.z = 0;//Don't change the Z; it looks funny.
 
			shakePercentage = shakeDuration / startDuration;//Used to set the amount of shake (% * startAmount).
 
			shakeAmount = startAmount * shakePercentage;//Set the amount of shake (% * startAmount).
			shakeDuration = Mathf.Lerp(shakeDuration, 0, Time.deltaTime);//Lerp the time, so it is less and tapers off towards the end.
 
 
			if(smooth)
				gameObjectShake.transform.localPosition = Vector3.Lerp(gameObjectShake.transform.localPosition, positionAmount, Time.deltaTime * smoothAmount);
			else
				gameObjectShake.transform.localPosition = positionAmount;//Set the local rotation the be the rotation amount.
 
			yield return null;
		}
		if(gameObjectShake != null)
			gameObjectShake.transform.localPosition = Vector3.zero;//Set the local rotation to 0 when done, just to get rid of any fudging stuff.
		isRunning = false;
	}
 
}
