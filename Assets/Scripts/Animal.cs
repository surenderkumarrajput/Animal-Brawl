using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script having control of animals
public class Animal : MonoBehaviour
{
    public AnimalInfo animalInfo;
    public Animator anim;
    public bool destinationReached=false;
    public Vector3 position;
    public Canvas canvas;
    private void Start()
    {
        anim = GetComponent<Animator>();
        position = transform.position;
        canvas.worldCamera = Camera.main;
    }
    private void Update()
    {
        canvas.transform.LookAt(Camera.main.transform);
    }

    public void MovetoBattlePoint(Transform pos)
    {
        if (Vector3.Distance(transform.position, pos.position) > 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos.position, 5 * Time.deltaTime);
            anim.SetFloat("Speed",1);
        }
        else 
        {
            destinationReached = true;
            anim.SetFloat("Speed", 0);
            anim.SetTrigger("Attack");
        }
    }
    public void WinSound()
    {
        FindObjectOfType<AudioManager>().Play(animalInfo.winAudioText);
    }
}
