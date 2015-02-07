using UnityEngine;
using System.Collections;

public class Dice : MonoBehaviour {

	private DieFace[] faces;
	public DieFace[] Faces {
		get { return faces; }
		set { faces = value; }
	}

	public Dice() {
		faces = new DieFace[6];
		faces [0] = new DieFace (0, 0);
		faces [1] = new DieFace (1, 1);
		faces [2] = new DieFace (2, 2);
		faces [3] = new DieFace (3, 3);
		faces [4] = new DieFace (4, 4);
		faces [5] = new DieFace (5, 5);

	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
