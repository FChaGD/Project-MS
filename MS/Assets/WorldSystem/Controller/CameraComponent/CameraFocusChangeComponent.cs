using SystemDefinition;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraFocusChangeComponent : MonoBehaviour
{
    //카메라 컨트롤러 공용 정보 컨테이너
    private CameraSettings _cameraSettings;

    //카메라 컴포넌트 변수
    private Camera worldCamera;
    private AudioListener worldListener;
    private Camera battleCamera;
    private AudioListener battleListener;

    void Start()
    {
        
    }

    public void Init(CameraSettings cameraSettings)
    {
        _cameraSettings = cameraSettings;
        worldCamera = _cameraSettings.worldCamera.GetComponent<Camera>();
        worldListener = _cameraSettings.worldCamera.GetComponent<AudioListener>();
        battleCamera = _cameraSettings.battleCamera.GetComponent<Camera>();
        battleListener = _cameraSettings.battleCamera.GetComponent<AudioListener>();
    }

    public void FocusOnWorldCamera()
    {
        worldCamera.depth = 1;
        battleCamera.depth = 0;
        worldListener.enabled = true;
        battleListener.enabled = false;
        _cameraSettings.foucsedCamera = _cameraSettings.worldCamera;
    }

    public void FocusOnBattleCamera()
    {
        worldCamera.depth = 0;
        battleCamera.depth = 1;
        worldListener.enabled = false;
        battleListener.enabled = true;
        _cameraSettings.foucsedCamera = _cameraSettings.battleCamera;
    }

}
