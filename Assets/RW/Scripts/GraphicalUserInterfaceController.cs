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
    private string m_PreviouslySelectedMagnet;
    public Dropdown DynamicPointRenderingDropDown;
    public Dropdown ColorCorrelationDropDown;
    public Dropdown ColorCorrelationAboveOrBelowMidPointDropDown;
    // Public Toggle Objects
    public Toggle MagnetActiveToggle;
    public Toggle MagnetVisible;
    public Toggle AllowColorClassifier;
    public Toggle AllowSpringVisibility;
    public Text ColorCorrelationMinimumValueTextField;
    public Text ColorCorrelationMaximumValueTextField;
    public InputField ColorCorrelationMidPointInputField;
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
    private ColorClassifier colorClassifierObject;
    public bool NewDataFileLoaded { get => m_NewDataFileLoaded; set => m_NewDataFileLoaded = value; }
    public List<string> MagnetList { get => m_MagnetList; set => m_MagnetList = value; }
    public PointRenderer PointRendererObject { get => m_PointRender; set => m_PointRender = value; }
    public ColorClassifier ColorClassifierObject { get => colorClassifierObject; set => colorClassifierObject = value; }
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        ColorClassifierObject = new ColorClassifier();
        PointRendererObject = new PointRenderer();
        MagnetList = new List<string>();
        AllowColorClassifier.isOn = ColorClassifierObject.Active;
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
            CheckScatterPlotAttributesUI();
            // CheckMagnetAttributesUI();
            CheckParticlePointAttributesUI();
            CheckDynamicLineRenderingUI();
            CheckColorCorrelationUI();
        }
        PlotController.OrientLables();
    }
    /// <summary>
    /// 
    /// </summary>
    private void CheckScatterPlotAttributesUI()
    {
        if ((PointRendererObject.XAxis != MagnetList[XAxisDropDown.value]) ||
            (PointRendererObject.YAxis != MagnetList[YAxisDropDown.value]) ||
            (PointRendererObject.ZAxis != MagnetList[ZAxisDropDown.value]))
        {
            UpdateFileDataPlot();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateFileDataPlot()
    {
        PointRendererObject.SetScatterPlotAxis(InputCSVFilename,
                                       MagnetList[XAxisDropDown.value],
                                       MagnetList[YAxisDropDown.value],
                                       MagnetList[ZAxisDropDown.value],
                                       PointHolder.transform);
        PointRendererObject.AlterPrefabParticlePoints(PointHolder.transform, ColorClassifierObject.Active);
    }
    /// <summary>
    /// 
    /// </summary>
    private void SetFileDataPlot()
    {
        List<Dictionary<string, object>> pointList = CSVReader.Read(InputCSVFilename);
        MagnetList = new List<string>(pointList[1].Keys);
        FillDropDowns();
        PointRendererObject.GeneratePrefabParticlePoints(pointList, MagnetList, PointHolder, PointPrefab);
        PointRendererObject.GenerateMagnets(MagnetList, MagnetHolder, MagnetPrefab, PointHolder);
        XAxisDropDown.value = 1;
        YAxisDropDown.value = 2;
        ZAxisDropDown.value = 3;
        PointRendererObject.SetScatterPlotAxis(InputCSVFilename, 
                                       MagnetList[XAxisDropDown.value], 
                                       MagnetList[YAxisDropDown.value], 
                                       MagnetList[ZAxisDropDown.value], 
                                       PointHolder.transform);
        PointRendererObject.PlacePrefabParticlePoints(PointHolder.transform, ColorClassifierObject.Active);
        PointRendererObject.PlaceMagnets(MagnetHolder.transform);
    }
    /// <summary>
    /// 
    /// </summary>
    private void CheckMagnetAttributesUI ()
    {
        GameObject magnetObject = GameObject.Find(SelectMagnetDropDown.options[SelectMagnetDropDown.value].text); 
        MagnetAttributes attributes = magnetObject.GetComponent<MagnetAttributes>();
        // Check if the Magnet Dropdown has changed
        if (attributes.name != m_PreviouslySelectedMagnet)
        {
            MagnetActiveToggle.isOn = attributes.MagnetActive;
            MagnetVisible.isOn = attributes.MagnetVisible;
            if (MagnetVisible.isOn == false)
            {
                magnetObject.SetActive(false);
            }
            else
            {
                magnetObject.SetActive(true);
            }
            m_PreviouslySelectedMagnet = attributes.name;
        }
        else
        {
            // Set
            if (MagnetActiveToggle.isOn != attributes.MagnetActive)
            {
                attributes.MagnetActive = MagnetActiveToggle.isOn;
            }

            if (MagnetVisible.isOn != attributes.MagnetActive)
            {

            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void CheckParticlePointAttributesUI ()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    private void CheckDynamicLineRenderingUI ()
    {

    }
    /// <summary>
    /// 
    /// </summary>
    private void CheckColorCorrelationUI()
    {
        // Get name of selected option.
        string magnetName = ColorCorrelationDropDown.options[ColorCorrelationDropDown.value].text;
        string cutOffOption = ColorCorrelationAboveOrBelowMidPointDropDown.options[ColorCorrelationAboveOrBelowMidPointDropDown.value].text;
        MagnetAttributes magnetAttributes = MagnetHolder.transform.Find(magnetName).transform.GetComponent<MagnetAttributes>();
        Color magnetColor = MagnetHolder.transform.Find(magnetName).transform.GetComponent<Renderer>().material.color;




        ColorCorrelationMinimumValueTextField.text = magnetAttributes.MinValue.ToString(); 
        ColorCorrelationMaximumValueTextField.text = magnetAttributes.MaxValue.ToString();


        Debug.Log(cutOffOption);

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

