using UnityEngine;
using System.Collections;

public class Centroid : MonoBehaviour
{
		public Vector3 prevpos;
		public Vector3 pos;
		
		
		
		public Texture2D texold;
		public Texture2D texture;//{ get; set;}
		
		public delegate void  dirty (GameObject movedObj);

		public static event dirty ondirty;
		// Use this for initialization
		void onEnable ()
		{
				ondirty (this.gameObject);
				prevpos = transform.localPosition;		
				texold = texture;

		}
		// Update is called once per frame
		void Update ()
		{
				// I should move the method that generates the default texture 
				// to here and have it generate if texold = null.
				if (texold == null) {
						texold = CentroidController.initialTex (256, 256);
						texture = texold;
				}

				if (texold.Equals (texture) == false) {

						Debug.Log("texture updated somehow");

				}
				
				pos = transform.localPosition;
				var dist = Vector3.Distance (pos, prevpos);
				if ((dist > 25) || (texold.Equals(texture ) == false)) {

						ondirty (this.gameObject);
						prevpos = transform.localPosition;
						texold = texture;
				}

		}


		


}
