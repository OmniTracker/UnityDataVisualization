/*----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *----------------------------------------------------------------------------
 *      Class:          PointRender
 *      Description:    Handles renders data points collected from csv file 
 *                      into a 3 dimensional scatterplot. The first three column 
 *                      variable from each row collected from the csv file are
 *                      used to construct the scatterplot. Any additional data
 *                      collected from each row are stored in the game object,
 *                      which in the case of this project are referred to as 
 *                      "Particles." This file also handles the rendering and
 *                      distribution of each object, which are referred to 
 *                      "Magnets", which are used to attrach and repel each of 
 *                      the "Particles" based on a weighted force derived from
 *                      the multivariate data points stored within each of the 
 *                      "Particles"
 *----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                   
 *      Date:           7/11/2019
 *      Notes:          This Class was derived from the work of Mark Simpson. 
 *                      The inspiration of this class can be found at the 
 *                      following link: 
 *                      
 *                      https://github.com/PrinzEugn/UnityScatterplot
 *                      
 *      Revision History:
 *      
 *      (7/11/2019) - Software Refactor - Extracting all functionality that
 *                                        doesn't assist when the rendering of
 *                                        the "Particle" and "Magnet" objects
 *----------------------------------------------------------------------------
 * This program is free software: you can redistribute it and/or modify it 
 * under the terms of the GNU General Public License as published by the Free 
 * Software Foundation, either version 3 of the License, or (at your option) any 
 * later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT 
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
 * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 */
using System;
using System.Collections.Generic;
using UnityEngine;

public class PointRenderer : MonoBehaviour
{
    // Full column names from CSV (as Dictionary Keys)
    private string m_XAxis;
    private string m_YAxis;
    private string m_ZAxis;
    // Minimum and maximum values of columns
    private float m_XMin, m_YMin, m_ZMin, m_XMax, m_YMax, m_ZMax, m_SetParticleMass;
    // Scale of particlePoints within graph, WARNING: Does not scale with graph frame
    private readonly float plotScale = 10;
    private readonly float pointScale = 0.20f;
    private readonly float magnetScale = 0.30f;
    public string XAxis { get => m_XAxis; set => m_XAxis = value; }
    public string YAxis { get => m_YAxis; set => m_YAxis = value; }
    public string ZAxis { get => m_ZAxis; set => m_ZAxis = value; }
    public float XMin { get => m_XMin; set => m_XMin = value; }
    public float YMin { get => m_YMin; set => m_YMin = value; }
    public float ZMin { get => m_ZMin; set => m_ZMin = value; }
    public float XMax { get => m_XMax; set => m_XMax = value; }
    public float YMax { get => m_YMax; set => m_YMax = value; }
    public float ZMax { get => m_ZMax; set => m_ZMax = value; }
    public float SetParticleMass { get => m_SetParticleMass;
                                   set => m_SetParticleMass = value; }

    /// <summary>
    /// Assign all the label for the scatter plot axis
    /// </summary>
    /// <param name="inputFile"></param>
    /// <param name="xAxis"></param>
    /// <param name="yAxis"></param>
    /// <param name="zAxis"></param>
    /// <param name="pointList"></param>
    public void SetScatterPlotAxis(string inputFile, string xAxis, string yAxis, 
                                   string zAxis,
                                    List<Dictionary<string, object>> pointList)
    {
        // 
        XAxis = xAxis;
        YAxis = yAxis;
        ZAxis = zAxis;
        // 
        XMin = PlotController.FindMinimumValueFromDataPointContainer(pointList, 
                                                                     XAxis);
        YMin = PlotController.FindMinimumValueFromDataPointContainer(pointList, 
                                                                     YAxis);
        ZMin = PlotController.FindMinimumValueFromDataPointContainer(pointList, 
                                                                     ZAxis);
        XMax = PlotController.FindMaximumValueFromDataPointContainer(pointList, 
                                                                     XAxis);
        YMax = PlotController.FindMaximumValueFromDataPointContainer(pointList, 
                                                                     YAxis);
        ZMax = PlotController.FindMaximumValueFromDataPointContainer(pointList, 
                                                                     ZAxis);
        //
        PlotController.AssignLabels(XAxis,YAxis,ZAxis,XMin,XMax,YMin,YMax,ZMin,
                                    ZMax, pointList.Count,inputFile);
    }

