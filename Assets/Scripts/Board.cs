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

	private class Owner {
		public static int PLAYER = 0,
		AI = 1;
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
	List<GameObject> Graveyard;
	List<GameObject> PlayerMonsterList;
	List<GameObject> AIMonsterList;

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
	GameObject selectedMonsterGO;

	void Awake () {
		squareSpriteList = Resources.LoadAll<Sprite>("Sprites/square");
		diceFaceSpriteList[Color.WHITE] = Resources.LoadAll<Sprite>("Sprites/diceFaces");
		diceFaceSpriteList[Color.GREEN] = Resources.LoadAll<Sprite>("Sprites/diceFacesGreen");
		diceFaceSpriteList[Color.BLUE] = Resources.LoadAll<Sprite>("Sprites/diceFacesBlue");
		diceTemplateSpriteList = Resources.LoadAll<Sprite>("Sprites/DiceTemplates/masterTemplate");
	}

	void Start () {
		Screen.orientation = ScreenOrientation.LandscapeLeft;

		GameObject squarePrefab = (GameObject) Resources.Load ("Prefabs/Square");

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				GameObject newSquare = (GameObject) Instantiate (squarePrefab);
				newSquare.GetComponent<Square>().XPos = x;
				newSquare.GetComponent<Square>().YPos = y;
				newSquare.GetComponent<SpriteRenderer>().sprite = squareSpriteList[Color.WHITE];
				newSquare.transform.position = new Vector2(x + xOffset, y + yOffset);
				newSquare.transform.parent = this.transform;

				boardList[x,y] = newSquare;
			}
		}

		DiceList = parseDiceList("../Dice/Assets/Resources/testfile1");

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
		Graveyard = new List<GameObject>();
		PlayerMonsterList = new List<GameObject>();
		AIMonsterList = new List<GameObject>();

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

					if (checkTemplatePlacement(xTable, yTable, clickedX, clickedY)) {

						placeTemplate(xTable, yTable, clickedX, clickedY);

						currentlySelected.GetComponent<SpriteRenderer>().enabled = false;

						GameObject newMonsterGO = (GameObject) Instantiate (Resources.Load ("Prefabs/Monster"));
						newMonsterGO.transform.position = new Vector3(clickedX + xOffset, clickedY + yOffset, -2);

						//TODO change this part so it handles more monsters based on dice type/value, and ownership isn't
						//based on which color dice is rolled.
						if (selectedDice.GetComponent<Dice>().RolledFace.Color == Color.GREEN) {
							newMonsterGO.AddComponent<Bulbasaur>();
							PlayerMonsterList.Add (newMonsterGO);
						}
						else {
							newMonsterGO.AddComponent<Squirtle>();
							AIMonsterList.Add (newMonsterGO);
						}

						Monster newMonster = newMonsterGO.GetComponent<Monster>();
						newMonster.init ();
						newMonster.Position = newMonster.transform.position;
						newMonsterGO.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite> (newMonster.SpritePath);

						MonsterList.Add(newMonsterGO);

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
						selectedMonsterGO = hit.collider.gameObject;

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
					else if (clickedGO.GetComponent<Monster>() != null) {
						Monster clickedMonster = clickedGO.GetComponent<Monster>();
						Monster selectedMonster = selectedMonsterGO.GetComponent<Monster>();

						if (clickedMonster.Owner == selectedMonster.Owner) {
							selectedMonsterGO = clickedGO;
							currentlySelected.transform.position = clickedGO.transform.position;
						}
						else {
							//TODO implement fight stuff
							selectedMonster.attack(clickedMonster);
							resolveFight(selectedMonsterGO, clickedGO);
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
							selectedMonsterGO.transform.position = selectedMonsterGO.GetComponent<Monster>().move(clickedGO);

							currentlySelected.transform.position = selectedMonsterGO.transform.position;
						}
					}
				}
			}
		}
	}

	/*  
	 * Helper function that shows or hides all the template editing tools depending on the
	 * input bool value. 
	 * 
	 * Parameters:
	 * 		bool toggle: Shows template editing tools if true, hides them if false.
	 */
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

	/*
	 * Rolls 
	 */
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
		int face1 = Random.Range (0, 6);
		dice1.RolledFace = dice1.DieFaces[face1];
		DieFace rolledFace1 = dice1.RolledFace;
		newDice1.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[rolledFace1.Color][rolledFace1.Type];
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
		int face2 = Random.Range (0, 6);
		dice2.RolledFace = dice2.DieFaces[face2];
		DieFace rolledFace2 = dice2.RolledFace;
		newDice2.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[rolledFace2.Color][rolledFace2.Type];
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
		int face3 = Random.Range (0, 6);
		dice3.RolledFace = dice3.DieFaces[face3];
		DieFace rolledFace3 = dice3.RolledFace;
		newDice3.GetComponent<SpriteRenderer>().sprite = diceFaceSpriteList[rolledFace3.Color][rolledFace3.Type];
		newDice3.GetComponent<BoxCollider2D>().enabled = true;

		RolledDice = new List<GameObject>();
		RolledDice.Add(newDice1);
		RolledDice.Add(newDice2);
		RolledDice.Add(newDice3);
		
		state = State.ROLLED;
	}

	bool checkTemplatePlacement(int[] xTable, int[] yTable, int clickedX, int clickedY) {
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

		return clear;
	}

	void placeTemplate(int[] xTable, int[] yTable, int clickedX, int clickedY) {
		boardList[clickedX + xTable[0], clickedY + yTable[0]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().RolledFace.Color;
		boardList[clickedX + xTable[0], clickedY + yTable[0]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().RolledFace.Color];
		boardList[clickedX + xTable[1], clickedY + yTable[1]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().RolledFace.Color;
		boardList[clickedX + xTable[1], clickedY + yTable[1]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().RolledFace.Color];
		boardList[clickedX + xTable[2], clickedY + yTable[2]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().RolledFace.Color;
		boardList[clickedX + xTable[2], clickedY + yTable[2]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().RolledFace.Color];
		boardList[clickedX + xTable[3], clickedY + yTable[3]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().RolledFace.Color;
		boardList[clickedX + xTable[3], clickedY + yTable[3]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().RolledFace.Color];
		boardList[clickedX + xTable[4], clickedY + yTable[4]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().RolledFace.Color;
		boardList[clickedX + xTable[4], clickedY + yTable[4]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().RolledFace.Color];
		boardList[clickedX + xTable[5], clickedY + yTable[5]].GetComponent<Square>().SquareType = selectedDice.GetComponent<Dice>().RolledFace.Color;
		boardList[clickedX + xTable[5], clickedY + yTable[5]].GetComponent<SpriteRenderer>().sprite = squareSpriteList[selectedDice.GetComponent<Dice>().RolledFace.Color];
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

	void resolveFight(GameObject att, GameObject def) {
		Monster attacker = att.GetComponent<Monster> ();
		Monster defender = def.GetComponent<Monster> ();

		if (defender.HP <= 0) {
			destroyMonster(def);
		}
	}

	void destroyMonster(GameObject monsterGO) {
		Graveyard.Add(monsterGO);
		MonsterList.Remove(monsterGO);
		monsterGO.SetActive(false);
	}

	List<GameObject> parseDiceList(string listFile) {
		//create new parser/readers
		List<GameObject> toReturn = new List<GameObject>();

		string[] fileLines = System.IO.File.ReadAllLines(@listFile);

		//get next line of die faces
		for(int i = 0; i < fileLines.Length; i++) {
			string currLine = fileLines[i];

			if (currLine[0] != '#') {
				string[] splitLineString = currLine.Split(',');
				int[] splitLineInt = new int[12]; 

				GameObject newDice = (GameObject) Instantiate (Resources.Load ("Prefabs/Dice"));
				newDice.GetComponent<BoxCollider2D>().enabled = false;
				Dice dice = newDice.GetComponent<Dice>();
				DieFace[] faces = new DieFace[6];

				if (splitLineString.Length != 12) {
					Debug.Log ("(Line " + i + "): Dice List Parser could not parse invalid line: \n" + currLine);
				}
				else {
					for (int j = 0; j < splitLineString.Length; j++) {
						try {
							splitLineInt[j] = int.Parse (splitLineString[j]);
						}
						catch (System.FormatException e)
						{
							Debug.Log ("(Line " + i + "): Dice List Parser could not parse element \"" + splitLineString[j] + 
							           "\"\nException thrown: " + e );
						}
					}
					for (int j = 0; j < faces.Length; j++) {
						faces[j] = new DieFace();
						faces[j].Type = splitLineInt[j];
						faces[j].Color = splitLineInt[j + 6];
					}

					dice.DieFaces = faces;

					toReturn.Add(newDice);
				}
			}
		}
		return toReturn;
	}
}
