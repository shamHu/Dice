using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	private class State {
		public static int INITIAL = 0,
		ROLLED = 1,
		DICESELECTED = 2;
	}

	private class Color {
		public static int WHITE = 0,
		GREEN = 1,
		BLUE = 2;
	}

	static int width = 14;
	static int height = 9;

	int state;

	float xOffset = -(width / 2);
	float yOffset = -(height / 2);
	
	GameObject[,] boardList = new GameObject[width, height];
	Sprite[] squareSpriteList;
	Sprite[][] diceFaceSpriteList = new Sprite[3][];

	GameObject currentlySelected;

	GameObject spawnDiceButtonTest;
	
	List<GameObject> DiceList = new List<GameObject>();

	GameObject newDice1, newDice2, newDice3;
	List<GameObject> RolledDice;

	GameObject selectedDice;
	
	void Awake () {
		squareSpriteList = Resources.LoadAll<Sprite>("Sprites/square");
		diceFaceSpriteList[Color.WHITE] = Resources.LoadAll<Sprite>("Sprites/diceFaces");
		diceFaceSpriteList[Color.GREEN] = Resources.LoadAll<Sprite>("Sprites/diceFacesGreen");
		diceFaceSpriteList[Color.BLUE] = Resources.LoadAll<Sprite>("Sprites/diceFacesBlue");
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
			dice.Color = Random.Range (1, 3);

			DiceList.Add (newDice);
		}

		currentlySelected = (GameObject) Instantiate (Resources.Load ("Prefabs/Border"));
		currentlySelected.transform.parent = this.transform;

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
					currentlySelected.GetComponent<SpriteRenderer>().enabled = true;
					currentlySelected.transform.position = hit.collider.gameObject.transform.position;

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

							RolledDice = new List<GameObject>();
							RolledDice.Add(newDice1);
							RolledDice.Add(newDice2);
							RolledDice.Add(newDice3);

							state = State.ROLLED;
						}
					}
				}
			}
		}
		else if (state == State.ROLLED) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if (hit.collider != null) {

					currentlySelected.transform.position = hit.collider.gameObject.transform.position;

					if (RolledDice.Contains (hit.collider.gameObject)) {
						selectedDice = hit.collider.gameObject;

						state = State.DICESELECTED;
					}
				}
			}
		}
		else if (state == State.DICESELECTED) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
				
				if (hit.collider != null) {
					bool containsClicked = false;

					foreach (GameObject go in boardList) {
						if (go == hit.collider.gameObject) {
							containsClicked = true;
						}
					}

					if (containsClicked) {
						GameObject clickedGO = hit.collider.gameObject;
						Square clickedSquare = clickedGO.GetComponent<Square>();

						int clickedX = clickedSquare.XPos;
						int clickedY = clickedSquare.YPos;

						bool clear = true;

						if (boardList[clickedX, clickedY].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						}
						if (boardList[clickedX + 1, clickedY].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						}
						if (boardList[clickedX, clickedY + 1].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						}
						if (boardList[clickedX - 1, clickedY].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						}
						if (boardList[clickedX, clickedY - 1].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						}
						if (boardList[clickedX, clickedY - 2].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						}

						if (clear) {
							boardList[clickedX, clickedY].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
							boardList[clickedX, clickedY].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];
							boardList[clickedX + 1, clickedY].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
							boardList[clickedX + 1, clickedY].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];
							boardList[clickedX, clickedY + 1].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
							boardList[clickedX, clickedY + 1].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];
							boardList[clickedX - 1, clickedY].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
							boardList[clickedX - 1, clickedY].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];
							boardList[clickedX, clickedY - 1].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
							boardList[clickedX, clickedY - 1].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];
							boardList[clickedX, clickedY - 2].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
							boardList[clickedX, clickedY - 2].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];

							currentlySelected.GetComponent<SpriteRenderer>().enabled = false;

							state = State.INITIAL;
						}
					}
				}
			}
		}
	}
}
