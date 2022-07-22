using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animator : MonoBehaviour
{
    //in this dictionary we keep track of all player objects so we can move them in real time.
    Dictionary<string, GameObject> Player = new Dictionary<string, GameObject>();

    //private GameObject player;
    //private string playernum;
    //current frame of animation were on, starts at 1 cause 0 we init via the SpawnPlayers script
    private int frame = 1;
    //reference to the ball object
    private GameObject ball;
    //Bool to check if were current going forward in the dataset
    private bool forward = true;
    //Bool to check if were allowed to reverse the data set at the end
    private bool Reverbloop = false;
    //Static reference to this script
    public static Animator current;
    //Delay per frame, standard 25 fps
    private float AnimDelay = 25;


    private void Awake()
    {
        current = this;
    }

    private void Start()
    {
        //Create call events for onInitPlayerFinished to add player to local dictionary to be tracked
        SpawnPlayers.current.onInitPlayerFinished += AddPlayerToDictionary;
        //Create call events for onInitBallFinished to add ball to local reference variable
        SpawnPlayers.current.onInitBallFinished += AddBallToVariable;
        //Create call events for onTeamInitFinished to start animation
        SpawnPlayers.current.onTeamInitFinished += startAnimation;
    }


    //add player to local dictionary for reference and to direct
    public void AddPlayerToDictionary(GameObject player, string playernum)
    {
        Player.Add(playernum, player);
        //print(Player[playernum]);
    }

    //add ball to local variable for reference and to direct
    public void AddBallToVariable(GameObject b)
    {
        ball = b;
    }

    //call function to set animation delay to something else when needed
    public void SetAnimDelay(float delay)
    {
        CancelInvoke("Animate");
        AnimDelay = delay;
        startAnimation();
    }

    //Update Reverse sets if were allowed to reverse
    public void UpdateReverse(bool b)
    {
        CancelInvoke("Animate");
        Reverbloop = b;
        startAnimation();
    }


    //Standard call to start animation, this is done through a invokerepeating on a delay
    public void startAnimation()
    {
        InvokeRepeating("Animate", 1f / AnimDelay, 1f / AnimDelay);
    }


    //Animate players/balls
    public void Animate()
    {
        //Get data from "Frame"
        Data data = DataLoader.current.getData(frame);

        //if this data isnt null
        if (data != null)
        {
            //loop through all players in dictionary (done through "Team+PlayerNum"), set position
            foreach (string[] item in data.PlayerData)
            {
                Player[item[0] + item[2]].transform.position = new Vector3(float.Parse(item[3]) / 30, 0, float.Parse(item[4]) / 30);
            }
            //set ball position
            ball.transform.position = new Vector3(float.Parse(data.Balldata[0]) / 30, 0, float.Parse(data.Balldata[1]) / 30);
        }
        //if data was null (this is cause were out of bounds of the array index) and were allowed to reverse, reverse animation
        else if(Reverbloop)
        {
            forward = !forward;
            //clamp frame since its now out of bounds
            frame = Mathf.Clamp(frame, 0, DataLoader.current.getDataLength());
        }
        else 
        {
            //if were out of bounds and arent allowed to reverse cancel the invokerepeating on animation to stop trying to animate when we cant.
            CancelInvoke("Animate"); 
        }
        //if were going forward in animation ++ else -- the frame index
        if (forward)
        {
            frame++;
        } else
        {
            frame--;
        }
    }

    //When paused look at current frame and -1 or +1 it and animate that frame, animation is same as above just manually triggered via a button
    public void ManualAnimate(int d)
    {
        frame += d;
        frame = Mathf.Clamp(frame, 0, DataLoader.current.getDataLength());
        Data data = DataLoader.current.getData(frame);

        if (data != null)
        {
            foreach (string[] item in data.PlayerData)
            {
                Player[item[0] + item[2]].transform.position = new Vector3(float.Parse(item[3]) / 30, 0, float.Parse(item[4]) / 30);
            }
            ball.transform.position = new Vector3(float.Parse(data.Balldata[0]) / 30, 0, float.Parse(data.Balldata[1]) / 30);
        }
    }
}
