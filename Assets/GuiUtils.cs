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
		// Use this for initialization
	void Start () {
				texts = new Dictionary<GameObject,GameObject> ();
				refTransform = Camera.main.transform;

	}
	
	// Update is called once per frame
	void Update () {
	
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
