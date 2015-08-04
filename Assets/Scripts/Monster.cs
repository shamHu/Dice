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

	private int owner;
	public int Owner {
		get { return owner; }
		set { owner = value; }
	}

	private Vector3 position;
	public Vector3 Position {
		get { return position; }
		set { position = value; }
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 move(GameObject clickedGO) {

		if (clickedGO.GetComponent<Square>().SquareType != 0) {
			float clickedX = clickedGO.transform.position.x;
			float clickedY = clickedGO.transform.position.y;
			float monsterX = this.position.x;
			float monsterY = this.position.y;
			float monsterZ = this.position.z;
			
			if (clickedX == monsterX) {
				if (clickedY == monsterY + 1) {
					this.position = new Vector3(monsterX, monsterY + 1, monsterZ);
				}
				else if (clickedY == monsterY - 1) {
					this.position = new Vector3(monsterX, monsterY - 1, monsterZ);
				}
			}
			else if (clickedY == monsterY) {
				if (clickedX == monsterX + 1) {
					this.position = new Vector3(monsterX + 1, monsterY, monsterZ);
				}
				else if (clickedX == monsterX - 1) {
					this.position = new Vector3(monsterX - 1, monsterY, monsterZ);
				}
			}
		}
		return this.position;
	}
}
