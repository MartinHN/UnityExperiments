using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;



public class Container : Interact {
//		public List<Container> childs;
//		public Container container;
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

				//childs = new List<Container> ();

	}
	
	// Update is called once per frame
	void Update () {

	
	}

		public List<Container> getSiblings(){

				List<Container> childs = new List<Container> ();
				for(int i = 0; i < transform.parent.childCount; i++){
						childs.Add(transform.parent.GetChild(i).GetComponent<Container>());
				}
				return childs;
		}

		public void getLevel(int l, ref List<Container> res){
				//List<Container> res = new List<Container> ();
				List<Container> childs = new List<Container> ();
				for(int i = 0; i < transform.childCount; i++){
						childs.Add(transform.GetChild(i).GetComponent<Container>());
				}
		
		if (level == l - 1)
						res.AddRange( childs);
				else{
						foreach(Container c in childs){
								c.getLevel (l,ref res);
								//res.AddRange (res);
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

		[SerializeField]
	public int level {
		get {


			return _level;
		}
				set{
					int i = 1;
						Transform t = transform;
						while (t.parent != ContainerHandler.rootContainer.transform && i < 10) {
								t = transform.parent;

						i++;
					}						//gameObject.layer = 1<<LayerMask.NameToLayer (value.ToString ());
						_level = i;
				}
	}


}
