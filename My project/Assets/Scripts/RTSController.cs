using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RTSController : MonoBehaviour
{
    Camera cam;

    public LayerMask groundLayer;
    private LineRenderer lr;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private bool currentlySelecting = false;
    private RaycastHit hit;

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector2[] corners;
    Vector3[] verts;

    private UnitEnumerable unitsObject = new UnitEnumerable();

    public NavMeshAgent playerAgent;

    // Start is called before the first frame update
    void Awake()
    {
        cam = Camera.main;
    }

    void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            playerAgent.SetDestination(GetPointUnderCursor());
        }

        if (Input.GetMouseButtonDown(0))
        {
            startPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            if ((startPosition - Input.mousePosition).magnitude > 30)
            {
                currentlySelecting = true;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentlySelecting == false)
            {
                Ray ray = cam.ScreenPointToRay(startPosition);

                if (Physics.Raycast(ray, out hit, 50000.0f))
                {
                    if (Input.GetKey(KeyCode.LeftShift)) // Inclusive select
                    {
                        unitsObject.SelectUnit(hit.transform.gameObject);
                    }
                    else // Exclusive select
                    {
                        unitsObject.DeselectAll();
                        unitsObject.SelectUnit(hit.transform.gameObject); // Create some class that handles selection, needs to enable visuals on characters
                    }
                }
                else // We didnt hit anything, deselect everything
                {
                    unitsObject.DeselectAll();

                }
            }
            currentlySelecting = false;
        }
        else
        {
            verts = new Vector3[4];
            int i = 0;
            endPosition = Input.mousePosition;
            corners = getBoundingBox(startPosition, endPosition);


            //CHECK YOUTUBE TUTORIAL https://www.youtube.com/watch?v=vAVi04mzeKk





            if (!Input.GetKey(KeyCode.LeftShift))
            {
                unitsObject.DeselectAll();
            }

        }
        // currentlySelecting = false;

    }

    private Vector3 GetPointUnderCursor()
    {
        /*
        Vector2 screenPosition = Input.mousePosition;
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(screenPosition);
        Debug.Log(mouseWorldPosition);
        */
        RaycastHit hitPosition;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        Physics.Raycast(ray, out hitPosition);

        return hitPosition.point;
    }


    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        Vector2 newP1;
        Vector2 newP2;
        Vector2 newP3;
        Vector2 newP4;

        if (p1.x < p2.x)
        {
            if (p1.y > p2.y)
            {
                newP1 = p1;
                newP2 = new Vector2(p2.x, p1.y);
                newP3 = new Vector2(p1.x, p2.y);
                newP4 = p2;
            }
            else
            {
                newP1 = new Vector2(p1.x, p2.y);
                newP2 = p2;
                newP3 = p1;
                newP4 = new Vector2(p2.x, p1.y);
            }
        }
        else
        {
            if (p1.y > p2.y)
            {
                newP1 = new Vector2(p2.x, p1.y);
                newP2 = p1;
                newP3 = p2;
                newP4 = new Vector2(p1.x, p2.y);
            }
            else
            {
                newP1 = p2;
                newP2 = new Vector2(p1.x, p2.y);
                newP3 = new Vector2(p2.x, p1.y);
                newP4 = p1;
            }
        }

        Vector2[] corners = { newP1, newP2, newP3, newP4 };
        return corners;

    }


    private void OnGUI()
    {
        if (currentlySelecting == true)
        {
            var rect = Utils.GetScreenRect(startPosition, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0f, 0.95f, 0.05f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0f, 0.95f, 0));
        }
    }

    Mesh generateSelectionMesh(Vector3[] corners)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 };

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + Vector3.up * 100.0f;

        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        unitsObject.SelectUnit(other.gameObject);
    }
}
