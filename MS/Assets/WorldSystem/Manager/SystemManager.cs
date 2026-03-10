using UnityEngine;
using SystemDefinition;
using UnityEditorInternal;
using System;

public class SystemManager : MonoBehaviour, ISystemFunc
{
    private GameModeType gameMode = GameModeType.World;

    void Start()
    {
        
    }

    public GameModeType GetGameMode()
    {
        return gameMode;
    }

    //게임모드 변경
    public void SetGameMode(GameModeType gameMode)
    {
        this.gameMode = gameMode;
        switch (gameMode)
        {
            case GameModeType.World:
                TurnWorld();
                break;
            case GameModeType.Base:
                TurnBase();
                break;
            case GameModeType.Dialog:
                TurnDialog();
                break;
            case GameModeType.Battle:
                TurnBattle();
                break;
            default:
                break;
        }
    }

    // 월드 게임모드 변경시 작업
    private void TurnWorld()
    {

    }

    // 거점 게임모드 변경시 작업
    private void TurnBase()
    {

    }

    // 대화창 게임모드 변경시 작업
    private void TurnDialog()
    {

    }
    
    //전투 게임모드 변경시 작업
    private void TurnBattle()
    {
        
    }

}
