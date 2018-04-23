using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using UnityEngine;  // if in a unity project this will give access to application data path, otherwise you need to define the folder location

public class CSEventHandler
{

    static string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop) + Path.DirectorySeparatorChar; // folder directory for the desktop
    //static string folderPath = Application.dataPath + Path.DirectorySeparatorChar; // unity folder directory, if in engine will export to the assets folder, if in a build will place inside the data folder
    static string fileNameStart = "Session_";
    static string jsonExtensionString = ".json";
    static string eventTag = "Event_";

    public static void BasicEvent(string sessionID, string userID, string eventName)
    {
        string finalFilePath = folderPath + fileNameStart + sessionID + jsonExtensionString;
        // if the file exists then execute the function
        if (File.Exists(finalFilePath))
        {
            // open the json file and read all its lines to a string list
            List<string> jsonFile = File.ReadAllLines(finalFilePath).ToList();
            // the line position to add too
            int insertPos = 0;

            // if there isnt anything in the file path, add the first event along with the set up brackets for a json file
            if (jsonFile.Count == 0)
            {
                string lineToAdd = "";
                // add the first line to the json file, should just be a {
                lineToAdd = "{";
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++; // increase insert pos by 1
                             // add the initial event name set up // use "\"" to add a " to the string
                             // this should add the event name with an array to extend
                lineToAdd = ("\"" + eventTag + eventName + "\"" + ":" + "[");
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // insert a { to begin the event variables
                lineToAdd = "{";
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // add the sessionID
                lineToAdd = ("\"" + "SessionID" + "\"" + ":" + "\"" + sessionID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // add the userID
                lineToAdd = ("\"" + "UserID" + "\"" + ":" + "\"" + userID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // get the time as a string in a hour:minutes:seconds format
                string time = System.DateTime.Now.ToString("d/M/yyyy hh:mm:ss tt");
                lineToAdd = ("\"" + "Time" + "\"" + ":" + "\"" + time + "\"" + "}"); // this ends with a } closing off the data for this
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // close the array with ]
                lineToAdd = "]";
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // end the json file with the } for the one at the start{
                lineToAdd = "}";
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // add all lines to the file
                File.WriteAllLines(finalFilePath, jsonFile.ToArray());
            }
            // file exists and there is something in it
            else
            {
                // loop through all the lines searching for the event
                for (int mainSearchPos = 0; mainSearchPos < jsonFile.Count; mainSearchPos++)
                {
                    // check if this event has happened before
                    string searchFor = eventTag + eventName; // string will be: Event_eventName , where eventName is replaced with what ever the event is named
                    
                    // if file contains that event
                    if (jsonFile[mainSearchPos].Contains(string.Format("{0}", searchFor)))
                    {

                        int eventFoundAtPos = mainSearchPos; // position in the file that the event was originally found
                        int endSearchPos = jsonFile.Count; // the position that the next event type in the JSON file was found
                        

                        string endTag = "Time"; // currently this for looking for the time the event happened, this should be the last part of the event variable allowing us to insert an new event variable after it, continuing the array
                        int posOfInsert = 0;

                        // this is really jank and i hate it, but its the only way i can get it to filter through the jsonFile and search if it contains "time" or not
                        // loops through i -> jsonFile.count, updates the posOfInsert until it has reached the end of the list
                        // find index isnt working and i cant work it out, think its just to do with what the return value actually is
                        // start from the i line count and search till the end
                        // if you find another event type then exit the loop
                        for (int j = eventFoundAtPos; j < endSearchPos; j++)
                        {
                            // if found another event
                            // if another event isnt found will just continue till the end of the file
                            bool foundAnEventType = jsonFile[j].Contains(string.Format("{0}", eventTag));
                            bool foundThisEventType = jsonFile[j].Contains(string.Format("{0}", searchFor));
                            if (foundAnEventType && !foundThisEventType)
                            {
                                // stop searching through positions, we dont need to look further into the JSON file 
                                // because we have found another event type
                                break;
                            }

                            // found the tag for end of last log of this event
                            if (jsonFile[j].Contains(string.Format("{0}", endTag)))
                            {
                                posOfInsert = j + 1;
                            }
                        }
                        

                        string lineToAdd = "";

                        // position to insert the next event set of variables
                        // should take the position of the last event and then add onto that until complete
                        insertPos = posOfInsert;

                        // insert a , to indicate a new set of variables for this event type
                        lineToAdd = ",";
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // insert a { to begin the event variables
                        lineToAdd = "{";
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // add the sessionID
                        lineToAdd = ("\"" + "SessionID" + "\"" + ":" + "\"" + sessionID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // add the userID
                        lineToAdd = ("\"" + "UserID" + "\"" + ":" + "\"" + userID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // get the time as a string in a hour:minutes:seconds format
                        string time = System.DateTime.Now.ToString("d/M/yyyy hh:mm:ss tt");
                        lineToAdd = ("\"" + "Time" + "\"" + ":" + "\"" + time + "\"" + "}"); // this ends with a } closing off the data for this
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // add all lines to the file
                        File.WriteAllLines(finalFilePath, jsonFile.ToArray());
                        break;

                    }

                    // if this event hasnt happened before then create a new event of that type
                    // to do this we will search through for the first time ANY OTHER EVENT has happened then create this event type above that,
                    // need to extend the if an event has happened before to accomadate different event types
                    // this will make the change so that it also searches for another "Event_" and store thats location
                    // it will then make the search smaller when hunting for the last entry of that event
                    // so it will for example first search for the event name, if found it will then continue searching from there for any other events after it in the file
                    // if it finds one then it will search between the two locations for the last entry of its events and create an event there.

                    // if reached the bottom and that event was never found, create that event
                    else if (mainSearchPos == jsonFile.Count - 1) // 1 less than the max, since max will never be hit
                    {
                        // search for the start of an event
                        // this will allow us to enter the new event above it
                        // doing so will be faster than hunting for the end of that event and inserting a new event type there, especially when a file could have thousands of event entries
                        // 
                        for (int searchForEventTagPos = 0; searchForEventTagPos < jsonFile.Count; searchForEventTagPos++)
                        {
                            if (jsonFile[searchForEventTagPos].Contains(string.Format("{0}", eventTag)))
                            {
                                insertPos = searchForEventTagPos; // set the insert position to a line before the found event

                                // stop searching through positions, we dont need to look further into the JSON file 
                                // because we have found another event type
                                break;
                            }
                        }

                        string lineToAdd = "";
                        // starting a new event type
                        lineToAdd = ("\"" + eventTag + eventName + "\"" + ":" + "[");
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // insert a { to begin the event variables
                        lineToAdd = "{";
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // add the sessionID
                        lineToAdd = ("\"" + "SessionID" + "\"" + ":" + "\"" + sessionID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // add the userID
                        lineToAdd = ("\"" + "UserID" + "\"" + ":" + "\"" + userID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // get the time as a string in a hour:minutes:seconds format
                        string time = System.DateTime.Now.ToString("d/M/yyyy hh:mm:ss tt");
                        lineToAdd = ("\"" + "Time" + "\"" + ":" + "\"" + time + "\"" + "}"); // this ends with a } closing off the data for this
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // close the array with ]
                        lineToAdd = "]";
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // end the new event with a comma so that it is valid JSON
                        lineToAdd = ",";
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // add all lines to the file
                        File.WriteAllLines(finalFilePath, jsonFile.ToArray());
                        break;
                    }
                }
            }
        }
        else // file doesnt exist so create one and populate it
        {
            // create a new file for that path
            FileStream file = File.Create(finalFilePath); // assign to a filestream so it can be closed after creation, otherwise errors about access will be thrown
            file.Dispose();

            // create the string list for the empty file
            List<string> jsonFile = new List<string>();
            // the line position to add too
            int insertPos = 0;

            string lineToAdd = "";
            // add the first line to the json file, should just be a {
            lineToAdd = "{";
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++; // increase insert pos by 1
                         // add the initial event name set up // use "\"" to add a " to the string
                         // this should add the event name with an array to extend
            lineToAdd = ("\"" + eventTag + eventName + "\"" + ":" + "[");
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // insert a { to begin the event variables
            lineToAdd = "{";
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // add the sessionID
            lineToAdd = ("\"" + "SessionID" + "\"" + ":" + "\"" + sessionID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // add the userID
            lineToAdd = ("\"" + "UserID" + "\"" + ":" + "\"" + userID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // get the time as a string in a hour:minutes:seconds format
            string time = System.DateTime.Now.ToString("d/M/yyyy hh:mm:ss tt");
            lineToAdd = ("\"" + "Time" + "\"" + ":" + "\"" + time + "\"" + "}"); // this ends with a } closing off the data for this
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // close the array with ]
            lineToAdd = "]";
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // end the json file with the } for the one at the start{
            lineToAdd = "}";
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // add all lines to the file
            File.WriteAllLines(finalFilePath, jsonFile.ToArray());
        }

    }

    public static void BasicDictionaryEvent(string sessionID, string userID, string eventName, Dictionary<string, object> eventData)
    {
        string finalFilePath = folderPath + fileNameStart + sessionID + jsonExtensionString;

        // if the file exists then execute the function
        if (File.Exists(finalFilePath))
        {
            // open the json file and read all its lines to a string list
            List<string> jsonFile = File.ReadAllLines(finalFilePath).ToList();
            // the line position to add too
            int insertPos = 0;

            // if there isnt anything in the file path, add the first event along with the set up brackets for a json file
            if (jsonFile.Count == 0)
            {

                string lineToAdd = "";
                // add the first line to the json file, should just be a {
                lineToAdd = "{";
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++; // increase insert pos by 1
                             // add the initial event name set up // use "\"" to add a " to the string
                             // this should add the event name with an array to extend
                lineToAdd = ("\"" + eventTag + eventName + "\"" + ":" + "[");
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // insert a { to begin the event variables
                lineToAdd = "{";
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // loop through all the data in the dictionary, will return the updated insert position
                insertPos = BreakDownAndAddDictionaryData(eventData, lineToAdd, insertPos, jsonFile);

                // add the sessionID
                lineToAdd = ("\"" + "SessionID" + "\"" + ":" + "\"" + sessionID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // add the userID
                lineToAdd = ("\"" + "UserID" + "\"" + ":" + "\"" + userID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // get the time as a string in a hour:minutes:seconds format
                string time = System.DateTime.Now.ToString("d/M/yyyy hh:mm:ss tt");
                lineToAdd = ("\"" + "Time" + "\"" + ":" + "\"" + time + "\"" + "}"); // this ends with a } closing off the data for this
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // close the array with ]
                lineToAdd = "]";
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // end the json file with the } for the one at the start{
                lineToAdd = "}";
                jsonFile.Insert(insertPos, lineToAdd);
                insertPos++;

                // add all lines to the file
                File.WriteAllLines(finalFilePath, jsonFile.ToArray());
            }
            // file exists and there is something in it
            else
            {
                // loop through all the lines searching for the event
                for (int mainSearchPos = 0; mainSearchPos < jsonFile.Count; mainSearchPos++)
                {
                    // check if this event has happened before
                    string searchFor = eventTag + eventName; // string will be: Event_eventName , where eventName is replaced with what ever the event is named
                                                             // if file contains that event
                    if (jsonFile[mainSearchPos].Contains(string.Format("{0}", searchFor)))
                    {

                        int eventFoundAtPos = mainSearchPos; // position in the file that the event was originally found
                        int endSearchPos = jsonFile.Count; // the position that the next event type in the JSON file was found

                        string endTag = "Time"; // currently this for looking for the time the event happened, this should be the last part of the event variable allowing us to insert an new event variable after it, continuing the array
                        int posOfInsert = 0;

                        // this is really janky and i hate it, but its the only way i can get it to filter through the jsonFile and search if it contains "time" or not
                        // loops through i -> jsonFile.count, updates the posOfInsert until it has reached the end of the list
                        // find index isnt working and i cant work it out, think its just to do with what the return value actually is
                        // start from the i line count and search till the end
                        // if you find another event type then exit the loop
                        for (int j = eventFoundAtPos; j < endSearchPos; j++)
                        {
                            // if found another event
                            // if another event isnt found will just continue till the end of the file
                            bool foundAnEventType = jsonFile[j].Contains(string.Format("{0}", eventTag));
                            bool foundThisEventType = jsonFile[j].Contains(string.Format("{0}", searchFor));
                            if (foundAnEventType && !foundThisEventType)
                            {
                                // stop searching through positions, we dont need to look further into the JSON file 
                                // because we have found another event type
                                break;
                            }

                            // found the tag for end of last log of this event
                            if (jsonFile[j].Contains(string.Format("{0}", endTag)))
                            {
                                posOfInsert = j + 1;
                            }
                        }
                        

                        string lineToAdd = "";

                        // set the insert position to insert the next event set of variables
                        // should take the position of the last event found and then add onto that until complete
                        insertPos = posOfInsert;

                        // insert a , to indicate a new set of variables for this event type
                        lineToAdd = ",";
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // insert a { to begin the event variables
                        lineToAdd = "{";
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // loop through all the data in the dictionary, will return the updated insert position
                        insertPos = BreakDownAndAddDictionaryData(eventData, lineToAdd, insertPos, jsonFile);

                        // add the sessionID
                        lineToAdd = ("\"" + "SessionID" + "\"" + ":" + "\"" + sessionID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // add the userID
                        lineToAdd = ("\"" + "UserID" + "\"" + ":" + "\"" + userID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // get the time as a string in a hour:minutes:seconds format
                        string time = System.DateTime.Now.ToString("d/M/yyyy hh:mm:ss tt");
                        lineToAdd = ("\"" + "Time" + "\"" + ":" + "\"" + time + "\"" + "}"); // this ends with a } closing off the data for this
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // add all lines to the file
                        File.WriteAllLines(finalFilePath, jsonFile.ToArray());
                        break;

                    }

                    // if this event hasnt happened before then create a new event of that type
                    // to do this we will search through for the first time ANY OTHER EVENT has happened then create this event type above that,
                    // need to extend the if an event has happened before to accomadate different event types
                    // this will make the change so that it also searches for another "Event_" and store thats location
                    // it will then make the search smaller when hunting for the last entry of that event
                    // so it will for example first search for the event name, if found it will then continue searching from there for any other events after it in the file
                    // if it finds one then it will search between the two locations for the last entry of its events and create an event there.

                    // if reached the bottom and that event was never found, create that event
                    else if (mainSearchPos == jsonFile.Count - 1) // 1 less than the max, since max will never be hit
                    {
                        // search for the start of an event
                        // this will allow us to enter the new event above it
                        // doing so will be faster than hunting for the end of that event and inserting a new event type there, especially when a file could have thousands of event entries
                        // 
                        for (int searchForEventTagPos = 0; searchForEventTagPos < jsonFile.Count; searchForEventTagPos++)
                        {
                            if (jsonFile[searchForEventTagPos].Contains(string.Format("{0}", eventTag)))
                            {
                                insertPos = searchForEventTagPos; // set the insert position to a line before the found event

                                // stop searching through positions, we dont need to look further into the JSON file 
                                // because we have found another event type
                                break;
                            }
                        }

                        string lineToAdd = "";
                        // starting a new event type
                        lineToAdd = ("\"" + eventTag + eventName + "\"" + ":" + "[");
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // insert a { to begin the event variables
                        lineToAdd = "{";
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // loop through all the data in the dictionary, will return the updated insert position
                        insertPos = BreakDownAndAddDictionaryData(eventData, lineToAdd, insertPos, jsonFile);

                        // add the sessionID
                        lineToAdd = ("\"" + "SessionID" + "\"" + ":" + "\"" + sessionID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // add the userID
                        lineToAdd = ("\"" + "UserID" + "\"" + ":" + "\"" + userID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // get the time as a string in a hour:minutes:seconds format
                        string time = System.DateTime.Now.ToString("d/M/yyyy hh:mm:ss tt");
                        lineToAdd = ("\"" + "Time" + "\"" + ":" + "\"" + time + "\"" + "}"); // this ends with a } closing off the data for this
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // close the array with ]
                        lineToAdd = "]";
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // end the new event with a comma so that it is valid JSON
                        lineToAdd = ",";
                        jsonFile.Insert(insertPos, lineToAdd);
                        insertPos++;

                        // add all lines to the file
                        File.WriteAllLines(finalFilePath, jsonFile.ToArray());
                        break;
                    }
                }
            }
        }
        else // file doesnt exist so create one and populate it
        {
            // create a new file for that path
            FileStream file = File.Create(finalFilePath); // assign to a filestream so it can be closed after creation, otherwise errors about access will be thrown
            file.Dispose();

            // create the string list for the empty file
            List<string> jsonFile = new List<string>();
            // the line position to add too
            int insertPos = 0;

            string lineToAdd = "";
            // add the first line to the json file, should just be a {
            lineToAdd = "{";
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++; // increase insert pos by 1
                         // add the initial event name set up // use "\"" to add a " to the string
                         // this should add the event name with an array to extend
            lineToAdd = ("\"" + eventTag + eventName + "\"" + ":" + "[");
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // insert a { to begin the event variables
            lineToAdd = "{";
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;


            // loop through all the data in the dictionary, will return the updated insert position
            insertPos = BreakDownAndAddDictionaryData(eventData, lineToAdd, insertPos, jsonFile);


            // add the sessionID
            lineToAdd = ("\"" + "SessionID" + "\"" + ":" + "\"" + sessionID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // add the userID
            lineToAdd = ("\"" + "UserID" + "\"" + ":" + "\"" + userID + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // get the time as a string in a hour:minutes:seconds format
            string time = System.DateTime.Now.ToString("d/M/yyyy hh:mm:ss tt");
            lineToAdd = ("\"" + "Time" + "\"" + ":" + "\"" + time + "\"" + "}"); // this ends with a } closing off the data for this
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // close the array with ]
            lineToAdd = "]";
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // end the json file with the } for the one at the start{
            lineToAdd = "}";
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;

            // add all lines to the file
            File.WriteAllLines(finalFilePath, jsonFile.ToArray());
        }

    }

    // take in the dictionary, loop through all the data inside the directroy and add it to the json file
    static int BreakDownAndAddDictionaryData(Dictionary<string, object> eventData,string lineToAdd,int insertPos, List<string> jsonFile)
    {
    foreach (var ed in eventData)
    {
        // if is int
        if (ed.Value.GetType().Equals(typeof(int)))
        {
            lineToAdd = ("\"" + ed.Key + "\"" + ":" + ed.Value + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;
        }
        else if (ed.Value.GetType().Equals(typeof(uint)))
        {
            lineToAdd = ("\"" + ed.Key + "\"" + ":" + ed.Value + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;
        }
        else if (ed.Value.GetType().Equals(typeof(short)))
        {
            lineToAdd = ("\"" + ed.Key + "\"" + ":" + ed.Value + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;
        }
        else if (ed.Value.GetType().Equals(typeof(ushort)))
        {
            lineToAdd = ("\"" + ed.Key + "\"" + ":" + ed.Value + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;
        }
        else if (ed.Value.GetType().Equals(typeof(long)))
        {
            lineToAdd = ("\"" + ed.Key + "\"" + ":" + ed.Value + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;
        }
        else if (ed.Value.GetType().Equals(typeof(ulong)))
        {
            lineToAdd = ("\"" + ed.Key + "\"" + ":" + ed.Value + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;
        }
        else if (ed.Value.GetType().Equals(typeof(double)))
        {
            lineToAdd = ("\"" + ed.Key + "\"" + ":" + ed.Value + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;
        }
        else if (ed.Value.GetType().Equals(typeof(float)))
        {
            lineToAdd = ("\"" + ed.Key + "\"" + ":" + ed.Value + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;
        }
        else if (ed.Value.GetType().Equals(typeof(bool)))
        {
            bool boolValue = (bool)ed.Value;
            if (boolValue == true)
            {
                lineToAdd = ("\"" + ed.Key + "\"" + ":" + "true" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            }
            else if (boolValue == false)
            {
                lineToAdd = ("\"" + ed.Key + "\"" + ":" + "false" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            }
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;
        }
        else
        {
            lineToAdd = ("\"" + ed.Key + "\"" + ":" + "\"" + ed.Value + "\"" + ","); // make sure there is a comma between the data, the last data type having a comma will be fine because all event data will then have a time stamp added to them
            jsonFile.Insert(insertPos, lineToAdd);
            insertPos++;
        }
    }
        return insertPos;
    }


}
