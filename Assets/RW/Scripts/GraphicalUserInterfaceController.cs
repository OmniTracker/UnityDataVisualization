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
 *      (07/18/2019) - Added class to be the controller of all GUI 
 *                     interactions. Standalone and VR classes will interact 
 *                     with this class
 *      (08/01/2019) - Final comments before handing project over.           
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
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class GraphicalUserInterfaceController : MonoBehaviour
{
    // All the GUI fields needs to be properly set in the inspector. If any of the values 
    // are not set correctly, this will cause a failure some where in the files. 
    // There if not a lot of code to check for NULL objects.
    
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
    public bool NewDataFileLoaded { get => m_NewDataFileLoaded;
                                    set => m_NewDataFileLoaded = value; }
    public List<string> MagnetList { get => m_MagnetList;
                                     set => m_MagnetList = value; }
    public PointRenderer PointRendererObject { get => m_PointRender;
                                               set => m_PointRender = value; }
    public ColorClassifier ColorClassifierObject { get => colorClassifierObject;
                                                   set => colorClassifierObject = value; }
    public LineRenderer DynamicLineRenderer { get => m_DynamicLineRenderer;
                                              set => m_DynamicLineRenderer = value; }

    GameObject GUICanvas;
    /// <summary>
    ///  Used to instantiate all objects and setup all the necessary flags and 
    ///  data structures needed for this class to properly operate.
    /// </summary>
    private void Start()
    {
        EnableLineRender.isOn = false; 
        // instantiate objects needed for this project
        ColorClassifierObject = new ColorClassifier();
        PointRendererObject = new PointRenderer();
        MagnetList = new List<string>();
        AllowColorClassifier.isOn = ColorClassifierObject.Active;
        DynamicLineRenderer = GetComponent<LineRenderer>();
        DynamicLineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        GUICanvas = GameObject.Find("MainContolPanel");
        // Add Listeners to recieve notifications that a button has been pressed.
        LoadDataButton.onClick.AddListener(() => LoadDataFile());
        SelectDataButton.onClick.AddListener(() => SelectDataFile());

    }
    /// <summary>
    /// Used to load a new data file to the scene. If a new has not been 
    /// selected, the load button will opt tt load the previous data file 
    /// again. If the file is incorrect for some reason,
    /// </summary>
    private void LoadDataFile()
    {
        try
        {
            List<Dictionary<string, object>> pointList = 
                CSVReader.Read(InputCSVFilename.Replace(".csv", ""));

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
    /// Used to select a new file to attempt to plot to the scene. This
    /// </summary>
    private void SelectDataFile()
    {
        // Set the internal flie name. This step will not change the GUI.
        string[] selectFileReturnArray = OpenInFileBrowser.SelectCSVFile();
        // Need to check
        if ((selectFileReturnArray.Length == 2) && 
            (selectFileReturnArray[0].Contains(".csv")))
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
    /// Used to handle all updates associated with
    /// </summary>
    private void Update()
    {
        if (NewDataFileLoaded == true)
        {
            // This try is a little messy. I tried to fix a bug by reloading the scene twice.
            // It didn't really help, but I ran out of time to fix this issue.
            try
            {
                NewDataFileLoaded = false;
                List<Dictionary<string, object>> pointList 
                = CSVReader.Read(InputCSVFilename.Replace(".csv", ""));
                SetFileDataPlot(pointList);
            }
            catch
            {
                return;
            }
        }
        else
        {  
            // This section is the main focus of the update function. If the canvas
            // is removed from view, there is no reason to check any of these functions
            // because they have been removed from the field of view of the user. 
            // This is the same for both the VR and standalone version of the application.
            if (GUICanvas.activeInHierarchy)
            {
                // Check for all the fields specified in the
                CheckScatterPlotAttributesUI();
                CheckMagnetAttributesUI();
                CheckDynamicLineRenderingUI();
                CheckColorCorrelationUI();
            }
            // This needs to be checked whether or not the GUI canvas is already in the scene.
            CheckParticlePointAttributesUI();
            GUICanvas.SetActive(EnableGUICanvas.isOn);
        }
        
        // The plot labels should constantly be adjusted based on the location of the
        // main camera in the scene.
        PlotController.OrientLables();
    }
    /// <summary>
    ///  Used to update the particle points in the scene based the selected 
    ///  drop down menu options. If the droddown menu options are changes, this 
    ///  will cause the plots to adjust accordingly.
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
    /// Used to update the scatter plot based off the current filename stored. 
    /// The seleted file
    /// </summary>
    private void UpdateFileDataPlot()
    {
        List<Dictionary<string, object>> pointList = 
                          CSVReader.Read(InputCSVFilename.Replace(".csv", ""));

        PointRendererObject.SetScatterPlotAxis(InputCSVFilename,
                                       MagnetList[XAxisDropDown.value],
                                       MagnetList[YAxisDropDown.value],
                                       MagnetList[ZAxisDropDown.value],
                                       pointList);
        PointRendererObject.AlterPrefabParticlePoints(PointHolder.transform, 
                                                      ColorClassifierObject.Active);
    }
    /// <summary>
    /// Handle the generation of the particle points and the magnets in the 
    /// scene. This fuction also handles placing the points in the correct 
    /// places in the scene and connecting the particle point to the "springs"
    /// </summary>
    private void SetFileDataPlot(List<Dictionary<string, object>> pointList)
    {
        MagnetList.Clear();
        MagnetList = new List<string>(pointList[0].Keys);
        FillDropDowns();
        XAxisDropDown.value = 1;
        YAxisDropDown.value = 2;
        ZAxisDropDown.value = 3;
        // The ordering of how these functions are called actual matters.
        // The first thing that need to be done is for the data points to be 
        // generated.
        PointRendererObject.GeneratePrefabParticlePoints(pointList, 
                                                         MagnetList, 
                                                         PointHolder, 
                                                         PointPrefab);
        // Since we now have data points to plot, we can now fill in 
        // the scatter plot attributes
        PointRendererObject.SetScatterPlotAxis(InputCSVFilename,
                               MagnetList[XAxisDropDown.value],
                               MagnetList[YAxisDropDown.value],
                               MagnetList[ZAxisDropDown.value],
                               pointList);
        // The magnets generate depends on the data points generated. If the
        // point are not generted, this function should fail.
        PointRendererObject.GenerateMagnets(MagnetList, 
                                            MagnetHolder, 
                                            MagnetPrefab, 
                                            pointList);
        // The final thing which needs to be done is placing all the game objects in the
        // scene. These function should be done last because both of these calls depend
        // on the generation of the data point and the magnets.
        PointRendererObject.PlacePrefabParticlePoints(PointHolder.transform, 
                                                      ColorClassifierObject.Active);
        PointRendererObject.PlaceMagnets(MagnetHolder.transform);
        // Generate Springs Between Particles and origin 
        DynamicPointToPointLineRender
            .GenerateLinksBetweenParticleAndOrigin(PointHolder.transform); 
    }
    /// <summary>
    ///  Used to handle the Magnet Attributes. 
    /// </summary>
    private void CheckMagnetAttributesUI ()
    {
        // Get the magnet name based on the current position of the dropdown menu. 
        string magnetName = 
                 SelectMagnetDropDown.options[SelectMagnetDropDown.value].text;
        MagnetAttributes magnetAttributes = MagnetHolder.transform
                                            .Find(magnetName)
                                            .transform.GetComponent<MagnetAttributes>();
        // Check if the current dropdown value matches the previously stored selected magnet
        if (magnetAttributes.name != m_PreviouslySelectedMagnet)
        {
            // Updates the GUI if the dropdown of the menu has changed.
            MagnetActiveToggle.isOn = magnetAttributes.MagnetActive;
            MagnetVisible.isOn = magnetAttributes.MagnetVisible;
            m_PreviouslySelectedMagnet = magnetAttributes.name;
        }
        else
        {
            //  Changes the GUI to reflect the attribute of the magnet.
            if (magnetAttributes.MagnetActive != MagnetActiveToggle.isOn)
            {
                magnetAttributes.MagnetActive = MagnetActiveToggle.isOn;
            }
            if (magnetAttributes.MagnetVisible != MagnetVisible.isOn)
            {
                magnetAttributes.MagnetVisible = MagnetVisible.isOn;
            }
            
            // Check to see whether or not the magnet is currently in view in the 
            // scene.
            bool magnetInScene 
                = MagnetHolder.transform.Find(magnetName)
                              .transform.GetComponent<Renderer>().enabled;

            if (magnetInScene != MagnetVisible.isOn)
            {
                MagnetHolder.transform.Find(magnetName).transform
                            .GetComponent<Renderer>().enabled 
                            = MagnetVisible.isOn;
            }
            
            // Try to the new magnet strenght from the Input 
            if ((float.TryParse(MagnetStrengthPercentInputField.text, 
                 out float percentage) == true) &&
                percentage >= -100.0f &&
                percentage <= 100.0f)  {
                magnetAttributes.MagnetismStrength = (percentage / 10.0f);
            }
            else
            {
                // This will automatically set the magnet strenght of every magnet 
                // to 100 percent if a proper value have not been set.
                magnetAttributes.MagnetismStrength = 100.0f;
            }
        }
    }
    /// <summary>
    ///  Used to handle all the attribute of the Particle Points. This includes 
    ///  updating the the linerender of the particle points to the origin lines
    ///  and setting the mass of the particle points
    /// </summary>
    private void CheckParticlePointAttributesUI ()
    {
        // Check to see it the radio button for AllowSrping have been updated from its
        // previous state.
        if( AllowSpringVisibility.isOn != m_AllowSpringVisibilityFlag)
        {
            m_AllowSpringVisibilityFlag = AllowSpringVisibility.isOn;
            
            // Set the visibilty
            DynamicPointToPointLineRender
                .EnableLinksBetweenParticleAndOrigin(PointHolder.transform, 
                                                     m_AllowSpringVisibilityFlag); 
        }
        
        // Update the spring end points if the spring are allowed to be viewed. 
        if (m_AllowSpringVisibilityFlag)
        {
            DynamicPointToPointLineRender
                .UpdateSpringPositions(PointHolder.transform);
        }
        
        // Give the user the ability to set the mass of the data points in the scatter plot. 
        if (float.TryParse(ParticleMassInputField.text, 
            out float particleMass) == true &&
            particleMass >= 0               &&
            particleMass <= 100)
        {
            // Only update the mass if it is different from all the previously selected mass 
            // values.
            if (PointRendererObject.SetParticleMass != particleMass)
            {
                if ((0 <= particleMass) && (particleMass <= 100))
                {
                    PointRendererObject
                        .UpdateParticlePointsMass(PointHolder.transform, 
                        particleMass);
                }
            }
        }
        else
        {
            // Set the text field to be nothing. Easy to clear this field.
            ParticleMassInputField.text = "";
        }
    }
    /// <summary>
    ///  Used to handle the view of the LineRender in the scene.
    /// </summary>
    private void CheckDynamicLineRenderingUI ()
    {
        // Check if the LineRender radio button is set to active.
        if ( (EnableLineRender.isOn == true))
        {
            // If true, continuously check and update the linerenderer points.
            // If the points move, the linerender will handle this fuctionality.
            string magnetName = 
                DynamicPointRenderingDropDown.options[DynamicPointRenderingDropDown.value].text;
            //
            DynamicPointToPointLineRender.SortPointsForLineRender(PointHolder.transform, 
                                                                  DynamicLineRenderer, 
                                                                  magnetName);
            // I think this line can be removed.
            m_DynamicPointRenderingMagnetName = magnetName;
            DynamicLineRenderer.enabled = true;
        }
        
        else if ((EnableLineRender.isOn == false))
        {
            // Remove the linerender from view of the scene.
            DynamicLineRenderer.enabled = false;
        }
    }
    /// <summary>
    /// Handles the color correlation portion of the graph. The color of the 
    /// points above and below the selected threshhold takes
    /// on the color of the magnet select to change color.
    /// </summary>
    private void CheckColorCorrelationUI()
    {
        ColorClassifierObject.Active = AllowColorClassifier.isOn;
        // Get the current name of the magnet in the dropdown.
        string magnetName = ColorCorrelationDropDown
                            .options[ColorCorrelationDropDown.value].text;
        // Get the attributes of the dropdown selcted in the dropdown.
        MagnetAttributes magnetAttributes = MagnetHolder.transform.Find(magnetName)
                                            .transform.GetComponent<MagnetAttributes>();
        // If the current magnet is not the same as the magnet
        if (ColorClassifierObject.PreviousMagnet != magnetName)
        {
            ColorCorrelationMinimumValueTextField.text = 
                magnetAttributes.MinValue.ToString();
            ColorCorrelationMaximumValueTextField.text = 
                magnetAttributes.MaxValue.ToString();
        }
        
        // check if the radio box have been set for this function.
        if (ColorClassifierObject.Active == true)
        {
            float minValue = magnetAttributes.MinValue;
            float maxvalue = magnetAttributes.MaxValue;
            
            // Try to parse a valid floating point value from the ColorCorrelation text
            // field and make sure it is in the range 0 and the magnets max value.
            // The reason we are checking the min valus of the text field is because this 
            // introduced a bug. If this code is ever used again, there is a better way of
            // writing this if statement. I didn't have enough time to really rewrite this 
            // in a better way.
            if ( ( float.TryParse(ColorCorrelationMidPointInputField.text, 
                   out float cutOffValue) == true) &&
                 ( 0 <= cutOffValue )              && 
                 ( cutOffValue <= maxvalue ) )
            {
                // Get  the 
                string cutOffOption = ColorCorrelationAboveOrBelowMidPointDropDown
                    .options[ColorCorrelationAboveOrBelowMidPointDropDown.value].text;

                // Check all the previously stored values with the currently collected
                // values. If there is any change between the GUI and the ColorClassifier
                // object, adjustment to the scene must be conducted.
                if (ColorClassifierObject.PreviousCutOffOption != cutOffOption ||
                     ColorClassifierObject.PreviousValue != cutOffValue ||
                     ColorClassifierObject.PreviousMagnet != magnetName)
                {
                    // Find the magnet currently in scene. You may have issues with this
                    // line if you have two magents with the same name.
                    Color magnetColor = MagnetHolder.transform.Find(magnetName)
                                       .transform.GetComponent<Renderer>()
                                       .material.color;

                    // Alters all the point in the plot based on all the collected data.
                    ColorClassifierObject
                        .AlterScatterPlotColorsBasedOnInput(PointHolder.transform, 
                                                            magnetColor, 
                                                            magnetName, 
                                                            cutOffOption, 
                                                            cutOffValue);
                }
            }
            else
            {
                // Set the color Correlation text field to nothing. There isn't a
                // clear option for text for the input field.
                ColorCorrelationMidPointInputField.text = "";
            }
        }
    }
    /// <summary>
    /// Automatically Fills all the dropdown menus in the scene.
    /// </summary>
    private void FillDropDowns ()
    {
        // Clear all the data from the dropdowns. This need to be done for start up
        // and for if the plot data has been changed. We don't want any uselss data 
        // stored in the dropdowns because it can really break things.
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
        // These opions are for the color correlation function calls.
        options.Add("Above");
        options.Add("Below");
        ColorCorrelationAboveOrBelowMidPointDropDown.AddOptions(options);
    }
}
