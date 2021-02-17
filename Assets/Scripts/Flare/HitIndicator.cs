using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitIndicator : MonoBehaviour
{
    [SerializeField] private Material indicatorMaterial;
    private MeshRenderer meshRenderer;
    private Material material;
    private void Awake() 
    {
        meshRenderer = GetComponent<MeshRenderer>();
        material = meshRenderer.material;
        material.color = new Color(1, 1, 1, 0);
    }

    private void OnDisable() 
    {
        StopAllCoroutines();
    }

    public void Hit()
    {
        if (gameObject.activeInHierarchy)
            StartCoroutine(FlashMaterial());
    }

    public void Disable()
    {
        material.color = new Color(1, 1, 1, 0);
    }

    private IEnumerator FlashMaterial()
    {
        material.color = new Color(1, 1, 1, 0.3f);
        // Wait a couple of frames
        // Each yield return null waits for 1 frame
        yield return null;
        yield return null;
        material.color = new Color(1, 1, 1, 0f);
    }

#if UNITY_EDITOR
    // This function runs when an object is changed in the inspector
    private void OnValidate() 
    {
        if (indicatorMaterial)
            GetComponent<MeshRenderer>().material = indicatorMaterial;
    }
#endif
}
