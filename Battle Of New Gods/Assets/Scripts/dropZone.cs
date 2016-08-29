using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class dropZone : MonoBehaviour, IDropHandler {

	public void OnDrop(PointerEventData eventData){
		//print("OnDrop to " + gameObject.name);

		dragg d = eventData.pointerDrag.GetComponent<dragg>();
		if(d != null){
			bool canPlay = true;
			int cost = d.gameObject.GetComponent<dragg>().getCardCost();
			int resources = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<deck>().curResources;
			bool requiresWeap = d.gameObject.GetComponent<dragg>().requiresWeapon;
			if(requiresWeap) canPlay = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<cardFunctionality>().axeWeap;
			if(cost <= resources && canPlay){
				if(this.transform.tag == "ConstantDrop" && d.gameObject.tag == "Constant"){
					this.transform.tag = "InUse";
					d.OGParent = this.transform;
					d.playedCard();
				}
				else if(this.transform.tag == "WeaponDrop" && d.gameObject.tag == "Weapon"){
					this.transform.tag = "InUse";
					d.OGParent = this.transform;
					d.playedCard(true);
				}
				else if(this.transform.tag == "SpellDrop" && d.gameObject.tag == "Spell"){
					d.OGParent = this.transform;
					d.playedCard();
				}
			}
		}
	}
}
