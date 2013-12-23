using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// player 被 enemy发现时，会发出警报声，灯光颜色也会发生变化
/// </summary>
public class DoneLastPlayerSighting : MonoBehaviour
{
    // 垂直方向的 光强度（报警器关闭时）
    public Single lightHighIntensity = 0.25f;
    // 垂直方向的 光强度（报警器打开时）
    public Single lightLowIntensity = 0f;
    // player 最后一次被看到的位置
    public Vector3 position = new Vector3(1000f, 1000f, 1000f);
    // player 默认被看到的位置
    public Vector3 resetPosition = new Vector3(1000f, 1000f, 1000f);
    // light intensity 渐变的速度
    public Single fadeSpeed = 7f;
    // music volumn 渐变的速度
    public Single musicFadeSpeed = 1f;

    // 警报声的声源
    private AudioSource panicAudio;
    private AudioSource[] Sirens;
    // 场景中的 MainLight
    private Light mainLight;

    void Awake()
    {
        mainLight = GameObject.FindGameObjectWithTag(DoneTags.mainLight).GetComponent<Light>();
        panicAudio = transform.FindChild("secondaryMusic").GetComponent<AudioSource>();
        GameObject[] sirenGameObjects = GameObject.FindGameObjectsWithTag(DoneTags.siren);

        // 将每个 Siren中的 audio填充到 Sirens数组
        Sirens = new AudioSource[sirenGameObjects.Length];
        for (int i = 0; i < Sirens.Length; i++)
        {
            Sirens[i] = sirenGameObjects[i].audio;
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SwitchAlarm();
        MusicFading();
    }

    // 控制 Alarm 开关
    void SwitchAlarm()
    {
        // 控制 alarmlight的 播放
        // 只要 player 进入 Enemy视野范围内  position 就会被跟新



        // 控制 light intensity
        Single newIntensity;
        if (position != resetPosition)
        {
            newIntensity = lightLowIntensity;
        }
        else
        {
            newIntensity = lightHighIntensity;
        }

        mainLight.intensity = Mathf.Lerp(mainLight.intensity, newIntensity, fadeSpeed * Time.deltaTime);

        // 依次播放 Sirens中的 audio
        for (int i = 0; i < Sirens.Length; i++)
        {
            if (!Sirens[i].isPlaying && position == resetPosition)
            {
                Sirens[i].Play();
            }
            else if (position == resetPosition)
            {
                Sirens[i].Stop();
            }
        }
    }

    void MusicFading()
    {
        // position 会在 DoneEnemyAI中 被重置成 resetPosition
        if (position != resetPosition)
        {
            audio.volume = Mathf.Lerp(audio.volume, 0f, musicFadeSpeed*Time.deltaTime);
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0.8f, musicFadeSpeed*Time.deltaTime);
        }
        else
        {
            audio.volume = Mathf.Lerp(audio.volume,0.8f,musicFadeSpeed*Time.deltaTime);
            panicAudio.volume = Mathf.Lerp(panicAudio.volume, 0f , musicFadeSpeed*Time.deltaTime);
        }
    }
}
