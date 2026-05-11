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
    private EncounterManager encounterManager;
    [SerializeField]
    private PartyManager partyManager;

    //컨트롤러 라인
    [Header("컨트롤 라인")]
    [SerializeField]
    private SystemController systemController;
    [SerializeField]
    private InputController inputController;
    [SerializeField]
    private CameraController cameraController;
    [SerializeField]
    private MovementController movementController;
    [SerializeField]
    private UIController uiController;
    [SerializeField]
    private BattleController battleController;

    void Awake()
    {
        //매니저 라인
        SetGridBinder();
        //SetEncountBinder();
        SetPartyBinder();

        //컨트롤러 라인
        SetSystemBinder();
        SetInputBinder();
        SetMovementBinder();
        SetCameraBinder();
        SetUIBinder();
        SetBattleBinder();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Destroyer());
    }


    private void SetGridBinder()
    {
        dataManager.GridSetter(gridManager);
        
        cameraController.GridSetter(gridManager);
        movementController.GridSetter(gridManager);
        uiController.GridSetter(gridManager);

    }

    private void SetUIBinder()
    {

        systemController.UISetter(uiController);
        movementController.UISetter(uiController);
    }

    private void SetSystemBinder()
    {
        encounterManager.SystemSetter(systemController);
        uiController.SystemSetter(systemController);

        movementController.SystemSetter(systemController);
    }

    private void SetEncountBinder()
    {
    }

    private void SetPartyBinder()
    {
        uiController.PartySetter(partyManager);
    }

    private void SetInputBinder()
    {
        systemController.InputSetter(inputController);
    }

    private void SetCameraBinder()
    {

        systemController.CameraControlSetter(cameraController);
        movementController.CameraControlSetter(cameraController);
    }

    private void SetMovementBinder()
    {
        encounterManager.MoveControlSetter(movementController);
        partyManager.MoveControlSetter(movementController);

        uiController.MoveControlSetter(movementController);
    }

    private void SetBattleBinder()
    {
        systemController.BattleControlSetter(battleController);
    }

    private IEnumerator Destroyer()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}
