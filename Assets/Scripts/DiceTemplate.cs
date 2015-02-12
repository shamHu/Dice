using UnityEngine;
using System.Collections;

public class DiceTemplate : MonoBehaviour {

	private int[] xTable;
	public int[] XTable {
		get { return xTable; }
	}

	private int[] yTable;
	public int[] YTable {
		get { return yTable; }
	}

	private int templateID;
	public int TemplateID {
		get { return templateID; }
	}

	// Use this for initialization
	void Start () {
		setTemplate(0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setTemplate(int templateID) {

		this.templateID = templateID;

		switch (templateID) {
		case 0:
			xTable = new int[6] {0, -1, 0, 1, 0, 0};
			yTable = new int[6] {1, 0, 0, 0, -1, -2};
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
