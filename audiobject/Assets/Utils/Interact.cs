using UnityEngine;
using System.Collections;

public class Interact : MonoBehaviour {
		public bool isDraggable = false;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

		public virtual void onHover(bool t){
		}

		public virtual void hovering(Vector3 p){
		}

		public virtual void action0(bool click=true){
		}

		public virtual void action1(bool click=true){
		}
}
