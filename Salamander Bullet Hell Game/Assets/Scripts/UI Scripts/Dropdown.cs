using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class Dropdown : MonoBehaviour
{
    [SerializeField] List<GameObject> objects = new List<GameObject>();
    Button dropDownbutton;
    private bool dropped;
    private void Awake()
    {
        dropDownbutton = GetComponent<Button>();
    }
    private void OnEnable() => dropDownbutton.onClick.AddListener(ToggleDropdown);
    private void OnDisable() => dropDownbutton.onClick.RemoveListener(ToggleDropdown);

    private void ToggleDropdown()
    {
        if(!dropped)
        {
            foreach(GameObject obj in objects)
            {
                obj.SetActive(true);
            }
            dropped = true;
        }
        else
        {
            foreach(GameObject obj in objects)
            {
                obj.SetActive(false);
            }
            dropped = false;
        }
    }
}
