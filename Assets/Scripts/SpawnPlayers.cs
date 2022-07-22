using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnPlayers : MonoBehaviour
{
    //create data variable so it can be used together with action
    private List<Data> data;

    //This is the current frame that were on right now
    private int CurrentFrame;

    //this is the container to put players/ball in so that the UI in editor stays clean
    public GameObject PC;


    //Action for when your done with initualizing players,ball,team
    public event Action<GameObject,string> onInitPlayerFinished;
    public event Action<GameObject> onInitBallFinished;
    public event Action onTeamInitFinished;
    //this makes static reference to this script
    public static SpawnPlayers current;

    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //when on dataset gets called from dataloader, call spawnplayer to setup player models.
        DataLoader.current.onDatasetLoaded += SpawnPlayer;
    }


    public void SpawnPlayer(List<Data> data)
    {
        //Load in all playerdata from frame 0
        foreach (string[] item in data[0].PlayerData)
        {
            //spawn players as cubes and give color based on team, 0 = Green, 1 = red
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            if (int.Parse(item[0]) == 0)
            {
                cube.GetComponent<MeshRenderer>().material.color = Color.green;
            }
            else
            {
                cube.GetComponent<MeshRenderer>().material.color = Color.red;
            }
            //Set each players position where it was at frame #0
            cube.transform.position = new Vector3(float.Parse(item[3])/30, 0, float.Parse(item[4])/30);
            cube.transform.name = $"Player: {item[2]}";
            //add cube to parent object to keep Editor UI clean
            cube.transform.parent = PC.transform;
            //Call onInitPlayerFinished to sent data over to the animator, this will add it to its dictionary to keep track in world space.
            if (onInitPlayerFinished != null)
            {
                onInitPlayerFinished(cube, item[0]+item[2]);
            }
        }
        //spawn ball as sphere, set postion, set color, add to parent object to keep editor UI clean
        GameObject Ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Ball.transform.position = new Vector3(float.Parse(data[CurrentFrame].Balldata[0]) / 30, 0, float.Parse(data[CurrentFrame].Balldata[1]) / 30);
        Ball.GetComponent<MeshRenderer>().material.color = Color.black;
        Ball.transform.parent = PC.transform;

        //if onInitBallFinished is not null call it to add it do a reference in animator so it can be tracked/manipulated.
        if (onInitBallFinished!=null)
        {
            onInitBallFinished(Ball);
        }
        //if onTeamInitFinished is not null call it to say to the animator that were ready to start animating the dataset.
        if (onTeamInitFinished != null)
        {
            onTeamInitFinished();
        }
    }
}
