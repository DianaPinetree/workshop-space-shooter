using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentFillHelper : MonoBehaviour
{
    [SerializeField] private string folderPath = "Ships/";
    [SerializeField] private ShipSelectButton prefabButton;
    
#if UNITY_EDITOR

    public void ReloadObjects()
    {
        foreach(Transform t in transform)
        {
            DestroyImmediate(t.gameObject);
        }

        GameObject[] ships = Resources.LoadAll<GameObject>(folderPath);
        foreach(GameObject s in ships)
        {
            ShipSelectButton button = Instantiate(prefabButton, transform);
            button.SetShipObject(s);
            button.SetShipName(s.name);
        }
    }
#endif
}
