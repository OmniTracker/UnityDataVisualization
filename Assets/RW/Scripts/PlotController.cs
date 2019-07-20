/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          PlotController
 *      Description:    Handle the orientation and alteration of the labels for 
 *                      the scatterplot. This script finds objects with an 
 *                      appropriate tag, and makes them rotate according to the 
 *                      camera. This script does no have to be placed on a 
 *                      particular object (finds them using tags). The tags 
 *                      must be added to the desired game objects in the 
 *                      Editor. The tag must be defined in the inspector of 
 *                      this script. Remember to have an active camera!       
 *-----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                  
 *      Date:           7/11/2019
 *      Notes:           
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
using UnityEngine;

public class PlotController
{
    /// <summary>
    /// Searches the scene and assign the appropiate name to the Labels within 
    /// the scatterplot. This include the plot axis, name of data file, and
    /// number of data points associated with the plot.
    /// </summary>
    /// <param name="xAxix">Name of the X Axis</param>
    /// <param name="yAxis">Name of the Y Axis</param>
    /// <param name="zAxis">Name of the Z Axis</param>
    /// <param name="xMin">Minimum X value for plot Axis</param>
    /// <param name="xMax">Maximum X value for plot Axis</param>
    /// <param name="yMin">Minimum Y value for plot Axis</param>
    /// <param name="yMax">Maximum Y value for plot Axis</param>
    /// <param name="zMin">Minimum Z value for plot Axis</param>
    /// <param name="zMax">Maximum Z value for plot Axis</param>
    /// <param name="pointCount">Number of data point within dataset</param>
    /// <param name="inputFile">File used to collect data point</param>
    public static void AssignLabels(string xAxix, string yAxis, string zAxis, 
                             float xMin, float xMax, 
                             float yMin, float yMax, 
                             float zMin, float zMax, 
                             int pointCount, string inputFile)
    {
        // Update point counter
        GameObject.Find("Point_Count").GetComponent<TextMesh>().text = pointCount.ToString("");
        // Update title according to inputfile name
        GameObject.Find("Dataset_Label").GetComponent<TextMesh>().text = inputFile;
        // Update axis titles to ColumnNames
        GameObject.Find("X_Title").GetComponent<TextMesh>().text = xAxix;
        GameObject.Find("Y_Title").GetComponent<TextMesh>().text = yAxis;
        GameObject.Find("Z_Title").GetComponent<TextMesh>().text = zAxis;
        // Set x Labels by finding game objects and setting TextMesh and assigning value (need to convert to string)
        GameObject.Find("X_Min_Lab").GetComponent<TextMesh>().text = xMin.ToString("0.0");
        GameObject.Find("X_Mid_Lab").GetComponent<TextMesh>().text = (xMin + (xMax - xMin) / 2f).ToString("0.0");
        GameObject.Find("X_Max_Lab").GetComponent<TextMesh>().text = xMax.ToString("0.0");
        // Set y Labels by finding game objects and setting TextMesh and assigning value (need to convert to string)
        GameObject.Find("Y_Min_Lab").GetComponent<TextMesh>().text = yMin.ToString("0.0");
        GameObject.Find("Y_Mid_Lab").GetComponent<TextMesh>().text = (yMin + (yMax - yMin) / 2f).ToString("0.0");
        GameObject.Find("Y_Max_Lab").GetComponent<TextMesh>().text = yMax.ToString("0.0");
        // Set z Labels by finding game objects and setting TextMesh and assigning value (need to convert to string)
        GameObject.Find("Z_Min_Lab").GetComponent<TextMesh>().text = zMin.ToString("0.0");
        GameObject.Find("Z_Mid_Lab").GetComponent<TextMesh>().text = (zMin + (zMax - zMin) / 2f).ToString("0.0");
        GameObject.Find("Z_Max_Lab").GetComponent<TextMesh>().text = zMax.ToString("0.0");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="DataPointContainerTransform"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public static float FindMaximumValueFromDataPointContainer(Transform DataPointContainerTransform, string column)
    {
        float maxValue = 0;
        ParticleAttributes particleAttributes;
        // If the transform is null, return a value of zero
        if (DataPointContainerTransform == null)
        {
            return 0;
        }
        // Iterate through all the point to figure out what the Lowest value is based off the input string.
        foreach (Transform childDataPoint in DataPointContainerTransform)
        {
            particleAttributes = childDataPoint.GetComponent<ParticleAttributes>();
            // If the Transform does not contain a ParticleAttributes component, return a value of zero
            if (particleAttributes == null)
            {
                return 0;
            }
            // Get the stored key value for the column value passed in
            if (maxValue < particleAttributes.KeyValue(column))
            {
                maxValue = childDataPoint.GetComponent<ParticleAttributes>().KeyValue(column);
            }
        }

        return maxValue;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="DataPointContainerTransform"></param>
    /// <param name="column"></param>
    /// <returns></returns>
    public static float FindMinimumValueFromDataPointContainer(Transform DataPointContainerTransform, string column)
    {
        float minValue = float.MaxValue;
        ParticleAttributes particleAttributes;
        // If the transform is null, return a value of zero
        if (DataPointContainerTransform == null)
        {
            return 0;
        }
        // Iterate through all the point to figure out what the Lowest value is based off the input string.
        foreach (Transform childDataPoint in DataPointContainerTransform)
        {
            particleAttributes = childDataPoint.GetComponent<ParticleAttributes>();
            // If the Transform does not contain a ParticleAttributes component, return a value of zero
            if (particleAttributes == null)
            {
                return 0;
            }
            // Get the stored key value for the column value passed in
            if (particleAttributes.KeyValue(column) < minValue)
            {
                minValue = childDataPoint.GetComponent<ParticleAttributes>().KeyValue(column);
            }
        }

        return minValue;
    }
    /// <summary>
    /// Orients the game objects with the tag "Label" in the scene.
    /// </summary>
    public static void OrientLables()
    {
        // Array, stores all GameObjects that should be kept aligned with camera
        GameObject[] m_Labels;
        // The tag of the target object, the ones that will track the camera
        string m_TargetTag = "Label";
        m_Labels = GameObject.FindGameObjectsWithTag(m_TargetTag);
        // If no Labels are found, Exit from application
        if (m_Labels.Length == 0)
        {
            return;
        }
        // go through "labels" array and aligns each object to the Camera.main (built-in) position and orientation
        foreach (GameObject go in m_Labels)
        {
            // create new position Vector 3 so that object does not rotate around y axis
            Vector3 targetPosition = new Vector3(Camera.main.transform.position.x,
                                                 go.transform.position.y,
                                                 Camera.main.transform.position.z);
            // Here the internal math reverses the direction so 3D text faces the correct way
            go.transform.LookAt(2 * go.transform.position - targetPosition);
        }
    }
}