using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ElevatorController : MonoBehaviour
{
    public List<Floor> floors = new List<Floor>();
    public List<Elevator> elevators = new List<Elevator>();
    public List<passenger> users = new List<passenger>();

    
    // Start is called before the first frame update
    private void Start() {
        getFloorLayout();       //analyze floors
        getElevatorLayout();    //analayze elevators
    }

    private void getFloorLayout() {
        Floor[] allFloors = FindObjectsOfType<Floor>(); // find objects that has the floor class
        floors.AddRange(allFloors);                    //adds these objects to floor list.

        floors =  floors.OrderBy(floor => floor.transform.position.y).ToList();

        int temp = 0;
        foreach (Floor floor in floors) {
            floor.setFloor(temp);
            temp++;
        }
    }

    private void getElevatorLayout() {
        Elevator[] allelevators = FindObjectsOfType<Elevator>();
        elevators.AddRange(allelevators);

        elevators = elevators.OrderBy(elevator => elevator.transform.position.x).ToList();

        int temp = 0;
        foreach(Elevator elevator in elevators) {
            elevator.setElevatorNumber(temp);
            temp++;
        }
    }


    private void showElevatorInfo(Elevator elevator) {
        Debug.Log("Elevator " + elevator.elevatorNumber + 
                  "\nCurrent Floor: " + elevator.getCurrentFloorIndex() + 
                   "\nVelocity : " + elevator.getVelocity() + 
                   "\nHeight : " + elevator.getyPosition() );
    }

    private int updatedFloor(Elevator elevator) {
        float downDistance;
        float upDistance;

        if (elevator.getCurrentFloorIndex() < 44) {
            upDistance = floors[elevator.getCurrentFloorIndex() + 1].yPosition - elevator.getyPosition();
        }
        else {
            upDistance = 100000f;
        }
        
        if (elevator.getCurrentFloorIndex() > 0) {
            downDistance = elevator.getyPosition() - floors[elevator.getCurrentFloorIndex() - 1].yPosition;
        }
        else {
            downDistance = 100000f;
        }
        float currentDistance = elevator.getyPosition() - floors[elevator.getCurrentFloorIndex()].yPosition;

        downDistance = Mathf.Abs(downDistance);
        upDistance = Mathf.Abs(upDistance);
        currentDistance = Mathf.Abs(currentDistance);
        float minDifference = Mathf.Min(upDistance, Mathf.Min(currentDistance, downDistance));

        //Debug.Log("MinDifference: " + minDifference + 
        //           "\nUpDistance: " + upDistance + 
        //           "\ndownDistance " + downDistance + 
        //           "\ncurrentDistance " + currentDistance
        //           );

        if (minDifference == upDistance) {
            return elevator.getCurrentFloorIndex() + 1;
        }
        else if (minDifference == downDistance) {
            return elevator.getCurrentFloorIndex() - 1;
        }
        else {
            return elevator.getCurrentFloorIndex();
        }
        
    }
    
    //setters and getters
    public Vector3 getFloorHeight(int floor) {
        return floors[floor].transform.position;
    }

    public void controlElevator(int elevatorNumber, int targetFloor) {
        elevators[elevatorNumber].setTargetFloor(targetFloor);
    }

    // Update is called once per frame
    void Update() {   
            //causes elevator to accelarate
        foreach(Elevator elevator in elevators) {
            elevator.setCurrentFloorIndex(updatedFloor(elevator)); //inefficient way to get the current floor of the elevator.
        }
        showElevatorInfo(elevators[0]);
    }

}
