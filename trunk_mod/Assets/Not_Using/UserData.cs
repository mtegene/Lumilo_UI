using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class UserData
{
    static Dictionary<string, Dictionary<string, string>> _dict = new Dictionary<string, Dictionary<string, string>>
    {
        {"testUser01",
        new Dictionary<string, string>
            {
                {"help_model", "unacceptable / hint abuse"},
                {"blabla", "20"},
                {"fufu", "4"}
            }
        }
    };

    public static Dictionary<string, string> GetUserData(string username)
    {
        // Try to get the result in the static Dictionary
        Dictionary<string, string> result;
        if (_dict.TryGetValue(username, out result))
        {
            return result;
        }
        else
        {
            return null;
        }
    }

    //public static void SetUserData(string username, string detectorName, string detectorValue)
    //{
        //if username is in _dict... 
            //if detectorName is in _dict[username].keys()
                // update value
            //else
                // add {detectorName, detectorValue} to _dict[username]
        //else 
            //add {username, {detectorName, detectorValue} } to _dict
        //
    //}

}