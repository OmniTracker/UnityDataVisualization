/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          ObjectForceHandler
 *      Description:       
 *-----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                  
 *      Date:           7/11/2019
 *      Notes:           
 *                      
 *      Revision History:
 *      
 *      (7/11/2019) - Software Refactor - This file was generated to handle the
 *                                        Raycast of trigger handle
 *      (7/23/2019) - Software Refactor - Adjusted Raycast to handle GUI similar
 *                                        to standalone.
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
using Valve.VR;
using UnityEngine.UI;

public class Raycaster : MonoBehaviour
{
    public SteamVR_Input_Sources LeftInputSource = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Input_Sources RightInputSource = SteamVR_Input_Sources.RightHand;

    string m_lastPointDataName = "";
    string m_lastMagnetName = "";
    float m_distanceFromHandAndMagnet = 0;
    Quaternion m_LastRotation;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
         InvokeRepeating("UpdateRaycaster", 2.0f, 0.2f);
         InvokeRepeating("UpdateRaycasterForPointDataAndMagnets", 2.0f, 0.02f);
    }
    private void UpdateRaycaster()
    {
        RaycastHit raycastHit;
        GameObject gameObject = null;
        if (SteamVR_Actions._default.Squeeze.GetAxis(LeftInputSource) == 1 || SteamVR_Actions._default.Squeeze.GetAxis(RightInputSource) == 1)
        {
            if (Physics.Raycast(transform.position, transform.forward, out raycastHit))
            {
                gameObject = raycastHit.collider.gameObject;

                if (gameObject.tag.Contains("RaycasterUI"))
                {
                    InteractWithGUI(gameObject);
                }
            }  
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateRaycasterForPointDataAndMagnets()
    {
        RaycastHit raycastHit;
        GameObject gameObject = null;
        if (SteamVR_Actions._default.Squeeze.GetAxis(LeftInputSource) == 1 || SteamVR_Actions._default.Squeeze.GetAxis(RightInputSource) == 1)
        {
            if (Physics.Raycast(transform.position, transform.forward, out raycastHit))
            {
                gameObject = raycastHit.collider.gameObject;

                if (gameObject.tag.Contains("Magnet"))
                {
                    InteractWithMagnet(gameObject, raycastHit);
                }
                else if (gameObject.tag.Contains("DataPoint"))
                {
                    InteractWithPointData(gameObject);
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="raycastHit"></param>
    private void InteractWithMagnet(GameObject gameObject, RaycastHit raycastHit)
    {
        if (m_lastMagnetName != gameObject.name)
        {
            m_lastMagnetName = gameObject.name;
            m_distanceFromHandAndMagnet = Vector3.Distance(gameObject.transform.position, this.transform.position);
        }
        // Normalize vector between hand and raycast
        Vector3 normalizedVector = (raycastHit.point - this.transform.position).normalized;
        gameObject.transform.position = this.transform.position + ( normalizedVector * m_distanceFromHandAndMagnet);
    }    /// <summary>
         /// 
         /// </summary>
         /// <param name="gameObject"></param>
    private void InteractWithPointData(GameObject gameObject)
    {
        if (m_lastPointDataName != gameObject.name)
        {
            gameObject.transform.localScale *= 4;
            if (m_lastPointDataName != "")
            {
                GameObject.Find(m_lastPointDataName).transform.localScale /= 4;
            }
            m_lastPointDataName = gameObject.transform.name;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameObject"></param>
    private void InteractWithGUI(GameObject gameObject)
    {
        if (gameObject.name.Contains("DropDown"))
        {
            Dropdown dropdown = GameObject.Find(gameObject.name).GetComponent<Dropdown>();
            if (dropdown.options.Count > dropdown.value + 1)
            {
                dropdown.value = dropdown.value + 1;
            }
            else
            {
                dropdown.value = 0;
            }
        }
        if (gameObject.name.Contains("Toggle"))
        {
            Toggle toggle = GameObject.Find(gameObject.name).GetComponent<Toggle>();

            if (toggle.isOn == true)
            {
                toggle.isOn = false;
            }
            else if (toggle.isOn == false)
            {
                toggle.isOn = true;
            }
        }
    }
}
