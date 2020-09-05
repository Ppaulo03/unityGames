using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSettings : MonoBehaviour
{   
    [SerializeField] private GameObject[] disables = null;
    [SerializeField] private GameObject[] enables = null;

    [SerializeField] private Texture2D cursorTexturePause = null;
    [SerializeField] private Texture2D cursorTexturePlay = null;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    private Vector2 hotSpot = Vector2.zero;

    private void OnDisable()
    {
        foreach( GameObject i in disables) i.SetActive(false);
        foreach( GameObject i in enables) i.SetActive(true);
        hotSpot = new Vector2(cursorTexturePlay.width / 2 , cursorTexturePlay.height / 2 );
        Cursor.SetCursor(cursorTexturePlay, hotSpot, cursorMode);
    }
    private void OnEnable()
    {
        hotSpot = Vector2.zero;
        Cursor.SetCursor(cursorTexturePause, hotSpot, cursorMode);
    }
}
