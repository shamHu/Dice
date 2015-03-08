using UnityEngine;
using System.Collections;

public class DiceTemplate : MonoBehaviour {

	private int[] xBaseTable;
	private int[] xTable;
	public int[] XTable {
		get { 
			if (mirrored) {
				int[] mirroredXTable = new int[6];
				for (int x = 0; x < xTable.Length; x++) {
					mirroredXTable[x] = (0 - xTable[x]);
				}
				return mirroredXTable;
			}
			else {
				return xTable;
			}
		}
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

	private bool mirrored;
	public bool Mirrored {
		get { return mirrored; }
		set { mirrored = value; }
	}

	// Use this for initialization
	void Start () {
		setTemplate(0);
		mirrored = false;
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
			yTemp = new int[6];
			for (int x = 0; x < xBaseTable.Length; x++) {
				yTemp[x] = (0 - xBaseTable[x]);
			}

			xTable = yBaseTable;
			yTable = yTemp;
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
			yTemp = new int[6];
			
			for (int x = 0; x < yBaseTable.Length; x++) {
				yTemp[x] = (0 - yBaseTable[x]);
			}

			xTable = yTemp;
			yTable = xBaseTable;
			break;
		}
	}

	public void setTemplate(int templateID) {

		this.templateID = templateID;

		switch (templateID) {
		case 0:
			xBaseTable = new int[6] {1, 1, 1, 0, 0, 0};
			yBaseTable = new int[6] {2, 1, 0, 0, -1, -2};

			setRotation(rotation);
			break;

		case 1:
			xBaseTable = new int[6] {-1, -1, 0, 1, 1, 2};
			yBaseTable = new int[6] {-1, 0, 0, 0, 1, 1};
			
			setRotation(rotation);
			break;

		case 2:
			xBaseTable = new int[6] {-1, 0, 0, 0, 0, 1};
			yBaseTable = new int[6] {1, 1, 0, -1, -2, 1};
			
			setRotation(rotation);
			break;

		case 3:
			xBaseTable = new int[6] {-1, 0, 0, 1, 1, 2};
			yBaseTable = new int[6] {-1, -1, 0, 0, 1, 1};
			
			setRotation(rotation);
			break;
			
		case 4:
			xBaseTable = new int[6] {-2, -1, -1, 0, 0, 1};
			yBaseTable = new int[6] {-1, -1, 0, 0, 1, 0};
			
			setRotation(rotation);
			break; 
		
		case 5:
			xBaseTable = new int[6] {-1, 0, 0, 0, 0, 1};
			yBaseTable = new int[6] {0, -1, 0, 1, 2, 0};
			
			setRotation(rotation);
			break; 
		
		case 6:
			xBaseTable = new int[6] {-1, 0, 0, 0, 0, 1};
			yBaseTable = new int[6] {0, -2, -1, 0, 1, 1};

			setRotation(rotation);
			break; 
		
		case 7:
			xBaseTable = new int[6] {-1, 0, 0, 0, 0, 1};
			yBaseTable = new int[6] {0, -1, 0, 1, 2, 1};

			setRotation(rotation);
			break; 
		
		case 8:
			xBaseTable = new int[6] {-1, -1, 0, 0, 0, 1};
			yBaseTable = new int[6] {1, 0, 0, -1, -2, 0};

			setRotation(rotation);
			break; 
			
		case 9:
			xBaseTable = new int[6] {-1, 0, 0, 0, 0, 1};
			yBaseTable = new int[6] {0, -1, 0, 1, 2, 2};

			setRotation(rotation);
			break; 
			
		case 10:
			xBaseTable = new int[6] {-1, 0, 0, 0, 0, 1};
			yBaseTable = new int[6] {1, 1, 0, -1, -2, -2};

			setRotation(rotation);
			break; 
			
		default:
			xBaseTable = new int[6] {0, 0, 0, 0, 0, 0};
			yBaseTable = new int[6] {0, 0, 0, 0, 0, 0};

			setRotation(rotation);
			break; 
		}
	}
}
