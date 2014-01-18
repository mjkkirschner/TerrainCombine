using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class TerrainGenFromPixel : MonoBehaviour
{
		public Terrain terrain;
		public Texture2D image1;
		public Texture2D[] textures;
		
	public GameObject[] testcenters;	

		// Use this for initialization

		void regenterrain (GameObject movedObj){

				
		//generate textures based on centers
		float[,,] splatmap = generateSplatDataFromCenters (terrain, testcenters);
		// uses the slatmap alphas to set the 
		terrain.terrainData.SetAlphamaps (0, 0, splatmap);

	}

		void Start ()
		{

				
				
				var heights = heightsFromPixels (image1, terrain);
				

				terrain.terrainData.SetHeights (0, 0, heights);	
				
				setTerrainTextures (textures, terrain);
				
				testcenters = generatePoints_from_textures (terrain);
				
				float[,,] splatmap = generateSplatDataFromCenters (terrain, testcenters);
			

				// uses the slatmap alphas to set the 
				terrain.terrainData.SetAlphamaps (0, 0, splatmap);
				//terrain.Flush ();
				//RandomHeights ();

		Centroid.ondirty += regenterrain;
	
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

		public GameObject[] generatePoints_from_textures(Terrain terrain){

				List<GameObject> centerlist = new List<GameObject> ();
				var width = terrain.terrainData.size.x;
				var height = terrain.terrainData.size.y;
		foreach (SplatPrototype texture in terrain.terrainData.splatPrototypes){

				
						GameObject centerPoint = GameObject.CreatePrimitive (PrimitiveType.Sphere);
						
						centerPoint.transform.parent = terrain.transform;
						centerPoint.transform.localScale = new Vector3(width/25,width/25,width/25);
						centerPoint.transform.localPosition = new Vector3 (UnityEngine.Random.Range (0, width), height, UnityEngine.Random.Range (0, width));
						centerPoint.AddComponent<Centroid> ();
						// next line is in the wrong method, just for testing custom editor
						centerPoint.GetComponent<Centroid> ().texture = texture.texture;
						centerlist.Add (centerPoint);	

		}

				return centerlist.ToArray ();

	}
			
		private void setTerrainTextures (Texture2D[] texArray, Terrain terrain)
		{
			
				// sets the textures in the texture array 
				// to the textures in the splat array of the terrain
				
				List<SplatPrototype> splats = new List<SplatPrototype> ();
					
				foreach (Texture2D texture in texArray) {
						
						SplatPrototype	currentsplat = new SplatPrototype ();	
						currentsplat.texture = texture;
						currentsplat.tileSize = new Vector2 (terrain.terrainData.size.x, terrain.terrainData.size.z);
						splats.Add (currentsplat);
				
				}

				terrain.terrainData.splatPrototypes = splats.ToArray ();


		}

		public float[,,] generateSplatDataFromCenters (Terrain terrain, GameObject[] centers)
		{
				// create a splat map that contains an x, and y position for each alphamap width, and for each texture we're inputting.		

				float[,,] newsplat = new float[terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight, terrain.terrainData.splatPrototypes.Length];
				
			// create an array for each of these points to each centroid ... ([0,0,2] = 100 is the point 0,0 for the third center = distance of 100)
				float[,,] distance_array = new float[newsplat.GetLength (0), newsplat.GetLength (1), newsplat.GetLength (2)];

				// iterate each point and get the distance to each centroid

				for (int i = 0; i < newsplat.GetLength(0); i++) {
						for (int j = 0; j < newsplat.GetLength(1); j++) {
								for (int k = 0; k < newsplat.GetLength(2); k++) {
										// transform into space of the terrain before calculating the distance
										// possibly the centroids should be children of the terrain
										
										//	Vector3 transformedCenterPoint = terrain.transform.InverseTransformPoint(centers[k].transform.position);
										
										Vector3 transformedCenterPoint = centers [k].transform.localPosition;
					float distance = Vector3.Distance (transformedCenterPoint/(terrain.terrainData.size.x/terrain.terrainData.alphamapWidth), new Vector3 (i, 0, j));
										distance_array [i, j, k] = distance;


								}
						}
				}



				for (int i = 0; i < newsplat.GetLength(0); i++) {
						for (int j = 0; j < newsplat.GetLength(1); j++) {
								
								List<float> curDistances = new List<float> ();
								for (int k = 0; k < newsplat.GetLength(2); k++) {

										// get all distances between current x,y and each centroid
										curDistances.Add (distance_array [i, j, k]);
										

								}

								// get index of the closest center - this is the closest texture
								// could also sort the list and use the values somehow				
								
								int minIndex = curDistances.IndexOf (curDistances.Min ());
								
								// set that x,y value, and texture to 1, but leave others set to zero.
								newsplat [j, i, minIndex] = 1;
									
								
						}
				}

			

				return newsplat;
				
				
		}

		public float[,] heightsFromPixels (Texture2D image, Terrain terrain)
		{
			
				TerrainData terData = terrain.terrainData;
				float[,] rHeight = new float[terData.heightmapWidth, terData.heightmapHeight];

				for (int x = 0; x< terData.heightmapWidth; x++) {
						for (int y=0; y< terData.heightmapHeight; y++) {

								Color color = image.GetPixel (x, y);
								float greycolor = (color.r + color.b + color.g) / 3;
										
								rHeight [y, x] = greycolor;				

								

						}
				}
				return rHeight;

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

								float randomint = (float)gaussint ((x * y), (100));
								Debug.Log (randomint);
								rHeight [y, x] = randomint / 1000;
				
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
