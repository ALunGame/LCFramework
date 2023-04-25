using System.Collections.Generic;
using LCToolkit;

namespace LCGAS
{
    [GroupAssetTypeNameAttribute("LCGAS.GEGroupAsset")]
    public class GameplayEffectName : GroupChildAssetName
    {
        private static Dictionary<string, GameplayEffect> modelDict = new Dictionary<string, GameplayEffect>();
        
        public GameplayEffectSpec CreateSpec(AbilitySystemCom pSourceCom, AbilitySystemCom pTargetCom, AbilitySystemCom pOwnerCom)
        {
            GameplayEffect effect = GetModel();
            GameplayEffectSpec effectSpec = null;
            if (effect.type == GameplayEffectType.Instand)
            {
                effectSpec = new GameplayEffectInstandSpec(pSourceCom, pTargetCom, pOwnerCom, effect);
            }
            else if (effect.type == GameplayEffectType.Infinite)
            {
                effectSpec = new GameplayEffectInfiniteSpec(pSourceCom, pTargetCom, pOwnerCom, effect);
            }
            else if (effect.type == GameplayEffectType.HasDuration)
            {
                effectSpec = new GameplayEffectHasDurationSpec(pSourceCom, pTargetCom, pOwnerCom, effect);
            }
            return effectSpec;
        }

        public GameplayEffect GetModel()
        {
            if (modelDict.ContainsKey(Name))
            {
                return modelDict[Name];
            }

            string str = LCLoad.LoadHelper.LoadString(Name);
            GameplayEffect effect = LCJson.JsonMapper.ToObject<GameplayEffect>(str);
            modelDict.Add(Name,effect);
            return effect;
        }
    }
}