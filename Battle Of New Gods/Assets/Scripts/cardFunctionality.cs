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
	public GameObject aBonusDamage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void activateBossCard(int cardNumber, GameObject cardGO){
		waitingOnCard = true;
		switch(cardNumber){
		case 0:
			//boss card
			waitingOnCard = false;
			break;
		case 1:
			//clid constant
			waitingOnCard = false;
			break;
		case 2:
			//chill constant
			waitingOnCard = false;
			break;
		case 3:
			//eighth constant
			waitingOnCard = false;
			break;
		case 4:
			//alah drogbar
			//track all player resources spent
			//deal damage based on how many spent
			waitingOnCard = false;
			break;
		case 5:
			//random constant activates twice
			waitingOnCard = false;
			break;
		case 6:
			//all palyers take 2x damage for 1 turn
			waitingOnCard = false;
			break;
		case 7:
			//random player cant cast spells during their next turn
			waitingOnCard =false;
			break;
		case 8:
			//random players graveyard removed from game
			//deal damage based on number of cards removed
			waitingOnCard = false; 
			break;
		case 9:
			//boss takes 2 turns next turn
			waitingOnCard = false;
			break;
		case 10:
			//play another card
			waitingOnCard = false;
			break;
		case 11:
			//disarm all players for x turns
			waitingOnCard = false;
			break;
		case 12:
			//sweets o clock on random constant
			waitingOnCard = false;
			break;
		case 13:
			//link 2 players, damage dealt to one is also dealt to the other
			waitingOnCard = false;
			break;
		case 14:
			//clid, chill, eighth gain +3 attack
			waitingOnCard = false;
			break;
		case 15:
			//deal damage to all palyers, enrage eighth
			waitingOnCard = false;
			break;
		case 16:
			//players next turn lasts 10s
			waitingOnCard = false;
			break;
		case 17:
			//enrage chill
			waitingOnCard = false;
			break;
		case 18:
			//deal damage to all palyers, enrage clid
			waitingOnCard = false;
			break;
		case 19:
			//players discard a random card and take damage equal to its cost
			waitingOnCard = false;
			break;
		default:
			waitingOnCard = false;
			break;
		}
	}

	public void activateCard(int cardNumber, GameObject cardGO){
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
			StartCoroutine(targetedDamage(5 + axeWeaponBonusDamage + axeBonusDamage));
			break;
		case 3:
			aoeDamage((3+ axeWeaponBonusDamage + axeBonusDamage),true,false,false);
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
			GameObject abd = (GameObject)Instantiate(aBonusDamage,Vector3.zero,Quaternion.identity);
			abd.GetComponent<axeBonusDamage>().setBonus(3,3);
			abd.GetComponent<axeBonusDamage>().parent = cardGO;
			waitingOnCard = false;
			break;
		case 9:
			StartCoroutine(targetedDamage(3 + axeBonusDamage));
			//random other target aswell
			break;
		case 10:
			cardGO.GetComponent<bloodRage>().turnPass();
			cardGO.tag = "bloodRage";
			waitingOnCard = false;
			break;
		case 11:
			StartCoroutine(targetedDamage(4 + axeBonusDamage,"BossMinion"));
			if(axeWeap){
				tempBossHP -= (4+axeBonusDamage);
				Text tarHP = findTarHP(GameObject.FindGameObjectWithTag("BossHeroDrop"));
				if(tarHP)tarHP.text = ("Health: "+tempBossHP.ToString());
			}
			break;
		case 12:
			//cards 1 less w.o weap, cards 1 more  + increase axebonusdamage w/ weap.
			waitingOnCard = false;
			break;
		case 13:
			int bDmg = aoeDamage((4+axeBonusDamage),true,false,true);
			GameObject abDmg = (GameObject)Instantiate(aBonusDamage,Vector3.zero,Quaternion.identity);
			abDmg.GetComponent<axeBonusDamage>().setBonus(bDmg,1);
			abDmg.GetComponent<axeBonusDamage>().parent = cardGO;
			waitingOnCard = false;
			break;
		case 14:
			//random target
			waitingOnCard = false;
			break;
		case 15:
			int cardsToDraw = 1;
			deck Deckk = this.gameObject.GetComponent<deck>();
			if(Deckk.getHP() < 18){
				cardsToDraw++;
				if(Deckk.getHP() < 9) cardsToDraw++;
			}
			for(int i = 0; i < cardsToDraw; i++){
				Deckk.drawFunction();
			}
			waitingOnCard = false;
			break;
		case 16:
			StartCoroutine(targetedDamage((cardGO.GetComponent<dragg>().xCost * 4) + axeBonusDamage));
			this.gameObject.GetComponent<deck>().skipTurn = true;
			break;
		default:
			waitingOnCard = false;
			break;
		}
	}

	IEnumerator targetedDamage(int dmg){
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
	IEnumerator targetedDamage(int dmg, string type){
		GameObject tarCard;
		SelectWarning.gameObject.SetActive(true);
		//print("targeting");
		while(true){
			if(Input.GetMouseButtonDown(0)){
				tarCard = findTarget(type);
				if(tarCard){
					if(type == "BossMinion"){
						if(tarCard)tarCard.GetComponent<bossMinion>().dealDamage(dmg);
					}
					
					waitingOnCard = false;
					yield break;
				}
			}
			yield return null;
		}


	}
	IEnumerator targetedDamage(int dmg, int effectID){
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
		GameObject tarCard;

		PointerEventData point = new PointerEventData(EventSystem.current);
		point.position = Input.mousePosition;

		List<RaycastResult> raycastResults = new List<RaycastResult>();
		EventSystem.current.RaycastAll(point,raycastResults);

		if(raycastResults.Count > 0){
			for(int i = 0; i < raycastResults.Count; i++){
				//if(raycastResults[i].gameObject.tag == "Minion" || raycastResults[i].gameObject.tag == "Hero"){
				//Will use the above condition check once we have minions and a boss card to target. Below is for testing purposes only.
				if(raycastResults[i].gameObject.tag == typeTag){
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

	int aoeDamage(int dmg, bool hitEnemies, bool hitAllies, bool reNumTargets){
		List<GameObject> targets;
		targets = new List<GameObject>();
		if(hitEnemies)targets.AddRange(GameObject.FindGameObjectsWithTag("BossMinion"));
		if(hitEnemies){
			tempBossHP -= dmg;
			Text tarHP = findTarHP(GameObject.FindGameObjectWithTag("BossHeroDrop"));
			if(tarHP)tarHP.text = ("Health: "+tempBossHP.ToString());
		}
		if(targets.Count > 0){
			for(int i = 0; i < targets.Count; i++){
				if(targets[i].tag == "BossMinion")targets[i].GetComponent<bossMinion>().dealDamage(dmg);
			}
		}
		if(reNumTargets)return (targets.Count+1);
		else waitingOnCard = false;
		return 0;
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
	public void addAxeBonus(int bdmg){
		axeBonusDamage += bdmg;
	}
}
