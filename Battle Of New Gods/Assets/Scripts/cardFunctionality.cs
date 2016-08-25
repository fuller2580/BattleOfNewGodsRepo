using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class cardFunctionality : MonoBehaviour {

	int tempBossHP = 50;
	public Text bossHP;
	public Text SelectWarning;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void activateCard(int cardNumber){
		//print("activating");
		print(cardNumber);
		switch(cardNumber){
		case 0:
			StartCoroutine(targetedDamage(1));
			break;
		case 1:
			StartCoroutine(targetedDamage(3));
			break;
		case 2:
			StartCoroutine(targetedDamage(5));
			break;
		case 3:
			StartCoroutine(targetedDamage(4));
			break;
		default:
			break;
		}
	}

	IEnumerator targetedDamage(int dmg){
		bool waitingForTarget = true;
		GameObject tarCard;
		SelectWarning.gameObject.SetActive(true);
		//print("targeting");
		while(true){
			if(Input.GetMouseButtonDown(0)){
				PointerEventData point = new PointerEventData(EventSystem.current);
				point.position = Input.mousePosition;

				List<RaycastResult> raycastResults = new List<RaycastResult>();
				EventSystem.current.RaycastAll(point,raycastResults);

				if(raycastResults.Count > 0){
					for(int i = 0; i < raycastResults.Count; i++){
						//if(raycastResults[i].gameObject.tag == "Minion" || raycastResults[i].gameObject.tag == "Hero"){
						//Will use the above condition check once we have minions and a boss card to target. Below is for testing purposes only.
						if(raycastResults[i].gameObject.tag == "BossHeroDrop"){
							tarCard = raycastResults[i].gameObject;
							//print("target aquired");
							SelectWarning.gameObject.SetActive(false);
							//print("target is "+tarCard.name);


							//will need to add a stats script to handle health and what not later on, for now just changes visual number.
							tempBossHP -= dmg;
							bossHP.text = ("Health: "+tempBossHP.ToString());
							break;
						}
					}
				}
			}
			yield return null;
		}


	}
	//IEnumerator targetedDamage(int dmg, int extraEffect, int efDmg){
	//
	//}
}
