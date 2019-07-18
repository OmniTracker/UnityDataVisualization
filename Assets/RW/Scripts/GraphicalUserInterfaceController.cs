/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          GraphicalUserInterfaceController
 *      Description:       
 *-----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                  
 *      Date:           7/18/2019
 *      Notes:          
 *                      
 *      Revision History:
 *      
 *      (7/18/2019) - Added class to be the controller of all GUI interactions.
 *                    Standalone and VR classes will interact with this class
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
using UnityEngine.UI;
using UnityEngine;

public class GraphicalUserInterfaceController : MonoBehaviour
{
    // The prefab for the data Magnet that will be instantiated
    public GameObject MagnetHolder;
    public GameObject PointHolder;
    // Public Dropdown menus
    public Dropdown XCoordinateDropDown;
    public Dropdown YCoordinateDropDown;
    public Dropdown ZCoordinateDropDown;
    public Dropdown MagnetNameDropDown;
    public Dropdown PointColorClassifierDropDown;

    // Public Toggle Objects
    public Toggle MagnetActiveToggle;
    public Toggle MagnetVisible;
    public Toggle AllowColorClassifier;
    public Toggle AllowSpringVisibility;
    public Toggle AllowPointSelectionAndMovement;

    // Allows the user to Show or hide the GUI
    public Toggle EnableGUICanvas;
    public Toggle EnablePointSelectionResize;

    public Button RevertToPreviousScene;
    public Button RestartScene;

    // Public
    public Text MagnetDataMenu;
    public Text PointDataMenu;

    private List<string> m_magnetList;
    private string m_CSVFilename;

    private void Start()
    {
        m_magnetList = new List<string>();
    }

    public void EnableGUIView ()
    {
        if (EnableGUICanvas.isOn == true)
        {

        }



    }

}

