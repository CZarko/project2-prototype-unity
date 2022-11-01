using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableIfFar : MonoBehaviour
{
    // ---------------------------------------------------------
    // Variables:

    private GameObject itemActivatorObject;
    private ItemActivator activationScript;

    // ---------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        itemActivatorObject = GameObject.Find("itemActivatorObject");
        activationScript = itemActivatorObject.GetComponent<ItemActivator>();

        StartCoroutine("AddToList");
    }

    IEnumerator AddToList()
    {
        yield return new WaitForSeconds(0.1f);

        activationScript.addList.Add(new ActivatorItem { item = this.gameObject });
    }
}
