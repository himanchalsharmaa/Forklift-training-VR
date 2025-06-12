using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandlUserInstructions : MonoBehaviour
{
    [SerializeField]
    private GameObject GlobalInfoCanvas;
    [SerializeField]
    [Tooltip("How long should one message be displayed")]
    uint PerMessageDuration = 5;

    private TMP_Text InfoCanvasText;

    private static Queue<GlobalMessage> MessageQueue = new();
    private static bool PreviousMessageDisplaying = false;

    public static Action<GlobalMessage> ShowGlobalMessage;

    private string PreviousMessage = "";
    private void Awake()
    {
        ShowGlobalMessage += GlobalUIMessage;

        InfoCanvasText = GlobalInfoCanvas.GetComponentInChildren<TMP_Text>(true);
        GlobalInfoCanvas.SetActive(false);

        StartCoroutine(ClearLastMessage());
    }
    private void GlobalUIMessage(GlobalMessage _GlobalMessage)
    {
        if (!PreviousMessageDisplaying)
            StartCoroutine(MessageQueueDelay(_GlobalMessage));
        else
            MessageQueue.Enqueue(_GlobalMessage);
    }
    private IEnumerator ClearLastMessage()
    {
        WaitForSeconds _WFS = new(2.5f);
        while (true)
        {
            yield return _WFS;
            PreviousMessage = "";
        }
    }
    private IEnumerator MessageQueueDelay(GlobalMessage _GlobalMessage)
    {
        if (!PreviousMessage.Equals(_GlobalMessage.Message))
        {
            PreviousMessage = _GlobalMessage.Message;
            InfoCanvasText.text = _GlobalMessage.Message;
            if (_GlobalMessage.MessageType.Equals(GlobalMessageType.EngagementFailure))
                InfoCanvasText.color = Color.red;
            else
                InfoCanvasText.color = Color.white;

            GlobalInfoCanvas.SetActive(true);
            PreviousMessageDisplaying = true;

            yield return new WaitForSeconds(PerMessageDuration);

            PreviousMessageDisplaying = false;
        }
        if (MessageQueue.Count > 0)
            GlobalUIMessage(MessageQueue.Dequeue());
        else
            GlobalInfoCanvas.SetActive(false);
    }
    private void OnDestroy()
    {
        ShowGlobalMessage -= GlobalUIMessage;
    }
}
public class GlobalMessage
{
    public string Message;
    public GlobalMessageType MessageType;

    public GlobalMessage(string _Message, GlobalMessageType _MessageType)
    {
        Message = _Message;
        MessageType = _MessageType;
    }
    public GlobalMessage(string _Message)
    {
        Message = _Message;
        MessageType = GlobalMessageType.Instruction;
    }
}
public enum GlobalMessageType
{
    Instruction,
    EngagementFailure
}