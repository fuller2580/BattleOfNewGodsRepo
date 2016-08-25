using UnityEngine;
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
	int currentCardID = 0;
	public Transform Hand;
	premadeDecks Decks;
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
		resources++;
		if(resources>maxResources)resources = maxResources;
		curResources = resources;
		text.text = ("Resources: "+curResources.ToString());
	}

	public void playedCard(int id, int cost){
		hand.RemoveAt(hand.IndexOf(id));
		curResources -= cost;
		text.text = ("Resources: "+curResources.ToString());
	}

	void drawFunction(){
		if (hand.Count < 7) {

			int cID = CardID();
			GameObject cardGO = (GameObject)Instantiate (Decks.Axecards [cards[cID]], Vector3.zero, Quaternion.identity);
			cardGO.transform.SetParent (Hand);
			cardGO.GetComponent<dragg>().setCardID(cards[cID]);
			hand.Add(cards[cID]);
		}
		else{
			print("Hand is full");
		}
		startTurn();
	}

	//this is only a visual representation of the cards still need a way to track them through code so we can make them actually have effects.
	void drawStartHand(){
		for(int i = 0; i < 5; i++){
			drawFunction();
		}
		resources = 1;
		curResources = resources;
		text.text = ("Resources: "+curResources.ToString());
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
		
		if(Input.GetKeyDown(KeyCode.Space)){
			if(firstDraw){
				drawStartHand(); 
				firstDraw = false;
			}
			else drawFunction();
		}
	}
}





//test