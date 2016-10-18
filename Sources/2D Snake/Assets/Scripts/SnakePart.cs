using UnityEngine;
using System.Collections;

public class SnakePart : MonoBehaviour 
{
	public bool isNeedToGrow; 
	public bool isGameOver; 

	void Awake () 
	{
		isNeedToGrow = false;
		isGameOver = false;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag("Food")) 
			{ isNeedToGrow = true; }
		else if (other.gameObject.CompareTag("Snake")) 
			{ isGameOver = true; }
	}
}