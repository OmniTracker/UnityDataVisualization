/*----------------------------------------------------------------------------- * 3 Dimensional Multivariate Data Visualization *----------------------------------------------------------------------------- *      Class:          DynamicPointToPointLineRender *      Description:    Class is used to create links between particle game objects. *                      Algorithms for how to create links can be conducted within  *                      or outside of class. There are a number of ways to conduct the *                      line rendering between vertex.  *----------------------------------------------------------------------------- *      Author:         Ronald H. Baker (Brown University Masters Student)                   *      Date:           7/11/2019 *      Notes:          In this file, the first linerendering technique will to render  *                      each line between each point separately. When more algorithmic  *                      intensive calculation are conducted, I will explore the option *                      of just updating single vertex in the line. *                       *      Revision History: *       *      (7/11/2019) - Software Refactor - This file was generated to handle the *----------------------------------------------------------------------------- * This program is free software: you can redistribute it and/or modify it  * under the terms of the GNU General Public License as published by the Free  * Software Foundation, either version 3 of the License, or (at your option) any  * later version. *  * This program is distributed in the hope that it will be useful, but WITHOUT  * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS  * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. */

using System.Collections.Generic;
using UnityEngine;

public class DynamicPointToPointLineRender : MonoBehaviour
{
    /// <summary>
    /// Struct which indicates which Particle points are connected. Struct contains a pointer
    /// to the linked game object, along with there most up to date stored location. This 
    /// location is used to signify if the Line needs to be moved if the on of the linked game
    /// objects coodinates have been moved
    /// </summary>
    private struct DataPointLink
    {
        GameObject startLink;
        GameObject endLink;
        LineRenderer lineRenderer; 
    }
    // Object which will contain instantiated prefabs in hiearchy
    public GameObject PointHolder;
    // Linked list which holds all the Particle data points which are connected.
    private List<DataPointLink> m_DataPointLinkedList;
    private List<DataPointLink> DataPointLinkedList { get => m_DataPointLinkedList; set => m_DataPointLinkedList = value; }
    // If the Point Linked List has not been updated, there isn't a reason to constuct a new list.
    private bool m_DataPointLinkedListUpdated = false;
    public bool DataPointLinkedListUpdated { get => m_DataPointLinkedListUpdated; set => m_DataPointLinkedListUpdated = value; }

    LineRenderer m_LineRenderer;

    /// <summary>
    /// Instantiate empty List for Data Point Links
    /// </summary>
    void Start()
    {
        DataPointLinkedList = new List<DataPointLink>();
        m_LineRenderer = GetComponent<LineRenderer>();

        DataPointLinkedListUpdated = true;
    }
    /// <summary>
    /// Update check if the Data Point Linked List has been updated. If the list has been updated, then all prexisting
    /// links should be deleted, and new lines needs to be redrawn. The update function will also check whether or not
    /// the start and end points of the links have been updated. If there is an update in either of the coordinate 
    /// points, the link needs to be altered to maintain the visual link.
    /// </summary>
    void Update()
    {

        if ( DataPointLinkedListUpdated == false || m_LineRenderer.positionCount != PointHolder.transform.childCount)
        {
            // Do further calcs in this section. For now, we will only call the
            // GenerateLinksOfPointBasedOnAscendingOrder();
            GenerateLinksBetweenParticleAndOrigin();
            DataPointLinkedListUpdated = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void GenerateLinksOfPointBasedOnAscendingOrder()
    {
        int index = 0;
        m_LineRenderer.positionCount = PointHolder.transform.childCount; 
        foreach (Transform childDataPoint in PointHolder.transform)
        {
            m_LineRenderer.SetPosition(index,childDataPoint.transform.position);
            index++;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void GenerateLinksBetweenParticleAndOrigin()
    {
        int positionCount = 2;
        int originIndex = 0;
        int particleIndex = 1;
        float startWidth = 0.002f;
        float endWidth = 0.02f;
        Color color1 = Color.green;
        Color color2 = Color.red;
        foreach (Transform childDataPoint in PointHolder.transform)
        {
            LineRenderer lineRenderer = childDataPoint.GetComponent<LineRenderer>();
            // Set the start and end widths for the spring
            lineRenderer.startWidth = startWidth;
            lineRenderer.endWidth = endWidth;
            // Set the start and end colors of the spring
            lineRenderer.startColor = color1;
            lineRenderer.endColor = color2;
            lineRenderer.positionCount = positionCount;
            lineRenderer.SetPosition(originIndex, childDataPoint.GetComponent<ParticleAttributes>().OriginLocation);
            lineRenderer.SetPosition(particleIndex, childDataPoint.transform.position);
        }
    }
}
