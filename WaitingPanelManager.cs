using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaitingPanelManager : MonoBehaviour
{
    public void SetText(string text)
    {
        GetComponentInChildren<TMP_Text>().text = text;
    }
}
