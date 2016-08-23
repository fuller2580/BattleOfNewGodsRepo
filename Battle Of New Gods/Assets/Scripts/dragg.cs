using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class dragg : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{

	public Transform OGParent = null;
	GameObject placeHolder = null;

	public void OnBeginDrag(PointerEventData eventData){
		//print("OnBeginDrag");
		placeHolder = new GameObject();
		placeHolder.transform.SetParent(this.transform.parent);
		LayoutElement le = placeHolder.AddComponent<LayoutElement>();
		le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
		le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
		le.flexibleWidth = 0;
		le.flexibleHeight = 0;

		placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

		OGParent = this.transform.parent;
		transform.SetParent(this.transform.parent.parent);
		GetComponent<CanvasGroup>().blocksRaycasts = false;
	}
	public void OnDrag(PointerEventData eventData){
		//print("OnDrag");

		transform.position = eventData.position;

		int newSiblingIndex = OGParent.childCount;
		for(int i = 0; i < OGParent.childCount; i++){
			if(transform.position.x < OGParent.GetChild(i).position.x){
				newSiblingIndex = i;
				if(placeHolder.transform.GetSiblingIndex() < newSiblingIndex){
					newSiblingIndex--;
				}
				break;
			}
		}

		placeHolder.transform.SetSiblingIndex(newSiblingIndex);
	}
	public void OnEndDrag(PointerEventData eventData){
		//print("OnEndDrag");
		transform.SetParent(OGParent);
		transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
		GetComponent<CanvasGroup>().blocksRaycasts = true;

		Destroy(placeHolder);
	}
}
