using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GraduationProject
{
	public class UnitMarker {
			
		private Image marker;

		public UnitMarker ()
		{
			GameObject obj = BattleUI.canvas.transform.Find ("unit_enemy").gameObject;
			GameObject o = MonoBehaviour.Instantiate (obj) as GameObject;
			marker = o.GetComponent<Image> ();
			marker.color = new Color (1, 1, 1, 1);
			marker.transform.SetParent (BattleUI.battleMapCanvas.transform);
		}

		public UnitMarker (string file, Vector2 size)
		{
			GameObject obj = BattleUI.canvas.transform.Find ("unit_enemy").gameObject;
			GameObject o = MonoBehaviour.Instantiate (obj) as GameObject;
			marker = o.GetComponent<Image> ();
			marker.sprite = Resources.Load ("UserInterface/"+file, typeof(Sprite)) as Sprite;
			marker.rectTransform.sizeDelta = new Vector2 (size.x*BattleUI.ratio_width, size.y*BattleUI.ratio_height);
			marker.color = new Color (1, 1, 1, 1);
			marker.transform.SetParent (BattleUI.battleMapCanvas.transform);
		}

		public void SetMarkerPosition (Vector3 position)
		{
			marker.rectTransform.position = new Vector3 ((position.x - BattleUI.FIELD_MIN_X)* (BattleUI.mapSize.x/BattleUI.fieldSize.x) + BattleUI.MAP_MIN_X,
			                                             ((position.z - BattleUI.FIELD_MIN_Z)*-1)* (BattleUI.mapSize.y/BattleUI.fieldSize.y) + BattleUI.MAP_MAX_Z,
			                                            0);
		}

		public Image GetMarkerImage ()
		{
			return marker;
		}

		public void Destory ()
		{
			MonoBehaviour.Destroy (marker.gameObject);
		}
	}
}
