using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class bossMinion : MonoBehaviour {
	public int health;
	Text hpText;
	// Use this for initialization
	void Start () {
		hpText = this.GetComponentInChildren<Text>();
		hpText.text = "Health: "+health;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void dealDamage(int dmg){
		health -= dmg;
		hpText.text = "Health: "+health;
		if(health <= 0){
			Destroy(this.gameObject);
		}
	}
}
