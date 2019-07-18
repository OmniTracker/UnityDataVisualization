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
using UnityEngine.UI;
using UnityEngine;

public class PointRenderer : MonoBehaviour
{
    // The prefab for the data particlePoints that will be instantiated
    public GameObject PointPrefab;
    // Object which will contain instantiated prefabs in hiearchy
    public GameObject PointHolder;
    // The prefab for the data Magnet that will be instantiated
    public GameObject MagnetPrefab;
    // Object which will contain instantiated prefabs in hiearchy
    public GameObject MagnetHolder;
    // Used to get the new axis for new graph
    public Dropdown XCoordinateDropDown;
    public Dropdown YCoordinateDropDown;
    public Dropdown ZCoordinateDropDown;
    // Name of the input file, no extension
    public string InputFile;
    // Full column names from CSV (as Dictionary Keys)
    private string m_XColumn;
    private string m_YColumn;
    private string m_ZColumn;
    // Scale of particlePoints within graph, WARNING: Does not scale with graph frame
    private readonly float plotScale = 10;
    private readonly float pointScale = 0.20f;
    private readonly float magnetScale = 0.30f;
    //********Private Variables********
    // Minimum and maximum values of columns
    private float m_XMin, m_YMin, m_ZMin, m_XMax, m_YMax, m_ZMax;
    // List for holding data from CSV reader
    private List<Dictionary<string, object>> pointList;
    // Store dictionary keys (column names in CSV) in a list
    private List<string> m_columnList;
    private bool m_renderColorForPoints = true;

