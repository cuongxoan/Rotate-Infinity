using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool{
	public GameObject[] Circles;
	public int Key;
	public Pool(GameObject[] circles, int key){
		Circles = circles;
		this.Key = key;
	}
}
public class ObjectPoolSystem : Singleton<ObjectPoolSystem> {

	public Dictionary<int, Queue<GameObject>> dictionary = new Dictionary<int, Queue<GameObject>>();
	public int key = 0;
	[SerializeField] private Transform _parCircle;
	private List<Transform> pools = new List<Transform>();
	public Pool CreatePool(GameObject gameobject, int count, Transform parent = null){
		//if(gameObject.GetComponent<SpriteRenderer>() == null) return null;
		Debug.Log("Key : " + key);
		{
			Queue<GameObject> queue = new Queue<GameObject>();
			dictionary.Add(key, queue);
			Transform pool = new GameObject("pool").transform;
			for(int i = 0; i < count; i++){
				GameObject newItem = Instantiate(gameobject);
				if(parent == null){
					pool.SetParent(transform);
					newItem.transform.SetParent(pool);
				}
				var par = Instantiate(_parCircle, newItem.transform);
				par.transform.localPosition = Vector3.zero;
				newItem.SetActive(false);
				queue.Enqueue(newItem);
				Debug.Log(queue.Count);
			}
			pools.Add(pool);
			key++;
			return new Pool(queue.ToArray(), key);
		}
		
	}
	public Transform GetAPool(int key){
		for(int j = 0; j < pools[key].childCount; j++)
			pools[key].GetChild(key).gameObject.SetActive(true);
		
		return pools[key];
	}
	public Queue<GameObject> GetQueuePool(int key){
		return dictionary[key];
	}
	public Dictionary<int, Queue<GameObject>> GetAllQueuePool(){
		return dictionary;
	}
}
