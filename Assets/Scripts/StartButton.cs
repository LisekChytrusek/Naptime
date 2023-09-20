using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Button>().interactable = false;
    }
}
