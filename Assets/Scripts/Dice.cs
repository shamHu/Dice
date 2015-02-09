using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

	private int color;
	public int Color {
		get { return color; }
		set { color = value; } 
	}

	private int[] faces;
	public int[] Faces {
		get { return faces; }
		set { faces = value; }
	}

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
