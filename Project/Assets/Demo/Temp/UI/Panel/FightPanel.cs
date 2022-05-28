using Demo.Com;
using LCECS.Core;
using System.Collections.Generic;
using UnityEngine.UI;

public class FightPanel : LCUI.LCUIPanel
{
    public Slider HpSlider;
    public Slider EnergySlider;

    public Dictionary<string, float> PlayerAttrConfDict = new Dictionary<string, float>();

    private Entity PlayerEntity = null;

    public override void OnAwake()
    {
        
    }

    private void Init()
    {
        if (PlayerEntity!=null)
        {
            return;
        }
    }

    public override void OnShow(params object[] parms)
    {

    }

    public override void OnHide()
    {

    }

    private void Update()
    {
        Init();
    }

}
