﻿/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          FileManager
 *      Description:       
 *-----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                  
 *      Date:           7/18/2019
 *      Notes:          
 *                      
 *      Revision History:
 *      
 *      (7/18/2019) - Added to handle CSV file selection.
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class FileManager : MonoBehaviour
{
    string path;
    public RawImage image;

    public void OpenExplore()
    {
        path = EditorUtility.OpenFilePanel("Overwrite with new CSV", "","csv");
        GetFile();
    }

    void GetFile ()
    {
        if (path != null)
        {
            UpdateCSVFile();
        }
    }

    void UpdateCSVFile ()
    {
        WWW www;

        string operatingSystem = SystemInfo.operatingSystem;

        if (operatingSystem.Contains("Windows"))
        {
            
        }
        else if (operatingSystem.Contains("Mac"))
        {

        }
        else
        {
            return;
        }

        www.



    }
}