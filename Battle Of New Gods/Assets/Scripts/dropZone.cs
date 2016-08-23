using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class dropZone : MonoBehaviour, IDropHandler {

	public void OnDrop(PointerEventData eventData){
		//print("OnDrop to " + gameObject.name);

		dragg d = eventData.pointerDrag.GetComponent<dragg>();
		if(d != null){
			if(this.transform.tag == "ConstantDrop" && d.gameObject.tag == "Constant"){
				this.transform.tag = "InUse";
				d.OGParent = this.transform;
			}
			else if(this.transform.tag == "WeaponDrop" && d.gameObject.tag == "Weapon"){
				this.transform.tag = "InUse";
				d.OGParent = this.transform;
			}
			else if(this.transform.tag == "SpellDrop" && d.gameObject.tag == "Spell"){
				d.OGParent = this.transform;
			}
		}
	}
}
