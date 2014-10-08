using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.InteropServices;



public class Spring : MonoBehaviour {
		public Rigidbody[][] target;
		public Rigidbody[] origins;
		public float[] l0;

		public static float spring = 1;
		public Vector3 AxesMask = Vector3.one;
		static int count;
		Vector3 totalForce ;
		Vector3 tmpPosition ; 
		public static Spring instance;

		List<Container> Idxs;

		[DllImport ("ASimplePlugin")]
		private static extern void updatePhysics( [In, Out]float [][] res, int squareSize);


//		[DllImport ("ASimplePlugin")]
//		private static extern IntPtr PrintHello();
//		[DllImport ("ASimplePlugin")]
//		private static extern void tst(float res);

		float[] pos;
		float[][] l0s;
		Vector3 [][] ds;
		float [] res;
		IntPtr[] buffer;

	// Use this for initialization
	void Awake () {
				if (instance == null) {
						instance = this;

				}
			
//				count++;
//				//target = new Dictionary<Rigidbody,float> ();
//				if (count > 1000)
//						print (count);



	}


	
	// Update is called once per frame
	void FixedUpdate () {
//				totalForce = Vector3.zero;
//				tmpPosition = transform.position;
//				Vector3 dist = Vector3.zero;
//				Vector3 force = Vector3.zero;
//				for (int i  = 0; i  < target.Length ; i++){
//						dist = (target[i].transform.position - tmpPosition);
//						force = (dist.magnitude - l0[i])  * spring * dist.normalized;
//						force.Scale (AxesMask);
//						totalForce +=force;
//						target[i].AddForce (-force);
//
//				}
//				rigidbody.AddForce (totalForce);
				if (ContainerHandler.endLoad && Idxs != null) {
						print (Idxs.Count);
						for (int i = 0; i < Idxs.Count; i++) {
								for (int j = i + 1; j < Idxs.Count; j++) {
										Vector3 dist = Idxs [j].transform.position - Idxs [i].transform.position;
										ds [i] [j] = dist;
										ds [j] [i] = -1 * dist;

								}
						}
						if (res != null){

								unsafe{
										updatePhysics (l0s,  l0s.Length);
								}
//								for (int i = 0; i < l0s.Length; i++) {
////										Marshal.Copy (l0s[i], 0, buffer[i], l0s.Length);
//										buffer [i] = new IntPtr(l0s[i]);
//
//								}
//								}
//
//								print ("////////////////////");
//								for (int i = 0; i < l0s.Length; i++) {
//										string s = "";
//										for (int j = 0; j < l0s [i].Length; j++) {
//												s += l0s [i] [j] + "//";
//										}
//										print (s);
//								}
//								print ("/////////////////////////");



//								Marshal.Copy( buffer, arrayRes, 0, size );

				}

				}
		}

//		public static Spring makeSpring(Container origin, Container target,float spring,float l0,bool scaleIt = true){
//				print ("ms"+origin);
//				Spring res =  origin.gameObject.AddComponent<Spring> ();
//
//				res.target = new Rigidbody[1][1];
//				res.target.GetLength(0)
//				res.target[0] = target.rigidbody;
//				res.l0 = new float[1];
//				res.l0[0] = scaleIt? origin.transform.lossyScale.magnitude*l0 : l0;
//				//res.spring = spring;
//
//				return res;
//
//		}

		public void makeSpring(Container origin, Dictionary<Container,float> target,float spring,bool scaleIt = true){


				print ("Make");
				// first
				if (l0s == null) {
						print ("init");
						l0s = new float[target.Count+1][];
						for(int i = 0; i< target.Count+1 ; i++)
								{
								l0s[i] = new float[target.Count+1];
								}
						Idxs = new List<Container> ();
						Idxs.Add (origin);
						int ii = 1;
						l0s [0][0] = -1;
						foreach (Container c in target.Keys){
								Idxs.Add(c);
								l0s [0] [ii] = scaleIt ? origin.transform.lossyScale.magnitude * target [c] : target [c];
								l0s [ii] [0] = scaleIt ? origin.transform.lossyScale.magnitude * target [c] : target [c];
								ii++;
						}




				}
				// add content
				else {


								
						// check consistency
						if (Idxs.IndexOf ( origin) < 0) {

								insertOne (origin);

						}
						foreach (Container c in target.Keys) {
								if (Idxs.IndexOf (c) < 0) {

										insertOne (c);
								}
						}

//						Assign values
						foreach (Container c in target.Keys) {
								int curi = Idxs.IndexOf (c);
								int orii = Idxs.IndexOf (origin);
								if (curi < 0)
										print ("BUG");
								l0s [orii] [curi] =  scaleIt ? origin.transform.lossyScale.magnitude * target [c] : target [c];
								l0s [curi] [orii] =  scaleIt ? origin.transform.lossyScale.magnitude * target [c] : target [c];
						}


							
						}
						
//				ResizeVal ();
//				res.spring = spring;



		}

		void insertOne(Container c){
				print ("insert");
				Idxs.Add (c);
		for (int i = 0; i < l0s.Length; i++) {
						Array.Resize (ref l0s [i], l0s[0].Length + 1);
						l0s [i] [l0s [i].Length - 1] = -1;
		}
				Array.Resize (ref l0s, l0s.Length + 1);
				l0s [l0s.Length - 1] = new float[l0s[0].Length];

				ResizeVal ();


		}

		public void ResizeVal(){


				ds = new Vector3[l0s.Length ][];
				for (int i = 0; i < l0s.Length; i++) {
						ds [i] = new Vector3[l0s.Length];
						for (int j = 0; j < l0s.Length; j++) {
								ds [i] [j] = Vector3.zero;

						}
				}

				res = new float[l0s.Length];


//				buffer = new IntPtr[l0s.Length];

//				for (int i = 0; i < l0s.Length; i++) {
//						buffer [i] = Marshal.AllocCoTaskMem (Marshal.SizeOf (l0s [0] [0])
//								* l0s.Length);
//				}
		}


		void OnDestroy(){
				for(int i = 0 ; i < l0s.Length ; i ++){

						Marshal.FreeCoTaskMem( buffer[i] );
				}
		}


}
