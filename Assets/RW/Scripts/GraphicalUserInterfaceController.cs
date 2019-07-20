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
    // The prefab for the Point Data and Magnets
    public GameObject MagnetHolder;
    public GameObject MagnetPrefab;
    public GameObject PointHolder;
    public GameObject PointPrefab;

    // Public Dropdown menus
    public Dropdown XAxisDropDown;
    public Dropdown YAxisDropDown;
    public Dropdown ZAxisDropDown;
    public Dropdown SelectMagnetDropDown;
    public Dropdown DynamicPointRenderingDropDown;
    public Dropdown ColorCorrelationDropDown;
    public Dropdown ColorCorrelationAboveOrBelowMidPointDropDown;
    // Public Toggle Objects
    public Toggle MagnetActiveToggle;
    public Toggle MagnetVisible;
    public Toggle AllowColorClassifier;
    public Toggle AllowSpringVisibility;
    // Allows the user to Show or hide the GUI
    public Toggle EnableGUICanvas;
    public Button RevertToPreviousScene;
    public Button RestartScene;
    // Public
    public Text MagnetDataMenu;
    public Text PointDataMenu;
    // Set the first data file that needs to be loaded
    public string m_InputCSVFilename;
    private List<string> m_MagnetList;
    private bool m_DataFileLoaded = false;
    PointRenderer m_PointRender;

    public bool DataFileLoaded { get => m_DataFileLoaded; set => m_DataFileLoaded = value; }
    public List<string> MagnetList { get => m_MagnetList; set => m_MagnetList = value; }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        m_PointRender = new PointRenderer();
        MagnetList = new List<string>();
    }
    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (DataFileLoaded == false)
        {
            UpdateDataView();
            DataFileLoaded = true;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateDataView()
    {
        List<Dictionary<string, object>> pointList = CSVReader.Read(m_InputCSVFilename);
        MagnetList = new List<string>(pointList[1].Keys);
        FillDropDowns();


    }
    /// <summary>
    /// 
    /// </summary>
    private void FillDropDowns ()
    {
        List<string> options = new List<string>();
        XAxisDropDown.ClearOptions();
        YAxisDropDown.ClearOptions();
        ZAxisDropDown.ClearOptions();
        SelectMagnetDropDown.ClearOptions();
        DynamicPointRenderingDropDown.ClearOptions();
        ColorCorrelationDropDown.ClearOptions();
        ColorCorrelationAboveOrBelowMidPointDropDown.ClearOptions();
        // Iterate though the column list to create the individual magnets
        foreach (string magnet in MagnetList)
        {
            options.Add(magnet);
        }
        XAxisDropDown.AddOptions(options);
        YAxisDropDown.AddOptions(options);
        ZAxisDropDown.AddOptions(options);
        SelectMagnetDropDown.AddOptions(options);
        DynamicPointRenderingDropDown.AddOptions(options);
        ColorCorrelationDropDown.AddOptions(options);
        options.Clear();
        options.Add("Above");
        options.Add("Below");
        ColorCorrelationAboveOrBelowMidPointDropDown.AddOptions(options);
    }
}

