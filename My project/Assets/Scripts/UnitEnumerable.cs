using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class UnitEnumerable : MonoBehaviour
{


    private Dictionary<int, GameObject> selectedTable = new Dictionary<int, GameObject>();

    public void SelectUnit(GameObject g)
    {
        int id = g.GetInstanceID();

        if (!selectedTable.ContainsKey(id))
        {
            selectedTable.Add(id, g);
            g.AddComponent<SelectionComponent>();
        }

    }

    public void DeselectUnit(int id)
    {
        selectedTable.Remove(id);
    }

    public void DeselectAll()
    {
        foreach (KeyValuePair<int, GameObject> pair in selectedTable)
        {
            if (pair.Value != null)
            {
                Destroy(selectedTable[pair.Key].GetComponent<SelectionComponent>());
            }
        }
        selectedTable.Clear();
    }


    private void AddSelectionToGameObject(GameObject g)
    {
        /*    SelectionComponent selectionGraphics = g.GetComponent<SelectionComponent>();

            selectionGraphics.active = true;*/
    }

    private void RemoveSelectionFromGameObject(GameObject g)
    {
        /* SelectionComponent selectionGraphics = g.GetComponent<SelectionComponent>();

         selectionGraphics.active = false;*/
    }
}

