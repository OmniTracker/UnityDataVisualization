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
    public string InputCSVFilename;
    private List<string> m_MagnetList;
    private bool m_NewDataFileLoaded = true;
    PointRenderer m_PointRender;
    public bool NewDataFileLoaded { get => m_NewDataFileLoaded; set => m_NewDataFileLoaded = value; }
    public List<string> MagnetList { get => m_MagnetList; set => m_MagnetList = value; }
    public PointRenderer PointRender { get => m_PointRender; set => m_PointRender = value; }
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        PointRender = new PointRenderer();
        MagnetList = new List<string>();
    }
    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (NewDataFileLoaded == true)
        {
            SetFileDataPlot();
            NewDataFileLoaded = false;
        }
        else
        {
            // Check to see if the Axis have changed.
            if ((PointRender.XAxis != MagnetList[XAxisDropDown.value]) ||
                (PointRender.YAxis != MagnetList[YAxisDropDown.value]) ||
                (PointRender.ZAxis != MagnetList[ZAxisDropDown.value]) )
            {
                UpdateFileDataPlot();
            }

        }
        PlotController.OrientLables();
    }
    private void UpdateFileDataPlot()
    {
        PointRender.SetScatterPlotAxis(InputCSVFilename,
                                       MagnetList[XAxisDropDown.value],
                                       MagnetList[YAxisDropDown.value],
                                       MagnetList[ZAxisDropDown.value],
                                       PointHolder.transform);
        PointRender.AlterPrefabParticlePoints(PointHolder.transform);
    }
    /// <summary>
    /// 
    /// </summary>
    private void SetFileDataPlot()
    {
        List<Dictionary<string, object>> pointList = CSVReader.Read(InputCSVFilename);
        MagnetList = new List<string>(pointList[1].Keys);
        FillDropDowns();
        PointRender.GeneratePrefabParticlePoints(pointList, MagnetList, PointHolder, PointPrefab);
        PointRender.GenerateMagnets(MagnetList, MagnetHolder, MagnetPrefab, PointHolder);
        XAxisDropDown.value = 1;
        YAxisDropDown.value = 2;
        ZAxisDropDown.value = 3;
        PointRender.SetScatterPlotAxis(InputCSVFilename, 
                                       MagnetList[XAxisDropDown.value], 
                                       MagnetList[YAxisDropDown.value], 
                                       MagnetList[ZAxisDropDown.value], 
                                       PointHolder.transform);
        PointRender.PlacePrefabParticlePoints(PointHolder.transform);
        PointRender.PlaceMagnets(MagnetHolder.transform);

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

