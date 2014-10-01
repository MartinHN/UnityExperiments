using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Utils;

public class AudioLoader : MonoBehaviour {
		string audioFolder = "/Users/mhermant/Documents/Work/Datasets/beatles/audio/wav/";
		string annotationFolder = "/Users/mhermant/Documents/Work/Datasets/beatles/annotations/segmentation/";


		Dictionary<string,audioMeta> dict;
	
		Vector3 lastMP;
		float radius = 1;
		float audioUpdateTime = 0.5f;
		float lastAudioUpdate=0;
		List<audioSlicer> assL;
	// Use this for initialization
	void Awake () {
				DirectoryInfo info = new DirectoryInfo(audioFolder);
				FileInfo[] fileInfo = info.GetFiles();
				info = new DirectoryInfo (annotationFolder);
				List<FileInfo> annotationInfo = new List<FileInfo>(info.GetFiles ());

				dict = new Dictionary<string, audioMeta> ();
				foreach (FileInfo file in fileInfo){
						string fn = Path.GetFileNameWithoutExtension(file.Name);
						//print ("processing : " +fn);

						foreach (FileInfo an in annotationInfo) {
								if (fn.Contains (Path.GetFileNameWithoutExtension (an.Name))) {
										dict.Add (fn, new audioMeta (file.FullName, an.FullName));
										annotationInfo.Remove (an);
										break;
								}

						}

				}
				assL = new List<audioSlicer> ();
				int idx = 0;
				int maxnum = 2;
				int numinst = Mathf.Min (maxnum, dict.Count);
				float gridsize=5;
				float side = gridsize / (Mathf.Sqrt (numinst));
				Segment.rin = side / (10.0f );
				Segment.rout = side / (2.0f);
				gridsize -= Segment.rout;
				foreach (string n in dict.Keys) {
						GameObject go = Instantiate(Resources.Load("Song"),Utils.Utils.grid(idx,numinst,gridsize),Quaternion.LookRotation(transform.TransformDirection(Vector3.forward))) as GameObject;
						go.name = "song : "+n;			
						audioSlicer asl = go.GetComponent<audioSlicer> ();
						asl.fname = n;
						asl.annotationpath = dict [n].annotationpath;
						asl.audiopath = dict [n].audiopath;
						StartCoroutine(asl.LoadData());
						asl.GetComponent<BoxCollider> ().size = new Vector3 (side/2, side/2,.1f);
						//asl.transform.localPosition += Vector3.forward * .06f;
						assL.Add (asl);
								idx ++;
						if (idx >= maxnum) break;
				}


				lastMP = Input.mousePosition;


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

		void Start(){

				foreach (audioSlicer ass in assL)
				{
						{
								ConfigurableJoint sj = ass.transform.gameObject.AddComponent<ConfigurableJoint> ();
								sj.connectedBody = null;
								makeSpring (ref sj, 2);




						}
						for (int i = 0; i < 1; i++) {
								Rigidbody r = assL [Random.Range (0, assL.Count - 1)].rigidbody;
								if (r != ass.rigidbody) {
										ConfigurableJoint sj = ass.gameObject.AddComponent<ConfigurableJoint> ();
										sj.autoConfigureConnectedAnchor = false;
										sj.connectedAnchor = Vector3.zero;

										makeSpring (ref sj, 2);

										sj.connectedBody = r;
								} else {
										print (ass.name);
								}


						}
				}

		}
	// Update is called once per frame
	void Update () {
				float speed = (lastMP - Input.mousePosition).magnitude;

				if ( speed > 0.1 && Time.time-lastAudioUpdate>audioUpdateTime && speed/Time.deltaTime < 500.0f) {
						Vector3 p = Camera.main.ScreenToWorldPoint (Input.mousePosition-Vector3.forward*transform.position.z);

						//print (Input.mousePosition);


						foreach(audioSlicer ass in assL){
								if ((ass.transform.position - p).magnitude < radius) {

										if (ass.isLoading || ass.hasClip)
												break;
										 else {
												StartCoroutine (ass.LoadAudio ());
										}
								}
								else if (ass.hasClip && !ass.audioSource.isPlaying) {
										ass.unloadAudio ();
								}

						}



						lastAudioUpdate = Time.time;
				}
				lastMP = Input.mousePosition;
	
	}


	



}
