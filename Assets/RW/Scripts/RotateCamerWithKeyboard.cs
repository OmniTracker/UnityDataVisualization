/*----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *----------------------------------------------------------------------------
 *      Class:          RotateCamerWithKeyboard
 *      Description:    Used to add the funtion of rotating the player camera
 *                      using the Q key and E key from the keyboard. There 
 *                      is currently build in funtionality build into the project
 *                      which allows for the following functionality: 
 *                      
 *                      A Key - Move player to the left
 *                      W Key - Move player to the right
 *                      S Key - Move Player backwords
 *                      W Key - Move Player forward
 *                      
 *                      Features this file adds:
 *                      
 *                      Q Key - Rotate player Left
 *                      E Key - Rotate player Right
 *                      
 *                      This class is mainly used for the standalone version, 
 *                      without the HTC Vive, of the 3 Dimensional plotting 
 *                      application
 *----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                     
 *      Date:           7/11/2019
 *      Notes:           
 *                      
 *      Revision History:
 *      
 *      (7/11/2019) - Software Refactor - File was just commented for refactor
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
using UnityEngine;

public class RotateCamerWithKeyboard : MonoBehaviour
{
    private Transform m_FPSControllerTransform = null;
    /// <summary>
    /// Upon startup, searches for the FPSController game object and stores
    /// a point to it's transforms component
    /// </summary>
    private void Start()
    {
        m_FPSControllerTransform = GameObject.Find("FPSController").GetComponent<Transform>(); 
    }
    /// <summary>
    /// Upon periodic update, the input for the Q and E inputs tracked. This 
    /// action rotates the camera of the First Person Controller, or 
    /// FPSController left or right.
    /// </summary>
    private void Update()
    {
        // If FPSController Transform was not set upon startup, return.
        if (m_FPSControllerTransform == null)
        {
            return;
        }
        // Rotate to the right if E key is hit
        if (Input.GetKey(KeyCode.E))
        {
            m_FPSControllerTransform.Rotate(0,2,0);
        }
        // Rotate to the left if Q key is hit
        if (Input.GetKey(KeyCode.Q))
        {
            m_FPSControllerTransform.Rotate(0,-2, 0);
        }


    }
}
