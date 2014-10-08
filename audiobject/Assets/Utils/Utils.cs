using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Utils{



		public class audioMeta{
				public string audiopath;
				public string annotationpath;
				public audioMeta(string fp,string ap){
						audiopath = fp;
						annotationpath = ap;
				}
		}



	public class Utils : MonoBehaviour {

	// Use this for initialization
	void Start () {

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

				public static List<Vector2> Csv2List(string textin){
						List<Vector2> res = new List<Vector2> ();
						string[] sll = textin.Split ('\n');
						foreach( string line in sll){
								Vector2 c = new Vector2 ();
								string[] sl = line.Split ('\t');
								//print (line.Split ('\t'));
								if (sl.Length > 1) {
										bool psed = float.TryParse (sl [0], out c.x);
										psed &= float.TryParse (sl [1], out c.y);
										if (psed)
												res.Add (c);
								}

						}
						return res;

				}
//		public static List<Vector2> readCsv(string fp){
//			
//			List<Vector2> res = new List<Vector2> ();
//			var contents = File.ReadAllText(fp).Split('\n');
//			foreach( string line in contents){
//								Vector2 c = new Vector2 ();
//								string[] sl = line.Split ('\t');
//								//print (line.Split ('\t'));
//								if (sl.Length > 1) {
//										bool psed = float.TryParse (sl [0], out c.x);
//										psed &= float.TryParse (sl [1], out c.y);
//										if (psed)
//												res.Add (c);
//								}
//								
//		}
//			return res;
//
//		}
		
				public static Vector3 circle(int idx,int total,float radius=1){
						return Quaternion.AngleAxis(360.0f*idx/(total),Vector3.forward)* Vector3.left*radius;
				}

				public static Vector3 grid(int idx,int total,float size){
						int num =(int) Mathf.Sqrt (total);
						float step = size/(num);

						return Vector3.left * ((idx/num-(num-1)/2.0f)*step) + Vector3.up * step* ((idx % num)-(num-1)/2.0f);
				}

				public static Vector3 cube(int idx,int total,float size){
						int num =(int) Mathf.Pow (total,1/3.0f);
						float step = size/(num);

						return Vector3.left * ((idx/num-(num-1)/2.0f)*step) + Vector3.up * step* ((idx % num)-(num-1)/2.0f) + Vector3.forward* ((int)(idx/(num*num))*step);
				}

				public static Mesh  arc(float rin,float rout,Vector2 deg,float thick = 1){

						Mesh m = new Mesh ();

						Vector3[] res = new Vector3[4];
						res [0] = Quaternion.AngleAxis (deg.x , Vector3.forward)*Vector3.left * rin  + Vector3.forward*thick/2;
						res [1] = Quaternion.AngleAxis (deg.y , Vector3.forward)*Vector3.left * rin  + Vector3.forward*thick/2;
						res [2] = Quaternion.AngleAxis (deg.x , Vector3.forward)*Vector3.left * rout + Vector3.forward*thick/2;
						res [3] = Quaternion.AngleAxis (deg.y , Vector3.forward)*Vector3.left * rout + Vector3.forward*thick/2;
//						res [4] = res [0] - 2*Vector3.forward*thick/2;
//						res [5] = res [1] - 2*Vector3.forward*thick/2;
//						res [6] = res [2]-  2*Vector3.forward*thick/2;
//						res [7] = res [3]-  2*Vector3.forward*thick/2;
//						foreach (Vector3 v in res) {
//								print (v);
//						}
						int[] id = {1, 2, 0,
									1, 3, 2,

//									0, 2, 4,
//									4, 6, 2,
//										
//									3, 1, 5,
//									5, 7, 3,
//
//									0, 1, 4,
//									4, 5, 1
//
//									3, 2, 6,
//									6, 7, 3,
//
//									4, 5, 6,
//									5, 6, 7
						};


						Vector2[] uv = {Vector2.zero,
								Vector2.right,
								Vector2.up,
								Vector2.one
						};
						m.vertices = res;
						m.triangles = id;
						m.uv=uv;
						m.RecalculateNormals ();
						//m.RecalculateBounds ();


						return m;







//						res [8] = res [2];
//						res [9] = res [3];
//						res [10]= res [4];
//						res [11]= res [5];
//	                    res [12]= res [2];
//	                    res [13]= res [3];
//	                    res [14]= res [6];
//	                    res [15]= res [7];
//
//					 res [16]= res [0];
//                     res [17]= res [1];
//                     res [18]= res [4];
//                     res [19]= res [5];
//                     res [20]= res [0];
//                     res [21]= res [1];
//                     res [22]= res [6];
//                     res [23]= res [7];




				}

	}



		public class CircularBuffer<T> : List<T>
		{
				public int Capacity = 1;

				public CircularBuffer(int capacity)
				{
						Capacity = capacity;
				}

				public CircularBuffer()
				{
				}

				public void Add (T item)
				{
						base.Add(item);
						if(Count > Capacity)
								RemoveAt(0);

				}
				public void Insert (int index, T item)
				{
						base.Insert(index,item);
						if(Count > Capacity)
								RemoveAt(0);
				}
		}


}// namespace utils
