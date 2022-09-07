using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Linq;

public class PlayerEffects : MonoBehaviour
{
    public static PlayerEffects Instance;

    [SerializeField] private List<MeshRenderer> _playerMeshes;
    [Header("Camera shake")]
    [SerializeField] private CinemachineVirtualCamera _virtualCam;
    [SerializeField] private float _shakeTime = 0.1f, _shakeAmplitude = 1f, _shakeFrequency = 1f;
    [Header("Player inviciblity time")]
    [SerializeField] private Color _DamageColor = Color.black, _NormalColor = Color.white;
    void Awake()
    {
         if (Instance != null && Instance != this) 
                Destroy(this); 
            else 
                Instance = this; 
    }
    public void StartCameraShake()
    {
        StopCoroutine(CameraShake());
        StartCoroutine(CameraShake());
    }
    IEnumerator CameraShake()
    { 
        _virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = _shakeAmplitude;
        _virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = _shakeFrequency;
        yield return new WaitForSeconds(_shakeTime);
        _virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0f;
        _virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_FrequencyGain = 0f;
        yield return null; 
    }

    public void Invicibility(float timepcnt)
    {
        _playerMeshes.ForEach(x => x.material.color = Color.Lerp(_DamageColor, _NormalColor, timepcnt));
    }
}
