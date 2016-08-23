using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class deck : MonoBehaviour
{
	List<int> cards;

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



	}

	void Start ()
	{
		shuffle ();
	}

	void Update () {
	
	}
}
