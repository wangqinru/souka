//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.34209
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using LitJson;
using UnityEngine;
using UnityEngine.UI;

namespace GraduationProject
{
	public class ImageList
	{
		private Image[] imageList;
		private Image[] backGround;
		public string[] textData {get; set;}
		public JsonData Json{get; set;}

		protected string defaultPath = "UserInterface/";

		protected float ratio_width = Screen.width / 1920f;
		protected float ratio_height = Screen.height / 1080f;

		public ImageList ()
		{
		}

		public void CreateImageList (Canvas canvas)
		{
			string[] imagePath = JsonMapper.ToObject<string[]> (Json["filePath"].ToJson ());
			imageList = new Image[imagePath.Length];
			backGround = new Image[imagePath.Length];

			float[] startPosition = JsonMapper.ToObject<float[]> (Json["startposition"].ToJson ());
			float[] size = JsonMapper.ToObject<float[]> (Json["size"].ToJson ());
			float[] size_min = JsonMapper.ToObject<float[]> (Json["size_min"].ToJson ());
			float[] Interval = JsonMapper.ToObject<float[]> (Json["Interval"].ToJson ());
			int width_max = (int)Json["width_number"];

			for (int i=0; i<imagePath.Length; i++)
			{
				GameObject buttonObject = new GameObject ();
				buttonObject.name = "background00"+i.ToString ();
				buttonObject.transform.parent = canvas.transform;
				
				backGround[i] = buttonObject.AddComponent <Image> ();
				backGround[i].sprite = Resources.Load (defaultPath+(string)Json["background"], typeof(Sprite)) as Sprite;
				backGround[i].rectTransform.anchorMax = Vector2.zero;
				backGround[i].rectTransform.anchorMin = Vector2.zero;
				backGround[i].rectTransform.pivot = Vector2.one/2f;
				
				backGround[i].rectTransform.position = new Vector3 ((startPosition[0]+(i%width_max)*Interval[0])*ratio_width, 
				                                                    (startPosition[1]+(i/width_max)*Interval[1])*ratio_width, 
				                                                    0f);
				backGround[i].rectTransform.sizeDelta = new Vector2 (size[0]*ratio_width, size[1]*ratio_height);

				buttonObject = new GameObject ();
				buttonObject.name = "image00"+i.ToString ();
				buttonObject.transform.parent = canvas.transform;

				imageList[i] = buttonObject.AddComponent <Image> ();
				imageList[i].sprite = Resources.Load (defaultPath+"Gallery/"+imagePath[i], typeof(Sprite)) as Sprite;

				imageList[i].rectTransform.anchorMax = Vector2.zero;
				imageList[i].rectTransform.anchorMin = Vector2.zero;
				imageList[i].rectTransform.pivot = Vector2.one/2f;

				imageList[i].rectTransform.position = backGround[i].rectTransform.position;
				imageList[i].rectTransform.sizeDelta = new Vector2 ((size[0]+size_min[0])*ratio_width, (size[1]+size_min[1])*ratio_height);
			}

			textData = JsonMapper.ToObject<string[]> (Json["remark"].ToJson ());
		}

		public Vector3 ChangeSelectImage (int index)
		{
			Vector3 position = imageList[0].rectTransform.position;
			for (int i=0; i<imageList.Length; i++)
			{
				if (index == i)
				{
					imageList[i].color = new Color (1, 1, 1, 1);
					position = imageList[i].rectTransform.position;
				}
				else
					imageList[i].color = new Color (1, 1, 1, 0.5f);
			}

			return position;
		}

		public Sprite GetCurrentImage (int index)
		{
			return imageList[index].sprite;
		}
	}
}

