using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContainerHandler : MonoBehaviour {
		[SerializeField]
		public static Container rootContainer;
		float fractalScale = .4f;
		public static bool endLoad = false;

	// Use this for initialization
	void Awake () {
				rootContainer = GetComponent<Container> ();
				//rootContainer.level = 0;
		}
		void Start(){
				StartCoroutine(LoadInstantsData());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		public IEnumerator LoadInstantsData () {


				int idx = 0;
				int maxnum = 5;
				int numinst = Mathf.Min (maxnum, AudioLoader.dict.Count);
				float gridsize=5;
				float side = gridsize / (Mathf.Sqrt (numinst));

				int nsongs = AudioLoader.dict.Count;

				foreach(string fn in AudioLoader.dict.Keys){
						GameObject go = Instantiate (Resources.Load ("Container"),Utils.Utils.grid(idx,numinst,gridsize),Quaternion.LookRotation(transform.TransformDirection(Vector3.forward))) as GameObject;
						go.name = fn;
						go.rigidbody.isKinematic = true;
						go.transform.parent = transform;
						go.transform.localScale = side/2*Vector3.one;
						go.renderer.enabled = true;
						Container cont = go.GetComponent<Container> ();
						cont.filename = fn;
						cont.isPlaying = false;
						cont.isDraggable = true;
						cont.level = 1;
						WWW www = new WWW ("file://"+AudioLoader.dict[fn].annotationpath);
						yield return www;

					

						{
						List<Vector2> instants = Utils.Utils.Csv2List (www.text);

								cont.begin = 0;
								cont.end = instants [instants.Count - 1].y;
								createBlocks (instants,go.transform,fn);
						}


						idx ++;
						if (idx >= maxnum) break;
				}
				StartCoroutine(CreateSimilarities ());

	}


		public IEnumerator CreateSimilarities(){
				List<Container> co = new List<Container>();
				rootContainer.getLevel (2,ref co);

				int stackIdx = 0;
				int count = 0;
				Dictionary<Container,float> dest = new Dictionary<Container,float> ();

				for(int i = 0 ; i  < co.Count;i++){
						Container c = co [i];


						for(int j = i+1 ; j < co.Count;j++){

								if(co[j]!=c){float l0 = Random.Range (1, 9);

//										if (l0 < 2)
										dest [co[j]] = l0;
								}
						}

						yield return null;
						print (count++);
						Spring.instance.makeSpring (c, dest, 1, false);
						print ("fin");
						yield return null;


				}
				Spring.instance.ResizeVal ();
				endLoad = true;;
				yield return null;
			
		}



//		public static  List<Container> getLevel(int l){
//				int curl = 0;
//				int curi = 0;
////				while(curi!=rootContainer.childs.Count)
//
//		}

		void createBlocks(List<Vector2> inst,Transform pa,string file){
				float totalLength = inst [inst.Count - 1].y;
				int idx = 0;
				float anglestep = 2.0f*Mathf.PI / inst.Count;

				float radius = 1;
				foreach(Vector2 i in inst){
						GameObject go = Instantiate(Resources.Load ("Container")) as GameObject;
						go.name = "segment" + idx;
						go.transform.parent = pa;
						go.transform.localPosition = Utils.Utils.circle (idx, inst.Count, radius);
						go.transform.localScale = fractalScale*Vector3.one;
						Container c = go.GetComponent<Container> ();
						c.filename = file;
						c.displayName = false;
						c.begin = i.x;
						c.end = i.y;
						c.isDraggable = true;

						//Spring.makeSpring (c, pa.GetComponent<Container> (),  1,radius);

						c.isPlaying = false;
						c.level = 2;
						c.idx = idx++;
						c.renderer.enabled = true;


				}

//				List<Container> cc = pa.GetChild (0).GetComponent<Container> ().getSiblings();
//				float cScale = pa.GetChild(0).lossyScale.magnitude ; 
////				print (cc);
//				for (int i = 0; i < cc.Count; i++) {
//						Spring.makeSpring (cc[(i+1)%(cc.Count)],cc[i], 1,  2*radius* Mathf.Sin (anglestep/2));
//
//				}
		
		}



	
	public float simSpring {
		get {
						return Spring.spring;
		}
				set{

						Spring.spring=value;
				}
	}

		void OnGUI(){
				Spring.spring = GUI.HorizontalSlider (new Rect (0, 0, 100, 100), Spring.spring, 0, 10);

		}
}
