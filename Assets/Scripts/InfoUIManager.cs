using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

//Class responsible for ui management
public class InfoUIManager : MonoBehaviour
{
    private Canvas canvas;
    private VideoPlayer videoPlayer;
    private BattleManager battleManager;
    private LeanTweenUI leanTweenUI;
    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        battleManager = GetComponent<BattleManager>();
    }
    void Update()
    {
        if(battleManager.battleStatus==BattleStatus.RUNNING||battleManager.battleStatus==BattleStatus.FIGHTING)
        {
            return;
        }
        else
        {
            Raycast();
        }
    }
    //Raycasting for animals,if it hits animal it will popup panel and show info.
    public void Raycast()
    {
        Ray ray = new Ray(Camera.main.transform.position,Camera.main.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.GetComponent<Animal>() != null)
            {
                Animal animal = hit.collider.gameObject.GetComponent<Animal>();
                canvas = animal.canvas;
                videoPlayer.clip = animal.animalInfo.videoClip;
                leanTweenUI = canvas.GetComponent<LeanTweenUI>();
                leanTweenUI.enableMe();
                canvas.GetComponent<CanvasRef>().nameText.text = animal.animalInfo.animalName;
                canvas.GetComponent<CanvasRef>().height.text = "HEIGHT= "+animal.animalInfo.height;
                canvas.GetComponent<CanvasRef>().weight.text = "WEIGHT= " + animal.animalInfo.weight;
                canvas.GetComponent<CanvasRef>().foundIn.text = "FOUND IN= " + animal.animalInfo.mostlyFoundArea;
                canvas.GetComponent<CanvasRef>().speed.text = "SPEED= " + animal.animalInfo.speed;
                canvas.GetComponent<CanvasRef>().fact.text =  animal.animalInfo.fact;
                canvas.GetComponent<CanvasRef>().playButton.onClick.AddListener(() => PlayVideo(canvas.GetComponent<CanvasRef>().playButton));
            }
            else
            {
                if (canvas != null)
                {
                    leanTweenUI.UIDisable(canvas.gameObject);
                    StopPlay();
                }
            }
        }
        else
        {
            if (canvas != null)
            {
                leanTweenUI.UIDisable(canvas.gameObject);
                StopPlay();
            }
        }
    }
    //Function for playing video 
    public void PlayVideo(Button button)
    {
       videoPlayer.Play();
       button.gameObject.SetActive(false);
    }
    //Function for stop playing video 
    public void StopPlay()
    {
        videoPlayer.Stop();
        videoPlayer.clip = null;
        videoPlayer.targetTexture.Release();
        canvas.GetComponent<CanvasRef>().playButton.gameObject.SetActive(true);
    }
}
