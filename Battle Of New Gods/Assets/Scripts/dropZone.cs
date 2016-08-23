using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class dropZone : MonoBehaviour, IDropHandler {

	public void OnDrop(PointerEventData eventData){
		print("OnDrop to " + gameObject.name);

		dragg d = eventData.pointerDrag.GetComponent<dragg>();
		if(d != null){
			d.OGParent = this.transform;
		}
	}
}
