using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class dragg : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{

	public Transform OGParent = null;
	GameObject placeHolder = null;
	GameObject zCard = null;
	int cardID;
	public int cardNumber;
	Vector3 oldPosition;
	public int cardCost;

	public void OnBeginDrag(PointerEventData eventData){
		if(eventData.button == PointerEventData.InputButton.Left){
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
	}
	public void OnDrag(PointerEventData eventData){
		//print("OnDrag");
		if(eventData.button == PointerEventData.InputButton.Left){
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
	}
	public void OnEndDrag(PointerEventData eventData){
		//print("OnEndDrag");
		if(eventData.button == PointerEventData.InputButton.Left){
			transform.SetParent(OGParent);
			transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
			GetComponent<CanvasGroup>().blocksRaycasts = true;
		}
			Destroy(placeHolder);

	}

	void Update(){
		//zoom in code
		if(Input.GetMouseButtonDown(1)){
			PointerEventData point = new PointerEventData(EventSystem.current);
			point.position = Input.mousePosition;

			List<RaycastResult> raycastResults = new List<RaycastResult>();
			EventSystem.current.RaycastAll(point,raycastResults);

			if(raycastResults.Count > 0){
				for(int i = 0; i < raycastResults.Count; i++){
					if(raycastResults[i].gameObject.tag == "Card"){
						zCard = raycastResults[i].gameObject;
						break;
					}
				}
			}
			if(zCard != null){
				oldPosition = zCard.transform.position;
				zCard.transform.localScale = new Vector3(1,1,1);
				zCard.transform.position = new Vector3(Screen.width - 88, Screen.height - 126, 0);
			}
		}
		//reset and zoom out
		if(Input.GetMouseButtonUp(1)&&zCard){
			zCard.transform.localScale = new Vector3(.5f,.5f,1);
			zCard.transform.position = oldPosition;
			zCard = null;
		}
	}

	public void playedCard(){
		GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
		cam.GetComponent<deck>().playedCard(getCardID(),getCardCost());
		cam.GetComponent<cardFunctionality>().activateCard(cardNumber);
		print("played card: "+this.gameObject.name);
	}

	public void setCardID(int id){
		cardID = id;
	}

	public int getCardID(){
		return cardID;
	}

	public int getCardCost(){
		int cCost = cardCost;
		if(cCost==100){
			GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
			cCost = cam.GetComponent<deck>().curResources;
		}
		return cCost;
	}
}
