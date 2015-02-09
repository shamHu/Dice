using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	private class State {
		public static int INITIAL = 0;
	}

	private class Color {
		public static int WHITE = 0,
		GREEN = 1;
	}

	static int width = 14;
	static int height = 9;

	int state;

	float xOffset = -(width / 2);
	float yOffset = -(height / 2);
	
	GameObject[,] boardList = new GameObject[width, height];
	Sprite[] squareSpriteList;
	Sprite[][] diceFaceSpriteList = new Sprite[2][];

	GameObject spawnDiceButtonTest;
	
	List<GameObject> DiceList = new List<GameObject>();

	GameObject newDice1, newDice2, newDice3;
	
	void Awake () {
		squareSpriteList = Resources.LoadAll<Sprite>("Sprites/square");
		diceFaceSpriteList[Color.WHITE] = Resources.LoadAll<Sprite>("Sprites/diceFaces");
		diceFaceSpriteList[Color.GREEN] = Resources.LoadAll<Sprite>("Sprites/diceFacesGreen");
	}

	void Start () {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				GameObject newSquare = (GameObject) Instantiate (Resources.Load ("Prefabs/Square"));
				newSquare.GetComponent<Square>().XPos = x;
				newSquare.GetComponent<Square>().YPos = y;
				newSquare.GetComponent<SpriteRenderer>().sprite = squareSpriteList[Color.WHITE];
				newSquare.transform.position = new Vector2(x + xOffset, y + yOffset);
				newSquare.transform.parent = this.transform;

				boardList[x,y] = newSquare;
			}
		}

		for (int i = 0; i < 30; i++) {
			GameObject newDice = (GameObject) Instantiate (Resources.Load ("Prefabs/Dice"));

			Dice dice = newDice.GetComponent<Dice>();
			int[] tempFaces = new int[6] { 0, 1, 2, 3, 4, 5 };

			dice.Faces = tempFaces;
			dice.Color = Random.Range (0, 2);

			DiceList.Add (newDice);
		}

		state = State.INITIAL;

		spawnDiceButtonTest = (GameObject) Instantiate (Resources.Load ("Prefabs/DiceButton"));
		spawnDiceButtonTest.transform.position = new Vector2 (15 + xOffset, 8 + yOffset);
		spawnDiceButtonTest.transform.parent = this.transform;

	}

	void Update () {
		if (state == State.INITIAL) 
		{
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if (hit.collider != null)
				{
					if (hit.collider.gameObject == spawnDiceButtonTest) {

						if (DiceList.Count > 0) {
							int index1 = Random.Range (0, DiceList.Count);

							if (newDice1 != null) {
								newDice1.GetComponent<SpriteRenderer>().enabled = false;
							}

							newDice1 = DiceList[index1];
							DiceList.RemoveAt (index1);
							Dice dice1 = newDice1.GetComponent<Dice>();

							newDice1.transform.position = new Vector2(15 + xOffset, 6 + yOffset);
							newDice1.transform.parent = this.transform;
							newDice1.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[dice1.Color][
								dice1.Faces[Random.Range(0, 6)]];

							int index2 = Random.Range (0, DiceList.Count);

							if (newDice2 != null) {
								newDice2.GetComponent<SpriteRenderer>().enabled = false;
							}

							newDice2 = DiceList[index2];
							DiceList.RemoveAt (index2);
							Dice dice2 = newDice2.GetComponent<Dice>();
							
							newDice2.transform.position = new Vector2(15 + xOffset, 5 + yOffset);
							newDice2.transform.parent = this.transform;
							newDice2.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[dice2.Color][
								dice2.Faces[Random.Range(0, 6)]];

							int index3 = Random.Range (0, DiceList.Count);

							if (newDice3 != null) {
								newDice3.GetComponent<SpriteRenderer>().enabled = false;
							}

							newDice3 = DiceList[index3];
							DiceList.RemoveAt (index3);
							Dice dice3 = newDice3.GetComponent<Dice>();
							
							newDice3.transform.position = new Vector2(15 + xOffset, 4 + yOffset);
							newDice3.transform.parent = this.transform;
							newDice3.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[dice3.Color][
								dice3.Faces[Random.Range(0, 6)]];

						}
					}
//					else {
//						if (hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == squareSpriteList[Color.WHITE]) {
//							hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = squareSpriteList[Color.GREEN];
//						}
//						else if (hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == squareSpriteList[Color.GREEN]) {
//							hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = squareSpriteList[Color.WHITE];
//						}
//					}
				}
			}
		}
	}
}
