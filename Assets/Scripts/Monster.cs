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

	private static string spritePath;
	public string SpritePath {
		get { return spritePath; }
		set { spritePath = value; }
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

	public virtual void init() {
		Debug.Log ("Initializing base Monster class.");
	}

	public virtual void attackTarget(Monster target) {
		Debug.Log ("Attacking from Base Monster class (this should never happen).");
	}
}

public class Bulbasaur : Monster {

	override public void init() {
		Health = 8;
		Attack = 5;
		Defense = 1;
		Owner = 0;
		SpritePath = "Sprites/Monsters/bulbasaur";
	}

	override public void attackTarget(Monster target) {
		Debug.Log ("attacking from NEW BULBASAUR CLASS??");
	}

}

public class Squirtle : Monster {

	override public void init() {
		Health = 10;
		Attack = 3;
		Defense = 1;
		Owner = 1;
		SpritePath = "Sprites/Monsters/squirtle";
	}

	override public void attackTarget(Monster target) {
		Debug.Log ("attacking from NEW Squirtle CLASS??");
	}
	
}
