/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          ParticleAttributes
 *      Description:       
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
using System.Collections.Generic;
using UnityEngine;

public class ParticleAttributes : MonoBehaviour
{
    //********Public Variables********
    // Editor options
    Vector3 m_OriginLocation;
    public Vector3 OriginLocation { get => m_OriginLocation; set => m_OriginLocation = value; }
    public Dictionary<string, float> PointData { get => m_pointData; set => m_pointData = value; }
    private Dictionary<string, float> m_pointData;
    /// <summary>
    /// Sets the origin point of the Particle to where the gameobject was instantiated.
    /// </summary>
    void Start()
    {
        // Get the current cooridinate of the object.
        OriginLocation = this.gameObject.transform.position;
    }
    /// <summary>
    /// Print the data associated with each variable contained in Particle game object.
    /// This function is primarily used for debugging.
    /// </summary>
    public void PrintPointData ()
    {
        foreach(var data in m_pointData)
        {
            Debug.Log("key : " + data.Key + " item : " + KeyValue(data.Key));
        }
    }
    /// <summary>
    /// Retrieve the data point associated with the appropiate
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public float KeyValue(string key)
    {
        return m_pointData[key];
    }
}
