using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class dragg : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler{

	[HideInInspector]public Transform OGParent = null;
	GameObject placeHolder = null;
	bool zCard = false;
	int cardID;
	public int cardNumber;
	Vector3 oldPosition;
	public int cardCost;
	public bool isWeapon;
	[HideInInspector]public bool canDrag = true;
	bool dragMe = true;
	GameObject cam;
	public bool requiresWeapon;
	public int xCost;
	void Start(){
		cam = GameObject.FindGameObjectWithTag("MainCamera");
	}

	public void OnBeginDrag(PointerEventData eventData){
		if(eventData.button == PointerEventData.InputButton.Left && dragMe){
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
		if(eventData.button == PointerEventData.InputButton.Left && dragMe && cam.GetComponent<cardFunctionality>().freeToMove() && !cam.GetComponent<deck>().skipping){
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

		if(eventData.button == PointerEventData.InputButton.Left && dragMe){
			transform.SetParent(OGParent);
			transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
			GetComponent<CanvasGroup>().blocksRaycasts = true;
			if(!canDrag) dragMe = false;
		}
			Destroy(placeHolder);

	}

	void Update(){
		//zoom in code
		if(Input.GetMouseButtonDown(0) && isWeapon && !canDrag){
			PointerEventData point = new PointerEventData(EventSystem.current);
			point.position = Input.mousePosition;

			List<RaycastResult> raycastResults = new List<RaycastResult>();
			EventSystem.current.RaycastAll(point,raycastResults);

			if(raycastResults.Count > 0){
				for(int i = 0; i < raycastResults.Count; i++){
					if(raycastResults[i].gameObject == this.gameObject){
						activateWeapon();
						break;

					}
				}
			}
		}
		else if(Input.GetMouseButtonDown(1)){
			PointerEventData point = new PointerEventData(EventSystem.current);
			point.position = Input.mousePosition;

			List<RaycastResult> raycastResults = new List<RaycastResult>();
			EventSystem.current.RaycastAll(point,raycastResults);

			if(raycastResults.Count > 0){
				for(int i = 0; i < raycastResults.Count; i++){
					if(raycastResults[i].gameObject == this.gameObject){
						zCard = true;
						break;
					}
				}
			}
			if(zCard){
				oldPosition = transform.position;
				transform.localScale = new Vector3(2,2,1);
				transform.position = new Vector3(Screen.width - 88, Screen.height - 126, 0);
			}
		}
		//reset and zoom out
		if(Input.GetMouseButtonUp(1)&&zCard){
			transform.localScale = new Vector3(1,1,1);
			transform.position = oldPosition;
			zCard = false;
		}
	}

	public void activateWeapon(){
		deck Deck = cam.GetComponent<deck>();
		if(Deck.curResources > getCardCost()){
			cam.GetComponent<cardFunctionality>().activateCard(cardNumber,this.gameObject);
			Deck.curResources -= getCardCost();
			Deck.text.text = ("Resources: "+Deck.curResources.ToString());
		}
	}

	public void playedCard(){
		cam.GetComponent<deck>().playedCard(getCardID(),getCardCost());
		cam.GetComponent<cardFunctionality>().activateCard(cardNumber,this.gameObject);
		canDrag = false;
	}
	public void playedCard(bool isWeapon){
		cam.GetComponent<deck>().playedCard(getCardID(),getCardCost());
		if(!isWeapon)cam.GetComponent<cardFunctionality>().activateCard(cardNumber,this.gameObject);
		else cam.GetComponent<cardFunctionality>().weapEquip();
		canDrag = false;
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
			cCost = cam.GetComponent<deck>().curResources;
			xCost = cCost;
		}
		return cCost;
	}
}
