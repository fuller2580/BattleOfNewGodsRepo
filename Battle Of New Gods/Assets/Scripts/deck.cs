﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class deck : MonoBehaviour
{
	List<int> cards;
	List<int> hand;
	bool firstDraw = true;
	int totalCardsPlayed = 0;
	int resources = 0;
	public int curResources = 0;
	int maxResources = 5;
	public Text text;
	public Text HP;
	int currentCardID = 0;
	public Transform Hand;
	premadeDecks Decks;
	int maxHP;
	int curHP;
	public void shuffle()
	{
		if (hand == null) {
			hand = new List<int> ();
		} 
		if (cards == null) {
			cards = new List<int> ();
		} 
		else {
			cards.Clear ();
		}
		for (int i = 0; i < 31; i++) {
			cards.Add (i);
		}

		int n = cards.Count;
		while (n > 1) {
			n--;
			int s = Random.Range (0, n + 1);
			int temp = cards [s];
			cards [s] = cards [n];
			cards [n] = temp;
		}
		//testCardOrder();
		//drawStartHand();

	}

	void startTurn(){
		List<GameObject> axeBuffs = new List<GameObject>();
		axeBuffs.AddRange(GameObject.FindGameObjectsWithTag("axeBuff"));
		if(axeBuffs.Count > 0){
			for(int i = 0; i < axeBuffs.Count; i++){
				axeBuffs[i].GetComponent<axeBonusDamage>().turnPass();
			}
		}
		axeBuffs.Clear();
		axeBuffs.AddRange(GameObject.FindGameObjectsWithTag("bloodRage"));
		if(axeBuffs.Count > 0){
			for(int i = 0; i < axeBuffs.Count; i++){
				axeBuffs[i].GetComponent<bloodRage>().turnPass();
			}
		}
		resources++;
		if(resources>maxResources)resources = maxResources;
		curResources = resources;
		text.text = ("Resources: "+curResources.ToString());
		hitPlayer(1);
	}

	public void playedCard(int id, int cost){
		hand.RemoveAt(hand.IndexOf(id));
		curResources -= cost;
		text.text = ("Resources: "+curResources.ToString());
	}

	void drawFunction(){
		if (hand.Count < 7) {

			int cID = CardID();
			GameObject cardGO = (GameObject)Instantiate (Decks.classDeck[cards[cID]], Vector3.zero, Quaternion.identity);
			cardGO.transform.SetParent (Hand);
			cardGO.GetComponent<dragg>().setCardID(cards[cID]);
			hand.Add(cards[cID]);
		}
		else{
			print("Hand is full");
		}
	}

	void drawStartHand(){
		for(int i = 0; i < 5; i++){
			drawFunction();
		}
	}

	int CardID(){
		currentCardID++;
		return(currentCardID-1);
	}

	void testCardOrder(){
		for(int i = 0; i < 9; i++){
			print(cards[i]);
		}
	}

	void hitPlayer(int dmg){
		curHP -= dmg;
		HP.text = ("Health: "+curHP);
	}

	public int getHP(){
		return curHP;
	}

	void Start ()
	{
		Decks = this.gameObject.GetComponent<premadeDecks>();
		shuffle ();
		maxHP = Decks.maxHP;
		curHP = maxHP;
		HP.text = ("Health: "+curHP);
	}

	void Update () {
		
		if(Input.GetKeyDown(KeyCode.Space)){
			if(firstDraw){
				drawStartHand(); 
				firstDraw = false;
				startTurn();
			}
			else {
				drawFunction();
				startTurn();
			}
		}
	}
}





//test