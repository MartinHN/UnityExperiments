//using UnityEngine;
//using System.Collections;
//
//
//public class test : MonoBehaviour {
//		float [] a;
//		float [] b;
//		float [] res;
//		int num = 2000;
//
//
//
//
//	// Use this for initialization
//	void Start () {
//
//				a = new float[num*num];
//				for (int i = 0; i < num*num; i++) {
//						a [i] = UnityEngine.Random.Range (0, 10);
//				}
//				b = new float[num];
//				for (int i = 0; i < num; i++) {
//						b [i] = UnityEngine.Random.Range (0, 10);
//				}
//
//				res = new float[num];
//	}
//	
//	// Update is called once per frame
//	void FixedUpdate () {
//
//						
//							mulMat (res, a, num, b, 1, num);
////				for (int i = 0; i < num; i++) {
////						print (res [i]);
////				}
//
//	}
//}
