using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class ShipSelectButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private string ShipName;
    [SerializeField] private GameObject shipObject;
    private Button button;
    private ShipPickMenu menuManager;

    private void Awake()
    {
        menuManager = FindObjectOfType<ShipPickMenu>();
        button = GetComponent<Button>();
    }

    private void Start() 
    {
        button.onClick.AddListener(menuManager.ConfirmShip);
    }

    public void OnSelect(BaseEventData eventData)
    {
        shipSelectEvent?.Invoke(shipObject);
    }

    public void SetShipName(string name)
    {
        buttonText.text = name;
    }

    public void SetShipObject(GameObject shipObject)
    {
        this.shipObject = shipObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        shipSelectEvent?.Invoke(shipObject);
    }

    public static Action<GameObject> shipSelectEvent;
}
