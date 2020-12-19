using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleARCore;

//Script responsible for spawnning process management
public class SpawnManager : MonoBehaviour
{
    //Animal info
    public Animal selectedAnimal;
    public Animal[] animals;

    private BattleManager battleManager;

    //Positions
    [HideInInspector]
    public Transform firstPlace;
    [HideInInspector]
    public Transform secondPlace;

    //Spawnned Animals Info
    [HideInInspector]
    public Animal spawnnedFirstAnimal;
    [HideInInspector]
    public Animal spawnnedSecondAnimal;

    public GameObject planeOverlay;

    //Mapping
    private Dictionary<Transform,Animal> posCheck = new Dictionary<Transform, Animal>();
    private List<AugmentedImage> augmentedImages = new List<AugmentedImage>();
    private List<DetectedPlane> allPlanes = new List<DetectedPlane>();
    private Dictionary<string, Animal> mapping = new Dictionary<string, Animal>();

    public GameObject sceneObject;

    //bool for is plane spawnned
    private bool isspawnned=false;

    private void Start()
    {
        battleManager = GetComponent<BattleManager>();
    }
    private void Update()
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            return;
        }
        Touch touch;
        if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
        {
            return;
        }
        else
        {
            #region Plane Detection 
            Session.GetTrackables<DetectedPlane>(allPlanes, TrackableQueryFilter.All);
            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                if ((hit.Trackable is DetectedPlane) &&
                 Vector3.Dot(Camera.main.transform.position - hit.Pose.position,
                     hit.Pose.rotation * Vector3.up) > 0)
                {
                    if (!isspawnned)
                    {
                        FindObjectOfType<AudioManager>().Play("PlaneDetected");
                        var go = Instantiate(sceneObject, hit.Pose.position - new Vector3(0, 12, -20), Quaternion.identity);
                        isspawnned = true;
                        Destroy(planeOverlay);
                        planeOverlay = null;
                        Anchor anchor = Session.CreateAnchor(hit.Pose);
                        go.transform.parent = anchor.transform;
                        firstPlace = go.GetComponent<References>().spawnPoint1;
                        secondPlace = go.GetComponent<References>().spawnPoint2;
                        battleManager.movePoint = go.GetComponent<References>().battlePoint;
                    }
                }
            }
            #endregion
            //if plane spawnned then..
            if (isspawnned)
            {
                Session.GetTrackables<AugmentedImage>(augmentedImages);
                #region Augmented Image Detection
                foreach (AugmentedImage img in augmentedImages)
                {
                    if (!mapping.ContainsKey(img.Name))
                    {
                        foreach (Animal anim in animals)
                        {
                            if (img.Name == anim.animalInfo.name)
                            {
                                selectedAnimal = anim;
                                Animal temp = GetAnimal(selectedAnimal);
                                if (temp != null)
                                {
                                    mapping.Add(img.Name, temp);
                                }
                            }
                            else if (img.Name == "Battle")
                            {
                                battleManager.Battle();
                                mapping.Add(img.Name, null);
                            }
                            else if (img.Name == "Reset")
                            {
                                StartCoroutine(ResetGame());
                                mapping.Add(img.Name, null);
                            }
                        }
                    }
                    #endregion
                }
            }
            // Should not handle input if the player is pointing on UI.
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }
        }

    }
  
    //Function for spawnning animals
    public Animal GetAnimal(Animal animal)
    {
        foreach (Animal anim in animals)
        {
            if(anim==animal)
            {
                if(!posCheck.ContainsKey(firstPlace) && !posCheck.ContainsValue(animal))
                {
                    Animal temp=Instantiate(anim, firstPlace);
                    spawnnedFirstAnimal = temp;
                    posCheck.Add(firstPlace,spawnnedFirstAnimal);
                    FindObjectOfType<AudioManager>().Play(spawnnedFirstAnimal.animalInfo.spawnSound);
                    return spawnnedFirstAnimal;
                }
                else if(!posCheck.ContainsKey(secondPlace) && !posCheck.ContainsValue(animal))
                {
                    Animal temp =Instantiate(anim, secondPlace);
                    spawnnedSecondAnimal = temp;
                    posCheck.Add(secondPlace, spawnnedSecondAnimal);
                    FindObjectOfType<AudioManager>().Play(spawnnedSecondAnimal.animalInfo.spawnSound);
                    return spawnnedSecondAnimal;
                }
            }
        }
        return null;
    }

    //Function for reset animals
    public IEnumerator ResetGame()
    {
        FindObjectOfType<AudioManager>().Play("Reset");
        posCheck.Clear();
        mapping.Clear();
        augmentedImages.Clear();
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Game");
    }
}
