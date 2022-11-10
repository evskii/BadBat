using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;

using TMPro;

using UnityEngine;
using UnityEngine.Analytics;

using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class LevelGenerator : MonoBehaviour
{
    public LevelSettings levelSettings;
    public GameObject[] rooms;
    public int roomDistances = 30;
    public int connectorDistances = 12;
    public List<GameObject> roomsSpawned = new List<GameObject>();

    private void Start() {
        StartCoroutine(GenerateLevel(levelSettings));
    }

    [ContextMenu("New Generation")]
    public void GenerateLevel() {
        StartCoroutine(GenerateLevel(levelSettings));
    }
    
    public IEnumerator GenerateLevel(LevelSettings levelSettings) {
        roomsSpawned.Clear();
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        
        //Always start with spawning a 4 door room [For now]
        var firstRoom = GetRoom(4);
        Instantiate(firstRoom, Vector3.zero, Quaternion.identity, transform);
        firstRoom.name = "Room_Main";
        roomsSpawned.Add(firstRoom);
        
        //Pick a door to start spawning from
        var connectors = firstRoom.GetComponentsInChildren<RoomConnector>().Where(connector => connector.ConnectorType == Room.ConnectorType.Door).ToList();
        RoomConnector mainConnector = connectors[Random.Range(0, connectors.Count)];
        
        //Spawn in the first run of rooms
        for (int i = 1; i < levelSettings.mainPathLength; i++) {
            var nextRoomPos = GetNextRoomPosition(roomsSpawned[roomsSpawned.Count - 1].transform.position, mainConnector.transform.position);
            
            if (!IsSpaceFree(nextRoomPos)) { //If the space is not free we pick a different connector to use [Gets rid of rooms spawning on top of each other]
                Debug.Log("Space [" + nextRoomPos + "] is not free!");
                connectors.Clear();
                connectors = roomsSpawned[roomsSpawned.Count - 1].GetComponentsInChildren<RoomConnector>().Where(connector => connector.ConnectorType == Room.ConnectorType.Door).ToList();
                connectors.Remove(mainConnector);
                mainConnector = connectors[Random.Range(0, connectors.Count)];
                i--;
                continue;
            }


            List<Vector3> surroundingPositions = new List<Vector3>();
            surroundingPositions.Add(nextRoomPos + new Vector3(roomDistances, 0, 0));
            surroundingPositions.Add(nextRoomPos + new Vector3(-roomDistances, 0, 0));
            surroundingPositions.Add(nextRoomPos + new Vector3(0, 0, roomDistances));
            surroundingPositions.Add(nextRoomPos + new Vector3(0, 0, -roomDistances));

            List<GameObject> surroundingRooms = new List<GameObject>();
            foreach (var position in surroundingPositions) {
                for (int x = 0; x < roomsSpawned.Count; x++) {
                    if (roomsSpawned[x].transform.position == position) {
                        surroundingRooms.Add(roomsSpawned[x]);
                    }
                }
            }

            int doorCount = 4;
            foreach (var room in surroundingRooms) {
                Vector3 connectorPosition = GetConnectorPosition(room.transform.position, nextRoomPos);
                var roomConnectors = room.GetComponentsInChildren<RoomConnector>();
                foreach (var connector in roomConnectors) {
                    if (Vector3.Distance(connector.transform.position , connectorPosition) < 2) {
                        if (connector.ConnectorType == Room.ConnectorType.Wall) {
                            doorCount--;
                        }
                    }
                }
            }
            doorCount = Mathf.Clamp(doorCount, 2, 4);
            var possibleRooms = GetRooms(2, doorCount).ToList();
            GameObject chosenRoom = null;
            if (possibleRooms.Count() == 0) {
                Debug.LogError("NO POSSIBLE ROOMS");
            } else {
                while (possibleRooms.Count != 0) {
                    int roomIndex = Random.Range(0, possibleRooms.Count());
                    var newRoom = Instantiate(possibleRooms[roomIndex], nextRoomPos, Quaternion.identity, transform); 
                    
                    bool connected = false;
                    int itereations = 0;
                    while (!connected) {
                        var roomsConnectors = newRoom.GetComponentsInChildren<RoomConnector>();
                        int connetionScore = 4;
                        foreach (var connector in roomsConnectors) {
                            var temp = GetNextRoomPosition(newRoom.transform.position, connector.transform.position);
                            GameObject tempRoom = null;
                            for (int x = 0; x < roomsSpawned.Count; x++) {
                                if (roomsSpawned[x].transform.position == temp) {
                                    tempRoom = roomsSpawned[x];
                                }
                            }
                            if (tempRoom == null) {
                                continue;
                            } 
                            
                            var tempRoomConnectors = tempRoom.GetComponentsInChildren<RoomConnector>();
                            foreach (var con in tempRoomConnectors) {
                                if (Vector3.Distance(con.transform.position, connector.transform.position) < 2) {
                                    if (con.ConnectorType != connector.ConnectorType) {
                                        connetionScore--;
                                    } else {
                                        continue;
                                    }
                                }
                            }
                        }
                    
                        if (connetionScore < 4) {
                            newRoom.transform.Rotate(0, 90, 0);
                            itereations++;
                            if (itereations == 4) {
                                
                                possibleRooms.Remove(possibleRooms[roomIndex]);
                                Destroy(newRoom);
                                Debug.Log("Iterations reached. Room destroyed");
                                break;
                            }
                        } else {
                            if (IsRoomClosed(newRoom)) {
                                possibleRooms.Remove(possibleRooms[roomIndex]);
                                Destroy(newRoom);
                                Debug.Log("Attempted lockin. Room destroyed");
                                break;
                            }
                            
                            Debug.Log("Conenction Made");
                            possibleRooms.Clear();
                            chosenRoom = newRoom;
                            break;
                        }
                    }
                }
            }
            
            
            //Get the next connector
            connectors.Clear();
            connectors = chosenRoom.GetComponentsInChildren<RoomConnector>().Where(connector => connector.ConnectorType == Room.ConnectorType.Door).ToList();
            roomsSpawned.Add(chosenRoom);
            chosenRoom.name = "Room_" + i;
            mainConnector = connectors[Random.Range(0, connectors.Count)];
            yield return new WaitForSeconds(0.1f);
        }
        
        //Loop through all rooms and get a list of rooms that are still open
        foreach (var room in roomsSpawned) {
            // Debug.Log("Room ["  + room.name + "] IsRoomClosed = " + IsRoomClosed(room));
            if (!IsRoomClosed(room)) {
                //Find the sides of the room that are open
                var openDoors = room.GetComponentsInChildren<RoomConnector>().Where(connector => connector.ConnectorType == Room.ConnectorType.Door).ToList();
                foreach (var door in openDoors) {
                    var nextRoomPos = GetNextRoomPosition(room.transform.position, door.transform.position);
                    if (IsSpaceFree(nextRoomPos)) {
                        //Find how many doors are needed to close the space
                        List<Vector3> surroundingPositions = new List<Vector3>();
                        surroundingPositions.Add(nextRoomPos + new Vector3(roomDistances, 0, 0));
                        surroundingPositions.Add(nextRoomPos + new Vector3(-roomDistances, 0, 0));
                        surroundingPositions.Add(nextRoomPos + new Vector3(0, 0, roomDistances));
                        surroundingPositions.Add(nextRoomPos + new Vector3(0, 0, -roomDistances));
                        
                        List<GameObject> surroundingRooms = new List<GameObject>();
                        foreach (var position in surroundingPositions) {
                            for (int x = 0; x < roomsSpawned.Count; x++) {
                                if (roomsSpawned[x].transform.position == position) {
                                    surroundingRooms.Add(roomsSpawned[x]);
                                }
                            }
                        }

                        int doorCount = 0;
                        foreach (var tempRoom in surroundingRooms) {
                            Vector3 connectorPosition = GetConnectorPosition(tempRoom.transform.position, nextRoomPos);
                            var roomConnectors = tempRoom.GetComponentsInChildren<RoomConnector>();
                            foreach (var connector in roomConnectors) {
                                if (Vector3.Distance(connector.transform.position , connectorPosition) < 2) {
                                    if (connector.ConnectorType == Room.ConnectorType.Door) {
                                        doorCount++;
                                    }
                                }
                            }
                        }
                        
                        //Get a list of rooms with those doors
                        var possibleRooms = GetRooms(doorCount, doorCount).ToList();
                        GameObject chosenRoom = null;
                        if (possibleRooms.Count() == 0) {
                            Debug.LogError("NO POSSIBLE ROOMS");
                        } else {
                            while (possibleRooms.Count != 0) {
                                int roomIndex = Random.Range(0, possibleRooms.Count());
                                var newRoom = Instantiate(possibleRooms[roomIndex], nextRoomPos, Quaternion.identity, transform); 
                                
                                bool connected = false;
                                int itereations = 0;
                                while (!connected) {
                                    var roomsConnectors = newRoom.GetComponentsInChildren<RoomConnector>();
                                    int connetionScore = 4;
                                    foreach (var connector in roomsConnectors) {
                                        var temp = GetNextRoomPosition(newRoom.transform.position, connector.transform.position);
                                        GameObject tempRoom = null;
                                        for (int x = 0; x < roomsSpawned.Count; x++) {
                                            if (roomsSpawned[x].transform.position == temp) {
                                                tempRoom = roomsSpawned[x];
                                            }
                                        }
                                        if (tempRoom == null) {
                                            continue;
                                        } 
                                        
                                        var tempRoomConnectors = tempRoom.GetComponentsInChildren<RoomConnector>();
                                        foreach (var con in tempRoomConnectors) {
                                            if (Vector3.Distance(con.transform.position, connector.transform.position) < 2) {
                                                if (con.ConnectorType != connector.ConnectorType) {
                                                    connetionScore--;
                                                } else {
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                
                                    if (connetionScore < 4) {
                                        newRoom.transform.Rotate(0, 90, 0);
                                        itereations++;
                                        if (itereations == 4) {
                                            
                                            possibleRooms.Remove(possibleRooms[roomIndex]);
                                            Destroy(newRoom);
                                            Debug.Log("Iterations reached. Room destroyed");
                                            break;
                                        }
                                    } else {
                                        Debug.Log("Conenction Made");
                                        possibleRooms.Clear();
                                        chosenRoom = newRoom;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private GameObject GetRoom(int doors) {
        foreach (var room in rooms) {
            if (room.GetComponent<Room>().doorCount == doors) {
                return room;
            }
        }
        Debug.LogError("Cannot find a door with [" + doors + "] doors...");
        return null;
    }

    private GameObject GetRoom(int doorsMin, int doorsMax) {
        var possibleChoices = rooms.Where(rooms => rooms.GetComponent<Room>().doorCount >= doorsMin && rooms.GetComponent<Room>().doorCount <= doorsMax).ToArray();
        return possibleChoices[Random.Range(0, possibleChoices.Length)];
    }

    private GameObject[] GetRooms(int doorsMin, int doorsMax) {
        return rooms.Where(rooms => rooms.GetComponent<Room>().doorCount >= doorsMin && rooms.GetComponent<Room>().doorCount <= doorsMax).ToArray();
    }

    private Vector3 GetNextRoomPosition(Vector3 currentPos, Vector3 doorPos) {
        Vector3 raw = new Vector3((doorPos.x - currentPos.x), 0, (doorPos.z - currentPos.z));
        Vector3 normal = Vector3.Normalize(raw);
        Vector3 rounded = new Vector3(Mathf.RoundToInt(normal.x), 0, Mathf.RoundToInt(normal.z));
        Vector3 scaled = rounded * roomDistances;
        return scaled + currentPos;
    }

    private Vector3 GetConnectorPosition(Vector3 firstRoom, Vector3 secondRoom) {
        Vector3 raw = new Vector3((secondRoom.x - firstRoom.x), 0, (secondRoom.z - firstRoom.z));
        Vector3 normal = Vector3.Normalize(raw);
        Vector3 rounded = new Vector3(Mathf.RoundToInt(normal.x), 0, Mathf.RoundToInt(normal.z));
        Vector3 scaled = rounded * connectorDistances;
        return scaled + firstRoom;
    }

    private bool IsSpaceFree(Vector3 spaceToCheck) {
        foreach (var room in roomsSpawned) {
            // Debug.Log("SpaceToCheck: " + spaceToCheck + "\nRoomPos: " + room.transform.position);
            if (room.transform.position == spaceToCheck) {
                return false;
            }
        }
        return true;
    }

    private bool IsRoomClosed(GameObject roomToCheck) {
        //Get each door in the room
        var doors = roomToCheck.GetComponentsInChildren<RoomConnector>().Where(connector => connector.ConnectorType == Room.ConnectorType.Door).ToList();
        var totalDoors = doors.Count();
        if (totalDoors == 0) { return true;}
        
        //Check if space is not free on side of room there is a door
        foreach (var door in doors) {
            var nextPos = GetNextRoomPosition(roomToCheck.transform.position, door.transform.position);
            if (!IsSpaceFree(nextPos)) {
                totalDoors--;
            }
        }

        return totalDoors == 0;
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        if (roomsSpawned.Count() > 0) {
            for (int i = 1; i < roomsSpawned.Count; i++) {
                Debug.DrawLine(roomsSpawned[i].transform.position, roomsSpawned[i-1].transform.position);
            }
        }
    }
}
