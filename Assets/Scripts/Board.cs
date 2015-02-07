using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	static int width = 14;
	static int height = 9;

	float xOffset = -(width / 2);
	float yOffset = -(height / 2);
	
	GameObject[,] boardList = new GameObject[width, height];
	Sprite[] squareSpriteList;
	Sprite[] diceFaceSpriteList;

	GameObject spawnDiceButtonTest;

	const int WHITE = 0, GREEN = 1;


	void Awake () {
		squareSpriteList = Resources.LoadAll<Sprite>("Sprites/square");
		diceFaceSpriteList = Resources.LoadAll<Sprite>("Sprites/diceFaces");
	}

	void Start () {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				GameObject newSquare = (GameObject) Instantiate (Resources.Load ("Prefabs/Square"));
				newSquare.GetComponent<Square>().XPos = x;
				newSquare.GetComponent<Square>().YPos = y;
				newSquare.GetComponent<SpriteRenderer>().sprite = squareSpriteList[WHITE];
				newSquare.transform.position = new Vector2(x + xOffset, y + yOffset);
				newSquare.transform.parent = this.transform;

				boardList[x,y] = newSquare;
			}
		}

		spawnDiceButtonTest = (GameObject) Instantiate (Resources.Load ("Prefabs/DiceButton"));
		spawnDiceButtonTest.transform.position = new Vector2 (15 + xOffset, 8 + yOffset);
		spawnDiceButtonTest.transform.parent = this.transform;

	}

	void Update () {

		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if (hit.collider != null)
			{

				if (hit.collider.gameObject == spawnDiceButtonTest) {

					GameObject newDiceFace1 = (GameObject) Instantiate (Resources.Load ("Prefabs/DiceFace"));
					GameObject newDiceFace2 = (GameObject) Instantiate (Resources.Load ("Prefabs/DiceFace"));
					GameObject newDiceFace3 = (GameObject) Instantiate (Resources.Load ("Prefabs/DiceFace"));
					
					newDiceFace1.transform.position = new Vector2(15 + xOffset, 6 + yOffset);
					newDiceFace2.transform.position = new Vector2(15 + xOffset, 5 + yOffset);
					newDiceFace3.transform.position = new Vector2(15 + xOffset, 4 + yOffset);
					
					newDiceFace1.transform.parent = this.transform;
					newDiceFace2.transform.parent = this.transform;
					newDiceFace3.transform.parent = this.transform;
					
					newDiceFace1.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[Random.Range(0, 9)];
					newDiceFace2.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[Random.Range(0, 9)];
					newDiceFace3.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[Random.Range(0, 9)];

				}
				else {
					if (hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == squareSpriteList[WHITE]) {
						hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = squareSpriteList[GREEN];
					}
					else if (hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == squareSpriteList[GREEN]) {
						hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = squareSpriteList[WHITE];
					}
				}
			}
		}



	}
}
