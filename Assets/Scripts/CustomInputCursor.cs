using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class CustomInputCursor : MonoBehaviour
{
    public float caretSize = 12;
    private TMP_InputField inputField;
    private void Awake()
    {
        PlayerPrefs.DeleteKey("LootLockerGuestPlayerID");
        inputField = GetComponent<TMP_InputField>();
        inputField.caretWidth = 12;
    }
}
