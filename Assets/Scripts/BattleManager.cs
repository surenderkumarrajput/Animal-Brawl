using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Battle states
public enum BattleStatus
{
    IDLE,RUNNING,FIGHTING,FightOVER
}
//Script responsible for battles
public class BattleManager : MonoBehaviour
{
    private SpawnManager spawnManager;
    [HideInInspector]
    public Transform movePoint;
    public GameObject explosion;
    public BattleStatus battleStatus;
    private Animal winnerAnimal;
    void Start()
    {
        spawnManager = GetComponent<SpawnManager>();
        battleStatus = BattleStatus.IDLE;
    }

    void Update()
    {
        if (spawnManager.spawnnedFirstAnimal != null && spawnManager.spawnnedSecondAnimal != null)
        {
            if (battleStatus == BattleStatus.IDLE)
            {
                return;
            }
            else if (battleStatus == BattleStatus.RUNNING)
            {
                spawnManager.spawnnedFirstAnimal.MovetoBattlePoint(movePoint);
                spawnManager.spawnnedSecondAnimal.MovetoBattlePoint(movePoint);
                if (spawnManager.spawnnedFirstAnimal.destinationReached && spawnManager.spawnnedSecondAnimal.destinationReached)
                {
                    battleStatus = BattleStatus.FIGHTING;
                }
            }
            else if (battleStatus == BattleStatus.FIGHTING)
            {
                StartCoroutine(Fight());
            }
        }
    }
    //Function for battle
    public void Battle()
    {
        FindObjectOfType<AudioManager>().Play("GO");
        battleStatus = BattleStatus.RUNNING;
    }
    //Function which handles the fight
    public IEnumerator Fight()
    {
       
        if (spawnManager.spawnnedFirstAnimal.animalInfo.power > spawnManager.spawnnedSecondAnimal.animalInfo.power)
        {
            Destroy(spawnManager.spawnnedSecondAnimal.gameObject);
            winnerAnimal = spawnManager.spawnnedFirstAnimal;
            Instantiate(explosion, movePoint.position+new Vector3(0,1,0), Quaternion.identity);
        }
        else 
        {
            Destroy(spawnManager.spawnnedFirstAnimal.gameObject);
            winnerAnimal = spawnManager.spawnnedSecondAnimal;
            Instantiate(explosion, movePoint.position+new Vector3(0,1,0), Quaternion.identity);
        }
        FindObjectOfType<AudioManager>().Play("Blast");
        yield return new WaitForSeconds(2);
        winnerAnimal.anim.SetTrigger("Emote");
        yield return new WaitForSeconds(3f);
        FindObjectOfType<AudioManager>().Play(winnerAnimal.animalInfo.winSpeakerAudio);
        yield return new WaitForSeconds(2f);
        battleStatus = BattleStatus.FightOVER;
    }
    
}
