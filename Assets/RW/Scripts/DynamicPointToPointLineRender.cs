/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          DynamicPointToPointLineRender
 *      Description:    Class is used to create links between particle game objects.
 *                      Algorithms for how to create links can be conducted within 
 *                      or outside of class. There are a number of ways to conduct the
 *                      line rendering between vertex. 
 *-----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                  
 *      Date:           7/11/2019
 *      Notes:          In this file, the first linerendering technique will to render 
 *                      each line between each point separately. When more algorithmic 
 *                      intensive calculation are conducted, I will explore the option
 *                      of just updating single vertex in the line.
 *                      
 *      Revision History:
 *      
 *      (7/11/2019) - Software Refactor - This file was generated to handle the
 *      (8/01/2019) - Finished Comments for file.
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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DynamicPointToPointLineRender : MonoBehaviour
{
    // Flag indicating whether or not the spring should be viewable.
    bool m_springEnabled = true;
    public bool SpringEnabled { get => m_springEnabled; 
                                set => m_springEnabled = value; }
    /// <summary>
    /// Used to update the end points for the linerender component attached to 
    /// each of the children particle point points. This function only changes 
    /// the end point values once. To achieve a real-time visual update this 
    /// funciton should be called in some sort of update function.
    /// </summary>
    /// <param name="pointHolderTransform">Parent point holder transform. Used to
    /// access the children data points.
    /// </param>
    public static void UpdateSpringPositions(Transform pointHolderTransform)
    {
        foreach (Transform childDataPoint in pointHolderTransform)
        {
            // Get the line render attached to the child data point object.
            LineRenderer lineRenderer = childDataPoint.GetComponent<LineRenderer>();
            // Set the start and end for the selected gameobject based on it's current
            // location and the origin point.
            lineRenderer.SetPosition(0, childDataPoint.GetComponent<ParticleAttributes>()
                                                        .OriginLocation);
            lineRenderer.SetPosition(1, childDataPoint.transform.position);
        }
    }
    /// <summary>
    ///  Used to set the attributes and positions for the linerender objects 
    ///  attached to the individual data point gameobjects. This function sets 
    ///  the start and end point for the line render.
    /// </summary>
    /// <param name="pointHolderTransform">Parent point holder transform. Used to
    /// access the children data points.
    /// </param>
    public static void GenerateLinksBetweenParticleAndOrigin(Transform pointHolderTransform)
    {
        // Since the line render only care about the location of the game object and the 
        // origin location. These value we selected to properly assign the number of 
        // tracked point and their index within the linerenderer
        int positionCount = 2;
        int originIndex = 0;
        int particleIndex = 1;
        // These values were arbitrarily 
        float startWidth = 0.002f;
        float endWidth = 0.02f;
        foreach (Transform childDataPoint in pointHolderTransform)
        {
            // Get the line render attached to the child data point object.
            LineRenderer lineRenderer = childDataPoint.GetComponent<LineRenderer>();
            lineRenderer.positionCount = positionCount;
            // Set the start and end widths for the spring.
            lineRenderer.startWidth = startWidth;
            lineRenderer.endWidth = endWidth;
            // Set the start and end colors of the spring.
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.red;
            // Set the start and end locations for the linerenderer. 
            lineRenderer.SetPosition(originIndex, childDataPoint.GetComponent<ParticleAttributes>()
                                                                .OriginLocation);
            lineRenderer.SetPosition(particleIndex, childDataPoint.transform.position);
        }
    }
    /// <summary>
    /// Enables or diables the LineRender object in the scene. If true
    /// </summary>
    /// <param name="pointHolderTransform">Parent point holder transform. Used to
    /// access the children data points.
    /// </param>    
    /// <param name="isEnable">Flag indicating whether or not the LineRender 
    /// should be viewable in the scene.
    /// </param>
    public static void EnableLinksBetweenParticleAndOrigin(Transform pointHolderTransform, 
                                                           bool isEnable)
    {
        foreach (Transform childDataPoint in pointHolderTransform)
        {
            // Iterate and set the linerender as enabled.
            childDataPoint.GetComponent<LineRenderer>().enabled = isEnable; 
        }
    }
    /// <summary>
    ///  Used to sort data points to be added to the linerender based off accending 
    /// order of the data points associated with a given magnet value and the 
    /// selected data point.
    /// </summary>
    /// <param name="pointHolderTransform">Parent point holder transform. Used to
    /// access the children data points.
    /// </param>    
    /// <param name="dynamicLineRenderer">LineRender object which
    /// is currently presently attached the canvas gameobject
    /// </param>
    /// <param name="Magnet"> The name of the magnet selected to generate the 
    /// linerender for.
    /// </param>
    public static void SortPointsForLineRender (Transform pointHolderTransoform, 
                                                LineRenderer dynamicLineRenderer, 
                                                string magnet)
    {
        Dictionary<string, float> pointDataDictionary = new Dictionary<string, float>();
        // Get all the values from the points that correspond to the magnets.
        foreach (Transform childDataPoint in pointHolderTransoform)
        {
            pointDataDictionary.Add(childDataPoint.name, childDataPoint.GetComponent<ParticleAttributes>().KeyValue(magnet));
        }
        int lineRenderIndex = 0;
        dynamicLineRenderer.positionCount = pointDataDictionary.Count; 
        // Sort the point data into the LineRenderer
        foreach (KeyValuePair<string,float> pointData in pointDataDictionary.OrderBy(key => key.Value))
        {
            dynamicLineRenderer.SetPosition(lineRenderIndex, pointHolderTransoform.Find(pointData.Key).transform.position);
            lineRenderIndex += 1;
        }
    }

}
