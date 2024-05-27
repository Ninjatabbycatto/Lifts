using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


public class Main : MonoBehaviour
{

    private GameObject elevatorcontrollerbody;
    private GameObject passengercontrollerbody;
    // Start is called before the first frame update
    public ElevatorController elevatorcontroller;
    public PassengerController passengercontroller;


    public List<int> floorQueue = new List<int>();

    public int[] elevatorpos = new int[7];
    private int poscounter = 0;

    

    public static Main instance;    

    void Awake() {
    if (instance == null) {
        instance = this;
    } else {
        Destroy(gameObject);
    }
}

    void Start() {
        //MOVED TO AGENTS
        initializeElevatorController();
        initializePassengerController(); 
        //passengercontroller.createPassenger(15, 8); //fix this to run random and put in a function along with behaviour
        //passengercontroller.setSpawnPosition(passengercontroller.user[0]);
        
        


        





    }

    // Update is called once per frame

    private void updateElevatorFloors() {   //strore elevator pos in elevatorpos
        foreach(Elevator elevator in elevatorcontroller.elevators) {
            elevatorpos[elevator.elevatorNumber] = elevator.getCurrentFloorIndex();
        }
    }

    public void initializeElevatorController() {
        //initialize elevatorcontroller
        elevatorcontrollerbody = GameObject.Find("elevatorcontroller");     
        elevatorcontrollerbody.AddComponent<ElevatorController>();          //assigns component to object elevatorcontroller
        elevatorcontroller = FindObjectOfType<ElevatorController>();
        //use elevatorcontroller to access the functions
    }

    public void initializePassengerController() {
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


        passengercontroller.startTimer(user);

        
    }

    IEnumerator StopForSeconds(float seconds) {
        // Stop executing code for the specified number of seconds
        yield return new WaitForSeconds(seconds);

        // Code execution resumes after the specified delay
        Debug.Log("Resuming code execution after 5 seconds");
    }

    public float passengerWaitingTime(int passengerNumber) {
        return passengercontroller.user[passengerNumber].waitingTime;
    }

    private void passengermovements(passenger user) {
        /////moves passenger to middle and wait
        if (!user.goneToMiddle) {           
            user.transform.position = passengercontroller.moveToMiddle(user.transform.position);
        }

        //if user isnt waiting, press button and wait
        if (user.transform.position.x == 0f && !user.waiting) {
            user.goneToMiddle = true;
            Debug.Log("pressing button");
            pressbutton(user);
            
        }
        
        /*
        if(floorQueue.Count > 0) {                                  //go to elevator if has queue
            elevatorcontroller.controlElevator(0, floorQueue[0]);
        }
        */

        //if user is not in elevator, and waiting, wait till elevator is == to pos
        if (!user.inElevator){
            foreach(int pos in elevatorpos) {
                if (user.getCurrent() == pos && elevatorcontroller.elevators[poscounter].isIdle()) {
                    user.transform.position = passengercontroller.moveToElevator(user.transform.position, elevatorcontroller.elevators[poscounter].transform.position);
                    user.inLift = poscounter;
                    Debug.Log("Position assigned to user[0]: " + poscounter);
                }
                poscounter++;
            }
        }
        poscounter = 0;

        //Debug.Log("Floorqueuenum" + floorQueue[0]);
        //if user is in elevator, press button
        if (user.transform.position.z == 0) {
            Debug.Log("target pressed in elevator " +  !user.elevatorTargetPressed);
            Debug.Log("elevator pos from here: " + user.inLift);
            user.inElevator = true;
            
            user.transform.position = new Vector3 (user.transform.position.x, elevatorcontroller.elevators[user.inLift].transform.position.y + 0.97f, user.transform.position.z);
            if (!user.elevatorTargetPressed) {
                user.elevatorTargetPressed = true;
                elevatorcontroller.elevators[user.inLift].queue.Add(user.getTarget());
                elevatorcontroller.elevators[user.inLift].passengerCount++;
                user.giveReward = true;
                floorQueue.Remove(user.getCurrent());
                
            }

        }
        //passenger walks outside
        if (user.getTarget() == elevatorcontroller.elevators[user.inLift].getCurrentFloorIndex() && elevatorcontroller.elevators[user.inLift].isIdle()) {
            user.success = true;
            user.waiting = false;
            elevatorcontroller.elevators[user.inLift].queue.Remove(user.getTarget());
            StartCoroutine(StopForSeconds(5f));
            user.transform.position = passengercontroller.moveToMiddle(user.transform.position);
            elevatorcontroller.elevators[user.inLift].passengerCount--;
            Debug.Log("removing from queue" + user.getCurrent());
            
            
            
        }

        if(user.transform.position.x == 0f && user.success) {
            passengercontroller.stopTimer(user); ///you can now get the waitingTime
            Debug.Log("378 passenger elapsedTime waiting" + user.waitingTime);
           //passengercontroller.removePassenger(user.passengernumber);
        }

    }



    void Update()
    {
        updateElevatorFloors();

        Debug.Log("FloorQueue" + floorQueue);

        ////--------- User zone---------------------
        foreach (passenger user in passengercontroller.user) {
            if (!user.success){
                Debug.Log("Updating passenger " + user.passengernumber);
                passengermovements(user);
            }
        }

        


        ////--------- User zone---------------------^^

        

        //if(passengercontroller.user[0].waiting && elevatorcontroller.


        
    }
}
