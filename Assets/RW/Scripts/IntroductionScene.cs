/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          IntroductionScene
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
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class IntroductionScene : MonoBehaviour
{
    List<string> m_DropOptions = new List<string> { "Standalone"};
    public Dropdown m_Dropdown;
    private bool m_Debug = true;

    private void Start()
    {
        m_Dropdown.ClearOptions();
        if (XRDevice.isPresent) {
            if (m_Debug)
                Debug.Log("Open VR is present :" + XRDevice.model);
            m_DropOptions.Add(XRDevice.model);
        } else {
            if (m_Debug)
                Debug.Log("Open VR is not Present");
        }
        m_Dropdown.AddOptions(m_DropOptions);

        XRSettings.enabled = false;
    }
    /// <summary>
    /// Load New Scene from the given VR Scenes
    /// </summary>
    public void LoadNewScene ()
    {
        // Increment index to correspond to the Build Scene Settings 
        int dropDownIndex = m_Dropdown.value + 1;
        if (m_Debug)
            Debug.Log("Selected Dropdown Index :" + dropDownIndex);
        SceneManager.LoadScene(dropDownIndex);
    }

}
