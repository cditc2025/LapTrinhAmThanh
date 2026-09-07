using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIEvent
{
    public static UIEvent instance = new UIEvent();
    public bool IsPointerOverUIObject()
    {
        //get GameObjects mouse hover
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        //if mouse hover on game object has tag named "NotBlockUI", return false
        //else return false
        if(results.Count == 0 || results[0].gameObject.CompareTag("NotBlockUI"))
        {
            return false;
        } else
        {
            return true;
        }
        
    }
}


