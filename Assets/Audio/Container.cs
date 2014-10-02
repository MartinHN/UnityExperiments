using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;



public class Container : Interact {
		public List<Container> childs;
		public Container container;
		int _level;
		public bool displayName=true;
		public float begin;
		public float end;
		public string filename;
		public int idx;
		bool _isPlaying = false;
		public AudioSlicePlayer player;


	// Use this for initialization
	void Start () {
				childs = new List<Container> ();

	}
	
	// Update is called once per frame
	void Update () {

	
	}

		public List<Container> getSiblings(){
				if (level == 0)
						return null;
				List<Container> res= new List<Container>();
				foreach(Container c in container.childs){
						res.AddRange(c.childs);
				}
				return res;
		}

		public List<Container> getLevel(int l){
				List<Container> res = new List<Container> ();
				if (level == l - 1)
						return childs;
				else{
						foreach(Container c in childs){
								res.AddRange (c.getLevel (l));
						}
				}
		}


		override public void action0(bool t){

				isPlaying = !isPlaying;

		}

		public bool isPlaying{
				get{return _isPlaying;
				}
				set{
						if (!value) {
								//print (player);
								if (player != null) {
										player.unloadAudio();
										player = null;
								}

								//_isPlaying = false;

								HOTween.To (renderer.material, 1, "color", new Color(0,0,0,0.5f) );
						} else { 
								//print (begin);
								player = AudioSlicePlayer.play (this, begin, end);

								//_isPlaying = true;
								HOTween.To (renderer.material, 1, "color", new Color(1,1,1,0.5f));

						}

						_isPlaying = value;
				}
		}


		public void buildMesh(){
				GetComponent<MeshCollider> ().sharedMesh = GetComponent<MeshFilter> ().sharedMesh;
		}


		override public void onHover(bool t){
				if (displayName) {
						if (t)
								GuiUtils.displayMe (transform.gameObject);
						else
								GuiUtils.hideMe (transform.gameObject);
				}
		}

	public int level {
		get {
			return _level;
		}
				set{
						//gameObject.layer = 1<<LayerMask.NameToLayer (value.ToString ());
						_level = value;
				}
	}


}
