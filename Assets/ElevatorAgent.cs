using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;


/// <summary>
///  Fix multiple passenger behaviour
///  
/// </summary>
public class ElevatorAgent : Agent {
    Main mainScript;
    bool initialized = false;
    
    int episodes = 0;
    int ActivePassengers;

    
    public override void Initialize() {
        //mainScript.initializeElevatorController();
        //mainScript.initializePassengerController();

        Debug.Log("Initialized!");
        //mainScript = Main.instance;     //does not work if not on OnEpisodeBegin
        mainScript = FindObjectOfType<Main>();
        ActivePassengers = 0;

    }

    public override void OnEpisodeBegin() {
        epStats();
        Debug.Log("Active Passengers" + ActivePassengers);
        //mainScript = GetComponent<Main>();  
        episodes++;
        if (ActivePassengers < 7) {     ///change if how many passengers can be active at once
            //Debug.Log("Creating passenger on elevator Agent");
            ActivePassengers++;
            mainScript.passengercontroller.createRandomPassenger();
        }
        
        //create only if there is no passenger for now
        //mainScript.passengercontroller.createPassenger(targetFloor,currentFloor);
        //set spawn position for the first passegner        //user                              //passengercount - 1
        //mainScript.passengercontroller.setSpawnPosition(mainScript.passengercontroller.user[mainScript.passengercontroller.passengercount]);
        //epStats();
        //foreach(mainScript.elevator )
    }

    private void epStats() {
        Debug.Log("Episode: " + episodes + 
                  "\ntotal passengercount: " + mainScript.passengercontroller.passengercount);
    }

    [SerializeField] private List<int> target;  /// <summary>
    /// Creates a new field which is the target
    /// </summary>
    /// <param name="sensor"></param>
    /// 

    public override void CollectObservations(VectorSensor sensor) {
        target = mainScript.floorQueue; ///observe each item in the floorqueue
        foreach (int floor in target) {
            sensor.AddObservation(floor);
            Debug.Log("Observing floor" + floor);
        }
        //sensor.AddObservation(mainScript.passengerWaitingTime(0));

        foreach (int pos in mainScript.elevatorpos) {
            sensor.AddObservation(pos);
        }
        foreach(passenger user in mainScript.passengercontroller.user) {
            sensor.AddObservation(user.inLift);
        }
        foreach(Elevator lift in mainScript.elevatorcontroller.elevators) {
            sensor.AddOneHotObservation(lift.getCurrentFloorIndex(), 44);
            sensor.AddObservation(lift.getIdle());
            foreach (int queue in lift.queue) {
                sensor.AddObservation(queue);
            }
        }
        

        Debug.Log("512 PassengerWaitingTime: " + mainScript.passengerWaitingTime(0));

    }
    public override void OnActionReceived(ActionBuffers actions) {

        int eNum = actions.DiscreteActions[0];
        int tFloor = actions.DiscreteActions[1];

        if (mainScript.elevatorcontroller.elevators[eNum].isIdle()) {
            mainScript.elevatorcontroller.controlElevator(eNum, tFloor);
        }

        AddReward(-0.00002f);


        
        //foreach(Elevator elevator in  mainScript.elevatorcontroller.elevators) {
//
//
        //    Debug.Log("511" + elevator.isIdle());
        //    if (elevator.isIdle()) {   //fix this
        //        Debug.Log("Discrete Actions: " + elevator.elevatorNumber +", " + tFloor);
        //        mainScript.elevatorcontroller.controlElevator(elevator.elevatorNumber, tFloor);
        //        break;          //break so discrete actions can only be applieed one at a time
        //    }
        //}

        
        
        foreach(passenger user in mainScript.passengercontroller.user) {
            //this ends episode prematurely 
            if (user.giveReward == true) {
                user.giveReward = false;
                AddReward(0.25f);
            }


            Debug.Log("checking passenger " + user.passengernumber);
            if (user.success && !user.ignore) {
                ActivePassengers--;
                AddReward(0.35f);
                Debug.Log("Giving Reward: ");
                user.ignore = true;
                
            }
            if (user.ignore) {
                Destroy(user.gameObject);
                mainScript.passengercontroller.user.Remove(user);
                Debug.Log("EndingEpisode");
                EndEpisode();
            }

        }
    }

    
}
