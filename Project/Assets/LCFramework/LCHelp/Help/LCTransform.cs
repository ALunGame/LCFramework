using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LCHelp
{
    /// <summary>
    /// 坐标转换
    /// </summary>
    public class LCTransform
    {
        //默认左下角，或者中间
        public static Vector2 WorldPointToUI(Vector3 worldPoint,RectTransform canvas,bool anchorLeft=true,Camera camera=null)
        {
            if (camera==null)
                camera = Camera.main;
            //世界坐标-》ViewPort坐标   
            Vector2 viewPos = camera.WorldToViewportPoint(worldPoint);

            Vector2 resPos;
            //ViewPort坐标-〉UGUI坐标
            if (anchorLeft)
            {
                resPos = new Vector2(canvas.rect.width * viewPos.x, canvas.rect.height * viewPos.y);
            }
            else
            {
                resPos = new Vector2(canvas.rect.width * viewPos.x - canvas.rect.width * 0.5f, canvas.rect.height * viewPos.y - canvas.rect.height * 0.5f);
            }
            return resPos;
        }
    }
}
