using SystemDefinition;
using UnityEngine;
using UnityEngine.UI;

public class BattleStartButtonAddListenner : MonoBehaviour
{

    public Button button;
    public SystemController sysControl;
    public UIController uicontroller;
    public CameraController cameraController;

    void Awake()
    {
        button.onClick.AddListener(ClickFunc);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    private void ClickFunc()
    {
        sysControl.SetGameMode(typeof(BattleMode));
        uicontroller.UIModeChange(false, typeof(WorldMode));
        uicontroller.UIModeChange(true, typeof(BattleMode));
        cameraController.FocusBattleCamera();

    }
}
