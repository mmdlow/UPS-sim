using UnityEngine;
using System.Collections;

public class DirectionLineController : MonoBehaviour {

    private GameObject waypoint;
	public GameManager gameManager;
    private BoardManager boardManager;
    private LineRenderer line;                           // Line Renderer

    // Use this for initialization
    void Start () {
        Debug.Log("line started");
        // Add a Line Renderer to the GameObject
        line = this.gameObject.AddComponent<LineRenderer>();
        // Set the width of the Line Renderer
        line.startWidth = 0.05F;
        line.endWidth = 0.05F;
        // Set the number of vertex fo the Line Renderer
        line.positionCount = 2;
		boardManager = gameManager.GetComponent<BoardManager>();
        waypoint = boardManager.GetCurrentWaypoint();
    }
    
    void Update () {
        if (waypoint != null)
        {
            // Update position of the two vertex of the Line Renderer
            waypoint = boardManager.GetCurrentWaypoint();
            line.SetPosition(0, this.gameObject.transform.position);
            line.SetPosition(1, waypoint.transform.position);
        }
    }
}