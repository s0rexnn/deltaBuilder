using UnityEngine;

public enum BattleState
{
    Encounter,
    PlayerTurn,
    PlayerSelect,
    EnemyTurn,
}

public class BattleManager : MonoBehaviour
{
   public BattleState currentState;
}
