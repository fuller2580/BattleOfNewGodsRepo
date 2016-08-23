﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class deck : MonoBehaviour
{
	List<int> cards;
	int currentCardID = 0;
	public Transform Hand;
	premadeDecks Decks;
	public void shuffle()
	{
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
		drawStartHand();

	}

	void drawStartHand(){
		for(int i = 0; i < 5; i++){
			GameObject cardGO = (GameObject)Instantiate(Decks.Axecards[cards[CardID()]], Vector3.zero,Quaternion.identity);
			cardGO.transform.SetParent(Hand);
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

	void Start ()
	{
		Decks = this.gameObject.GetComponent<premadeDecks>();
		shuffle ();
	}

	void Update () {
	
	}
}





//test