    /// <summary>
    /// Start this instance.
    /// </summary>
    void Start()
    {
        NewPlot(0, 1, 2);
    }
    /// <summary>
    /// News the plot.
    /// </summary>
    /// <param name="column1">Column1.</param>
    /// <param name="column2">Column2.</param>
    /// <param name="column3">Column3.</param>
    private void NewPlot(int column1, int column2, int column3)
    {
        //Run CSV Reader
        pointList = CSVReader.Read(InputFile);
        // Store dictionary keys (column names in CSV) in a list
        m_columnList = new List<string>(pointList[1].Keys);

        // Generate all the data points first
        GeneratePrefabParticlePoints();
        // Generate all the magnets next
        GenerateMagnet();
        // Set global column names 
        m_XColumn = m_columnList[column1];
        m_YColumn = m_columnList[column2];
        m_ZColumn = m_columnList[column3];
        // Set min and x values for all coordinates
        SetPlotMinumumAndMaximumValues();
        // Place all particle points based all collected data
        PlacePrefabParticlePoints();
        // Place all Particle Points based all collected data
        PlaceMagnets();
        // Assign all the label for the scatter plot axis
        PlotDataAndLabelController.AssignLabels(m_XColumn, 
                                                m_YColumn, 
                                                m_ZColumn,
                                                m_XMin, 
                                                m_XMax, 
                                                m_YMin, 
                                                m_YMax, 
                                                m_ZMin, 
                                                m_ZMax, 
                                                PointHolder.transform.childCount, 
                                                InputFile);
        // Clear all the data from the container. It is no longer needed at this point.
        pointList.Clear();
    }
    /// <summary>
    /// Load New Plot Axis. Changes the axis names along with the position of the actual data points
    /// </summary>
    public void LoadNewPlotAxis()
    {
        // Reset global column names 
        m_XColumn = m_columnList[XCoordinateDropDown.value];
        m_YColumn = m_columnList[YCoordinateDropDown.value];
        m_ZColumn = m_columnList[ZCoordinateDropDown.value];
        // Reset the new min and max values
        SetPlotMinumumAndMaximumValues();
        // Rearrange all particle points based all collected data
        AlterPrefabParticlePoints();
        // Reset the Labels for the 3D Scatterplot
        PlotDataAndLabelController.AssignLabels(m_XColumn,
                                         m_YColumn,
                                         m_ZColumn,
                                         m_XMin,
                                         m_XMax,
                                         m_YMin,
                                         m_YMax,
                                         m_ZMin,
                                         m_ZMax,
                                         PointHolder.transform.childCount,
                                         InputFile);
    }
    /// <summary>
    /// Simply sets the coordinate class members for the minimum and maximum values associated 
    /// with the x,y,z axis.
    /// </summary>
    private void SetPlotMinumumAndMaximumValues()
    {
        m_XMin = PlotDataAndLabelController.FindMinimumValueFromDataPointContainer(PointHolder.transform, 
                                                                                   m_XColumn);
        m_YMin = PlotDataAndLabelController.FindMinimumValueFromDataPointContainer(PointHolder.transform, 
                                                                                   m_YColumn);
        m_ZMin = PlotDataAndLabelController.FindMinimumValueFromDataPointContainer(PointHolder.transform, 
                                                                                   m_ZColumn);
        m_XMax = PlotDataAndLabelController.FindMaximumValueFromDataPointContainer(PointHolder.transform, 
                                                                                   m_XColumn);
        m_YMax = PlotDataAndLabelController.FindMaximumValueFromDataPointContainer(PointHolder.transform, 
                                                                                   m_YColumn);
        m_ZMax = PlotDataAndLabelController.FindMaximumValueFromDataPointContainer(PointHolder.transform, 
                                                                                   m_ZColumn);
    }
    /// <summary>
    /// Alter Prefab Particle Points to new plotted position.
    /// </summary>
    private void AlterPrefabParticlePoints()
    {
        // These values are up updated corresponding to the location
        float x, y, z;
        // Iterate and alter the positions of each of the particles stored in the point holder.
        foreach (Transform childDataPoint in PointHolder.transform)
        {
            x = (childDataPoint.GetComponent<ParticleAttributes>().KeyValue(m_XColumn) - m_XMin) / (m_XMax - m_XMin);
            y = (childDataPoint.GetComponent<ParticleAttributes>().KeyValue(m_YColumn) - m_YMin) / (m_YMax - m_YMin);
            z = (childDataPoint.GetComponent<ParticleAttributes>().KeyValue(m_ZColumn) - m_ZMin) / (m_ZMax - m_ZMin);
            childDataPoint.localScale = new Vector3(pointScale, pointScale, pointScale);
            childDataPoint.GetComponent<ParticleAttributes>().OriginLocation = (new Vector3(x, y, z) * plotScale) + childDataPoint.transform.parent.localPosition;
            // Sets color according to x/y/z value
            childDataPoint.GetComponent<Renderer>().material.color = new Color(x, y, z, 1.0f);
            
        }
    }
    /// <summary>
    /// Generate Prefab Particle Points
    /// </summary>
    private void GeneratePrefabParticlePoints()
    {
        // Store all data from files in data point
        for (var i = 0; i < pointList.Count; i++)
        {
            //instantiate as gameobject variable so that it can be manipulated within loop
            GameObject dataPoint = Instantiate(PointPrefab, Vector3.zero, Quaternion.identity);

            // Assigns name to the prefab
            dataPoint.transform.name = i.ToString();
            
            // Instantiate empty dictionary for particle data points
            dataPoint.GetComponent<ParticleAttributes>().PointData = new Dictionary<string, float>();
            
            // Iterate through the names of the columns to set the associated values
            foreach (string key in m_columnList)
            {
                dataPoint.GetComponent<ParticleAttributes>().PointData.Add(key, Convert.ToSingle(pointList[i][key]));
                // I have no idea why the program fails if this line isn't here. I think it may be a race condition but not sure
                Debug.Log(dataPoint.GetComponent<ParticleAttributes>().PointData);
            }
            
            // Disable Gravity
            dataPoint.GetComponent<Rigidbody>().useGravity = false;
            
            // Set the Drag value in the Rigidboby
            dataPoint.GetComponent<Rigidbody>().drag = 0.5f;
            
            // Make child of PointHolder object, to keep particlePoints within container in hiearchy
            dataPoint.transform.SetParent(PointHolder.transform);
        }
    }
    /// <summary>
    /// Places the prefab points.
    /// </summary>
	private void PlacePrefabParticlePoints()
    {
        float x, y, z;
        // Iterate and alter the positions of each of the particles stored in the point holder.
        foreach (Transform childDataPoint in PointHolder.transform)
        {
            // Normalize the data points to fit on the scatter plot.
            x = (childDataPoint.GetComponent<ParticleAttributes>().KeyValue(m_XColumn) - m_XMin) / (m_XMax - m_XMin);
            y = (childDataPoint.GetComponent<ParticleAttributes>().KeyValue(m_YColumn) - m_YMin) / (m_YMax - m_YMin);
            z = (childDataPoint.GetComponent<ParticleAttributes>().KeyValue(m_ZColumn) - m_ZMin) / (m_ZMax - m_ZMin);
            childDataPoint.localPosition = new Vector3(x, y, z) * plotScale;
            childDataPoint.localScale = new Vector3(pointScale, pointScale, pointScale);
            // Sets color according to x/y/z value
            childDataPoint.GetComponent<Renderer>().material.color = new Color(x, y, z, 1.0f);
        }
    }
    /// <summary>
    /// Generates Magnet game objects.
    /// </summary>
    private void GenerateMagnet()
    {
        float minPointValue, maxPointValue;
        // Iterate though the column list to create the individual magnets
        foreach (string key in m_columnList)
        {
            //instantiate as gameobject variable so that it can be manipulated within loop
            GameObject magnet = Instantiate(MagnetPrefab, Vector3.zero, Quaternion.identity);

            // Min and Max values for attraction 
            minPointValue = PlotDataAndLabelController.FindMinimumValueFromDataPointContainer(PointHolder.transform, key);
            maxPointValue = PlotDataAndLabelController.FindMaximumValueFromDataPointContainer(PointHolder.transform, key);
            magnet.GetComponent<MagnetAttributes>().MinValue = minPointValue;
            magnet.GetComponent<MagnetAttributes>().MaxValue = maxPointValue;

            // Set the name of the hovering label
            magnet.transform.Find("Name").GetComponent<TextMesh>().text = key;

            // Set rigidbody component attributes
            magnet.GetComponent<Rigidbody>().useGravity = false;
            magnet.GetComponent<Rigidbody>().drag = 20;

            // Assigns name to the magnet
            magnet.name = key;
            
            // Make child of PointHolder object, to keep particlePoints within container in hiearchy
            magnet.transform.SetParent(MagnetHolder.transform);

            // Set the Mass of the given rigid body to something large to keep the magnets from moving.
            magnet.GetComponent<Rigidbody>().mass = 1000;
        }
    }
    /// <summary>
    /// Place Magnets
    /// </summary>
    private void PlaceMagnets()
    {
        float x,z;
        // Index is used for the placement of the Magnet gameobjects. The math basically works
        // out to disperse the magnets in a circle around the scatterplot.
        float index = 0;
        float maxAngle = 360;
        float angle = (maxAngle / MagnetHolder.transform.childCount);
        float magnetRadius = 12;
        foreach (Transform childMagnet in MagnetHolder.transform)
        {
            // Calculates the x coordinate based of the predetermined angle, index, and specified magnet radius
            x = magnetRadius * (float)Math.Cos(angle * index);
            // Calculates the z coordinate based of the predetermined angle, index, and specified magnet radius
            z = magnetRadius * (float)Math.Sin(angle * index);
            // Position point at relative to parent
            childMagnet.localPosition = new Vector3(x, 0, z);
            childMagnet.localScale = new Vector3(magnetScale, magnetScale, magnetScale);   
            // Store the last position of the Magnets. This will be used somewhere else in the program.
            childMagnet.GetComponent<MagnetAttributes>().LastPosition = childMagnet.transform.position;
            // Increment to the next point placement index
            index += 1;
        }
    }
    /// <summary>
    /// Fixed update is used to handle the orientation of all game objects with the tag "Label"
    /// </summary>
    private void Update()
    {
        PlotDataAndLabelController.OrientLables();
    }
}