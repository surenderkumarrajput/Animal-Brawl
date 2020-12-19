using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

//Script having info of animals
[CreateAssetMenu(menuName ="Animal",fileName ="Animal/New Animal")]
public class AnimalInfo : ScriptableObject
{
    public string animalName;
    public float power;
    public VideoClip videoClip;
    public string weight;
    public string height;
    public string mostlyFoundArea;
    public string speed;
    [TextArea]
    public string fact;
    public string winAudioText;
    public string winSpeakerAudio;
    public string spawnSound;
}
