using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIManager : MonoBehaviour {

	private static UIManager _instance;

	public static UIManager Instance
	{
		get
		{
			if(_instance == null)
				_instance = GameObject.FindObjectOfType<UIManager>();
			return _instance;
		}
	}

	[SerializeField]
	private Canvas monsterUICanvas;
	public Canvas MonsterUICanvas
	{
		get { return monsterUICanvas; }
	}

	[SerializeField]
	private GameObject monsterUIPrefab;
	public GameObject MonsterUIPrefab
	{
		get { return monsterUIPrefab; }
	}

	[SerializeField]
	private Text selectedHP;

	[SerializeField]
	private Text selectedATT;

	[SerializeField]
	private Text selectedDEF;

	[SerializeField]
	private Text selectedDescription;

	[SerializeField]
	private Image monsterImage;

	public void UpdateSelectedMonsterUI(Monster monster, string description)
	{
		selectedHP.text = monster.HP.ToString();
		selectedATT.text = monster.ATT.ToString();
		selectedDEF.text = monster.DEF.ToString();
		selectedDescription.text = description;
		monsterImage.sprite = monster.GetComponent<SpriteRenderer>().sprite;
	}
}
