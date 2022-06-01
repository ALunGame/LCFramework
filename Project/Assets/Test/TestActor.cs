using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestActor : MonoBehaviour
{
    public Transform displayTrans;
    public Vector3 strength = new Vector3(0.1f,0,0);
    public int vibrato = 1;
    public float randomness = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shake();
        }
    }

    private void Shake()
    {
        displayTrans.DOComplete(false);
        displayTrans.DOPunchPosition(strength, 0.3f, vibrato,0);
        //displayTrans.DOShakePosition(0.3f, strength, vibrato, randomness);
    }
}
