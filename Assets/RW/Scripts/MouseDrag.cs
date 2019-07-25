/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          MouseDrag
 *      Description:       
 *-----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                  
 *      Date:           7/11/2019
 *      Notes:          This script was derived from the following tutorial
 *                      https://www.youtube.com/watch?v=pK1CbnE2VsI
 *                      
 *      Revision History:
 *      
 *      (7/11/2019) - Software Refactor - This file was generated to handle the
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

using UnityEngine.UI;
using UnityEngine;
using System.Text.RegularExpressions;

//this script colaborates to the tutorial
//https://www.youtube.com/watch?v=pK1CbnE2VsI

public class MouseDrag : MonoBehaviour
{
    float distance = 1;
    // This is basically connect to every point, so it should be easy to display the
    // data points.
    private void OnMouseDrag()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        transform.position = objectPos;
        DisplayPointData(transform);
    }
    private void DisplayPointData( Transform transform )
    {/*
        // If the name of the data point is a number, Then we know we can 
        // add the data to the display.
        if (Regex.IsMatch(transform.name, @"^\d+$")) {
            string dataString = " Data Point: " + transform.name;
            foreach (var data in transform.GetComponent<ParticleAttributes>().PointData)
                dataString += "\n Attribute: " + data.Key + ", Value: " + data.Value;
            GameObject.Find("DataMenu").GetComponent<Text>().text = dataString;
        }

        */
    }
}