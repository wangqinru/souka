using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace GraduationProject
{
	public class Loading : MonoBehaviour {

		private Image loadingImage;

		void Start() {

			GameObject obj = new GameObject ();
			obj.name = "Canvas";
			obj.AddComponent<Canvas> ().renderMode = RenderMode.ScreenSpaceOverlay;

			GameObject imageObject = new GameObject ();
			imageObject.name = "Image";
			imageObject.transform.parent = obj.transform;
			
			loadingImage = imageObject.AddComponent<Image> ();
			loadingImage.sprite = Resources.Load ("UserInterface/loading", typeof(Sprite)) as Sprite;
			
			loadingImage.rectTransform.anchorMax = Vector2.zero;
			loadingImage.rectTransform.anchorMin = Vector2.zero;
			loadingImage.rectTransform.pivot = Vector2.zero;
			loadingImage.rectTransform.position = new Vector3 (1470*BattleUI.ratio_width, 0f, 0f);
			loadingImage.rectTransform.sizeDelta = new Vector2 (450f*BattleUI.ratio_width, 140f*BattleUI.ratio_height);

			LoadAllTextures ();
		}
		
		// Update is called once per frame
		void Update () {
		
			float alpha = Mathf.PingPong (Time.time, 1.6f)/2f + 0.2f;
			loadingImage.color = new Color (1, 1, 1, alpha);
		}

		void LoadAllTextures ()
		{
			StartCoroutine (InstanceObjects ());
		}

		IEnumerator InstanceObjects ()
		{
			Resources.LoadAll ("UserInterface");
			Resources.LoadAll ("UserInterface/Gallery");
			Resources.LoadAll ("Prefabs");

			/*foreach(var obj in sprites)
			{
				var item = Instantiate(obj) as GameObject;
				yield return null;
			}*/

			AsyncOperation async = Application.LoadLevelAsync("TitleScene");
			yield return async;
			
			Resources.UnloadUnusedAssets ();
		}
	}
}