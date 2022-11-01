using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class ItemActivator : MonoBehaviour
{
    // ---------------------------------------------------------
    // Variables:

    [SerializeField]
    private int distanceFromPlayer;

    private GameObject player;

    public List<ActivatorItem> activatorItems;
    public List<ActivatorItem> addList;

    private float x_pos;
    private float y_pos;
    private float z_pos;



    // ---------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        activatorItems = new List<ActivatorItem>();
        addList = new List<ActivatorItem>();

        AddToList();
    }

    void AddToList()
    {
        if (addList.Count > 0) 
        {
            foreach(ActivatorItem item in addList)
            {
                if (item.item != null)
                {
                    activatorItems.Add(item);
                }
            }

            addList.Clear();
        }

        StartCoroutine("CheckActivation");
    }

    IEnumerator CheckActivation()
    {
        List<ActivatorItem> removeList = new List<ActivatorItem>();

        if (activatorItems.Count > 0)
        {
            foreach (ActivatorItem item in activatorItems)
            {
                if (Vector3.Distance(player.transform.position, item.item.transform.position) > distanceFromPlayer)
                {
                    if (item.item == null)
                    {
                        // because we're iterating through this list and instead of destroying and removing objects,
                        // we add it to another list we can remove
                        removeList.Add(item);
                        Debug.Log("Hello: 1");

                    }
                    else
                    {
                        x_pos = item.item.transform.position.x;
                        y_pos = item.item.transform.position.y;
                        z_pos = item.item.transform.position.z;
                        Debug.Log(player.transform.position.z - z_pos);
                        item.item.SetActive(false);
                        if((player.transform.position.x - x_pos) > distanceFromPlayer)
                        {
                            item.item.transform.position = new Vector3(x_pos + (2 * distanceFromPlayer), y_pos, z_pos);
                            Debug.Log("A");
                        }
                        else if ((player.transform.position.x - x_pos) < (distanceFromPlayer)*-1)
                        {
                            Debug.Log("B");
                            item.item.transform.position = new Vector3(x_pos - (2 * distanceFromPlayer), y_pos, z_pos);
                        }
                        else if ((player.transform.position.z - z_pos) < (distanceFromPlayer)*-1)
                        {
                            Debug.Log("C");
                            item.item.transform.position = new Vector3(x_pos, y_pos, z_pos - (2 * distanceFromPlayer));
                        }
                        else if ((player.transform.position.z - z_pos) > distanceFromPlayer)
                        {
                            item.item.transform.position = new Vector3(x_pos, y_pos, z_pos + (2 * distanceFromPlayer));
                        }

                    }
                }
                else
                {
                    if (item.item == null)
                    {
                        removeList.Add(item);
                        Debug.Log("Hello: 3");
                    }
                    else
                    {
                        item.item.SetActive(true);

                        Debug.Log("Hello: 4");
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        yield return new WaitForSeconds(0.01f);

        if (removeList.Count > 0)
        {
            foreach (ActivatorItem item in removeList)
            {
                activatorItems.Remove(item);
            }
        }

        yield return new WaitForSeconds(0.01f);

        AddToList();
    }
}

public class ActivatorItem
{
    public GameObject item;
}