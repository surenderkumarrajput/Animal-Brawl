using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for managing UI Animations
public class LeanTweenUI : MonoBehaviour
{
    public float duration;
    public Vector3 scale;

    public void UIDisable(GameObject gameObject)
    {
        LeanTween.scale(gameObject, Vector3.zero,duration).setOnComplete(disableMe);
    }
    private void disableMe()
    {
        gameObject.SetActive(false);
    }  
    public void enableMe()
    {
        gameObject.transform.localScale = scale;
        gameObject.SetActive(true);
    } 
}
