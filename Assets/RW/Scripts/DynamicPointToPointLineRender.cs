/*----------------------------------------------------------------------------- * 3 Dimensional Multivariate Data Visualization *----------------------------------------------------------------------------- *      Class:          DynamicPointToPointLineRender *      Description:    Class is used to create links between particle game objects. *                      Algorithms for how to create links can be conducted within  *                      or outside of class. There are a number of ways to conduct the *                      line rendering between vertex.  *----------------------------------------------------------------------------- *      Author:         Ronald H. Baker (Brown University Masters Student)                   *      Date:           7/11/2019 *      Notes:          In this file, the first linerendering technique will to render  *                      each line between each point separately. When more algorithmic  *                      intensive calculation are conducted, I will explore the option *                      of just updating single vertex in the line. *                       *      Revision History: *       *      (7/11/2019) - Software Refactor - This file was generated to handle the *----------------------------------------------------------------------------- * This program is free software: you can redistribute it and/or modify it  * under the terms of the GNU General Public License as published by the Free  * Software Foundation, either version 3 of the License, or (at your option) any  * later version. *  * This program is distributed in the hope that it will be useful, but WITHOUT  * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS  * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. */

using System.Collections.Generic;
using UnityEngine;

public class DynamicPointToPointLineRender : MonoBehaviour
{
    Transform PointHolderTransform;

    bool m_springEnabled = true;
    public bool SpringEnabled { get => m_springEnabled; set => m_springEnabled = value; }
    /// <summary>
    /// 
    /// </summary>
    private void GenerateLinksOfPointBasedOnAscendingOrder()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointHolderTransform"></param>
    public static void UpdateSpringPositions(Transform pointHolderTransform)
    {
        foreach (Transform childDataPoint in pointHolderTransform)
        {
            LineRenderer lineRenderer = childDataPoint.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, childDataPoint.GetComponent<ParticleAttributes>().OriginLocation);
            lineRenderer.SetPosition(1, childDataPoint.transform.position);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointHolderTransform"></param>
    public static void GenerateLinksBetweenParticleAndOrigin(Transform pointHolderTransform)
    {
        int positionCount = 2;
        int originIndex = 0;
        int particleIndex = 1;
        float startWidth = 0.002f;
        float endWidth = 0.02f;
        foreach (Transform childDataPoint in pointHolderTransform)
        {
            LineRenderer lineRenderer = childDataPoint.GetComponent<LineRenderer>();
            lineRenderer.positionCount = positionCount;
            // Set the start and end widths for the spring
            lineRenderer.startWidth = startWidth;
            lineRenderer.endWidth = endWidth;
            // Set the start and end colors of the spring
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.red;
            lineRenderer.SetPosition(originIndex, childDataPoint.GetComponent<ParticleAttributes>().OriginLocation);
            lineRenderer.SetPosition(particleIndex, childDataPoint.transform.position);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointHolderTransform"></param>
    /// <param name="isEnable"></param>
    public static void EnableLinksBetweenParticleAndOrigin(Transform pointHolderTransform, bool isEnable)
    {
        foreach (Transform childDataPoint in pointHolderTransform)
        {
            childDataPoint.GetComponent<LineRenderer>().enabled = isEnable; 
        }
    }
}
