using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionIndicatorController : MonoBehaviour {

	GameObject player;
	GameObject dropzone;

	void Start () {
		player = transform.parent.gameObject;
		ItemManager.instance.onPriorityItemChange += UpdateCurrentItem;
		UpdateCurrentItem(null);
	}
	
	void Update () {
		transform.position = player.transform.position;// + new Vector3(0, 1.2f, 0);
        Vector3 dir = dropzone.transform.position - player.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
	
	void UpdateCurrentItem(GameObject priorityItem) {
		if (priorityItem == null) {
			dropzone = null;
		} else {
			dropzone = priorityItem.transform.GetChild(0).gameObject;
		}
	}
}
