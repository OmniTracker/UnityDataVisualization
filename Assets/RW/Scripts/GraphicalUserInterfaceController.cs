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
using System.Collections;

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
    ///  Used to instantiate all objects and setup all the necessary flags and data structures needed
    ///  for this class to properly operate.
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
    /// Used to load a new data file to the scene. If a new has not been selected, the load button 
    /// will opt tt load the previous data file again. If the file is incorrect for some reason,
    /// 
    /// </summary>
    private void LoadDataFile()
    {
        try
        {
            List<Dictionary<string, object>> pointList = CSVReader.Read(InputCSVFilename.Replace(".csv", ""));

            if (MagnetHolder.transform.childCount != 0)
            {
                var magnetChildren = new List<GameObject>();
                foreach (Transform magnetchild in MagnetHolder.transform)
                {
                    magnetChildren.Add(magnetchild.gameObject);
                }
                magnetChildren.ForEach(magnetchild => Destroy(magnetchild));
            }
            if (PointHolder.transform.childCount != 0)
            {
                var pointChildren = new List<GameObject>();
                foreach (Transform pointChild in PointHolder.transform)
                {
                    pointChildren.Add(pointChild.gameObject);
                }
                pointChildren.ForEach(pointChild => Destroy(pointChild));
            }
            
            SetFileDataPlot(pointList);
        }
        catch
        {
            
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void SelectDataFile()
    {
        // Set the internal flie name. This step will not change the GUI.
        string[] selectFileReturnArray = OpenInFileBrowser.SelectCSVFile();
        // Need to check
        if ((selectFileReturnArray.Length == 2) && selectFileReturnArray[0].Contains(".csv") )
        {
            InputCSVFilename = selectFileReturnArray[0].Replace(".csv","");
        }
    }
    /// <summary>
    /// This is only in the file while attempting to debug an issue.
    /// </summary>
    IEnumerator Example()
    {
        print(Time.time);
        yield return new WaitForSeconds(20);
        print(Time.time);
    }
    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        if (NewDataFileLoaded == true)
        {
            try
            {
                NewDataFileLoaded = false;
                List<Dictionary<string, object>> pointList = CSVReader.Read(InputCSVFilename.Replace(".csv", ""));
                SetFileDataPlot(pointList);
            }
            catch
            {
                return;
            }

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
    ///  Used to update the particle points in the scene based the selected drop down menu options.
    ///  If the droddown menu options are changes, this will cause the plots to adjust accordingly.
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
        List<Dictionary<string, object>> pointList = CSVReader.Read(InputCSVFilename.Replace(".csv", ""));

        PointRendererObject.SetScatterPlotAxis(InputCSVFilename,
                                       MagnetList[XAxisDropDown.value],
                                       MagnetList[YAxisDropDown.value],
                                       MagnetList[ZAxisDropDown.value],
                                       pointList);
        PointRendererObject.AlterPrefabParticlePoints(PointHolder.transform, ColorClassifierObject.Active);
    }


    /// <summary>
    /// Handle the generation of the particle points and the magnets in the scene. This fuction also
    /// handles placing the points in the correct places in the scene and connecting the particle
    /// point to the "springs"
    /// </summary>
    private void SetFileDataPlot(List<Dictionary<string, object>> pointList)
    {
        MagnetList.Clear();
        MagnetList = new List<string>(pointList[0].Keys);
        FillDropDowns();
        XAxisDropDown.value = 1;
        YAxisDropDown.value = 2;
        ZAxisDropDown.value = 3;
        PointRendererObject.GeneratePrefabParticlePoints(pointList, MagnetList, PointHolder, PointPrefab);
        PointRendererObject.SetScatterPlotAxis(InputCSVFilename,
                               MagnetList[XAxisDropDown.value],
                               MagnetList[YAxisDropDown.value],
                               MagnetList[ZAxisDropDown.value],
                               pointList);
        PointRendererObject.GenerateMagnets(MagnetList, MagnetHolder, MagnetPrefab, pointList);
        PointRendererObject.PlacePrefabParticlePoints(PointHolder.transform, ColorClassifierObject.Active);
        PointRendererObject.PlaceMagnets(MagnetHolder.transform);
        // Generate Springs Between Particles and origin 
        DynamicPointToPointLineRender.GenerateLinksBetweenParticleAndOrigin(PointHolder.transform); 
    }
    /// <summary>
    ///  Used to handle the Magnet Attributes. 
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
    ///  Used to handle all the attribute of the Particle Points. This includes updating the 
    ///  the linerender of the particle points to the origin lines and setting the mass of the particle
    ///  points
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
    ///  Used to handle the view of the LineRender in the scene.
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
    /// Handles the color correlation portion of the graph. The color of the points above and below the selected threshhold takes
    /// on the color of the magnet select to change color.
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
                 ( 0 <= cutOffValue )  && 
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
    /// Automatically Fills all the dropdown menus in the scene.
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

