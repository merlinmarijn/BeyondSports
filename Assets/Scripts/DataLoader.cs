using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class DataLoader : MonoBehaviour
{

    //This is a list filled with variable Data and contains all info for each frame.
    private List<Data> FrameData = new List<Data>();
    //Make a static reference to this script
    public static DataLoader current;

    //Create event for when data set is fully loaded and ready to be used
    public event Action<List<Data>> onDatasetLoaded;

    private void Awake()
    {
        //set static reference
        current = this;
        //load in csv file
        var gamedataset = Resources.Load<TextAsset>("GameData");
        //split it up per line and save it in temp var
        var splitdataset = gamedataset.text.Split(new char[] { '\n' });

        //foreach line of data go and split it and parse it, remove bad data blocks (empty/null/white space)
        for (int i = 0; i < splitdataset.Length; i++)
        {
            var splitedblocks = splitdataset[i].Split(':');

            //clean up block so that no spots remain with "Null" or "Empty" or "White Space"
            splitedblocks = splitedblocks.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            splitedblocks = splitedblocks.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            //create a data variable which we will put in the parsed line info
            Data CurrentData = new Data();

            //save frame data
            CurrentData.FramePoint = splitedblocks[0];

            //Make temp block for player data which is split
            var playerData = splitedblocks[1].Split(new char[] { ';' });

            //clean up block so that no spots remain with "Null" or "Empty" or "White Space"
            playerData = playerData.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            playerData = playerData.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();

            //for each part of player data split, and save in playerdata list
            for (int o = 0; o < playerData.Length; o++)
            {
                var ParsedPlayerData = playerData[o].Split(new char[] { ',' });
                CurrentData.PlayerData.Add(ParsedPlayerData);
            }

            //save ball data
            CurrentData.Balldata = splitedblocks[2].Split(new char[] { ',' });
            //add current data to framedata list for later us
            FrameData.Add(CurrentData);
        }

        //print a specific frame point out
        //PrintData(FrameData[0]);
    }

    private void Start()
    {
        //make sure that onDatassetLoaded isnt null before calling it
        if (onDatasetLoaded != null)
        {
            onDatasetLoaded(FrameData);
            return;
        }
    }

    //print data out so its readable in console
    public void PrintData(Data data)
    {
        print("Frame: " + data.FramePoint);
        foreach(string[] item in data.PlayerData)
        {
            print($"PlayerData: Team:{item[0]}, TrackingID:{item[1]}, PlayerNum:{item[2]}, X:{item[3]}, Y:{item[4]}, Rotation?:{item[8]}");
        }
        print($"BallData: X:{data.Balldata[0]}, Y:{data.Balldata[1]}, Z:{data.Balldata[2]}, Speed:{data.Balldata[3]}, ");
    }

    //get data from specific frame point
    public Data getData(int id)
    {
        
        return (id < FrameData.Count && id >= 0) ? FrameData[id] : null;
    }

    //get entire length of the data set
    public int getDataLength()
    {
        return FrameData.Count;
    }

}


public class Data
{
    public string FramePoint;
    public List<string[]> PlayerData = new List<string[]>();
    public string[] Balldata;
}


//Frame:
//FrameCount:[TrackedObjects][BallData]



//TrackedObject:
//Team,TrackingID,PlayerNumber,X - Position,Y - Position,Speed;



//BallData:
//:X - Position,Y - Position,Z - Position,BallSpeed,[ClickerFlags] 