    /// <summary>
    /// Alter Prefab Particle Points to new plotted position.
    /// </summary>
    /// <param name="pointHolderTransform"></param>
    /// <param name="colorClassifierActive"></param>
    public void AlterPrefabParticlePoints(Transform pointHolderTransform, 
                                          bool colorClassifierActive)
    {
        // These values are up updated corresponding to the location
        float x, y, z;
        // Iterate and alter the positions of each of the particles stored in 
        // the point holder.
        foreach (Transform childDataPoint in pointHolderTransform)
        {

            // 
            x = (childDataPoint.GetComponent<ParticleAttributes>()
                .KeyValue(XAxis) - XMin) / (XMax - XMin);
            y = (childDataPoint.GetComponent<ParticleAttributes>()
                .KeyValue(YAxis) - YMin) / (YMax - YMin);
            z = (childDataPoint.GetComponent<ParticleAttributes>()
                .KeyValue(ZAxis) - ZMin) / (ZMax - ZMin);

            // 
            childDataPoint.localScale = new Vector3(pointScale, 
                                                    pointScale, 
                                                    pointScale);
            //
            childDataPoint.GetComponent<ParticleAttributes>().OriginLocation = 
                (new Vector3(x, y, z) * plotScale) + 
                childDataPoint.transform.parent.localPosition;
            
            // Sets color according to x/y/z value
            if (colorClassifierActive == false)
            {
                childDataPoint.GetComponent<Renderer>().material.color 
                    = new Color(x, y, z, 1.0f);
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointList"></param>
    /// <param name="magnetList"></param>
    /// <param name="pointHolder"></param>
    /// <param name="pointPrefab"></param>
    public void GeneratePrefabParticlePoints(List<Dictionary<string, object>> pointList, 
                                             List<string> magnetList,  
                                             GameObject pointHolder, 
                                             GameObject pointPrefab)
    {
        // Store all data from files in data point
        for (var i = 0; i < pointList.Count; i++)
        {
            //instantiate as gameobject variable so that it can be manipulated 
            // within loop
            GameObject dataPoint = Instantiate(pointPrefab, 
                                               Vector3.zero, 
                                               Quaternion.identity);

            // Assigns name to the prefab
            dataPoint.transform.name = i.ToString();
            // Instantiate empty dictionary for particle data points
            dataPoint.GetComponent<ParticleAttributes>().PointData 
                = new Dictionary<string, float>();

            // Iterate through the names of the columns to set the associated 
            // values
            foreach (string key in magnetList)
            {
                dataPoint.GetComponent<ParticleAttributes>()
                    .PointData.Add(key, Convert.ToSingle(pointList[i][key]));

                // I have no idea why the program fails if this line isn't
                // here. I think it may be a race condition but not sure
                Debug.Log(key + ":  " + dataPoint
                    .GetComponent<ParticleAttributes>().KeyValue(key));
            }
            // Disable Gravity
            dataPoint.GetComponent<Rigidbody>().useGravity = false;
            // Set the Drag value in the Rigidboby
            dataPoint.GetComponent<Rigidbody>().drag = 0.3f;
            // Make child of PointHolder object, to keep particlePoints within 
            // container in hiearchy
            dataPoint.transform.SetParent(pointHolder.transform);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointHolderTransform"></param>
    /// <param name="colorClassifierActive"></param>
	public void PlacePrefabParticlePoints(Transform pointHolderTransform,
                                          bool colorClassifierActive)
    {
        float x, y, z;
        // Iterate and alter the positions of each of the particles stored in 
        // the point holder.
        foreach (Transform childDataPoint in pointHolderTransform)
        {
            // Normalize the data points to fit on the scatter plot.
            x = (childDataPoint.GetComponent<ParticleAttributes>()
                .KeyValue(XAxis) - XMin) / (XMax - XMin);
            y = (childDataPoint.GetComponent<ParticleAttributes>()
                .KeyValue(YAxis) - YMin) / (YMax - YMin);
            z = (childDataPoint.GetComponent<ParticleAttributes>()
                .KeyValue(ZAxis) - ZMin) / (ZMax - ZMin);
            // 
            childDataPoint.localPosition = new Vector3(x, y, z) * plotScale;
            // 
            childDataPoint.localScale = new Vector3(pointScale, 
                                                    pointScale, 
                                                    pointScale);
            // 
            childDataPoint.GetComponent<ParticleAttributes>().OriginLocation 
                = childDataPoint.position; 

            // Sets color according to x/y/z value
            if (colorClassifierActive == false)
            {
                childDataPoint.GetComponent<Renderer>().material.color 
                    = new Color(x, y, z, 1.0f);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="magnetList"></param>
    /// <param name="magnetHolder"></param>
    /// <param name="magnetPrefab"></param>
    /// <param name="pointList"></param>
    public void GenerateMagnets(List<string> magnetList,
                                GameObject magnetHolder, 
                                GameObject magnetPrefab, 
                                List<Dictionary<string, object>> pointList)
    {
        float minPointValue, maxPointValue;
        // Store all data from files in data point
        for (var i = 0; i < magnetList.Count; i++)
        {
            // Instantiate as gameobject variable so that it can be manipulated
            // within loop
            GameObject magnet = Instantiate(magnetPrefab, 
                                            Vector3.zero, 
                                            Quaternion.identity);
            // Assigns name to the magnet
            magnet.name = magnetList[i];
            // Set the name of the hovering label
            magnet.transform.Find("Name").GetComponent<TextMesh>().text 
                = magnetList[i];
            // Make child of PointHolder object, to keep particlePoints within 
            // container in hiearchy
            magnet.transform.SetParent(magnetHolder.transform);
            // Set rigidbody component attributes
            magnet.GetComponent<Rigidbody>().useGravity = false;
            magnet.GetComponent<Rigidbody>().drag = 20;
            // Set the Mass of the given rigid body to something large to keep 
            // the magnets from moving.
            magnet.GetComponent<Rigidbody>().mass = 1000;
            // 
            maxPointValue = PlotController
                .FindMaximumValueFromDataPointContainer(pointList, 
                                                        magnetList[i]);

            minPointValue = PlotController
                .FindMinimumValueFromDataPointContainer(pointList, 
                                                        magnetList[i]);
            // 
            magnet.GetComponent<MagnetAttributes>().MinValue = minPointValue;
            magnet.GetComponent<MagnetAttributes>().MaxValue = maxPointValue;

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="magnetHolderTransform"></param>
    public void PlaceMagnets(Transform magnetHolderTransform)
    {
        float x,z;
        // Index is used for the placement of the Magnet gameobjects. The math 
        // basically works out to disperse the magnets in a circle around the 
        // scatterplot.
        float index = 0;
        float maxAngle = 360;
        float angle = (maxAngle / magnetHolderTransform.childCount);
        float magnetRadius = 12.0f;
        // 
        foreach (Transform childMagnet in magnetHolderTransform)
        {
            // Calculates the x coordinate based of the predetermined angle, 
            // index, and specified magnet radius
            x = magnetRadius * (float)Math.Cos(angle * index);
            // Calculates the z coordinate based of the predetermined angle, 
            // index, and specified magnet radius
            z = magnetRadius * (float)Math.Sin(angle * index);
            // Position point at relative to parent
            childMagnet.localPosition = new Vector3(x, 0, z);
            childMagnet.localScale = new Vector3(magnetScale, 
                                                 magnetScale, 
                                                 magnetScale);
            // Store the last position of the Magnets. This will be used 
            // somewhere else in the program.
            childMagnet.GetComponent<MagnetAttributes>().LastPosition 
                = childMagnet.transform.position;
            
            // Increment to the next point placement index
            index += 1;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pointHolderTransform"></param>
    /// <param name="newMass"></param>
    public void UpdateParticlePointsMass(Transform pointHolderTransform, 
                                        float newMass)
    {
        // 
        SetParticleMass = newMass;
        // 
        foreach (Transform childDataPoint in pointHolderTransform)
        {
            childDataPoint.GetComponent<Rigidbody>().mass = newMass;
        }
    }
}