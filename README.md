# Collapse-Blast Match3 Game Mechanics Overview
This is a prototype of match3 game with the core mechanics **made in Unity**.

## Core Mechanics

**Blast System:** Players can blast groups of blocks by clicking on them, removing them from the board.

**Drop Mechanism:** Remaining blocks fall to fill empty spaces after a blast.

**Group Detection:** Identifies clusters of adjacent blocks based on size.

**Icon Updates:** Icons and effects update dynamically based on the size of the detected block group.

**Smart Shuffle:** Prevents deadlock scenarios by intelligently shuffling the board when no valid moves remain.


## About the Performance

Optimization is a key focus of this project. **Garbage collection is minimized** to prevent unnecessary memory allocations and reduce performance spikes.  
**Object pooling** is implemented to efficiently reuse game objects, avoiding the overhead of frequent instantiation and destruction. Additionally, optimized animations with DOTween ensure smooth transitions without adding unnecessary computational load.

![image](https://github.com/user-attachments/assets/0a51d02b-1236-40db-a4b5-b71bc415db86)
