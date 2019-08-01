/*----------------------------------------------------------------------------- * 3 Dimensional Multivariate Data Visualization *----------------------------------------------------------------------------- *      Class:          MagnetAttributes *      Description:        *----------------------------------------------------------------------------- *      Author:         Ronald H. Baker (Brown University Masters Student)                   *      Date:           7/11/2019 *      Notes:           *                       *      Revision History: *       *      (7/11/2019) - Software Refactor - This file was generated to handle the *----------------------------------------------------------------------------- * This program is free software: you can redistribute it and/or modify it  * under the terms of the GNU General Public License as published by the Free  * Software Foundation, either version 3 of the License, or (at your option) any  * later version. *  * This program is distributed in the hope that it will be useful, but WITHOUT  * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS  * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. */using UnityEngine;/// <summary>/// Magnet attributes./// </summary>public class MagnetAttributes : MonoBehaviour {    //********Public Variables********    // Editor options    private bool magnetActive = false;    private bool magnetVisible = true;    private float magnetismStrength = 1.000f;
    //********Private Variables********
    private Color m_SetColor;    private float m_MaxValue;    private float m_MinValue;    private Vector3 m_LastPosition;
    // This value is only relevant for the standalone part of the application.
    private readonly int distance = 3;    public float MaxValue { get => m_MaxValue; set => m_MaxValue = value; }    public float MinValue { get => m_MinValue; set => m_MinValue = value; }    public Vector3 LastPosition { get => m_LastPosition; set => m_LastPosition = value; }    public bool MagnetActive { get => magnetActive; set => magnetActive = value; }    public bool MagnetVisible { get => magnetVisible; set => magnetVisible = value; }    public float MagnetismStrength { get => magnetismStrength; set => magnetismStrength = value; }
    public string Name { get => name1; set => name1 = value; }
    private string name1;

    /// <summary>
    /// Upon Starting, the color for the magnet will be randomly set. 
    /// </summary>
    public void Start()
    {
        float minFloatColorValue = 0.0f;
        float maxFloatColorValue = 1.0f;
        float alpha = 1.0f;
        m_SetColor = ColorClassifier.NextRandomColor(minFloatColorValue, maxFloatColorValue, alpha);
        // Set the color for this Game Object
        this.GetComponent<Renderer>().material.color = m_SetColor;
    }
    /// <summary>    /// On the mouse drag will move the position of the magnet. For the purpose of the Standalone     /// portion of the project, the distance has been set to a static value. If you would like the     /// magnet to be further away from the player, just change the "distance" value.    /// </summary>    private void OnMouseDrag()     {        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);        Vector3 objPosition = Camera.main.ScreenToWorldPoint(mousePosition);        transform.position = objPosition;    }    /// <summary>    /// Calculates the attraction force.    /// </summary>    /// <returns>The attraction force.</returns>    /// <param name="incomingPointValue">Incoming point value.</param>    public float CalculateAttractionForce(float incomingPointValue )    {        if (magnetActive && MagnetVisible)        {            if ((m_MinValue <= incomingPointValue) && (incomingPointValue <= m_MaxValue))            {                return ((incomingPointValue - m_MinValue) / (m_MaxValue - m_MinValue)) * MagnetismStrength * 2;             }        }        return 0;    }    /// <summary>    /// Calculates the direction.    /// </summary>    /// <returns>The direction.</returns>    /// <param name="target">Target.</param>    public Vector3 CalculateDirection ( Vector3 target )    {        return LastPosition - target;     }    /// <summary>
    /// Sets and store the color of the of the magnet. This was written so the object has
    /// easy access to the selected color without having to access the Render of the game 
    /// object.
    /// </summary>    public void SetColorForMagnet(Color color)
    {
        m_SetColor = color;
        this.GetComponent<Renderer>().material.color = color; 
    }}