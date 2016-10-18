using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Snake : MonoBehaviour 
{
	enum Direction {LEFT, UP, RIGHT, DOWN};
	Direction dir = Direction.UP;

	int frames = 0;
	int framesPerStep = 10;
	int scoreCoef = 3;

	List<GameObject> snakeParts = new List<GameObject>();

	public GameObject Part;
	public Text ScoreText;

	void Start () 
	{
		int bound = (int) Camera.main.orthographicSize;
		int x = Random.Range (-bound, bound - 1);
		AddPart (new Vector2 (x + .5f, - (bound - .5f)));		
	}
	
	void Update () 
	{
		frames++;
		CheckDir();

		if (IsNeedToGrow()) 
			{ Grow (); }

		if (frames % framesPerStep == 0)
		{
			if ( IsGameOver () ) 
				{ Restart (); }
			else 
				{ Move (); }
		}		
	}

	void AddPart (Vector2 coor) 
	{
		GameObject partInst = Instantiate(Part, coor, Quaternion.identity) as GameObject;
		partInst.transform.SetParent(gameObject.transform, false);
		snakeParts.Insert(0, partInst);

		UpdateScore ();
	}

	void UpdateScore () 
	{
		ScoreText.text = snakeParts.Count.ToString ();
		UpdateFramesPerStep();
	}

	void UpdateFramesPerStep () 
	{
		int score = snakeParts.Count;
		if (score % scoreCoef == 0 && framesPerStep > 1) 
			{ framesPerStep--; }
	}

	void CheckDir () 
	{
		if (Input.GetKeyDown(KeyCode.LeftArrow) && dir != Direction.RIGHT) 
			{ dir = Direction.LEFT; }
		else if (Input.GetKeyDown(KeyCode.UpArrow) && dir != Direction.DOWN) 
			{ dir = Direction.UP; }
		else if (Input.GetKeyDown(KeyCode.RightArrow) && dir != Direction.LEFT) 
			{ dir = Direction.RIGHT; }
		else if (Input.GetKeyDown(KeyCode.DownArrow) && dir != Direction.UP) 
			{ dir = Direction.DOWN; }
	}

	bool IsNeedToGrow () 
	{
		if (GetHead().GetComponent<SnakePart>().isNeedToGrow) 
		{
			GetHead().GetComponent<SnakePart>().isNeedToGrow = false;
			return true;
		}
		else return false;
	}

	GameObject GetHead () 
	{
		return snakeParts[0] as GameObject;
	}

	void Grow () 
	{
		AddPart (GetNextPos());
	}

	Vector2 GetNextPos () 
	{
		Vector2 nextPos = snakeParts[0].transform.position;

		switch (dir) 
		{
			case Direction.LEFT:
				nextPos += Vector2.left;
				break;	
			case Direction.UP:
				nextPos += Vector2.up;
				break;	
			case Direction.RIGHT:
				nextPos += Vector2.right;
				break;	
			case Direction.DOWN:
				nextPos += Vector2.down;
				break;
		}

		return nextPos;
	}

	bool IsGameOver () 
	{
		if (GetHead().GetComponent<SnakePart>().isGameOver || 
			!IsValidPos(GetNextPos())) 
			{ return true; }
		else 
			{ return false; }
	}

	bool IsValidPos (Vector2 nextPos) 
	{
		if ((-Camera.main.orthographicSize - .5f) < nextPos.x &&
			nextPos.x < (Camera.main.orthographicSize + .5f) &&
			(-Camera.main.orthographicSize - .5f) < nextPos.y &&
			nextPos.y < (Camera.main.orthographicSize + .5f )) 
			{ return true; }
		else 
			{ return false; }
	}

	void Restart () 
	{
		DestroyAllParts ();

		frames = 0;
		framesPerStep = 10;

		dir = Direction.UP;

		Start ();
	}

	void DestroyAllParts () 
	{
		for (int i = snakeParts.Count - 1; i >= 0; i--)
		{
			Object.Destroy(snakeParts[i]);
			snakeParts.RemoveAt(i);
		}		
	}

	void Move () 
	{
		Vector2 nextPos = GetNextPos();
		GameObject tail = RemoveTail ();
		tail.transform.position = nextPos;
		snakeParts.Insert(0, tail);
	}

	GameObject RemoveTail () 
	{
		GameObject tail = snakeParts[snakeParts.Count-1];
		snakeParts.RemoveAt(snakeParts.Count-1);
		return tail;
	}
}