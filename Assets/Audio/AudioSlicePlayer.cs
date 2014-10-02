using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Utils;

using Holoville.HOTween;



// audio player & manager
public class AudioSlicePlayer : Interact {
		public string fname="noname";
		public string annotationpath = "/Users/mhermant/Desktop/TMP/unityAudio/";
		public string audiopath = "/Users/mhermant/Desktop/TMP/unityAudio/";
//		public AudioSource audio;
		public float totalLength;

		public bool hasClip=false;
		public bool isLoading = false;
		static List<AudioSlicePlayer> assL;
		public static List<AudioSlicePlayer> playing;
		public static int maxPolyphony = 5;
		Container callingContainer;
		float begin;
		float end;

		public void LoadAudio (){
				isLoading = true;

				StartCoroutine (loadFile (audiopath));

				hasClip = true;
				isLoading = false;

		}
		public IEnumerator loadFile(string path){
				WWW www = new WWW ("file://"+path);
				yield return www;

				audio.clip = www.GetAudioClip(false);
				audio.playOnAwake = false;
				play (begin, end);
		}

		public void unloadAudio(){
				DestroyImmediate (audio.clip);
				callingContainer.player = null;
				callingContainer.isPlaying = false;
				hasClip = false;
				playing.Remove (this);
				StopCoroutine ("autoPause");
		}




		public void play(float begin, float end){

						audio.time = begin;
						audio.Play ();
						


				audio.SetScheduledEndTime (AudioSettings.dspTime + end - begin);
				StartCoroutine(autoPause(end - begin));
				playing.Add (this);

		}

		static public  AudioSlicePlayer  play(Container c,float begin , float end){
				if(playing.Count<maxPolyphony){
						AudioSlicePlayer target=null;
						foreach(AudioSlicePlayer p in assL){
								bool found = false;
								foreach(AudioSlicePlayer pl in playing){
										if(pl==p){
												found = true;
										}
								}


								if (!found) {
										p.audiopath = AudioLoader.dict[c.filename].audiopath;
										p.begin = begin;
										p.end = end;
										p.LoadAudio();
										p.callingContainer = c;
										target = p;

										break;
								}
						}

						if (target == null)
								return null;

						return target;
				}
				else print("maxPolyphonyCount");
				return null;
		}


		IEnumerator autoPause(float t){
				yield return new WaitForSeconds (t);

				unloadAudio();

		}



		public static void CreateAudioPlayers(Transform t){
				playing = new List<AudioSlicePlayer> ();
				assL = new List<AudioSlicePlayer> ();
				for(int i = 0 ; i < AudioSlicePlayer.maxPolyphony; i++) {
						GameObject go = Instantiate (Resources.Load ("Song"))as GameObject;//,Utils.Utils.grid(idx,numinst,gridsize),Quaternion.LookRotation(transform.TransformDirection(Vector3.forward))) as GameObject;
						go.name = "player"+i;
						go.transform.parent = t;

						AudioSlicePlayer asl = go.GetComponent<AudioSlicePlayer> ();

						assL.Add (asl);

				}
		}




}
