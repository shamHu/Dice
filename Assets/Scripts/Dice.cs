using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

//	private int color;
//	public int Color {
//		get { return color; }
//		set { color = value; } 
//	}
//
//	private int[] faces;
//	public int[] Faces {
//		get { return faces; }
//		set { faces = value; }
//	}

	private DieFace[] dieFaces;
	public DieFace[] DieFaces {
		get { return dieFaces; }
		set { dieFaces = value; }
	}

	private DieFace rolledFace;
	public DieFace RolledFace {
		get { return rolledFace; }
		set { rolledFace = value; }
	}

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
