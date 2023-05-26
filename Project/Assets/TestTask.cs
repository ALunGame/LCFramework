using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine.UI;

public class TestTask : MonoBehaviour
{
    [SerializeField]
    private Button startBtn;
    [SerializeField]
    private Button cancleBtn;
    [SerializeField]
    private TextMeshProUGUI text;

    private UniTask<string> _task;
    private CancellationTokenSource _cancellationToken;
    

    private void Awake()
    {
        _cancellationToken = new CancellationTokenSource();
        //_cancellationToken.CancelAfterSlim(TimeSpan.FromSeconds(10));

        startBtn.onClick.AddListener(UniTask.UnityAction(StartTask));
        cancleBtn.onClick.AddListener(CancleTask);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private async UniTaskVoid StartTask()
    {
        CancleTask();

        Debug.Log("StartTask：");

        _task = UniTask.Defer(DemoAsync);
        _task.ToCancellationToken(_cancellationToken.Token);
        var (isCanceled, _) = await _task.SuppressCancellationThrow();
        
        if (isCanceled)
        {
            Debug.Log("取消线程："+ Thread.CurrentThread.Name + " " + Thread.CurrentThread.ManagedThreadId);
        }
    }

    private void CancleTask()
    {
        _cancellationToken.Cancel();
        _cancellationToken.Dispose();
        _cancellationToken = new CancellationTokenSource();
        //_cancellationToken.CancelAfterSlim(TimeSpan.FromSeconds(10));
    }

    private async UniTask<string> DemoAsync()
    {
        await UniTask.Yield(PlayerLoopTiming.Update);

        await UniTask.DelayFrame(100);

        await UniTask.Delay(2*1000);

        await UniTask.RunOnThreadPool(() =>
        {
            Debug.Log("线程："+ Thread.CurrentThread.Name + " " + Thread.CurrentThread.ManagedThreadId);

        });


        
        await UniTask.Yield(PlayerLoopTiming.Update);
        
        Debug.Log("主线程："+ Thread.CurrentThread.Name + " " + Thread.CurrentThread.ManagedThreadId);
        text.text = "aaaaaaaaaaaaaaaaaaaaa";
        return "aaaa";
    }
}
