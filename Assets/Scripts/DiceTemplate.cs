using UnityEngine;
using System.Collections;

public class DiceTemplate : MonoBehaviour {

	private int[] xBaseTable;
	private int[] xTable;
	public int[] XTable {
		get { return xTable; }
	}

	private int[] yBaseTable;
	private int[] yTable;
	public int[] YTable {
		get { return yTable; }
	}

	private int templateID;
	public int TemplateID {
		get { return templateID; }
	}

	private int rotation;
	public int Rotation {
		get { return rotation; }
	}

	// Use this for initialization
	void Start () {
		setTemplate(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setRotation(int rotato) {
		this.rotation = rotato;

		while (this.rotation < 0) {
			this.rotation += 4;
		}

		if (this.rotation > 3) {
			this.rotation = this.rotation % 4;
		}

		int[] xTemp;
		int[] yTemp;

		switch (this.rotation) {
		
		case 0:
			xTable = xBaseTable;
			yTable = yBaseTable;
			break;

		case 1:
			xTable = yBaseTable;
			yTable = xBaseTable;
			break;

		case 2:
			xTemp = new int[6];
			yTemp = new int[6];

			for (int x = 0; x < xBaseTable.Length; x++) {
				xTemp[x] = (0 - xBaseTable[x]);
			}

			for (int y = 0; y < yBaseTable.Length; y++) {
				yTemp[y] = (0 - yBaseTable[y]);
			}

			xTable = xTemp;
			yTable = yTemp;
			break;

		case 3:
			xTemp = new int[6];
			yTemp = new int[6];
			
			for (int x = 0; x < xBaseTable.Length; x++) {
				xTemp[x] = (0 - xBaseTable[x]);
			}
			
			for (int y = 0; y < yBaseTable.Length; y++) {
				yTemp[y] = (0 - yBaseTable[y]);
			}
			
			xTable = yTemp;
			yTable = xTemp;
			break;
		}
	}

	public void setTemplate(int templateID) {

		this.templateID = templateID;

		switch (templateID) {
		case 0:
			xBaseTable = new int[6] {0, -1, 0, 1, 0, 0};
			yBaseTable = new int[6] {1, 0, 0, 0, -1, -2};

			setRotation(rotation);
			break;

		case 1:

			break;

		case 2:

			break;

		case 3:

			break;
			
		case 4:
			
			break; 
		
		case 5:
			
			break; 
		
		case 6:
			
			break; 
		
		case 7:
			
			break; 
		
		case 8:
			
			break; 
			
		case 9:
			
			break; 
			
		case 10:
			
			break; 
			
		case 11:
			
			break; 
		}
	}
}
