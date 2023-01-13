using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LCUI;
using UnityEngine.UI;
using UnityEngine;

namespace Demo.UI
{
    public class FightUIPanelModel : UIModel
    {

    }
    
    public class FightUIPanel : UIPanel<FightUIPanelModel>
    {
        //左移动按钮
        private UIComGlue<RectTransform> leftBtn = new UIComGlue<RectTransform>("");

        //右移动按钮
        private UIComGlue<RectTransform> rightBtn = new UIComGlue<RectTransform>("");
    }
}
