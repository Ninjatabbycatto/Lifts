using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassengerController : MonoBehaviour {

    private List<GameObject> person = new List<GameObject>();
    
    public Vector3 targetPosition;
    public float moveDuration = 2f;
    private MeshFilter meshFilter;
    public List<passenger> user = new List<passenger>();

    public int passengercount = 0;

    private bool lockXComponent = false;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Controller Created");
        
        
        
        
        
    }


    public void createPassenger(int target, int current) {
        GameObject newPerson = new GameObject("person" + passengercount);
        meshFilter = newPerson.AddComponent<MeshFilter>();
        // Assign a mesh representing a cube to the MeshFilter component
        meshFilter.mesh = CreateCubeMesh();                
        // Add a MeshRenderer component to the GameObject (to make the cube visible)
        newPerson.AddComponent<MeshRenderer>();
        passenger newUser = new passenger();
        //newPerson.transform.position = spawnPosition;
        newPerson.AddComponent<passenger>();
        newUser = FindObjectOfType<passenger>();
        newUser.settargetcurrent(target, current);
        newUser.passengernumber = passengercount;
        //turn to a function where it has controlled randomness
        
        
        
        setSpawnPosition(newUser);
        person.Add(newPerson);
        user.Add(newUser);
        passengercount++;

        
        //StartCoroutine(MoveObject(transform, targetPosition.position, moveDuration));  
    }

    public void createRandomPassenger() { //full initialization of passenger with random values
        Debug.Log("Creating passenger on passengercontroller");
        int spawnPos = Random.Range(0, 44);
        int targetPos = Random.Range(0, 44);

        if (spawnPos != targetPos) createPassenger(spawnPos, targetPos);
        setSpawnPosition(user[passengercount]);
        passengercount++;
    }


    public void setSpawnPosition(passenger person) {
        //Debug.Log("new position = " + person.spawnPosition);
        Vector3 newPosition = new Vector3(8.16f, (person.getCurrent() * 2.5f) + 1.16f, -1.05f);
        person.spawnSet = true;
        person.spawnPosition = newPosition;       //😳😳
        
    }

    IEnumerator MoveObject(Transform objectToMove, Vector3 targetPosition, float duration) {   //walks object to the middle 
        float elapsedTime = 0f;
        Vector3 startingPosition = objectToMove.position;

        while (elapsedTime < duration)
        {
            // Calculate the interpolation factor (0 to 1)
            float t = Mathf.Clamp01(elapsedTime / duration);

            // Move the object towards the target position using Lerp
            objectToMove.position = Vector3.Lerp(startingPosition, targetPosition, t);

            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the object reaches the exact target position
        objectToMove.position = targetPosition;
    }

    Mesh CreateCubeMesh() {                         ///creates cube 
        Mesh mesh = new Mesh();

        // Vertices of the cube
        Vector3[] vertices =
        {
            new Vector3(-0.5f, -0.89f, -0.11f), // Bottom-Left-Front
            new Vector3(0.5f, -0.89f, -0.11f),  // Bottom-Right-Front
            new Vector3(0.5f, 0.89f, -0.11f),   // Top-Right-Front
            new Vector3(-0.5f, 0.89f, -0.11f),  // Top-Left-Front
            new Vector3(-0.5f, -0.89f, 0.11f),  // Bottom-Left-Back
            new Vector3(0.5f, -0.89f, 0.11f),   // Bottom-Right-Back
            new Vector3(0.5f, 0.89f, 0.11f),    // Top-Right-Back
            new Vector3(-0.5f, 0.89f, 0.11f)    // Top-Left-Back

        };
        // Triangles (indices of vertices)
        int[] triangles =
        {
            // Front face
            0, 2, 1, 0, 3, 2,
            // Back face
            4, 5, 6, 4, 6, 7,
            // Top face
            3, 6, 2, 3, 7, 6,
            // Bottom face
            0, 1, 5, 0, 5, 4,
            // Left face
            0, 4, 7, 0, 7, 3,
            // Right face
            1, 2, 6, 1, 6, 5
        };

        // Assign vertices and triangles to the mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Recalculate normals to ensure correct lighting
        mesh.RecalculateNormals();

        return mesh;
    }

    public Vector3 moveToMiddle(Vector3 currentpos) {
        //Debug.Log("Moving to middle");
        Vector3 targetposition = new Vector3(0f, currentpos.y, -1.05f);
        float movespeed = 3.5f;

        var step = movespeed * Time.deltaTime;
        Vector3 newPosition = Vector3.MoveTowards(currentpos, targetposition, step);
        return newPosition;

    }


    public Vector3 xComponent(Vector3 currentpos, Vector3 elevatorPos) {

        Vector3 targetposition = new Vector3(elevatorPos.x, currentpos.y, -1.05f);
        float movespeed = 4f;
        var step = movespeed * Time.deltaTime;
        Vector3 newPosition = Vector3.MoveTowards(currentpos, targetposition, step);

        return newPosition;
    }

    public Vector3 zComponent(Vector3 currentpos, Vector3 elevatorPos) {
        Vector3 targetposition = new Vector3(currentpos.x, currentpos.y, 0f);
        float movespeed = 2f;
        var step = movespeed * Time.deltaTime;
        Vector3 newPosition = Vector3.MoveTowards(currentpos, targetposition, step);
        //Debug.Log("Calculating new zPos\nCurrentpos = " + currentpos + 
        //          "\nelevatorPos = " + elevatorPos +
        //          "\ntargetPosition = " + targetposition + 
        //          "\nnewPosition = " + newPosition);
        return newPosition;
    }

    public Vector3 moveToElevator(Vector3 currentpos, Vector3 elevatorPos) {
        //    Debug.Log("currentPos = " + currentpos.x + 
        //              "\nelevatorPos = " + elevatorPos.x + 
        //              "\ndifference = " + Mathf.Abs(currentpos.x - elevatorPos.x));
        
        if (Mathf.Abs(currentpos.x - elevatorPos.x) >= 0.1f && !lockXComponent ){    
        //    Debug.Log("Moving xComponent to elevator");
            Vector3 newpos = xComponent(currentpos, elevatorPos);
            return newpos;
        }
        else /*if (Mathf.Abs(currentpos.x - elevatorPos.x) <= 0.4f)*/ {
            lockXComponent = true;
        //    Debug.Log("Moving zComponent to elevator");
            Vector3 newpos = zComponent(currentpos, elevatorPos);
            return newpos;
        }
        /*else {
            Debug.Log("Skipping both");
            return elevatorPos;
        }
        */
            

    }

    //-----------------Timer Area--------

    public void startTimer(passenger user) {
        
        user.startTime = Time.time;
        //Debug.Log("Timer Started: " + user.startTime);
    }

    public void stopTimer(passenger passenger) {
        passenger.waitingTime = passenger.elapsedTime;
        
    }

    public void removePassenger(int passenger) {
        user.RemoveAt(passenger);
    }

    //transfer function to Main
    void Update() {
               //move person to middle
    foreach (passenger person in user)
    if(person.waiting) {
        //Debug.Log("Calculating elapsedTime");
        person.elapsedTime = Time.time - person.startTime;
    }


    }
}
