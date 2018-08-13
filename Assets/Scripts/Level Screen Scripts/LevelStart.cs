using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStart : MonoBehaviour {

	Text levelNumText;
	Text levelHealthText;
	Text levelMoneyText;	
	Text levelItemNamesText;
	Text levelItemDrbText;
	Button levelStartBtn;
	Animator animator;

	void Awake() {
		levelNumText = transform.Find("Level Number").GetComponent<Text>();
		levelHealthText = transform.Find("Level Health").GetComponent<Text>();
		levelMoneyText = transform.Find("Level Money").GetComponent<Text>();
		levelItemNamesText = transform.Find("Level Item Names").GetComponent<Text>();
		levelItemDrbText = transform.Find("Level Item Drb").GetComponent<Text>();
		levelStartBtn = transform.Find("Level Start Button").GetComponent<Button>();
		animator = transform.parent.gameObject.GetComponent<Animator>();
	}

	public void ScreenIn() {
		levelNumText.text = "DAY " + GameManager.instance.GetLevel();
		levelHealthText.text = GameManager.instance.GetHealth().ToString();
		levelMoneyText.text = GameManager.instance.GetMoney().ToString();

		levelItemNamesText.text = "";
		levelItemDrbText.text = "";
		foreach(GameObject item in ItemManager.instance.items) {
			levelItemNamesText.text += item.GetComponent<ItemController>().GetItemName() + "\n";
			levelItemDrbText.text += item.GetComponent<ItemController>().GetItemDurability() + "\n";
		}

		levelStartBtn.onClick.RemoveAllListeners();
		levelStartBtn.onClick.AddListener(() => {
            Time.timeScale = 1;
            PlayerController.instance.UnmuteEngine();
			GameManager.instance.HideLevelStart();
			levelStartBtn.onClick.RemoveAllListeners();
		});
		if (GameManager.instance.GetLevel() > 1) animator.SetBool("isOpen", true);
	}

	public void ScreenOut() {
		animator.SetBool("isOpen", false);
	}
}
