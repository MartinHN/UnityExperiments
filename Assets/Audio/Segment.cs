using UnityEngine;
using System.Collections;
using Holoville.HOTween;


public class Segment : MonoBehaviour {

		public audioSlicer audio;
		public float begin;
		public float end;
		public int idx;

		bool _isPlaying = false;
		public static float rin = .2f;
		public static float rout = .8f;
		public static float thick = 0;

		float _padAngle = 1;

	// Use this for initialization
	void Start () {
				audio = transform.parent.parent.GetComponentInChildren<audioSlicer> ();
				isPlaying = false;
				//buildMesh ();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		void OnMouseDown(){



				if (audio.playingSegment!=null && audio.playingSegment.idx == this.idx)
						audio.playingSegment = null;
				else
						audio.playingSegment = this;
				//isPlaying = !isPlaying;

		}

		public bool isPlaying{
				get{return _isPlaying;
				}
				set{
						if (!value) {

								audio.pause ();
								//_isPlaying = false;
								HOTween.To (renderer.material, 1, "color", Color.black);
						} else { 
								//print (begin);
								audio.play (begin, end);
								//_isPlaying = true;
								HOTween.To (renderer.material, 1, "color", Color.white);

						}


				}
		}

		public void buildMesh(){
						
//				Mesh m = GetComponent<MeshFilter> ().sharedMesh;

//						if(m.vertices.Length!=8){
//						print ("mesh not valid"+m.vertices.Length);
//								return;
//								}

				//GetComponent<MeshFilter> ().sharedMesh = Utils.Utils.arc(rin,rout,new Vector2(begin,end)*360.0f/audio.totalLength- Vector2.up*_padAngle,thick);
				//transform.localPosition = Vector3.zero;
				GetComponent<MeshCollider> ().sharedMesh = GetComponent<MeshFilter> ().sharedMesh;
//						transform.Rotate (0, 0, 360.0f * begin/  audio.audio.clip.length);



		}

}
