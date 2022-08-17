using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayPersistentDataPath : MonoBehaviour
{
    TMP_Text text;
    void Awake()
    {
        text = GetComponent<TMP_Text>();
        text.text = Application.persistentDataPath;
    }
}
