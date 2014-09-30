using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

using Holoville.HOTween;


public class audioSlicer : MonoBehaviour {
		public string fname="noname";
		public string annotationpath = "/Users/mhermant/Desktop/TMP/unityAudio/";
		public string audiopath = "/Users/mhermant/Desktop/TMP/unityAudio/";
		public GameObject segment;
		[SerializeField]
		List<Vector2> instants;
		public AudioSource audioSource;
		public float totalLength;
		Segment _playingSegment;
		public bool hasClip=false;
		public bool isLoading = false;




	// Use this for initialization

		void Start(){


		}

		public IEnumerator LoadAudio (){
				isLoading = true;
				WWW www = new WWW ("file://"+audiopath);
				yield return www;
				audioSource.clip = www.GetAudioClip(false);
				audioSource.playOnAwake = false;
				hasClip = true;
				isLoading = false;
		
		}

		public void unloadAudio(){
				DestroyImmediate (audioSource.clip);
				hasClip = false;
		}
		public IEnumerator LoadData () {
				audioSource = gameObject.GetComponent<AudioSource> ();
				WWW www = new WWW ("file://"+annotationpath);
				yield return www;

				//print (audioSource.clip.length);
				instants = Utils.Utils.Csv2List (www.text);
				totalLength = instants [instants.Count - 1].y;
				//audio.time = 10;


				createBlocks ();
				//audioSource.Play();





	}
	
	// Update is called once per frame
	void Update () {
	
	}

		public void pause(){
				audioSource.Pause ();


		}
		public void play(float begin, float end){

						audioSource.time = begin;
						audioSource.Play ();
						


				audioSource.SetScheduledEndTime (AudioSettings.dspTime + end - begin);
				StartCoroutine(autoPause(end - begin));

		}
		IEnumerator autoPause(float t){
				yield return new WaitForSeconds (t);

				if(_playingSegment!=null)playingSegment = null;

		}

		public Segment playingSegment{
				get{
						return _playingSegment;
				}
				set{


						if (_playingSegment != null || value == null) {_playingSegment.isPlaying = false;}

						if(value!=null) {value.isPlaying = true;}
						_playingSegment = value;

						return;
				}
		}


		void createBlocks(){

				for (int i = 0; i < instants.Count; i++) {
						GameObject obj = Instantiate (Resources.Load("Segment")) as GameObject;
						obj.transform.parent = transform.parent.FindChild("Segments");
//						Segment s = obj.GetComponent<Segment> ();
						Segment s = obj.AddComponent<Segment> ();// new Segment(audioSource,instants [i - 1], instants [i]));
						s.begin = instants [i].x;
						s.end = instants [i].y;
						s.audio = this;
						s.buildMesh ();
						s.name = "segment" + (i);
						s.idx = i;

				}

		}

		void OnMouseEnter(){
				//print (name);
				GuiUtils.displayMe (transform.parent.gameObject);

		}

		void OnMouseExit(){
				//print("out");
				GuiUtils.hideMe (transform.parent.gameObject);
		}


}
