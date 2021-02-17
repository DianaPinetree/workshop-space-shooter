using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class MatchManager : MonoBehaviour
{
    [SerializeField] private Transform playerOneSpawnPosition;
    [SerializeField] private Transform playerTwoSpawnPosition;
    [Header("Match Music")]
    [SerializeField] AudioClip matchMusic;
    public static Ship[] Ships {get; private set;}
    private int p1Win;
    private int p2Win;
    public int Match => p2Win + p1Win;
    public int Player1Wins => p1Win;
    public int Player2Wins => p2Win;

    // Players with an assigned controller
    private int activePlayers;
    private Ship[] instancePlayers;

    private void Awake() 
    {
        instancePlayers = new Ship[2];
        SpawnPlayers();

        // Debug code
        foreach(Ship s in instancePlayers)
        {
            s.GetComponent<SpaceShipController>().Controller.Init();
        }
        
        Ships = instancePlayers;
    }

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnControllerChange;
        Ship.deathEvent += MatchEnd;
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange += OnControllerChange;
        Ship.deathEvent -= MatchEnd;
    }

    private void Start() 
    {
        AudioManager.PlayMusic(matchMusic);
        ResetMatch();
    }

    public void ResetMatch()
    {
        // Move and activate players to the correct positions
        instancePlayers[0].transform.position = playerOneSpawnPosition.position;
        instancePlayers[0].transform.rotation = playerOneSpawnPosition.rotation;
        instancePlayers[0].gameObject.SetActive(true);
        
        instancePlayers[1].transform.position = playerTwoSpawnPosition.position;
        instancePlayers[1].transform.rotation = playerTwoSpawnPosition.rotation;
        instancePlayers[1].gameObject.SetActive(true);

        // Reset any player values in match reset
        instancePlayers[0].Init();
        instancePlayers[1].Init();

        matchReset?.Invoke();
    }

    private void MatchEnd(Ship deadShip)
    {
        if (deadShip == instancePlayers[0])
        {
            p2Win++;
        }
        else
        {
            p1Win++;
        }

        StartCoroutine(MatchEndAnimation());
        
        AudioManager.PlayMusic(matchMusic);
    }

    private void SpawnPlayers()
    {
        Ship player1 = Instantiate(GameManager.Player1Prefab, 
            playerOneSpawnPosition.position,
            playerOneSpawnPosition.rotation);
    
        Ship player2 = Instantiate(GameManager.Player2Prefab, 
            playerTwoSpawnPosition.position,
            playerTwoSpawnPosition.rotation);

        instancePlayers[0] = player1;
        instancePlayers[1] = player2;
    }

    private IEnumerator MatchEndAnimation()
    {
        
        yield return new WaitForSeconds(2f);

        ResetMatch();
    }

    private void OnControllerChange(InputDevice device, InputDeviceChange change)
    {
        Debug.Log(Gamepad.current.name);
        switch (change)
        {
            case InputDeviceChange.Added:
                // New Device.
                foreach(Ship s in instancePlayers)
                {
                    SpaceShipController c = s.GetComponent<SpaceShipController>();
                    if (!c.Controller.Assigned)
                    {
                        c.Controller.AssignController(device);
                    }
                }
                break;
            case InputDeviceChange.Disconnected:
                
                // Device got unplugged.
                break;
            case InputDeviceChange.Reconnected:
                // Plugged back in.
                break;
            case InputDeviceChange.Removed:
                // Remove from Input System entirely; by default, Devices stay in the system once discovered.
                break;
            default:
                // See InputDeviceChange reference for other event types.
                break;
        }
    }

    public static event Action matchReset;
}
