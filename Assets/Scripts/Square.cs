﻿using UnityEngine;
using System.Collections;

public class Square : MonoBehaviour {

	private int xPos;
	public int XPos { 
		get { return xPos; } 
		set { xPos = value; }
	}

	private int yPos;
	public int YPos { 
		get { return yPos; } 
		set { yPos = value; }
	}

	private int squareType;
	public int SquareType {
		get { return squareType; }
		set { squareType = value; }
	}

	void Start () {
	
	}
	
	void Update () {

	}
}
