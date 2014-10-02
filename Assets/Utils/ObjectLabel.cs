using UnityEngine;
using System.Collections;


public class ObjectLabel : MonoBehaviour {


		public string text;
		public Color color;
		public int sideSize = 60;
		Camera cam ;
		void Start () 
		{		
				cam = Camera.main;

				color = Color.white;
		}


		void OnGUI()
		{		
				GUI.contentColor = color;
				Vector2 ori = cam.WorldToViewportPoint (transform.position);
				Rect r = new Rect (ori.x*1.0f*Screen.width-sideSize/2,(1-ori.y)*1.0f*Screen.height-sideSize/2,sideSize,sideSize);
				GUI.Label (r, text);

		}
}