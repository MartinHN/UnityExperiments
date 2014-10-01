using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;
using Holoville.HOTween;



public class GuiUtils : MonoBehaviour {

		static Dictionary<GameObject,GameObject> texts;

		static Color textColor= Color.white;
		static float fadeOut = 1;
		static float fadeIn=1;
		public static Transform refTransform;
		GameObject hovered;
		GameObject dragged;

		int dragMask;
		int hoverMask;
		int clickMask;
		bool clicked=false;
		bool onClick = false;

		// Use this for initialization
	void Start () {
				texts = new Dictionary<GameObject,GameObject> ();
				refTransform = Camera.main.transform;
				hovered = null;
				dragged = null;
				dragMask =  1<<LayerMask.NameToLayer ("Songs");
				hoverMask = 1<<LayerMask.NameToLayer("Songs");
				clickMask = 1<< LayerMask.NameToLayer("Segments");
	}
	
	// Update is called once per frame
	void Update () {
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (onClick)
						onClick = false;
				if (Input.GetMouseButtonDown (0) && !clicked){
						onClick = true;
						clicked = true;
					}
				if (Input.GetMouseButtonUp (0)) {
						clicked = false;
				}

				//clickMask
				if( onClick && Physics.Raycast(ray,out hit ,Mathf.Infinity, clickMask))
				{
								hit.collider.transform.GetComponent<Segment> ().clickAction();
				}
//				hover
				if (Physics.Raycast (ray, out hit, Mathf.Infinity, hoverMask) ) {
				
						if (hovered != hit.collider.transform.gameObject) {
								if (hovered != null)
										hovered.GetComponent<audioSlicer> ().hover (false);
								hovered = hit.collider.transform.gameObject;
								hovered.GetComponent<audioSlicer> ().hover (true);
								
						}

				}

				else {
						if (hovered != null) {
								hovered.GetComponent<audioSlicer> ().hover (false);
								hovered = null;
						}
				}
				// drag
				if ( Physics.Raycast (ray, out hit, Mathf.Infinity, dragMask)) {
						if(onClick )
								dragged = hit.collider.transform.gameObject;

				} 

				if ( dragged != null) {
						if (clicked) {
								dragged.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition - transform.position);//Vector3.Slerp (dragged.transform.position, Camera.main.ScreenToWorldPoint (Input.mousePosition-transform.position), .5f);

						} else {
								dragged = null;
						}
				}
			
								

	
	}

	public static void displayMe(GameObject go){



				destroyMe (go);
						


				GameObject goo = new GameObject() ;
				goo.transform.parent = refTransform;
				GUIText g = goo.AddComponent<GUIText> ();
				g.color = textColor* new Vector4(1,1,1,0);
				g.text = go.name;
				g.pixelOffset = Camera.main.WorldToScreenPoint(go.transform.position);

				HOTween.To (g, fadeIn, "color", textColor);
				texts.Add (go,goo);


		}
	public static void hideMe(GameObject go){
				if (texts.ContainsKey (go))
				{HOTween.To (texts [go].GetComponent<GUIText> (), fadeOut, new TweenParms ().Prop ("color", textColor * new Vector4 (1, 1, 1, 0)).OnComplete (destroyMe, go));}

		}

		static void  destroyMe(TweenEvent e){
				if (e.parms != null) {
						GameObject go = (GameObject)e.parms [0];
						destroyMe (go);
				}

		}

		static void destroyMe(GameObject go){
				if (texts.ContainsKey (go)) {
						List<Tweener> twl =HOTween.GetTweenersByTarget(texts [go].GetComponent<GUIText> (),false);
						foreach (Tweener tw in twl) {tw.Kill ();}
						Destroy (texts [go]);
						texts.Remove (go);
				}
		}
}
