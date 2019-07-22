/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          ColorClassifier
 *      Description:         
 *-----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                  
 *      Date:           7/11/2019
 *      Notes:           
 *                      
 *      Revision History:
 *      
 *      (7/11/2019) - Software Refactor - This file was generated to handle the
 *                    color of the magnets and also the color correlation for 
 *                    each of the data points if the color classifier is enabled
 *      (7/21/2019) - Adding color alteration based off specified values.              
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

public class ColorClassifier : MonoBehaviour
{
    public bool Active = false;
    /// <summary>
    /// Used to generate a random color based of the min and max values
    /// </summary>
    /// <param name="minimum"> Minimum range value to produce a color</param>
    /// <param name="maximum"> Maxium range value to produce a color.</param>
    /// <returns></returns>
    public static Color NextRandomColor (float minimum, float maximum, float alpha)
    {
        float minFloatColorValue = 0.0f;
        float maxFloatColorValue = 1.0f;
        // If the color values passed in are out of range, return a color value equal to white.
        if ( ( minFloatColorValue < minimum && minimum < maxFloatColorValue) ||
             (minFloatColorValue < maximum && maximum < maxFloatColorValue))
        {
            return new Color(0,0,0,1);
        }
        // If the minimum is higher than the max value, return a color of black.
        // This decision was more so visually debugging purposes. 
        if (minimum > maximum)
        {
            return new Color(1,1,1,1);
        }
        // Set the R,G,B,A
        float r = Random.Range(minimum, maximum);
        float g = Random.Range(minimum, maximum);
        float b = Random.Range(minimum, maximum);
        return new Color(r,g,b, 1.0f); 
    }

    public static void AlterScatterPlotColorsBasedOnInput(Transform pointHolderTransform, Color targetColor, string magnetName, float cutOff)
    {
        foreach (Transform childDataPoint in pointHolderTransform)
        {




        }
    }
}
