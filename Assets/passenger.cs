using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class passenger : MonoBehaviour
{
    public int targetFloor = 0, currentFloor = 0, passengernumber = 0;
    public bool waiting = false, 
                atTheCenter = true,
                success = false,
                spawnSet = false, 
                goneToMiddle = false, 
                inElevator = false,
                elevatorTargetPressed = false, 
                ignore = false;

    public int inLift = 0;
    public Vector3 spawnPosition;

    public float startTime;
    public float elapsedTime;
    public float waitingTime;

    //add time
    


    private void Start() {
       Debug.Log("Passenger Number: " + passengernumber + 
                  "\nTargetFloor" + targetFloor + 
                  "\nCurrentFloor" + currentFloor);

        moveToSpawnPosition();
    }

    

    //setters
    

    public int getTarget() {
        return targetFloor;
    }

    public int getCurrent() {
        return currentFloor;
    }

    public void settargetcurrent(int target, int current) {
        targetFloor = target;
        currentFloor = current;

    }

    public void moveToSpawnPosition() {
        transform.position = spawnPosition;
    }

    public int checkElevators(int[] elevators) {
        foreach(int elevator in elevators) {
            if (elevator == currentFloor) return elevator;
            else return 0;
        }
        return 0;
    }

    
    

    void update() {
        Debug.Log("Passenger Number: " + passengernumber + 
                  "\nTargetFloor" + targetFloor + 
                  "\nCurrentFloor" + currentFloor);


        Debug.Log("378 Time Details \n" +
                  "StartTime: " + startTime + 
                  "\nelapsedTime: " + elapsedTime+ 
                  "\nwaitingTime: " + waitingTime);
        
        

        if (!spawnSet) {            //add spawnset to passsenger class and put this into for each loop
            transform.position = spawnPosition; //set spawn position    
            Debug.Log("Spawn Position" + spawnPosition); 
            spawnSet = true;                            //set spawn position to true
            
        }
            
              //move person to middle
        
        

    }
    



    
}