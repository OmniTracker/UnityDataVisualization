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
    public bool changeColor = false;
    [Range(0.0f, 1.0f)]
    public float x, y, z;
    // Object which will contain instantiated prefabs in hiearchy
    public GameObject PointHolder;
    // Object which will contain instantiated prefabs in hiearchy
    public GameObject MagnetHolder;



    void Update()
    {


        if (changeColor == true)
        {
            Color col = new Color(x, y, z, 1.0f);

            // Iterate and alter the positions of each of the particles stored in the point holder.
            foreach (Transform childDataPoint in PointHolder.transform)
            {
                childDataPoint.GetComponent<Renderer>().material.color = col;
            }

        }

    }
}
