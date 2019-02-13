//------------------------------------------------------------------------------
// <auto-generated>
//     このコードはツールによって生成されました。
//     ランタイム バージョン:4.0.30319.34209
//
//     このファイルへの変更は、以下の状況下で不正な動作の原因になったり、
//     コードが再生成されるときに損失したりします。
// </auto-generated>
//------------------------------------------------------------------------------
using UnityEngine;
using UnityEngine.UI;
using System;


namespace GraduationProject
{
	public class MoveImage : BaseProduction
	{
		private Image image;

		private Vector2 targetPos;
		private Vector2 currentPos;

		private float speed = 3.0f;
		public MoveImage ()
		{

		}

		public override void Execute (ProductionInfo information, IProductionObserver observer)
		{
			counter -= speed;
			if (counter < 0) counter = 0;
			speed -= 0.1f;

			speed = Mathf.Clamp (speed, 0.1f, 3f);

			float next_x = Mathf.Lerp (currentPos.x, targetPos.x, 1f - ((float)counter / (float)time));
			float next_y = Mathf.Lerp (currentPos.y, targetPos.y, 1f - ((float)counter / (float)time));
			
			image.rectTransform.position = new Vector3 (next_x, next_y, 0f);

			if (counter <= 0)
			{
				observer.NextCommand ();
			}
		}
		
		public override void CreateProduction (Canvas canvas)
		{
			Transform child = canvas.gameObject.transform.Find ((string)Json["filePath"]);
			if (child != null)
			{
				image = child.GetComponent<Image> ();

				currentPos = new Vector2 (image.rectTransform.position.x, image.rectTransform.position.y);
				targetPos = currentPos + new Vector2 ((float)((int)Json ["tx"])*ratio_width, (float)((int)Json ["ty"])*ratio_height);
			}
			else
			{
				GameObject imageObject = new GameObject ();
				imageObject.name = "image";
				imageObject.transform.parent = canvas.transform;
				image = imageObject.AddComponent<Image> ();
				image.sprite = Resources.Load ((string)Json["filePath"], typeof(Sprite)) as Sprite;

				image.rectTransform.anchorMax = Vector2.zero;
				image.rectTransform.anchorMin = Vector2.zero;
				image.rectTransform.pivot = Vector2.zero;

				currentPos = new Vector2 ((float)((int)Json ["x"])*ratio_width, (float)((int)Json ["y"])*ratio_height);
				targetPos = new Vector2 ((float)((int)Json ["tx"])*ratio_width, (float)((int)Json ["ty"])*ratio_height);
				image.rectTransform.position = new Vector3 (currentPos.x, currentPos.y, 0f);
				image.rectTransform.sizeDelta = new Vector2 ((float)((int)Json["width"])*ratio_width, (float)((int)Json["height"])*ratio_height);
			}

			time = (int)Json["time"];
			counter = time;
		}
	}
}
