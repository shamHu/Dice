using UnityEngine;
using System.Collections;

public class Monster : MonoBehaviour {

	private int hp;
	public int HP {
		get { return hp; }
		set { hp = value; }
	}

	private int att;
	public int ATT {
		get { return ATT; } 
		set { ATT = value; }
	}

	private int def;
	public int DEF {
		get { return DEF; }
		set { DEF = value; }
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

	public virtual void attack(Monster target) {
		Debug.Log ("Attacking from Base Monster class (this should never happen).");
	}
}

public class Bulbasaur : Monster {

	override public void init() {
		HP = 8;
		ATT = 5;
		DEF = 1;
		Owner = 0;
		SpritePath = "Sprites/Monsters/bulbasaur";
	}

	override public void attack(Monster target) {
		Debug.Log ("attacking from NEW BULBASAUR CLASS??");
	}

}

public class Squirtle : Monster {

	override public void init() {
		HP = 10;
		ATT = 3;
		DEF = 1;
		Owner = 1;
		SpritePath = "Sprites/Monsters/squirtle";
	}

	override public void attack(Monster target) {
		Debug.Log ("attacking from NEW Squirtle CLASS??");
	}
	
}
