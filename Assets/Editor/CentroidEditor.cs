using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(Centroid))]
public class CentroidEditor : Editor {

	private Texture2D _texture;

	public override void OnInspectorGUI ()
	{
		//base.OnInspectorGUI ();

				var centroid = (Centroid)target;
				_texture = centroid.texture;
				_texture = AssetPreview.GetAssetPreview (_texture);
				GUILayout.Button (_texture);
				
				
				_texture = EditorGUILayout.ObjectField(_texture,typeof(Texture2D),false)as Texture2D;
				if (GUI.changed) {

						centroid.texture = _texture;
						Repaint ();
						
				}
				base.OnInspectorGUI ();

	}

}

