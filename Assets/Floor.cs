using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody physicalFloor;
    public int floor;
    public float yPosition, height = 2.5f;
    public bool upButton = false, downButton = false;
    private void Start()
    {
        
        physicalFloor = GetComponent<Rigidbody>();
        yPosition = transform.position.y;
        //Debug.Log("YPosition : " + yPosition + "\nFloor: " + floor);
    }

    public void setFloor(int floorNum) {
        floor = floorNum;
        //yPosition = height * floor;
        //add code to set the position of the object.
    }


    public void goingUp() {
        upButton = true;
    }

    public void goingDown() {
        downButton = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
