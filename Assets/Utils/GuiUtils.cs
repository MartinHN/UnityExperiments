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
		Interact hovered;
		GameObject dragged;


		int interactMask;
		bool clicked=false;
		bool onClick = false;

		// Use this for initialization
	void Start () {
				texts = new Dictionary<GameObject,GameObject> ();
				refTransform = Camera.main.transform;
				hovered = null;
				dragged = null;
				interactMask =  1<<LayerMask.NameToLayer ("Songs");
				interactMask |= 1<<LayerMask.NameToLayer("Segments");

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

				if (Physics.Raycast (ray, out hit, Mathf.Infinity, interactMask)) {
						Interact ob = hit.collider.transform.GetComponent<Interact> ();
						//clickMask
						if (onClick)
								ob.action0 ();
				
						//				hover

				
						if (hovered != ob) {
								if (hovered != null)
										hovered.onHover (false);
								hovered = ob;
								ob.onHover (true);

						}

						// drag
						if (onClick && ob.isDraggable)
										dragged = hit.collider.transform.gameObject;


				}
				//no Cast
				else {
						if (hovered != null) {
								hovered.onHover (false);
								hovered = null;
						}
				}

				if (dragged != null) {
						if (clicked) {
								dragged.transform.position = Camera.main.ScreenToWorldPoint (Input.mousePosition - transform.position);//Vector3.Slerp (dragged.transform.position, Camera.main.ScreenToWorldPoint (Input.mousePosition-transform.position), .5f);

						} else {
								dragged = null;
						}
				}
			
								

	
	}

	public static void displayMe(GameObject go){



				if (texts.ContainsKey (go)) {
						List<Tweener> twl = HOTween.GetTweenersByTarget (texts [go].GetComponent<ObjectLabel> (), false);
						foreach (Tweener tw in twl) {
								tw.ResetAndChangeParms(TweenType.To,1,new TweenParms().Prop("color",textColor));
						}

//						Destroy (texts [go]);
//						texts.Remove (go);
				} else {
						


						GameObject goo = new GameObject ();
						goo.transform.parent = go.transform;//refTransform;
						ObjectLabel g = goo.AddComponent<ObjectLabel> ();
						g.gameObject.transform.localPosition = Vector3.zero;
					//	g.anchor = TextAnchor.MiddleCenter;
						g.color = textColor * new Vector4 (1, 1, 1, 0);
						g.text = go.name;
						//g.pixelOffset = Camera.main.WorldToScreenPoint (go.transform.position);

						HOTween.To (g, fadeIn, "color", textColor);
						texts.Add (go, goo);
				}


		}
	public static void hideMe(GameObject go){
				if (texts.ContainsKey (go))
				{HOTween.To (texts [go].GetComponent<ObjectLabel> (), fadeOut, new TweenParms ().Prop ("color", textColor * new Vector4 (1, 1, 1, 0)).OnComplete (destroyMe, go));}

		}

		static void  destroyMe(TweenEvent e){
				if (e.parms != null) {
						GameObject go = (GameObject)e.parms [0];
						destroyMe (go);
				}

		}

		static void destroyMe(GameObject go){
				if (texts.ContainsKey (go)) {
						List<Tweener> twl =HOTween.GetTweenersByTarget(texts [go].GetComponent<ObjectLabel> (),false);
						foreach (Tweener tw in twl) {tw.Kill ();}
						Destroy (texts [go]);
						texts.Remove (go);
				}
		}
}
