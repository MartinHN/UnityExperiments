using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ContainerHandler : MonoBehaviour {
		public static Container rootContainer;
		float fractalScale = .4f;
	// Use this for initialization
	void Start () {
				rootContainer = GetComponent<Container>();
				rootContainer.level = 0;
				StartCoroutine(LoadInstantsData());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

		public IEnumerator LoadInstantsData () {


				int idx = 0;
				int maxnum = 300;
				int numinst = Mathf.Min (maxnum, AudioLoader.dict.Count);
				float gridsize=5;
				float side = gridsize / (Mathf.Sqrt (numinst));

				int nsongs = AudioLoader.dict.Count;

				foreach(string fn in AudioLoader.dict.Keys){
						GameObject go = Instantiate (Resources.Load ("Container"),Utils.Utils.grid(idx,numinst,gridsize),Quaternion.LookRotation(transform.TransformDirection(Vector3.forward))) as GameObject;
						go.name = fn;
						go.transform.parent = transform;
						go.transform.localScale = side/2*Vector3.one;
						go.renderer.enabled = true;
						Container cont = go.GetComponent<Container> ();
						cont.filename = fn;
						cont.isPlaying = false;
						cont.isDraggable = true;

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
				print (rootContainer.getLevel (1));
		}



		void makeSpring(ref ConfigurableJoint res,float spring){
				//ConfigurableJoint res;
				JointDrive jd = new JointDrive();
				jd.mode =JointDriveMode.Position;
				jd.positionSpring = spring;
				jd.maximumForce = Mathf.Infinity;

				res.xDrive=jd;
				res.yDrive=jd;
				res.zDrive=jd;

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
				foreach(Vector2 i in inst){
						GameObject go = Instantiate(Resources.Load ("Container")) as GameObject;
						go.name = "segment" + idx;
						go.transform.parent = pa;
						go.transform.localPosition = Utils.Utils.circle (idx, inst.Count, 1);
						go.transform.localScale = fractalScale*Vector3.one;
						Container c = go.GetComponent<Container> ();
						c.filename = file;
						c.displayName = false;
						c.begin = i.x;
						c.end = i.y;
						c.isPlaying = false;
						c.level = 1;
						c.idx = idx++;
						c.renderer.enabled = true;

				}
		
		}
}
