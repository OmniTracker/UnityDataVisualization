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

using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.UI;

public class Raycaster : MonoBehaviour
{
    public SteamVR_Input_Sources LeftInputSource = SteamVR_Input_Sources.LeftHand;
    public SteamVR_Input_Sources RightInputSource = SteamVR_Input_Sources.RightHand;
    private Dropdown m_dropdown = null;
    // The prefab for the data Magnet that will be instantiated
    public GameObject MagnetHolder;
    public GameObject PointHolder;
    // Public Dropdown menus
    public Dropdown XCoordinateDropDown;
    public Dropdown YCoordinateDropDown;
    public Dropdown ZCoordinateDropDown;
    public Dropdown MagnetNameDropDown;
    private int m_currentMagnetNameDropDownIndex = -1;
    // Public
    public Text MagnetStrengthTextValue;
    public Text SpringStrengthTextValue;
    // Public
    public Text MagnetDataMenu;
    public Text PointDataMenu;
    // Public Toggle Objects
    public Toggle MagnetActiveToggle;
    public Toggle AllowMagnetismToggle;
    public Toggle AllowSpringToggle;
    // Private Magnetism Value
    private int m_MinimumMagnetValue = -100;
    private int m_MaximumMagnetValue = 100;
    // Private Spring Strength
    private int m_MinimumSpringValue = 0;
    private int m_MaximumSpringValue = 50;
    // Used to make sure that all the child Magnets are properly counted
    private int m_ChildMagnetCount = 0;
    private string m_FileName;
    private List<string> m_magnetNameList;
    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        InvokeRepeating("UpdateRaycaster", 2.0f, 0.1f);
        // Set the text for the Magnetic
        MagnetStrengthTextValue.text = "0";
        SpringStrengthTextValue.text = "25";
        m_magnetNameList = new List<string>();
    }
    private void UpdateRaycaster()
    {
        RaycastHit raycastHit;
        GameObject gameObject = null;

        if( HasDataFileBeenUpdated() )
        {
            // Clear current Magnet Name List is are currently names stored
            if (m_magnetNameList.Count > 0)
                m_magnetNameList.Clear();
            // Fill the magnetList
            foreach (Transform childMagnet in MagnetHolder.transform)
                m_magnetNameList.Add(childMagnet.name);
            // Update Dropdowns
            UpdateDropDownList();
            // Set the current plot positions for the plot 
            SetXYZDropdownsToCurrentPlotAxis();
        }

        if (m_currentMagnetNameDropDownIndex != MagnetNameDropDown.value)
        {
            MagnetAttributes attributes = GameObject.Find(MagnetNameDropDown.options[MagnetNameDropDown.value].text).GetComponent<MagnetAttributes>();
            MagnetActiveToggle.isOn = attributes.MagnetActive;
            AllowMagnetismToggle.isOn = attributes.AllowMagnetism;
            MagnetStrengthTextValue.text = attributes.MagnetismStrength.ToString();
            m_currentMagnetNameDropDownIndex = MagnetNameDropDown.value;
        }

        if (SteamVR_Actions._default.Squeeze.GetAxis(LeftInputSource) == 1 || 
            SteamVR_Actions._default.Squeeze.GetAxis(RightInputSource) == 1)
        {
            if (Physics.Raycast(transform.position, transform.forward, out raycastHit))
            {
                gameObject = raycastHit.collider.gameObject;
                if (gameObject.tag.Contains("RaycasterUI"))
                {
                    InteractWithGUI(gameObject);
                }
                else if (gameObject.tag.Contains("Magnet") || gameObject.tag.Contains("DataPoint"))
                {
                    InteractWithParticleAndMagnet(gameObject);
                    if (gameObject.tag.Contains("DataPoint"))
                    {
                        // Print all the data to the screen.
                        InteractWithPointData(gameObject);
                    }
                }
            }  
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameObject"></param>
    private void InteractWithGUI(GameObject gameObject)
    {
        // If the name of the gameobject contains "Dropdown", this will shift
        if (gameObject.name.Contains("Dropdown"))
        {
            m_dropdown = GameObject.Find(gameObject.name).GetComponent<Dropdown>();
            if (m_dropdown.options.Count > m_dropdown.value + 1)
            {
                m_dropdown.value = m_dropdown.value + 1;
            }
            else
            {
                m_dropdown.value = 0;
            }
        }
        else if (gameObject.name.Contains("LoadPlotData"))
        {
            GameObject.Find("Plotter").GetComponent<PointRenderer>().LoadNewPlotAxis();
        }
        else if (gameObject.name.Contains("Previous"))
        {
            Debug.Log("Previous Pressed");
        }
        else if (gameObject.name.Contains("LevelReset"))
        {
            Debug.Log("Pressed Level Reset");
        }
        else if (gameObject.name.Contains("IncreaseMagnetStrength"))
        {
            if (Int32.Parse(MagnetStrengthTextValue.text) < m_MaximumMagnetValue)
            {
                MagnetAttributes attributes = GameObject.Find(MagnetNameDropDown.options[MagnetNameDropDown.value].text).GetComponent<MagnetAttributes>();
                MagnetStrengthTextValue.text = (Int32.Parse(MagnetStrengthTextValue.text) + 10).ToString();
                attributes.MagnetismStrength = Int32.Parse(MagnetStrengthTextValue.text) / 100;
            }
        }
        else if (gameObject.name.Contains("DecreaseMagnetStrength"))
        {
            if (Int32.Parse(MagnetStrengthTextValue.text) > m_MinimumMagnetValue)
            {
                MagnetAttributes attributes = GameObject.Find(MagnetNameDropDown.options[MagnetNameDropDown.value].text).GetComponent<MagnetAttributes>();
                MagnetStrengthTextValue.text = (Int32.Parse(MagnetStrengthTextValue.text) - 10).ToString();
                attributes.MagnetismStrength = Int32.Parse(MagnetStrengthTextValue.text) / 100;
            }
        }
        else if (gameObject.name.Contains("AllowMagnetismToggle"))
        {
            if (gameObject.GetComponent<Toggle>().isOn == true)
            {
                gameObject.GetComponent<Toggle>().isOn = false;
            }
            else
            {
                gameObject.GetComponent<Toggle>().isOn = true;
            }
            MagnetAttributes attributes = GameObject.Find(MagnetNameDropDown.options[MagnetNameDropDown.value].text).GetComponent<MagnetAttributes>();
            attributes.AllowMagnetism = gameObject.GetComponent<Toggle>().isOn;
        }
        else if (gameObject.name.Contains("MagnetActiveToggle"))
        {
            if (gameObject.GetComponent<Toggle>().isOn == true)
            {
                gameObject.GetComponent<Toggle>().isOn = false;
            }
            else
            {
                gameObject.GetComponent<Toggle>().isOn = true;
            }
            MagnetAttributes attributes = GameObject.Find(MagnetNameDropDown.options[MagnetNameDropDown.value].text).GetComponent<MagnetAttributes>();
            attributes.MagnetActive = gameObject.GetComponent<Toggle>().isOn;
        }
        else if (gameObject.name.Contains("AllowSpringToggle"))
        {
            if (gameObject.GetComponent<Toggle>().isOn == true)
            {
                gameObject.GetComponent<Toggle>().isOn = false;
            }
            else
            {
                gameObject.GetComponent<Toggle>().isOn = true;
            }
        }
        else if (gameObject.name.Contains("LoadData"))
        {
            Debug.Log("Test");

            FileManager.OpenExplore();
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameObject"></param>
    private void InteractWithParticleAndMagnet(GameObject gameObject)
    {
        gameObject.transform.position = transform.position;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameObject"></param>
    private void InteractWithPointData(GameObject gameObject)
    {
        if (MagnetHolder.transform.childCount != 0 && PointHolder.transform.childCount != 0)
        {
            string magnetDataMenuString = "";
            string pointDataMenuString = "";
            MagnetAttributes magnetAttribute;
            pointDataMenuString = "Point Name: " + gameObject.name;
            // Iterate through Point Data Dictionary
            foreach (var point in gameObject.GetComponent<ParticleAttributes>().PointData)
            {
                pointDataMenuString += "\nVariable : " + point.Key + " Value: " + point.Value;
                magnetAttribute = MagnetHolder.transform.Find(point.Key).GetComponent<MagnetAttributes>();
                magnetDataMenuString += "Magnet : " + point.Key +
                                        "  Strength: " + ((point.Value - magnetAttribute.MinValue) /
                                                        (magnetAttribute.MaxValue - magnetAttribute.MinValue)) +
                                        "\nMin: " + magnetAttribute.MinValue +
                                        " Max: " + magnetAttribute.MaxValue + "\n";

            }
            // Fill menus
            MagnetDataMenu.text = magnetDataMenuString;
            PointDataMenu.text = pointDataMenuString;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void SetXYZDropdownsToCurrentPlotAxis()
    {
        int index = -1;
        if (((index = GetIndexByName(XCoordinateDropDown,
                                     GameObject.Find("X_Title").GetComponent<TextMesh>().text.ToString())) != -1))
        { XCoordinateDropDown.value = index; }
        if (((index = GetIndexByName(YCoordinateDropDown,
                                     GameObject.Find("Y_Title").GetComponent<TextMesh>().text.ToString())) != -1))
        { YCoordinateDropDown.value = index; }
        if (((index = GetIndexByName(ZCoordinateDropDown,
                                     GameObject.Find("Z_Title").GetComponent<TextMesh>().text.ToString())) != -1))
        { ZCoordinateDropDown.value = index; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dropDown"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    private int GetIndexByName(Dropdown dropDown, string name)
    {
        if (dropDown == null) { return -1; } // or exception
        if (string.IsNullOrEmpty(name) == true) { return -1; }
        List<Dropdown.OptionData> list = dropDown.options;
        for (int i = 0; i < list.Count; i++)
            if (list[i].text.Equals(name)) { return i; }
        return -1;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool HasDataFileBeenUpdated()
    {
        if (MagnetHolder == null)
            return false;
        if (PointHolder == null)
            return false;
        // Check if the Magnet count has changed. This is an easy indicator that something has changed
        if (m_ChildMagnetCount != MagnetHolder.transform.childCount)
        {
            m_ChildMagnetCount = MagnetHolder.transform.childCount;
            return true;
        }
        // Check file name has changed
        if (m_FileName != "")
        {
            // return true;
        }
        return false;
    }
    /// <summary>
    /// 
    /// </summary>
    private void UpdateDropDownList()
    {
        if (m_magnetNameList.Count > 0)
        {
            // Clear Options
            XCoordinateDropDown.ClearOptions();
            YCoordinateDropDown.ClearOptions();
            ZCoordinateDropDown.ClearOptions();
            MagnetNameDropDown.ClearOptions();
            // Add Options
            XCoordinateDropDown.AddOptions(m_magnetNameList);
            YCoordinateDropDown.AddOptions(m_magnetNameList);
            ZCoordinateDropDown.AddOptions(m_magnetNameList);
            MagnetNameDropDown.AddOptions(m_magnetNameList);
        }
    }
}
