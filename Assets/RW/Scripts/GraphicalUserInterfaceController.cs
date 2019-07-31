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
    bool m_AllowSpringVisibilityFlag = false; 
    public Toggle EnableLineRender;
    public Text ColorCorrelationMinimumValueTextField;
    public Text ColorCorrelationMaximumValueTextField;
    
    public InputField ColorCorrelationMidPointInputField;
    public InputField MagnetStrengthPercentInputField;
    public InputField ParticleMassInputField;
    // Buttons used for Setting and Loading Data files
    public Button LoadDataButton;
    public Button SelectDataButton;
    public Text FileNameText;
    // Allows the user to Show or hide the GUI
    public Toggle EnableGUICanvas;
    public Button RevertToPreviousScene;
    public Button RestartScene;
    // Public
    public Text MagnetDataMenu;
    public Text PointDataMenu;
    // Set the first data file that needs to be loaded
    public string InputCSVFilename;
    private string m_DynamicPointRenderingMagnetName = "";
    // Getters and Setters
    private LineRenderer m_DynamicLineRenderer;
    private List<string> m_MagnetList;
    private bool m_NewDataFileLoaded = true;
    PointRenderer m_PointRender;
    private ColorClassifier colorClassifierObject;
    public bool NewDataFileLoaded { get => m_NewDataFileLoaded; set => m_NewDataFileLoaded = value; }
    public List<string> MagnetList { get => m_MagnetList; set => m_MagnetList = value; }
    public PointRenderer PointRendererObject { get => m_PointRender; set => m_PointRender = value; }
    public ColorClassifier ColorClassifierObject { get => colorClassifierObject; set => colorClassifierObject = value; }
    public LineRenderer DynamicLineRenderer { get => m_DynamicLineRenderer; set => m_DynamicLineRenderer = value; }

    GameObject GUICanvas;
    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        EnableLineRender.isOn = false; 
        ColorClassifierObject = new ColorClassifier();
        PointRendererObject = new PointRenderer();
        MagnetList = new List<string>();
        AllowColorClassifier.isOn = ColorClassifierObject.Active;
        DynamicLineRenderer = GetComponent<LineRenderer>();
        DynamicLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        GUICanvas = GameObject.Find("MainContolPanel");
        LoadDataButton.onClick.AddListener(() => LoadDataFile());
        SelectDataButton.onClick.AddListener(() => SelectDataFile());

    }
    /// <summary>
    /// 
    /// </summary>
    private void LoadDataFile()
    {
        Debug.Log("This");
    }
    /// <summary>
    /// 
    /// </summary>
    private void SelectDataFile()
    {
        // string fileName = OpenInFileBrowser.SelectCSVFile();

        // Debug.Log(fileName);
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
            if (GUICanvas.activeInHierarchy)
            {
                CheckScatterPlotAttributesUI();
                CheckMagnetAttributesUI();
                CheckDynamicLineRenderingUI();
                CheckColorCorrelationUI();
            }

            CheckParticlePointAttributesUI();
            GUICanvas.SetActive(EnableGUICanvas.isOn);
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
        // Generate Springs Between Particles and origin 
        DynamicPointToPointLineRender.GenerateLinksBetweenParticleAndOrigin(PointHolder.transform); 
    }
    /// <summary>
    /// 
    /// </summary>
    private void CheckMagnetAttributesUI ()
    {
        string magnetName = SelectMagnetDropDown.options[SelectMagnetDropDown.value].text;
        MagnetAttributes magnetAttributes = MagnetHolder.transform.Find(magnetName).transform.GetComponent<MagnetAttributes>();
        if (magnetAttributes.name != m_PreviouslySelectedMagnet)
        {
            MagnetActiveToggle.isOn = magnetAttributes.MagnetActive;
            MagnetVisible.isOn = magnetAttributes.MagnetVisible;
 
            m_PreviouslySelectedMagnet = magnetAttributes.name;
        }
        else
        {
            if (magnetAttributes.MagnetActive != MagnetActiveToggle.isOn)
            {
                magnetAttributes.MagnetActive = MagnetActiveToggle.isOn;
            }
            if (magnetAttributes.MagnetVisible != MagnetVisible.isOn)
            {
                magnetAttributes.MagnetVisible = MagnetVisible.isOn;
            }
            bool magnetInScene = MagnetHolder.transform.Find(magnetName).transform.GetComponent<Renderer>().enabled;
            if (magnetInScene != MagnetVisible.isOn)
            {
                MagnetHolder.transform.Find(magnetName).transform.GetComponent<Renderer>().enabled = MagnetVisible.isOn;
            }
            if ((float.TryParse(MagnetStrengthPercentInputField.text, out float percentage) == true) &&
                percentage >= -100.0f &&
                percentage <= 100.0f)  {
                magnetAttributes.MagnetismStrength = (percentage / 10.0f);
            }
            else
            {
                MagnetStrengthPercentInputField.text = "";
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void CheckParticlePointAttributesUI ()
    {
        if( AllowSpringVisibility.isOn != m_AllowSpringVisibilityFlag)
        {
            m_AllowSpringVisibilityFlag = AllowSpringVisibility.isOn;
            DynamicPointToPointLineRender.EnableLinksBetweenParticleAndOrigin(PointHolder.transform, m_AllowSpringVisibilityFlag); 
        }
        if (m_AllowSpringVisibilityFlag)
        {
            DynamicPointToPointLineRender.UpdateSpringPositions(PointHolder.transform);
        }
        if (float.TryParse(ParticleMassInputField.text, out float particleMass) == true &&
            particleMass >= 0 &&
            particleMass <= 100)
        {
            if (PointRendererObject.SetParticleMass != particleMass)
            {
                if ((0 <= particleMass) && (particleMass <= 100))
                {
                    PointRendererObject.UpdateParticlePointsMass(PointHolder.transform, particleMass);
                }
            }
        }
        else
        {
            ParticleMassInputField.text = "";
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void CheckDynamicLineRenderingUI ()
    {
        if ( (EnableLineRender.isOn == true))
        {
            string magnetName = DynamicPointRenderingDropDown.options[DynamicPointRenderingDropDown.value].text;
            DynamicPointToPointLineRender.SortPointsForLineRender(PointHolder.transform, DynamicLineRenderer, magnetName);
            m_DynamicPointRenderingMagnetName = magnetName;
            DynamicLineRenderer.enabled = true;
        }
        else if ((EnableLineRender.isOn == false))
        {
            DynamicLineRenderer.enabled = false;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void CheckColorCorrelationUI()
    {
        ColorClassifierObject.Active = AllowColorClassifier.isOn;
        string magnetName = ColorCorrelationDropDown.options[ColorCorrelationDropDown.value].text;
        MagnetAttributes magnetAttributes = MagnetHolder.transform.Find(magnetName).transform.GetComponent<MagnetAttributes>();
        if (ColorClassifierObject.PreviousMagnet != magnetName)
        {
            ColorCorrelationMinimumValueTextField.text = magnetAttributes.MinValue.ToString();
            ColorCorrelationMaximumValueTextField.text = magnetAttributes.MaxValue.ToString();
        }
        if (ColorClassifierObject.Active == true)
        {
            float minValue = magnetAttributes.MinValue;
            float maxvalue = magnetAttributes.MaxValue;

            if ( ( float.TryParse(ColorCorrelationMidPointInputField.text, out float cutOffValue) == true) &&
                 ( minValue <= cutOffValue )  && 
                 ( cutOffValue <= maxvalue ) )
            {
                string cutOffOption = ColorCorrelationAboveOrBelowMidPointDropDown.options[ColorCorrelationAboveOrBelowMidPointDropDown.value].text;
                if (ColorClassifierObject.PreviousCutOffOption != cutOffOption ||
                     ColorClassifierObject.PreviousValue != cutOffValue ||
                     ColorClassifierObject.PreviousMagnet != magnetName)
                {
                    Color magnetColor = MagnetHolder.transform.Find(magnetName).transform.GetComponent<Renderer>().material.color;
                    ColorClassifierObject.AlterScatterPlotColorsBasedOnInput(PointHolder.transform, magnetColor, magnetName, cutOffOption, cutOffValue);
                }
            }
            else
            {
                ColorCorrelationMidPointInputField.text = "";
            }
        }
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

