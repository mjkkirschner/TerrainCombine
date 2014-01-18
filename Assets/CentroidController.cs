using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class CentroidController : MonoBehaviour
{
	public Terrain terrain;
	public TerrainController tercontroller;	
	public List<GameObject> centers = new List<GameObject>();
	// Use this for initialization


	void Start ()
	{


		var point1 = genNewCenter_andAssignToTerrain (terrain,centers);
		var point2 = genNewCenter_andAssignToTerrain (terrain,centers);	

	// generate some random test points with the gen point method
	// keep track of these centers
	
	// after the centers are generated call the terrain controller or send an event to regen everything
	// on the terrain based on the centroids
	// centroids should also send a regen when they are first created, or when texture changes.
	// I need to grab a list of textures from all the centers somewhere and pass them
	// to the terraincontroller.... should each center handle this?


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

	public Texture2D initialTex (int x, int y){
				
		var tex = new Texture2D(x,y);
		for (int xx = 0; xx < tex.width;xx++){
			for (int yy = 0; yy < tex.height;yy++){

								tex.SetPixel (xx, yy, Color.grey);


			}	

		}

				return tex;	

	}

	public GameObject genNewCenter_andAssignToTerrain(Terrain terrain, List<GameObject> centers){

		var width = terrain.terrainData.size.x;
				var vertheight = terrain.terrainData.size.y;

		GameObject centerPoint = GameObject.CreatePrimitive (PrimitiveType.Sphere);

		centerPoint.transform.parent = terrain.transform;
		centerPoint.transform.localScale = new Vector3(width/25,width/25,width/25);
				centerPoint.transform.localPosition = new Vector3 (UnityEngine.Random.Range (0, width), vertheight, UnityEngine.Random.Range (0, width));
		centerPoint.AddComponent<Centroid> ();

				var defualttex = initialTex ((int)width, (int)width);
		centerPoint.GetComponent<Centroid> ().texture = defualttex;

				centers.Add (centerPoint);
				return centerPoint;
		}
	 

	// Update is called once per frame
	void Update ()
	{





	}
}
