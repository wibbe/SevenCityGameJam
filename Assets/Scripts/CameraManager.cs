using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public float time = 0.5f;
    public CinemachineVirtualCamera virtualCamera = null;

    public void Shake(float amount)
    {
        StartCoroutine(ShakeEffect(amount));
    }

    private IEnumerator ShakeEffect(float amount)
    {
        CinemachineBasicMultiChannelPerlin noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        /*
        cmFreeCam.topRig.Noise.m_AmplitudeGain = amplitudeGain;
        cmFreeCam.middleRig.Noise.m_AmplitudeGain = amplitudeGain;
        cmFreeCam.bottomRig.Noise.m_AmplitudeGain = amplitudeGain;

        cmFreeCam.topRig.Noise.m_FrequencyGain = frequencyGain;
        cmFreeCam.middleRig.Noise.m_FrequencyGain = frequencyGain;
        cmFreeCam.bottomRig.Noise.m_FrequencyGain = frequencyGain;
        */

        noise.m_AmplitudeGain = amount;
        noise.m_FrequencyGain = amount;
        yield return new WaitForSeconds(time * amount);
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
