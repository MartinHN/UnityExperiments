using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;



public class Spring : MonoBehaviour {

		public List<Transform> elements;


		public static float spring = 1;
		public Vector3 AxesMask = Vector3.one;

 
		public static Spring instance;

		[DllImport ("ASimplePlugin")]
		private static extern void updatePhysics( [In,Out] Vector3 [] ds, int size,float deltaTime);

		[DllImport ("ASimplePlugin")]
		private static extern void addElement(int UID1,int UID2, float sim);

		[DllImport ("ASimplePlugin")]
		private static extern void setSpring (float k);

		[DllImport ("ASimplePlugin")]
		private static extern void destroydata ();

		Vector3[] pos;


	// Use this for initialization
	void Awake () {
				if (instance == null) {
						instance = this;
						elements = new List<Transform> ();
				}



	}


	
	// Update is called once per frame
	void FixedUpdate () {

				if (ContainerHandler.endLoad && elements != null) {
					
						for (int i = 0; i < elements.Count; i++) {
										
										pos [i]  = elements [i].position;

						}

						if (pos != null){
								updatePhysics (pos,  pos.Length,Time.fixedDeltaTime);
				}

						//apply?
						for (int i = 0; i < elements.Count; i++) {

								elements [i].position=pos [i] ;

						}
				}
		}



		public void makeSpring(Container origin, Dictionary<Container,float> target,float spring,bool scaleIt = true){
//				setSpring (10);

			if(!elements.Contains(origin.transform))
						elements.Add(origin.transform);

			foreach (Container c in target.Keys){
					if(!elements.Contains(c.transform))
						elements.Add(c.transform);
						float l0 = scaleIt ? origin.transform.lossyScale.magnitude * target [c] : target [c];
						int id1 = elements.IndexOf (origin.transform);
						int id2 = elements.IndexOf (c.transform);
			print ("adding");
			print (id1);
			print (id2);
//						addElement(id1,id2,l0);


						}


				pos = new Vector3[elements.Count];


		}



	void OnDestroy(){
				print ("destroy");
		destroydata ();
	}

}
