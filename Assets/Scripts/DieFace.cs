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

	public DieFace(int color, int type) {
		this.color = color;
		this.type = type;
	}

	void Start () {
	
	}

	void Update () {
	
	}
}
