using System.Collections;
using UnityEngine;

namespace Demo.Com.MainActor
{
    public class MainActorMoveHelper : MonoBehaviour
    {
        public void BeginCoroutine(IEnumerator pRoutine)
        {
            StartCoroutine(pRoutine);
        }

        public void EndCoroutine(IEnumerator pRoutine)
        {
            StopCoroutine(pRoutine);
        }

        public void EndAllCoroutine()
        {
            StopAllCoroutines();
        }
    }
}