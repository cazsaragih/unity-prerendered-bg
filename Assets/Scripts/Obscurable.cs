using UnityEngine;

public class Obscurable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var renders = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renders)
        {
            r.material.renderQueue = 2002; // set their renderQueue
        }

    }
}
