using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
	public string EnemyStartingLocations;
	public Rect SpawnLocation;

	public GameObject EnemyPrefab;

	public Vector3 MoveVector { get; private set; }
	public float Speed { get; private set; }

	private enum MoveDirection { Left, Right, Down}

	private MoveDirection CurrentDirection;
	private MoveDirection NextDirection;

	private float AmountMovedDown;

	// Use this for initialization
	void Start ()
	{
		var rows = EnemyStartingLocations.Split(',');
		var currentLocation = new Vector2(SpawnLocation.x, SpawnLocation.y);
		var minSpacing = float.MaxValue;

		foreach (var row in rows)
		{
			int numEnemiesInRow = Convert.ToInt32(row);

			var spacing = (SpawnLocation.width - numEnemiesInRow) / (numEnemiesInRow - 1);

			minSpacing = spacing < minSpacing ? spacing : minSpacing;
		}

		foreach (var row in rows)
		{
			int numEnemiesInRow = Convert.ToInt32(row);

			var rowSize = numEnemiesInRow + ((numEnemiesInRow - 1) * minSpacing);
			var padding = (SpawnLocation.width - rowSize) / 2;

			currentLocation = new Vector2(currentLocation.x + padding, currentLocation.y);

			for (int i = 0; i < numEnemiesInRow; i++)
			{
				var enemy = (GameObject)Instantiate(EnemyPrefab, new Vector3(currentLocation.x, 0, currentLocation.y), Quaternion.identity);
				var enemyScript = enemy.GetComponent<EnemyScript>();
				enemyScript.EnemyManager = this;
				currentLocation = new Vector2(currentLocation.x + minSpacing + 1, currentLocation.y);
			}
			 
			currentLocation = new Vector2(SpawnLocation.x, currentLocation.y - 2);
		}

		this.MoveVector = new Vector3(1, 0, 0);
		this.CurrentDirection = MoveDirection.Right;
		this.Speed = 3;
	}

	public void EndReached()
	{
		if (this.CurrentDirection == MoveDirection.Down)
		{
			return;
		}

		if (this.CurrentDirection == MoveDirection.Right)
		{
			this.NextDirection = MoveDirection.Left;
		}
		else
		{
			this.NextDirection = MoveDirection.Right;
		}

		this.CurrentDirection = MoveDirection.Down;
		this.MoveVector = new Vector3(0, 0, -1);
		this.AmountMovedDown = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.CurrentDirection == MoveDirection.Down)
		{
			this.AmountMovedDown += Time.deltaTime * Speed;

			if (this.AmountMovedDown >= 1)
			{
				if (this.NextDirection == MoveDirection.Right)
				{
					this.MoveVector = new Vector3(1, 0, 0);
					this.CurrentDirection = MoveDirection.Right;
				}
				else
				{
					this.MoveVector = new Vector3(-1, 0, 0);
					this.CurrentDirection = MoveDirection.Left;
				}
			}
		}
	}
}
