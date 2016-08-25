using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class premadeDecks : MonoBehaviour {

	public List<GameObject> Axecards;
	public List<GameObject> Elecards;
	public List<GameObject> Solcards;

	public List<GameObject> classDeck;

	public void SelectDeck(int i){
		switch(i){
		case 0:
			classDeck = Axecards;
			break;
		case 1:
			classDeck = Elecards;
			break;
		case 2:
			classDeck = Solcards;
			break;
		}
	}
	void Start(){
		
	}
}
