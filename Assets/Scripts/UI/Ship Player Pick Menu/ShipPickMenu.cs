using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ShipPickMenu : MonoBehaviour
{
    private int activePlayer;
    [SerializeField] private ShipPreview p1ShipPreviewCamera;
    [SerializeField] private ShipPreview p2ShipPreviewCamera;
    [SerializeField] private GameObject startGameButton;

    private bool p1Locked, p2Locked;

    private void Awake() 
    {
        startGameButton.GetComponent<Button>().onClick.AddListener(GameSceneLoad);
        startGameButton.SetActive(false);
    }

    private void OnEnable() 
    {
        ShipSelectButton.shipSelectEvent += OnShipSelect;
    }

    private void OnDisable() 
    {
        ShipSelectButton.shipSelectEvent -= OnShipSelect;
    }

    public void OnShipSelect(GameObject ship)
    {
        if (p1Locked && p2Locked) return;
        
        GameManager.SetShip(ship, activePlayer);
        
        if (activePlayer == 0 && !p1Locked)
            p1ShipPreviewCamera?.SetPreview(ship);
        else if (activePlayer == 1 && !p2Locked)
            p2ShipPreviewCamera?.SetPreview(ship);
    }


    public void ConfirmShip()
    {
        if (activePlayer == 0)
            p1Locked = true;
        else if (activePlayer == 1)
            p2Locked = true;
        
        activePlayer++;

        if (activePlayer > 1)
        {
            startGameButton.SetActive(true);
            EventSystem.current.SetSelectedGameObject(startGameButton);
        }
    }

    public void GameSceneLoad()
    {
        SceneManager.LoadScene("GameScene");
    }
}
