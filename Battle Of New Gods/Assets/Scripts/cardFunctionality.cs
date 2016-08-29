using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class cardFunctionality : MonoBehaviour {

	int tempBossHP = 50;
	public Text bossHP;
	public Text SelectWarning;
	bool waitingOnCard = false;
	int axeBonusDamage = 0;
	int axeWeaponBonusDamage = 0;
	bool axeDouble = false;
	public bool axeWeap = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void activateCard(int cardNumber){
		waitingOnCard = true;
		int dub = Random.Range(0,5);
		int dmg;
		switch(cardNumber){
		case 0:
			dmg = 1 + axeWeaponBonusDamage + axeBonusDamage;
			if(axeDouble){
				if(dub == 4) dmg = dmg*2;
			}
			StartCoroutine(targetedDamage(dmg));
			break;
		case 1:
			dmg = 3 + axeWeaponBonusDamage + axeBonusDamage;
			if(axeDouble){
				if(dub == 4) dmg = dmg*2;
			}
			StartCoroutine(targetedDamage(dmg));
			break;
		case 2:
			StartCoroutine(targetedDamage(3 + axeWeaponBonusDamage + axeBonusDamage));
			break;
		case 3:
			StartCoroutine(targetedDamage(3 + axeWeaponBonusDamage + axeBonusDamage));
			break;
		case 4:
			//crit chance
			waitingOnCard = false;
			break;
		case 5:
			axeDouble = true;
			waitingOnCard = false;
			break;
		case 6:
			//killing blow gives card
			waitingOnCard = false;
			break;
		case 7:
			StartCoroutine(targetedDamage(3 + axeBonusDamage));
			break;
		case 8:
			//increase axebonusdamage
			waitingOnCard = false;
			break;
		case 9:
			StartCoroutine(targetedDamage(3 + axeBonusDamage));
			//random other target aswell
			break;
		case 10:
			//increase axebonusdamage based on missing health
			waitingOnCard = false;
			break;
		default:
			waitingOnCard = false;
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
				tarCard = findTarget();
				if(tarCard){
					tempBossHP -= dmg;
					Text tarHP = findTarHP(tarCard);
					if(tarHP)tarHP.text = ("Health: "+tempBossHP.ToString());
					waitingOnCard = false;
					yield break;
				}
			}
			yield return null;
		}


	}
	IEnumerator targetedDamage(int dmg, int effectID){
		bool waitingForTarget = true;
		GameObject tarCard;
		SelectWarning.gameObject.SetActive(true);
		//print("targeting");
		switch(effectID){
		case 0:
			
			break;
			
		}
		while(true){
			if(Input.GetMouseButtonDown(0)){
				tarCard = findTarget();
				if(tarCard){
					tempBossHP -= dmg;
					Text tarHP = findTarHP(tarCard);
					if(tarHP)tarHP.text = ("Health: "+tempBossHP.ToString());
					waitingOnCard = false;
				}
			}
			yield return null;
		}
	}

	GameObject findTarget(){
		GameObject tarCard;

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

					return tarCard;
					//will need to add a stats script to handle health and what not later on, for now just changes visual number.
				}
			}
		}
		return null;
	}
	//will eventually use this for specific targeting ex. only targets minions, only targets boss, ect.
	GameObject findTarget(string typeTag){
		return null;
	}

	Text findTarHP(GameObject tarGO){
		Text[] hps = tarGO.GetComponentsInChildren<Text>();
		foreach(Text hp in hps){
			if(hp.gameObject.name == "hpText"){
				return hp;
			}
		}
		return null;
	}

	public bool freeToMove(){
		return !waitingOnCard;
	}
	public void weapEquip(){
		axeWeap = true;
	}
}
