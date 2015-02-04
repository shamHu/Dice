using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	static int width = 16;
	static int height = 9;

	float xOffset = -(width / 2);
	float yOffset = -(height / 2);
	
	GameObject[,] boardList = new GameObject[width, height];
	Sprite[] squareSpriteList;

	void Awake () {
		squareSpriteList = Resources.LoadAll<Sprite>("Sprites/square");
	}

	void Start () {
		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				GameObject newSquare = (GameObject) Instantiate (Resources.Load ("Prefabs/Square"));
				newSquare.GetComponent<Square>().XPos = x;
				newSquare.GetComponent<Square>().YPos = y;
				newSquare.GetComponent<SpriteRenderer>().sprite = squareSpriteList[0];
				newSquare.transform.position = new Vector2(x + xOffset, y + yOffset);
				newSquare.transform.parent = this.transform;

				boardList[x,y] = newSquare;
			}
		}
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

			if(hit.collider != null)
			{
				if (hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == squareSpriteList[0]) {
					hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = squareSpriteList[1];
				}
				else if (hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite == squareSpriteList[1]) {
					hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = squareSpriteList[0];
				}
			}
		}
	}
}
