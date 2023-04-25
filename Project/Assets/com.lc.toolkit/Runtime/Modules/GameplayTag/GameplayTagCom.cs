using UnityEngine;

namespace LCToolkit
{
    public class GameplayTagCom : MonoBehaviour
    {
        [HideInInspector]
        [SerializeField]
        public GameplayTagContainer tags = new GameplayTagContainer();
    }
}