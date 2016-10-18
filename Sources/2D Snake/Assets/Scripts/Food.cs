using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour 
{
	void Awake () 
	{
		SetRandomPos ();
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag("Snake")) 
			{ SetRandomPos (); }
	}

	void SetRandomPos () 
	{
		int bound = (int) Camera.main.orthographicSize;

		int x = Random.Range (-bound, bound - 1);
		int y = Random.Range (-bound, bound - 1);

		gameObject.transform.position = new Vector2 (x + .5f, y + .5f);
	}
}