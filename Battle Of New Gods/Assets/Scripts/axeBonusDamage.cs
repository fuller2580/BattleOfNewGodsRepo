using UnityEngine;
using System.Collections;

public class axeBonusDamage : MonoBehaviour {
	int bonusDmg;
	int turns;
	GameObject cam;
	public GameObject parent;
	public bool oneTime = true;
	// Use this for initialization
	void Start () {
	
	}

	public void setBonus(int dmg, int turn){
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		bonusDmg = dmg;
		turns = turn;
		addBonus();
	}
	void addBonus(){
		cam.GetComponent<cardFunctionality>().addAxeBonus(bonusDmg);
	}
	public void turnPass(){
		if(turns <= 0){
			endBonus();
		}
		else turns --;
	}
	void endBonus(){
		bonusDmg *= -1;
		cam.GetComponent<cardFunctionality>().addAxeBonus(bonusDmg);
		if(oneTime){
			if(parent.gameObject.tag == "Constant")endConstant();
			else Destroy(parent.gameObject);
		}
		Destroy(this.gameObject);
	}
	void endConstant(){
		parent.transform.parent.tag = "ConstantDrop";
		Destroy(parent.gameObject);
	}
}
