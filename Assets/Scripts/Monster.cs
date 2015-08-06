using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Monster : MonoBehaviour {

	private int hp;
	public int HP {
		get { return hp; }
		set { hp = value; }
	}

	private int att;
	public int ATT {
		get { return att; } 
		set { att = value; }
	}

	private int def;
	public int DEF {
		get { return def; }
		set { def = value; }
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

	private string monsterName;
	public string MonsterName {
		get { return monsterName; }
		set { monsterName = value; }
	}

	private static string spritePath;
	public string SpritePath {
		get { return spritePath; }
		set { spritePath = value; }
	}

	protected RectTransform stats;
	public RectTransform Stats
	{
		get { return stats; }
	}

	protected Canvas monsterUICanvas;
	protected RectTransform canvasRect;

	void Update()
	{
		UpdateStatsPosition();
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

		monsterUICanvas = UIManager.Instance.MonsterUICanvas;
		canvasRect = monsterUICanvas.GetComponent<RectTransform>();

		stats = ((GameObject)Instantiate(UIManager.Instance.MonsterUIPrefab)).GetComponent<RectTransform>();
		stats.transform.FindChild("HP").GetComponent<Text>().text = HP.ToString();
		stats.transform.FindChild("ATT").GetComponent<Text>().text = ATT.ToString();
		stats.transform.FindChild("DEF").GetComponent<Text>().text = DEF.ToString();
		stats.transform.SetParent(monsterUICanvas.transform);
		stats.transform.localScale = Vector3.one;
		UpdateStatsPosition();
	}

	public virtual void attack(Monster target) {
		Debug.Log ("Attacking from Base Monster class (this should never happen).");
	}

	public void UpdateStatsPosition()
	{
		Vector2 ViewportPosition= Camera.main.WorldToViewportPoint(transform.position);
		
		Vector2 WorldObject_ScreenPosition = new Vector2(
			((ViewportPosition.x*canvasRect.sizeDelta.x)-(canvasRect.sizeDelta.x*0.5f)),
			((ViewportPosition.y*canvasRect.sizeDelta.y)-(canvasRect.sizeDelta.y*0.5f)));
		
		stats.anchoredPosition = WorldObject_ScreenPosition;
	}

	public bool isAdjacentTo(Monster target) {
		if (target.Position.x == this.Position.x) {
			if ((target.Position.y - this.Position.y) > -1 && 
			    (target.Position.y - this.Position.y < 1)) {
				return true;
			}
		}
		else if (target.Position.y == this.Position.y) {
			if ((target.Position.x - this.Position.x) > -1 && 
			    (target.Position.x - this.Position.x < 1)) {
				return true;
			}
		}
		return false;
	}
}

public class Bulbasaur : Monster {

	override public void init() {
		HP = 8;
		ATT = 5;
		DEF = 1;
		Owner = 0;
		MonsterName = "Bulbasaur";
		SpritePath = "Sprites/Monsters/bulbasaur";
		base.init();
	}

	override public void attack(Monster target) {
		if (isAdjacentTo(target)) {
			target.HP -= (ATT - target.DEF);
			Debug.Log ("Bulbasaur attacking! Target " + target.MonsterName + " at " + target.HP + "HP.");
		}
	}
}

public class Squirtle : Monster {

	override public void init() {
		HP = 10;
		ATT = 3;
		DEF = 1;
		Owner = 1;
		MonsterName = "Squirtle";
		SpritePath = "Sprites/Monsters/squirtle";
		base.init();
	}

	override public void attack(Monster target) {
		if (isAdjacentTo(target)) {
			target.HP -= (ATT - target.DEF);		
			Debug.Log ("Squirtle attacking! Target " + target.MonsterName + " at " + target.HP + "HP.");
		}
	}
}
