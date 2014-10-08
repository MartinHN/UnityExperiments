using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Utils;



// dispatch data from files
public class AudioLoader : MonoBehaviour {
		string audioFolder = "/Users/mhermant/Documents/Work/Datasets/beatles/audio/wav/";
		string annotationFolder = "/Users/mhermant/Documents/Work/Datasets/beatles/annotations/segmentation/";

		public static Dictionary<string,audioMeta> dict;
	
		Vector3 lastMP;
		float radius = 1;
		float audioUpdateTime = 0.5f;
		float lastAudioUpdate=0;




	// Use this for initialization
	void Awake () {
				LoadAudioMetadata ();
				AudioSlicePlayer.CreateAudioPlayers (transform);
				lastMP = Input.mousePosition;


	}




		void Start(){
		

		}
	// Update is called once per frame
	void Update () {
				float speed = (lastMP - Input.mousePosition).magnitude;

				if ( speed > 0.1 && Time.time-lastAudioUpdate>audioUpdateTime && speed/Time.deltaTime < 500.0f) {
						Vector3 p = Camera.main.ScreenToWorldPoint (Input.mousePosition-Vector3.forward*transform.position.z);



						lastAudioUpdate = Time.time;
				}
				lastMP = Input.mousePosition;
	
	}


		void LoadAudioMetadata(){
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
		}





		void OnDrawGizmos(){
			//	Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint (Input.mousePosition-Vector3.forward*transform.position.z),radius);
		}



}
