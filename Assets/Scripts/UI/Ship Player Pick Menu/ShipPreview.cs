using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPreview : MonoBehaviour
{
    const float ROTATION_SPEED = 10f;
    [SerializeField] private Transform viewPos;
    private Camera previewCamera;
    private GameObject active;
    public void SetPreview(GameObject obj)
    {
        if (active != null)
        {
            Destroy(active);
        }

        if (obj == null) return;
        active = Instantiate(obj, viewPos);
        active.transform.parent = viewPos;
        active.transform.localPosition = Vector3.zero;
        // active.transform.localScale = Vector3.one;
    }

    private void Update() 
    {
        if (active != null)
        {
            active.transform.Rotate(new Vector3(0, ROTATION_SPEED * Time.deltaTime, 0), Space.Self);
        }
    }
}
