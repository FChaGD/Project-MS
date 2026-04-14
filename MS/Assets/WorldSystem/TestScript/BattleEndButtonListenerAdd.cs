using SystemDefinition;
using UnityEngine;
using UnityEngine.UI;

public class BattleEndButtonListenerAdd : MonoBehaviour
{

    public Button button;
    public SystemController syscontrol;
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
        syscontrol.SetGameMode(typeof(WorldMode));
        uicontroller.UIModeChange(false, typeof(BattleMode));
        uicontroller.UIModeChange(true, typeof(WorldMode));
        cameraController.FocusWorldCamera();

    }
    

}
