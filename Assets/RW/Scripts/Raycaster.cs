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
 *      (7/23/2019) - Software Refactor - Adjusted Raycast to handle GUI 
 *                                        similar to standalone.
 *      (8/04/2019) - Software Additions - Adding additional comments and adding
 *                                        more button interaction.
 *-----------------------------------------------------------------------------
 * This program is free software: you can redistribute it and/or modify it 
 * under the terms of the GNU General Public License as published by the Free 
 * Software Foundation, either version 3 of the License, or (at your option) 
 * any later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT 
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or 
 * FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for 
 * more details.
 */

using UnityEngine;
using Valve.VR;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;

public class Raycaster : MonoBehaviour
{
    // 
    public SteamVR_Input_Sources LeftInputSource = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Input_Sources RightInputSource = SteamVR_Input_Sources.RightHand;
    
    // 
    string m_lastPointDataName = "";
    string m_lastMagnetName = "";
    float m_distanceFromHandAndMagnet = 0;
    Quaternion m_LastRotation;

    /// <summary>
    /// Used to setup the periodic calls to the UpdateRaycaster and 
    /// UpdateRaycasterForPointDataAndMagnets function calls. The rate at which
    /// these calls can be made
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
        if (SteamVR_Actions._default.Squeeze.GetAxis(LeftInputSource) == 1 ||
            SteamVR_Actions._default.Squeeze.GetAxis(RightInputSource) == 1)
        {
            if (Physics.Raycast(transform.position, 
                transform.forward, 
                out raycastHit))
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
    /// Use Raycaster to detect a data point or magnet hit.
    /// </summary>
    private void UpdateRaycasterForPointDataAndMagnets()
    {
        // Set Raycaster and Hit object
        RaycastHit raycastHit;
        GameObject gameObject = null;

        // Check whether the left or right trigger has been hit.
        if (SteamVR_Actions._default.Squeeze.GetAxis(LeftInputSource) == 1 || 
            SteamVR_Actions._default.Squeeze.GetAxis(RightInputSource) == 1)
        {
            // If the raycaster recieved a hit, return the hit object
            if (Physics.Raycast(transform.position, 
                transform.forward, 
                out raycastHit))
            {
                // set the hit object to the game object
                gameObject = raycastHit.collider.gameObject;

                // Check to see if the tag connect to the object is a magnet
                if (gameObject.tag.Contains("Magnet"))
                {
                    InteractWithMagnet(gameObject, raycastHit);
                }

                // Check to see if the tag connect to the object is a data point 
                else if (gameObject.tag.Contains("DataPoint"))
                {
                    InteractWithPointData(gameObject);
                }
            }
        }
    }
    /// <summary>
    /// If the raycaster detect that a magnet was hit, then this function will
    /// will be used to move the magnet around the scene as long as the user is
    /// holding the trigger down.
    /// </summary>
    /// <param name="gameObject">Game Object returned from Raycast hit</param>
    /// <param name="raycastHit">The Raycast hit collect from laser</param>
    private void InteractWithMagnet(GameObject gameObject, 
                                    RaycastHit raycastHit)
    {
        if (m_lastMagnetName != gameObject.name)
        {
            m_lastMagnetName = gameObject.name;
            m_distanceFromHandAndMagnet 
                = Vector3.Distance(gameObject.transform.position,
                                    this.transform.position);
        }
        // Normalize vector between hand and raycast
        Vector3 normalizedVector 
            = (raycastHit.point - this.transform.position).normalized;

        gameObject.transform.position 
            = this.transform.position + ( normalizedVector * m_distanceFromHandAndMagnet);
    }    
    /// <summary> 
    /// If the Raycast detects a data point, this function will handle all 
    /// interfacing with the data point itself.
    /// </summary>
    /// <param name="gameObject">Game Object returned from Raycast hit</param>
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
    /// If the Raycaster hits a UI element, this function will check which type
    /// of UI element was hit. Once the UI element type is indicated, the function
    /// will carry out further action.
    /// </summary>
    /// <param name="gameObject">Game Object returned from Raycast hit</param>
    private void InteractWithGUI(GameObject gameObject)
    {
        // check if the tag of the game object is DropDown
        if (gameObject.name.Contains("DropDown"))
        {
            Dropdown dropdown = GameObject.Find(gameObject.name)
                                .GetComponent<Dropdown>();

            // Check to see whether or not we are able to increment the dropdown
            // index by one.
            if (dropdown.options.Count > dropdown.value + 1)
            {
                dropdown.value = dropdown.value + 1;
            }
            else
            {
                // Reset the dropdown index if max index has already been reached
                dropdown.value = 0;
            }
        }

        // Check if the tag of the game object is a Toggle
        if (gameObject.name.Contains("Toggle"))
        {
            Toggle toggle = GameObject.Find(gameObject.name)
                .GetComponent<Toggle>();

            if (toggle.isOn == true)
            {
                toggle.isOn = false;
            }
            else if (toggle.isOn == false)
            {
                toggle.isOn = true;
            }
        }

        // Check if the tag of the game object is a Button
        if(gameObject.name.Contains("utton"))
        {
            // This is pretty dirty, but I want to finish this now.
            // I would have done this cleaner, but this is the last day before I do my final push.
            if (gameObject.name.Contains("TransitionButton"))
            {
                int m_MainSceneIndex = 0;
                SceneManager.LoadScene(m_MainSceneIndex);
            }
            else if (gameObject.name.Contains("MidPointInput")) 
            {
                // Get the field
                string currentStringValue = GameObject.Find("MidPointInputFieldText").GetComponent<InputField>().textComponent.ToString();
                float currentFloatValue;
                // This basically means the value is zero.
                if (currentStringValue == "")
                {
                    currentFloatValue = 0.0f;
                }
                else
                {
                    currentFloatValue = float.Parse(currentStringValue, CultureInfo.InvariantCulture.NumberFormat);
                }
                // I'm not capping either end. I don't want to restrict these numbers and 
                // introduce a bug.
                if (gameObject.name.Contains("Up"))
                {
                    currentFloatValue += 1.0f;
                }
                else if (gameObject.name.Contains("Down"))
                {
                    currentFloatValue -= 1.0f;
                }
                GameObject.Find("MidPointInputFieldText").GetComponent<InputField>().text = currentFloatValue.ToString();
            }
            else if (gameObject.name.Contains("ParticleMassInput"))
            {
                // Get the field

                string currentStringValue = GameObject.Find("ParticleMassInputFieldText")
                                                      .GetComponent<Text>().text;
                float currentFloatValue;

                // This basically means the value is zero.
                if (currentStringValue == "")
                {
                    currentFloatValue = 0.0f;
                }
                else
                {
                    currentFloatValue  = float.Parse(currentStringValue, CultureInfo.InvariantCulture.NumberFormat);
                }
                // We can't have a negative mass. So don't do this
                if (gameObject.name.Contains("Down") && (currentFloatValue > 0.0f))
                {
                    currentFloatValue -= 10.0f;
                }
                // I'm capping the mass of the mass to 200.
                else if (gameObject.name.Contains("Up") && (currentFloatValue < 200.0f) )
                {
                    currentFloatValue += 10.0f;
                }
                GameObject.Find("ParticleMassInputFieldText").GetComponent<Text>().text = currentFloatValue.ToString();
            }
            else if (gameObject.name.Contains("MagnetStrengthPercentInput"))
            {
                // Get the field
                string currentStringValue = GameObject.Find("MagnetStrengthPercentInputFieldText")
                                                      .GetComponent<Text>().text;
                float currentFloatValue;
                // This basically means the value is zero.
                if (currentStringValue == "")
                {
                    currentFloatValue = 0.0f;
                }
                else
                {
                    currentFloatValue = float.Parse(currentStringValue, CultureInfo.InvariantCulture.NumberFormat);
                }
                // I'm capping these values to 100 and -100.
                if (gameObject.name.Contains("Up") && (currentFloatValue < 100.0f)) 
                {
                    currentFloatValue += 10.0f;
                }
                else if (gameObject.name.Contains("Down") && ( currentFloatValue > -100.0f))
                {
                    currentFloatValue -= 10.0f;
                }
                GameObject.Find("MagnetStrengthPercentInputFieldText").GetComponent<Text>().text = currentFloatValue.ToString();
            }
        }
    }
}
