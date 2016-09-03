using UnityEngine;
using System.Collections;

public class bloodRage : MonoBehaviour {
	public GameObject aBonusDamage;
	GameObject cam;
	// Use this for initialization
	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void turnPass(){
		int bonus = 0;
		int hp = cam.GetComponent<deck>().getHP();

		if(hp > 24) bonus = 1;
		else if(hp > 18) bonus = 2;
		else if(hp > 12) bonus = 3;
		else if(hp > 6) bonus = 4;
		else bonus = 5;
		GameObject abd = (GameObject)Instantiate(aBonusDamage,Vector3.zero,Quaternion.identity);
		abd.GetComponent<axeBonusDamage>().setBonus(bonus,0);
		abd.GetComponent<axeBonusDamage>().oneTime = false;
		abd.GetComponent<axeBonusDamage>().parent = this.gameObject;
		abd.transform.parent = this.gameObject.transform;
	}
}
