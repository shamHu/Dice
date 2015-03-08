using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	private class State {
		public static int INITIAL = 0,
		ROLLED = 1,
		DICESELECTED = 2,
		TEMPLATESELECTED = 3,
		TEMPLATEPLACED = 4,
		MONSTERSELECTED = 5;
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

	int templateID;
	
	GameObject[,] boardList = new GameObject[width, height];
	Sprite[][] diceFaceSpriteList = new Sprite[3][];
	Sprite[] squareSpriteList;
	Sprite[] diceTemplateSpriteList;
	List<GameObject> DiceList = new List<GameObject>();
	List<GameObject> RolledDice;
	List<GameObject> MonsterList;

	GameObject currentlySelected;
	GameObject spawnDiceButtonTest;
	GameObject newDice1, newDice2, newDice3;
	GameObject selectedDice;
	GameObject overlay;
	GameObject diceTemplate;
	GameObject nextTemplateButton,
		prevTemplateButton,
		rotateRightButton,
		rotateLeftButton,
		mirrorButton,
		selectTemplateButton;
	GameObject selectedMonster;

	void Awake () {
		squareSpriteList = Resources.LoadAll<Sprite>("Sprites/square");
		diceFaceSpriteList[Color.WHITE] = Resources.LoadAll<Sprite>("Sprites/diceFaces");
		diceFaceSpriteList[Color.GREEN] = Resources.LoadAll<Sprite>("Sprites/diceFacesGreen");
		diceFaceSpriteList[Color.BLUE] = Resources.LoadAll<Sprite>("Sprites/diceFacesBlue");
		diceTemplateSpriteList = Resources.LoadAll<Sprite>("Sprites/DiceTemplates/masterTemplate");
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
			newDice.GetComponent<BoxCollider2D>().enabled = false;
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
		spawnDiceButtonTest.transform.position = new Vector3 (15 + xOffset, 8 + yOffset, 1);
		spawnDiceButtonTest.transform.parent = this.transform;

		overlay = (GameObject) Instantiate (Resources.Load ("Prefabs/Overlay"));
		overlay.GetComponent<SpriteRenderer>().color = new UnityEngine.Color(1f, 1f, 1f, 0.5f);
		overlay.GetComponent<SpriteRenderer>().enabled = false;
		overlay.transform.position = new Vector3 (0, 0, -2);

		templateID = 0;

		diceTemplate = (GameObject) Instantiate (Resources.Load ("Prefabs/DiceTemplate"));
		diceTemplate.GetComponent<SpriteRenderer>().sprite = diceTemplateSpriteList[0];
		diceTemplate.GetComponent<SpriteRenderer>().enabled = false;
		diceTemplate.GetComponent<DiceTemplate>().setTemplate(templateID);
		diceTemplate.transform.position = new Vector3 (0, 0, -3);

		nextTemplateButton = (GameObject) Instantiate (Resources.Load ("Prefabs/TriangleButton"));
		nextTemplateButton.GetComponent<SpriteRenderer>().enabled = false;
		nextTemplateButton.transform.position = new Vector3 (6, 0, -3);
		nextTemplateButton.transform.Rotate(new Vector3(0, 0, 180));

		prevTemplateButton = (GameObject) Instantiate (Resources.Load ("Prefabs/TriangleButton"));
		prevTemplateButton.GetComponent<SpriteRenderer>().enabled = false;
		prevTemplateButton.transform.position = new Vector3 (-6, 0, -3);

		rotateLeftButton = (GameObject) Instantiate (Resources.Load ("Prefabs/RotateButton"));
		rotateLeftButton.GetComponent<SpriteRenderer>().enabled = false;
		rotateLeftButton.transform.position = new Vector3 (-2, -4, -3);

		rotateRightButton = (GameObject) Instantiate (Resources.Load ("Prefabs/RotateButton"));
		rotateRightButton.GetComponent<SpriteRenderer>().enabled = false;
		rotateRightButton.transform.position = new Vector3 (2, -4, -3);
		rotateRightButton.transform.Rotate (new Vector3(0, 180, 0));

		mirrorButton = (GameObject) Instantiate (Resources.Load ("Prefabs/MirrorButton"));
		mirrorButton.GetComponent<SpriteRenderer>().enabled = false;
		mirrorButton.transform.position = new Vector3 (0, 4, -3);

		selectTemplateButton = (GameObject) Instantiate (Resources.Load ("Prefabs/SelectTemplateButton"));
		selectTemplateButton.GetComponent<SpriteRenderer>().enabled = false;
		selectTemplateButton.transform.position = new Vector3(0, -4, -3);

		MonsterList = new List<GameObject>();

	}

	void Update () {
		if (state == State.INITIAL) 
		{
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if (hit.collider != null)
				{
					if (hit.collider.gameObject == spawnDiceButtonTest) {

						currentlySelected.GetComponent<SpriteRenderer>().enabled = true;
						currentlySelected.transform.position = 
							new Vector3(hit.collider.gameObject.transform.position.x,
	                           hit.collider.gameObject.transform.position.y,
                               -1);

						if (DiceList.Count > 0) {
							rollDice();
						}
					}
				}
			}
		}
		else if (state == State.ROLLED) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if (hit.collider != null) {
					if (RolledDice.Contains (hit.collider.gameObject)) {
						currentlySelected.transform.position = 
							new Vector3(hit.collider.gameObject.transform.position.x,
                               hit.collider.gameObject.transform.position.y,
                               -1);

						selectedDice = hit.collider.gameObject;

						diceTemplate.GetComponent<DiceTemplate>().setRotation (0);
						diceTemplate.transform.localEulerAngles = new Vector3(0, 0, 0);
						diceTemplate.GetComponent<DiceTemplate>().Mirrored = false;

						toggleTemplateOverlay(true);

						state = State.DICESELECTED;
					}
				}
			}
		}
		else if (state == State.DICESELECTED) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);



				if (hit.collider != null) {
					if (hit.collider.gameObject == selectTemplateButton) {
						diceTemplate.GetComponent<DiceTemplate>().setTemplate(templateID);

						toggleTemplateOverlay(false);

						state = State.TEMPLATESELECTED;

					}
					else if (hit.collider.gameObject == rotateLeftButton) {
						diceTemplate.transform.Rotate (new Vector3(0, 0, 90));
						diceTemplate.GetComponent<DiceTemplate>().setRotation (diceTemplate.GetComponent<DiceTemplate>().Rotation - 1);
					}
					else if (hit.collider.gameObject == rotateRightButton) {
						diceTemplate.transform.Rotate (new Vector3(0, 0, -90));
						diceTemplate.GetComponent<DiceTemplate>().setRotation (diceTemplate.GetComponent<DiceTemplate>().Rotation + 1);
					}
					else if (hit.collider.gameObject == mirrorButton) {
						diceTemplate.GetComponent<DiceTemplate>().setRotation (0);
						diceTemplate.transform.Rotate (new Vector3(0, 180, 0));
						if (diceTemplate.GetComponent<DiceTemplate>().Mirrored) {
							diceTemplate.GetComponent<DiceTemplate>().Mirrored = false;
						}
						else {
							diceTemplate.GetComponent<DiceTemplate>().Mirrored = true;
						}
					}
					else if (hit.collider.gameObject == nextTemplateButton) {
						templateID++;

						if (templateID >= 11) {
							templateID = 0;
						}

						diceTemplate.GetComponent<SpriteRenderer>().sprite = diceTemplateSpriteList[templateID];

					}
					else if (hit.collider.gameObject == prevTemplateButton) {
						templateID --;

						if (templateID < 0) {
							templateID = 10;
						}

						diceTemplate.GetComponent<SpriteRenderer>().sprite = diceTemplateSpriteList[templateID];
					}
				}
			}
		}
		else if (state == State.TEMPLATESELECTED) {

			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				bool containsClicked = false;

				DiceTemplate template = diceTemplate.GetComponent<DiceTemplate>();

				int[] xTable = template.XTable;
				int[] yTable = template.YTable;
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

					if ( (0 <= (clickedX + arrayMin(xTable))) && (width > clickedX + arrayMax(xTable)) &&
					      (0 <= (clickedY + arrayMin(yTable))) && (height > clickedY + arrayMax(yTable)) ) {

						if (boardList[clickedX + xTable[0], clickedY + yTable[0]].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						}
						if (boardList[clickedX + xTable[1], clickedY + yTable[1]].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						}
						if (boardList[clickedX + xTable[2], clickedY + yTable[2]].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						}
						if (boardList[clickedX + xTable[3], clickedY + yTable[3]].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						}
						if (boardList[clickedX + xTable[4], clickedY + yTable[4]].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						}
						if (boardList[clickedX + xTable[5], clickedY + yTable[5]].GetComponent<Square>().SquareType != Color.WHITE) {
							clear = false;
						} 
					}
					else {
						clear = false;
					}

					if (clear) {
						boardList[clickedX + xTable[0], clickedY + yTable[0]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
						boardList[clickedX + xTable[0], clickedY + yTable[0]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];
						boardList[clickedX + xTable[1], clickedY + yTable[1]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
						boardList[clickedX + xTable[1], clickedY + yTable[1]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];
						boardList[clickedX + xTable[2], clickedY + yTable[2]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
						boardList[clickedX + xTable[2], clickedY + yTable[2]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];
						boardList[clickedX + xTable[3], clickedY + yTable[3]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
						boardList[clickedX + xTable[3], clickedY + yTable[3]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];
						boardList[clickedX + xTable[4], clickedY + yTable[4]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
						boardList[clickedX + xTable[4], clickedY + yTable[4]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];
						boardList[clickedX + xTable[5], clickedY + yTable[5]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().Color;
						boardList[clickedX + xTable[5], clickedY + yTable[5]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().Color];
						
						currentlySelected.GetComponent<SpriteRenderer>().enabled = false;

						GameObject newMonster = (GameObject) Instantiate (Resources.Load ("Prefabs/Monster"));
						newMonster.transform.position = new Vector3(clickedX + xOffset, clickedY + yOffset, -2);
						newMonster.GetComponent<Monster>().Position = newMonster.transform.position;
						MonsterList.Add(newMonster);

						state = State.TEMPLATEPLACED;
						
					}
				}
			}
		}
		else if (state == State.TEMPLATEPLACED) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if (hit.collider != null) {
					if (hit.collider.gameObject == spawnDiceButtonTest) {
						
						currentlySelected.GetComponent<SpriteRenderer>().enabled = true;
						currentlySelected.transform.position = 
							new Vector3(hit.collider.gameObject.transform.position.x,
                               hit.collider.gameObject.transform.position.y,
                               -1);
						
						if (DiceList.Count > 0) {
							rollDice();
						}
					}
					else if (MonsterList.Contains (hit.collider.gameObject)) {
						currentlySelected.transform.position = hit.collider.gameObject.transform.position;
						currentlySelected.GetComponent<SpriteRenderer>().enabled = true;
						selectedMonster = hit.collider.gameObject;

						state = State.MONSTERSELECTED;
					}
				}
			}
		}
		else if (state == State.MONSTERSELECTED) {
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

				if (hit.collider != null) {

					GameObject clickedGO = hit.collider.gameObject;

					if (clickedGO == spawnDiceButtonTest) {
						
						currentlySelected.GetComponent<SpriteRenderer>().enabled = true;
						currentlySelected.transform.position = 
							new Vector3(clickedGO.transform.position.x,
							            clickedGO.transform.position.y,
							            -1);
						
						if (DiceList.Count > 0) {
							rollDice();
						}
					}
					else {
						bool containsClicked = false;
						foreach (GameObject go in boardList) {
							if (go == clickedGO) {
								containsClicked = true;
							}
						}
						
						if (containsClicked) {
							selectedMonster.transform.position = selectedMonster.GetComponent<Monster>().move(clickedGO);

							currentlySelected.transform.position = selectedMonster.transform.position;
						}
					}
				}
			}
		}
	}

	void toggleTemplateOverlay(bool toggle) {
		diceTemplate.GetComponent<SpriteRenderer>().enabled = toggle;
		diceTemplate.GetComponent<BoxCollider2D>().enabled = toggle;
		overlay.GetComponent<SpriteRenderer>().enabled = toggle;
		nextTemplateButton.GetComponent<SpriteRenderer>().enabled = toggle;
		nextTemplateButton.GetComponent<BoxCollider2D>().enabled = toggle;
		prevTemplateButton.GetComponent<SpriteRenderer>().enabled = toggle;
		prevTemplateButton.GetComponent<BoxCollider2D>().enabled = toggle;
		rotateLeftButton.GetComponent<SpriteRenderer>().enabled = toggle;
		rotateLeftButton.GetComponent<BoxCollider2D>().enabled = toggle;
		rotateRightButton.GetComponent<SpriteRenderer>().enabled = toggle;
		rotateRightButton.GetComponent<BoxCollider2D>().enabled = toggle;
		selectTemplateButton.GetComponent<SpriteRenderer>().enabled = toggle;
		selectTemplateButton.GetComponent<BoxCollider2D>().enabled = toggle;
		mirrorButton.GetComponent<SpriteRenderer>().enabled = toggle;
		mirrorButton.GetComponent<BoxCollider2D>().enabled = toggle;
	}

	void rollDice() {
		int index1 = Random.Range (0, DiceList.Count);
		
		if (newDice1 != null) {
			newDice1.GetComponent<SpriteRenderer>().enabled = false;
			
		}
		
		newDice1 = DiceList[index1];
		DiceList.RemoveAt (index1);
		Dice dice1 = newDice1.GetComponent<Dice>();
		
		newDice1.transform.position = new Vector2(15 + xOffset, 6 + yOffset);
		newDice1.transform.parent = this.transform;
		newDice1.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[dice1.Color][dice1.Faces[Random.Range(0, 6)]];
		newDice1.GetComponent<BoxCollider2D>().enabled = true;
		
		int index2 = Random.Range (0, DiceList.Count);
		
		if (newDice2 != null) {
			newDice2.GetComponent<SpriteRenderer>().enabled = false;
		}
		
		newDice2 = DiceList[index2];
		DiceList.RemoveAt (index2);
		Dice dice2 = newDice2.GetComponent<Dice>();
		
		newDice2.transform.position = new Vector2(15 + xOffset, 5 + yOffset);
		newDice2.transform.parent = this.transform;
		newDice2.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[dice2.Color][dice2.Faces[Random.Range(0, 6)]];
		newDice2.GetComponent<BoxCollider2D>().enabled = true;
		
		int index3 = Random.Range (0, DiceList.Count);
		
		if (newDice3 != null) {
			newDice3.GetComponent<SpriteRenderer>().enabled = false;
		}
		
		newDice3 = DiceList[index3];
		DiceList.RemoveAt (index3);
		Dice dice3 = newDice3.GetComponent<Dice>();
		
		newDice3.transform.position = new Vector2(15 + xOffset, 4 + yOffset);
		newDice3.transform.parent = this.transform;
		newDice3.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[dice3.Color][dice3.Faces[Random.Range(0, 6)]];
		newDice3.GetComponent<BoxCollider2D>().enabled = true;
		
		RolledDice = new List<GameObject>();
		RolledDice.Add(newDice1);
		RolledDice.Add(newDice2);
		RolledDice.Add(newDice3);
		
		state = State.ROLLED;
	}

	int arrayMax(int[] array) {
		int toReturn = int.MinValue;

		foreach (int curr in array) {
			if (curr > toReturn) {
				toReturn = curr;
			}
		}
		return toReturn;
	}

	int arrayMin(int[] array) {
		int toReturn = int.MaxValue;

		foreach (int curr in array) {
			if (curr < toReturn) {
				toReturn = curr;
			}
		}
		return toReturn;
	}



	
}
