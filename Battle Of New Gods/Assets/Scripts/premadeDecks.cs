using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class premadeDecks : MonoBehaviour {

	public List<GameObject> Axecards;
	public List<GameObject> Elecards;
	public List<GameObject> Solcards;
	public List<GameObject> LightCards;

	public List<GameObject> classDeck;
	public int maxHP;
	public void SelectDeck(int i){
		switch(i){
		case 0:
			maxHP = 30;
			classDeck = Axecards;
			break;
		case 1:
			maxHP = 25;
			classDeck = Elecards;
			break;
		case 2:
			maxHP = 25;
			classDeck = LightCards;
			break;
		}
	}
	void Start(){
		
	}
}
