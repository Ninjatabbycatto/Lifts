# Elevator Simulation with Unity and ML Agents

This is a **Simulation of elevators** made using **Unity**.

---

## Project Overview

The main goal for this project was to **check, simulate, and test whether integrating AI in elevators will make them more efficient** using **ML Agents** in Unity.  
Elevators are a critical component of modern buildings, and optimizing their operation can significantly improve user experience by reducing wait times and energy consumption. By leveraging machine learning, the system aims to learn optimal elevator scheduling and passenger handling strategies that adapt dynamically to traffic patterns.

Unfortunately, due to hardware limitations (using an M2 processor), I had to drop this project before fully completing it. However, the current implementation lays a solid foundation for further development and experimentation.

---

## Environment

This is what the environment looks like:

![Environment Screenshot](https://github.com/user-attachments/assets/f0054069-ded9-4ead-8f40-99c13fc11fbd)

- **38 floors**  
- **7 elevators**  
- Realistic elevator shafts and floor layouts modeled to simulate a typical high-rise building environment.

The simulation environment is designed to mimic real-world conditions as closely as possible, including elevator movement constraints and passenger behavior.

---

## Elevators

Here are the elevators in the simulation:

![Elevators Screenshot](https://github.com/user-attachments/assets/6f51634b-ad89-4747-a369-89407e9d1668)

Each elevator is controlled by the Agent if you run mlagents-learn, but can be controlled manually. This is done by using the function where the elevatorNumber is needed and the FloorNumber where it goes to.


---

## Simulation Details

Once you start training, **passengers will spawn on random floors** and follow a step-by-step procedure including:

- Pressing the call button  
- Waiting for the elevator  
- Entering the elevator once it stops on their floor  
- Traveling to their destination floor  

![Passenger Simulation](https://github.com/user-attachments/assets/5195aa08-8067-425f-90f5-5c1901bca425)

Passengers behave according to realistic timing and queueing rules, which adds complexity to the simulation and challenges the AI agents to learn effective scheduling policies.

---

## Controllers and Source Code

This screenshot shows the **Passenger Controller**, **Elevator Controller**, and the **Main Controller**.  
You can check out the source code to understand how the simulation operates and how the ML Agents were integrated.

![Controllers Screenshot](https://github.com/user-attachments/assets/700b69fe-bc95-47d7-ae87-8dd733676bcf)

The source code includes:

- **Passenger Controller:** Handles passenger spawning, button pressing, and movement logic.  
- **Elevator Controller:** Manages elevator movement, door states, and passenger boarding/alighting.  
- **Main Controller:** Coordinates overall simulation timing, spawning logic, and ML Agent training setup.

---

## Usage

Feel free to use this project and test it out.  
It would be great if you manage to make it work and train the model successfully!

To get started:

1. Clone the repository.  
2. Open the project in Unity (version 2023.x or later recommended).  
3. Configure ML Agents settings as needed.  
4. Run the simulation and observe the elevators responding to passenger requests.  
5. Start training the ML Agents to improve elevator efficiency over time.

---

## Future Plans

- Implementing a **passenger spawn distribution** that reflects real-life behaviors, especially in residential and commercial buildings. This will include peak hour traffic, uneven floor usage, and group arrivals.  
- Properly setting a **reward system** for the agents to encourage minimizing wait times, travel times, and energy usage.  
- Adding more complex elevator behaviors such as express elevators, priority handling for emergency services, and adaptive scheduling algorithms.  
- Enhancing the simulation with detailed analytics and visualization tools to monitor agent performance and passenger satisfaction metrics.

---

## Conclusion

This project demonstrates the potential of using AI and machine learning to optimize elevator systems, which are traditionally rule-based and static. With further development and training, such systems could lead to smarter buildings that adapt to occupant needs in real-time, improving convenience and energy efficiency.

*This project is a work in progress and a proof of concept for AI integration in elevator systems.*

---

If you have any questions, suggestions, or want to contribute, feel free to reach out or submit a pull request!
