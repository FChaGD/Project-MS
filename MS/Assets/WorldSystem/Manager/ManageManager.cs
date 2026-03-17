using System.Collections;
using SystemDefinition;
using UnityEngine;

//종속성 컨트롤 담당 매니저 스크립트
public class ManageManager : MonoBehaviour
{
    //매니저 라인
    [Header("매니저 라인")]
    [SerializeField]
    private GridManager gridManager;
    [SerializeField]
    private DataManager dataManager;
    [SerializeField]
    private UIManager uiManager;
    [SerializeField]
    private SystemManager systemManager;
    [SerializeField]
    private EncounterManager encounterManager;

    //컨트롤러 라인
    [Header("컨트롤 라인")]
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private MovementController movementController;

    void Awake()
    {
        //매니저 라인
        SetGridBinder();
        SetUIBinder();
        SetSystemBinder();
        //SetEncountBinder();

        //컨트롤러 라인
        SetMovementBinder();
        SetCameraBinder();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Destroyer());
    }


    private void SetGridBinder()
    {
        dataManager.GridSetter(gridManager);
        uiManager.GridSetter(gridManager);
        
        cameraController.GridSetter(gridManager);
        movementController.GridSetter(gridManager);
        
    }

    private void SetUIBinder()
    {
        movementController.UISetter(uiManager);
    }

    private void SetSystemBinder()
    {
        encounterManager.SystemSetter(systemManager);

        movementController.SystemSetter(systemManager);
    }

    private void SetEncountBinder()
    {
    }

    private void SetCameraBinder()
    {
        movementController.CameraControlSetter(cameraController);
    }

    private void SetMovementBinder()
    {
        encounterManager.MoveControlSetter(movementController);
    }

    private IEnumerator Destroyer()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
