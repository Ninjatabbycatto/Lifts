using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

    private GameObject elevatorcontrollerbody;
    private GameObject passengercontrollerbody;
    // Start is called before the first frame update
    private ElevatorController elevatorcontroller;
    private PassengerController passengercontroller;


    private List<int> floorQueue = new List<int>();

    private int[] elevatorpos = new int[7];
    private int poscounter = 0;


    void Start() {
        
        initializeElevatorController();
        initializePassengerController(); 
        passengercontroller.createPassenger(21, 13);
        passengercontroller.setSpawnPosition(passengercontroller.user[0]);
        
        


        





    }

    // Update is called once per frame

    private void updateElevatorFloors() {   //strore elevator pos in elevatorpos
        foreach(Elevator elevator in elevatorcontroller.elevators) {
            elevatorpos[elevator.elevatorNumber] = elevator.getCurrentFloorIndex();
        }
    }

    private void initializeElevatorController() {
        //initialize elevatorcontroller
        elevatorcontrollerbody = GameObject.Find("elevatorcontroller");     
        elevatorcontrollerbody.AddComponent<ElevatorController>();          //assigns component to object elevatorcontroller
        elevatorcontroller = FindObjectOfType<ElevatorController>();
        //use elevatorcontroller to access the functions
    }

    private void initializePassengerController() {
        //initialize passengercontroller
        passengercontrollerbody = GameObject.Find("passengercontroller");
        passengercontrollerbody.AddComponent<PassengerController>();        //assigns component to object passengercontroller
        passengercontroller = FindObjectOfType<PassengerController>();
        

    }

    //make this only activate once it arrives at the center
    private void pressbutton(passenger user) {
        if (user.getCurrent() < user.getTarget()) {
            elevatorcontroller.floors[user.getCurrent()].goingUp();
            floorQueue.Add(user.getCurrent());
            user.waiting = true;
            Debug.Log("UpButton pressed"); 
        }
        else if (user.getCurrent() > user.getTarget()){
            elevatorcontroller.floors[user.getCurrent()].goingDown();
            Debug.Log("downButton pressed");
            floorQueue.Add(user.getCurrent());
            user.waiting = true;
        }


    

        
    }

    IEnumerator StopForSeconds(float seconds) {
        // Stop executing code for the specified number of seconds
        yield return new WaitForSeconds(seconds);

        // Code execution resumes after the specified delay
        Debug.Log("Resuming code execution after 5 seconds");
    }

    void Update()
    {
        updateElevatorFloors();

        Debug.Log("FloorQueue" + floorQueue);

        ////--------- User zone---------------------
        if (!passengercontroller.user[0].goneToMiddle) {
            passengercontroller.user[0].transform.position = passengercontroller.moveToMiddle(passengercontroller.user[0].transform.position);
        }
        if (passengercontroller.user[0].transform.position.x == 0f && !passengercontroller.user[0].waiting) {
            passengercontroller.user[0].goneToMiddle = true;
            Debug.Log("pressing button");
            pressbutton(passengercontroller.user[0]);
            
        }
        if(floorQueue.Count > 0) {                                  //go to elevator if has queue
            elevatorcontroller.controlElevator(0, floorQueue[0]);
        }
        
        if (!passengercontroller.user[0].inElevator){
            foreach(int pos in elevatorpos) {
                if (passengercontroller.user[0].getCurrent() == pos ) {
                    passengercontroller.user[0].transform.position = passengercontroller.moveToElevator(passengercontroller.user[0].transform.position, elevatorcontroller.elevators[poscounter].transform.position);
                    passengercontroller.user[0].inLift = poscounter;
                    Debug.Log("Position assigned to user[0]: " + poscounter);
                }
                poscounter++;
            }
        }
        poscounter = 0;

        Debug.Log("Floorqueuenum" + floorQueue[0]);
        if (passengercontroller.user[0].transform.position.z == 0) {
            
            Debug.Log("target pressed in elevator " +  !passengercontroller.user[0].elevatorTargetPressed);
            Debug.Log("elevator pos from here: " + passengercontroller.user[0].inLift);
            passengercontroller.user[0].inElevator = true;
            passengercontroller.user[0].transform.position = new Vector3 (passengercontroller.user[0].transform.position.x, elevatorcontroller.elevators[passengercontroller.user[0].inLift].transform.position.y + 0.97f, passengercontroller.user[0].transform.position.z);
            if (!passengercontroller.user[0].elevatorTargetPressed) {
                passengercontroller.user[0].elevatorTargetPressed = true;
                floorQueue.Add(passengercontroller.user[0].getTarget());
                Debug.Log("Queue added" );
                Debug.Log("Queue removed: " + floorQueue[0]);
                floorQueue.RemoveAt(0);
            }

        }

        if (passengercontroller.user[0].getTarget() == elevatorcontroller.elevators[passengercontroller.user[0].inLift].getCurrentFloorIndex() && elevatorcontroller.elevators[passengercontroller.user[0].inLift].isIdle()) {
            StartCoroutine(StopForSeconds(5f));
            passengercontroller.user[0].transform.position = passengercontroller.moveToMiddle( passengercontroller.user[0].transform.position);
            passengercontroller.user[0].success = true;


        }


        


        ////--------- User zone---------------------^^

        

        //if(passengercontroller.user[0].waiting && elevatorcontroller.


        
    }
}
