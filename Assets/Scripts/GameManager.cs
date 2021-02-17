using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private Ship player1Prefab;
    [SerializeField] private Ship player2Prefab;

    public static Ship Player1Prefab
    {
        get
        {
            return Instance.player1Prefab;
        }
        set
        {
            Instance.player1Prefab = value;
        }
    }

    public static Ship Player2Prefab
    {
        get
        {
            return Instance.player1Prefab;
        }
        set
        {
            Instance.player1Prefab = value;
        }
    }

    public static void SetP1Ship(GameObject ship)
    {
        if (ship.TryGetComponent<Ship>(out Ship s))
        {
            Instance.player1Prefab = s;
        }
    }

    public static void SetP2Ship(GameObject ship)
    {
        if (ship.TryGetComponent<Ship>(out Ship s))
        {
            Instance.player2Prefab = s;
        }
    }

    public static void SetShip(GameObject ship, int playerIndex)
    {
        if (ship == null) return;

        if (ship.TryGetComponent<Ship>(out Ship s))
        {
            if (playerIndex == 0)
                Instance.player1Prefab = s;
            else
                Instance.player2Prefab = s;
        }
    }
}
