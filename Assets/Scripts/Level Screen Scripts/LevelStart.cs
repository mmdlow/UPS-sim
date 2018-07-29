using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStart : MonoBehaviour {

	Text levelNumText;
	Text levelHealthText;
	Text levelMoneyText;
	Button levelStartBtn;
	
	Text levelItemNamesText;
	Text levelItemDrbText;

	void Awake() {
		levelNumText = transform.Find("Level Number").GetComponent<Text>();
		levelHealthText = transform.Find("Level Health").GetComponent<Text>();
		levelMoneyText = transform.Find("Level Money").GetComponent<Text>();
		levelItemNamesText = transform.Find("Level Item Names").GetComponent<Text>();
		levelItemDrbText = transform.Find("Level Item Drb").GetComponent<Text>();
		levelStartBtn = transform.Find("Level Start Button").GetComponent<Button>();
	}

	public void InitScreen() {
		levelNumText.text = "DAY " + GameManager.instance.GetLevel();
		levelHealthText.text = GameManager.instance.GetHealth().ToString();
		levelMoneyText.text = GameManager.instance.GetMoney().ToString();

		foreach(GameObject item in ItemManager.instance.items) {
			levelItemNamesText.text += item.GetComponent<ItemController>().GetItemName() + "\n";
			levelItemDrbText.text += item.GetComponent<ItemController>().GetItemDurability() + "\n";
		}

		levelStartBtn.onClick.AddListener(() => {
			GameManager.instance.HideLevelStart();
			levelStartBtn.onClick.RemoveAllListeners();
		});
	}
}
