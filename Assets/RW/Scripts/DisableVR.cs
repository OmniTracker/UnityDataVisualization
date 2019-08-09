/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          DisableVR
 *      Description:    File used to disable the use of VR for the Stanalone
 *                      version of the project. At this poiint in time, only
 *                      one flag is need to achieve this functionality.
 *-----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                  
 *      Date:           7/11/2019
 *      Notes:           
 *                      
 *      Revision History:
 *      (8/01/2019) - Finished comments for file
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
using UnityEngine.XR;

public class DisableVR : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Disable VR 
        XRSettings.enabled = false;
    }
}
