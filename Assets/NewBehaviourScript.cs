using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour
{
		// Use this for initialization
		void Start ()
		{
				float randomint = (float)gaussint (1, 1);
				float result = map (0, 1, -1, 1, randomint);
				Debug.Log (randomint);
				Debug.Log (result);	
				RandomHeights ();
	
		}

		float map (float outLow, float outMax, float inLow, float inMax, float value)
		{
		
				//if (value <= inLow)
				//		return outLow;
				//else if (value >= inMax)
				//		return outMax;
		
				return (((outMax - outLow) * ((value - inLow))) / (inMax - inLow)) + outLow;
		
		}

		public double gaussint (int mean, int stdDev)
		{
				System.Random randomGen = new System.Random ();
				double u1 = randomGen.NextDouble (); //these are uniform(0,1) random doubles
				double u2 = randomGen.NextDouble ();
				double randStdNormal = System.Math.Sqrt (-2.0 * System.Math.Log (u1)) * System.Math.Sin (2.0 * System.Math.PI * u2); //random normal(0,1)
				double randNormal = (mean + stdDev * randStdNormal); //random normal(mean,stdDev^2)	
				return randNormal;
		}

		public void RandomHeights ()
		{



				Terrain terrain = gameObject.GetComponent<Terrain> ();
				TerrainData terData = terrain.terrainData;
				Vector3 pos = gameObject.transform.localPosition;
		
				float[,] rHeight = new float[terData.heightmapWidth, terData.heightmapHeight];
				Debug.Log (terData.heightmapWidth);
				for (int x = 0; x< terData.heightmapWidth; x++) {
						for (int y=0; y< terData.heightmapHeight; y++) {

								float randomint = (float)gaussint ((x*y), (100));
								Debug.Log (randomint);
								rHeight [y, x] = randomint/1000;
				
								//double v = map (0, 1, -200, 200, randomint);
								//Debug.Log (v);
								//rHeight [y, x] = (float)v;
						}
				}
		
				terData.SetHeights (0, 0, rHeight);
		}
		// Update is called once per frame
		void Update ()
		{
	
		}
}
