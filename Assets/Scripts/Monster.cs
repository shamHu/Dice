using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	private int health;
	public int Health {
		get { return health; }
		set { health = value; }
	}

	private int attack;
	public int Attack {
		get { return attack; } 
		set { attack = value; }
	}

	private int defense;
	public int Defense {
		get { return defense; }
		set { defense = value; }
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
