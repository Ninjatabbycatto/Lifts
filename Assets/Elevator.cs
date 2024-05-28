using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class Elevator : MonoBehaviour
{
    // Start is called before the first frame update
    private float force,
                  yPosition,
                  idleTime; //constant force on the elevator

    public int    passengerCount, 
                  elevatorNumber;
    
    private int   capacity, 
                  
                  maxPassenger, 
                  
                  currentFloorIndex, 
                  targetFloor;

    private bool  goingUp = false, goingDown = false, idle = true;
    private float         applyForce;

    private float velocitybefore = 0;

    //rigidbodyPart
    private Rigidbody rb;
    new private ConstantForce constantForce;
    private Vector3 velocity;
    
    public List<int> queue = new List<int>();


    void Start() {
        //string name;
        constantForce = GetComponent<ConstantForce>();
        rb = GetComponent<Rigidbody>();
        force = 1.75f;
        capacity = 1350;
        passengerCount = 0;
        maxPassenger = 21;
        idleTime = 9f;
        currentFloorIndex = 0;
    }
    public bool getIdle() {
        return idle;
    }

    private bool ignoreDecelerate() {
        if(Mathf.Abs(velocity[1]) < 2f) {
            //Debug.Log("Ignoring decelarate;");
            return true;
        }
        return false;
    }

    public void goToFloor(int targetFloor) {   //will be called by elevatorController
        if (idle) {                            //if elevator is not doing any work
            
            idle = false;                      //set status to working
            if(currentFloorIndex < targetFloor) {
                ascend();
                

            }
            else if (currentFloorIndex > targetFloor) {
                
                descend();
                //check if near floor
                //decelereate to 0 on floor.
                
                
            }
            else if (currentFloorIndex == targetFloor) {
                 
            }
        }
        
    }

    IEnumerator StopForSeconds(float seconds) {
        // Stop executing code for the specified number of seconds
        yield return new WaitForSeconds(seconds);

        // Code execution resumes after the specified delay
        //Debug.Log("Resuming code execution after 5 seconds");
    }

    private void goIdle() {
        //wait for 5-10 seconds 
        goingUp = false;
        goingDown = false;
        if (!isMoving() && targetFloor == currentFloorIndex) {
            //Debug.Log("143");
            idle = true;
            StartCoroutine(StopForSeconds(idleTime)); 
        }
        
        
    }
    public bool isIdle() {
        return idle;
    }

    public void ascend() {
        
            goingUp = true;
            goingDown = false;
        
        constantForce.force = new Vector3(0f, force, 0f);
    }

    public bool isMoving() {
        if (velocity[1] == 0) {
            return false;
        }  
        else return true;
    }

    private bool near() {
        
        if(Mathf.Abs(currentFloorIndex - targetFloor) <= 4) {
                              //   ^ Tolerance
            //Debug.Log("Elevator " + elevatorNumber + " IS NEAR");
            return true;                                
        }
        //Debug.Log("Elevator " + elevatorNumber +" IS NOT NEAR");
        return false;
    }

    private void decelerate() {
        //full stop
        if (Mathf.Abs((yPosition - ((targetFloor * 2.5f) + 0.18f))) <= 0.06f) {
            rb.velocity = Vector3.zero;
            constantForce.force = new Vector3(0f, 0f, 0f);
            //Debug.Log("1423");
        }
        //slow down
        else if ((Mathf.Abs(rb.velocity[1]) <= 2f) && (currentFloorIndex == targetFloor)  && !goingDown) {
            applyForce = (float)(velocity[1] * -1) * 0.75f;
            //Debug.Log("Decelarating flat");
            constantForce.force = new Vector3(0f, force + ((velocity[1] * -velocity[1])), 0f);
        }
        else if (currentFloorIndex != targetFloor && (velocity[1] >= 1f & !goingDown)){
            applyForce = (float)(velocity[1] * 0.1);
            constantForce.force = new Vector3(0f, applyForce, 0f);
            //Debug.Log("Decelerating default");
        }
        else if (goingDown) {
            constantForce.force = new Vector3(0f, -1f, 0f);
        }
        else {
            //Debug.Log("No Changes on force;");
        }
    }
    private bool isDecelerating() {
        if (Mathf.Abs(velocitybefore) < Mathf.Abs(velocity[1])) return true;
        return false;
    }

   public  void descend() {
        
            goingUp = false;
            goingDown = true;
        
        constantForce.force = new Vector3(0f, -force, 0f);
    }
    

    //setters/getters
    public void setElevatorNumber(int number) {
        elevatorNumber = number;
    }

    public void setTargetFloor(int target) {
        targetFloor = target;
    }

    public void setCurrentFloorIndex(int floor) {
        currentFloorIndex = floor;
    }


    public float getyPosition() {
        return yPosition;
    }

    public float getVelocity() {
        return velocity[1];
    }

    public int getCurrentFloorIndex() {
        return currentFloorIndex;
    }

    //Debugger
    void showInfo() {
        Debug.Log("ElevatorNumber: " + elevatorNumber + 
                  "\nCurrentFloor: " + currentFloorIndex +
                  "\nTargetFloor: " + targetFloor + 
                  "\nIdle: " + idle + 
                  "\nGoing Up: " + goingUp +
                  "\nGoing Down: " + goingDown); 
    }




    //called once per frame
    void Update() {
        //showInfo();
        velocitybefore = velocity [1];
        velocity = rb.velocity;
        yPosition = transform.position.y;
        if (currentFloorIndex == targetFloor && !isMoving()) {
            goIdle();
        }
        else if (near()){
                                        //ignore when still slow()
            if (!isDecelerating() && (currentFloorIndex != targetFloor)) {
                goToFloor(targetFloor);
            } 
            else {
                decelerate();
            }
        }
        else if (currentFloorIndex != targetFloor && idle) {
            goToFloor(targetFloor);
            
        }
    }
}