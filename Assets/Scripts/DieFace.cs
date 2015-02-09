using UnityEngine;
using System.Collections;

public class DieFace : MonoBehaviour {

	private int color;
	public int Color {
		get { return color; }
		set { color = value; }
	}

	private int type;
	public int Type {
		get { return type; }
		set { color = value; }
	}

	void Start () {
		color = Random.Range (0, 9);
		type = Random.Range (0, 9);
	}

	void Update () {
	
	}
}
