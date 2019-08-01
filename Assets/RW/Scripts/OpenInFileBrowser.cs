/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          OpenInFileBrowser
 *      Description:    Class is used to find a .csv file on the file system
 *                      of Win and Mac OS. 
 *-----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                  
 *      Date:           7/30/2019
 *      Notes:          
 *                      
 *      Revision History:
 *-----------------------------------------------------------------------------
 * This program is free software: you can redistribute it and/or modify it 
 * under the terms of the GNU General Public License as published by the Free 
 * Software Foundation, either version 3 of the License, or (at your option) any 
 * later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT 
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
 * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 */
using UnityEngine;
using UnityEditor;

public class OpenInFileBrowser : MonoBehaviour
{
    /// <summary>
    /// Attempt to find a .csv file on the file system. This function will return a
    /// string array containing the file name (index 0) and the entire path to this 
    /// file (index 1). If this function tries to return a file that does not end
    /// with .csv, the return value will be a single array value containing the 
    /// word "error"
    /// </summary>
    public static string[] SelectCSVFile()
    {
        string path = EditorUtility.OpenFilePanel("Select .CSV File", "", "csv");
        string[] error = { "error" };
        // If the is something in the path, continue trying to get the file name.
        if (path.Length != 0)
        {
            // Parse for forward and backward slashes. This is done to handle both
            // Window and Mac OS
            string[] delimiters = { "/", "\\" };
            string[] words = path.Split(delimiters, System.StringSplitOptions.RemoveEmptyEntries);
            int lastIndex = words.Length - 1;
            string fileName = words[lastIndex];
            // I had a weird error, so I added this extra measure to get the file correctly
            if (IsInMacOS)
            {
                Debug.Log("MAC");
                path = path.Replace("\\","/");
            }
            else if (IsInWinOS)
            {
                Debug.Log("Window");
                path = path.Replace("/", "\\");
            }
            else
            {
                Debug.Log(error);
                return error;
            }

            if(fileName.Contains(".csv"))
            {
                string[] returnString = { fileName, path };
                // Return just the file name. If anything else is need at this 
                // point, another file should handle this
                return returnString;
            }
        }
        return error;
    }
    /// <summary>
    /// Checks whether or not the operating system running is a MAC
    /// </summary>
    public static bool IsInMacOS
    {
        get
        {
            if (SystemInfo.operatingSystem.Contains("MAC"))
            {
                return true;
            }
            return false;
        }
    }
    /// <summary>
    /// Check whether or not the operating system running is a Windows machine
    /// </summary>
    public static bool IsInWinOS
    {
        get
        {
            if(SystemInfo.operatingSystem.Contains("Window"))
            {
                return true;
            }
            return false;
        }
    }
}