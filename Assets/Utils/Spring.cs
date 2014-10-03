using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Spring : MonoBehaviour {
		public Rigidbody[] target;
		public float[] l0;

		public float spring = 1;
		public Vector3 AxesMask = Vector3.one;
		static int maxcount;
	// Use this for initialization
	void Awake () {
				//count++;
				//target = new Dictionary<Rigidbody,float> ();
//				print (count);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

				for (int i  = 0 ; i  < target.Length ; i++){
						Vector3 dist = (target[i].transform.position - transform.position);
						Vector3 force = (dist.magnitude - l0[i])  * dist.normalized;
						force.Scale (AxesMask);
						rigidbody.AddForce (force);
						target[i].AddForce (-force);
				}
	
	}

		public static Spring makeSpring(Container origin, Container target,float spring,float l0,bool scaleIt = true){

				Spring res =  origin.gameObject.AddComponent<Spring> ();

				res.target = new Rigidbody[1];
				res.target[0] = target.rigidbody;
				res.l0 = new float[1];
				res.l0[0] = scaleIt? origin.transform.lossyScale.magnitude*l0 : l0;
				res.spring = spring;

				return res;

		}

		public static Spring makeSpring(Container origin, Dictionary<Container,float> target,float spring,bool scaleIt = true){

				Spring res =  origin.gameObject.AddComponent<Spring> ();

				res.target = new Rigidbody[target.Count];
				res.l0 = new float[target.Count];
				int i = 0;
				foreach (Container tmp in target.Keys) {
						res.target [i] = tmp.rigidbody;
						res.l0 [i] = scaleIt ? origin.transform.lossyScale.magnitude * target[tmp] : target[tmp];
						i++;
				}
				res.spring = spring;

				return res;

		}
}
