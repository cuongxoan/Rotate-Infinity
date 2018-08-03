using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RocketSystem : Singleton<RocketSystem> {
	[SerializeField] private bool _isNextLevel;
	[SerializeField] private bool _isStop;
	[SerializeField] private bool _isStopLevel;
	public List<GameObject> Levels;
	public List<int> CircleParentCount;
	private int _currentLevel;
	[SerializeField] private float _maxLimitMoveSpeed = 5;
	[SerializeField] private Transform _target;
	private int _pointCount;
	[SerializeField] private TextScoreSystem _tWin; 
	[SerializeField] private Image _changeColor;
	private Transform _myTr;
	[SerializeField]private ParticleSystem _mySmokePar;
	[SerializeField]private ParticleSystem _myCollisionPar;
	private Rigidbody2D _myRb;
	private SpriteRenderer _mySprite;
	private TrailRenderer _myTrail;
	[SerializeField] private float _moveSpeed;
	[SerializeField] private float _rotateSpeed;
	[SerializeField] private float _moveGravity = 1f;
	private Color _myColor;
	private List<Color> _colorList = new List<Color>();
	private Color[] _backgroundColor;
	private ShakeSystem _shakeSystem;
	private Vector2 _originPosition;
	private float _moveSpeedOrigin;
	// Use this for initialization
	void Start () {
		_originPosition = transform.position;
		_moveSpeedOrigin = _moveSpeed;
		_shakeSystem = FindObjectOfType<ShakeSystem>();
		_myRb = GetComponent<Rigidbody2D>();
		_myTr = transform;
		_myTr.tag = TagComponent.TOMAHOK;
		_mySprite = GetComponent<SpriteRenderer>();
		_myTrail = GetComponentInChildren<TrailRenderer>();
		_backgroundColor = ColorTableComponent.Instance.BackgroundColorTable;
		Levels[0].SetActive(true);
		ResetTomahok();
	}
	public void ResetTomahok(){
		_moveSpeed = _moveSpeedOrigin;
		Color newCamColor;
		do{newCamColor = _backgroundColor[Random.Range(0, _backgroundColor.Length)];}
		while(newCamColor.Equals(Camera.main.backgroundColor));
		Camera.main.backgroundColor = newCamColor;
		transform.position= _originPosition;
		var colorTable = ColorTableComponent.Instance.ColorTable;
		for(int i = 0; i < CircleParentCount[_currentLevel]; i++)
			_colorList.AddRange(colorTable);
		SetNewColor();
		SetActiveCircleParent();
		
	}
	private void SetActiveCircleParent(){
		for(int i = 0; i < Levels[0].transform.childCount; i++){
			if(i < CircleParentCount[0]){
				Levels[0].transform.GetChild(i).GetComponent<ILevel>().ResetLevel();
			}else if(Levels[0].transform.GetChild(i).GetComponent<ParticleSystem>() == null){
				Levels[0].transform.GetChild(i).gameObject.SetActive(false);
			}
		}
	}
	public void NextLevel(){
		Levels[_currentLevel].SetActive(false);
		SetActiveCircleParent();
		Debug.Log("Next Level");
		if(Levels.Count - 1 == _currentLevel){
			Debug.LogError("Load Scene");
			SceneManager.LoadScene(0);
		}
		_currentLevel++;
		Levels[_currentLevel].SetActive(true);
		ResetTomahok();
	}
	void OnGUI()
    {
        //Press this Button to increase the sibling index number of the GameObject
        if (GUI.Button(new Rect(0, 0, 400, 80), "Eat Current Circle"))
        {
			for(int i = 0; i < CircleParentCount[_currentLevel]; i++){
				var circleParent = Levels[_currentLevel].transform.GetChild(i);
				for(int j = 0; j < circleParent.childCount; j++){
					var circle = circleParent.GetChild(j).gameObject;
					if(circle.activeInHierarchy && circle.GetComponent<SpriteRenderer>().color == _myColor){
						StartCoroutine(EatCircle(circle));
						return;
					}
				}
			}
		}
	}
	// Update is called once per frame
	void Update () {
		if(_isStop){
			_myRb.constraints = RigidbodyConstraints2D.FreezePosition;
		}
		else{
			_myRb.constraints = RigidbodyConstraints2D.None;
		}
		if(_isStopLevel){
			Levels[_currentLevel].gameObject.SetActive(false);
		}
		else{
			Levels[_currentLevel].gameObject.SetActive(true);
		}
		_myTrail.time = 0.3f/_moveSpeed;
		if(_isNextLevel){
			_isNextLevel = false;
			NextLevel();
		}
		Vector2 direction = (Vector2)_target.transform.position - _myRb.position;
		direction.Normalize();
		float rotateAmount = Vector3.Cross(direction, _myTr.up).z;

		_myRb.angularVelocity = -rotateAmount * _rotateSpeed;
		_myRb.velocity = transform.up * _moveSpeed;
		if(_moveSpeed <= _maxLimitMoveSpeed)
			_moveSpeed += Time.deltaTime * _moveGravity;
	}
	private void SetNewColor(){		
		_myColor = _colorList[Random.Range(0, _colorList.Count)];
		_mySprite.color = _myColor;
		_myTrail.material.color = _myColor;
		_changeColor.color = _myColor;
		Gradient gradient = new Gradient();
		gradient.SetKeys(new GradientColorKey[] { 
			new GradientColorKey(_myColor, 0.0f), 
			new GradientColorKey(_myColor, 1.0f) }, 
			new GradientAlphaKey[] { 
				new GradientAlphaKey(1.0f, 0.0f), 
				new GradientAlphaKey(0.0f, 1.0f) 
				});
		var col = _mySmokePar.colorOverLifetime;
		col.color = gradient;
		var col1 = _myCollisionPar.colorOverLifetime;
		col1.color = gradient;
		_myCollisionPar.Play();
	}
	WaitForSeconds wait = new WaitForSeconds(0.3f);
	private IEnumerator EatCircle(GameObject circle){
		circle.GetComponent<Collider2D>().enabled = false;
		circle.GetComponentInChildren<ParticleSystem>().Play();
		//_shakeSystem.ShakeCamera(circle.gameObject, .5f, .2f);
		_colorList.Remove(_myColor);
		if(circle.transform.localScale.x <= 0.5f) {
			_pointCount += 2;
			_tWin.AddScore(_pointCount, true);
		}
		else {
			_pointCount += 1;
			_tWin.AddScore(_pointCount, false);
		}
		circle.GetComponent<CircleCollider2D>().enabled = false;
		yield return wait; 
		circle.gameObject.SetActive(false);
		if(_colorList.Count == 0) {
			//SceneManager.LoadScene(0);
			NextLevel();
		}
		else SetNewColor();
	}
	private void NotEatCircle(GameObject circle){
		_myRb.AddForce((_myTr.position - circle.transform.position).normalized  * 2000f);
		ResetTomahok();
	}
	private IEnumerator OnTriggerEnter2D(Collider2D collisionInfo)
	{
		if(collisionInfo.gameObject.Equals(_changeColor.gameObject)){
			SetNewColor();
		}
		if(collisionInfo.tag == TagComponent.OBTACLE){
			var collisionColor = collisionInfo.GetComponent<SpriteRenderer>().color;
			if(collisionColor == _myColor ){
				StartCoroutine(EatCircle(collisionInfo.gameObject));
			}
			else{
				NotEatCircle(collisionInfo.gameObject);
			}
			yield return null;
		}
	}
}
