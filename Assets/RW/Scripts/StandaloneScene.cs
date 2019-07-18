/*-----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          MouseDrag
 *      Description:       
 *-----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                  
 *      Date:           7/11/2019
 *      Notes:          This script was derived from the following tutorial
 *                      https://www.youtube.com/watch?v=pK1CbnE2VsI
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
using UnityEngine.UI;
using UnityEngine.XR;

public class StandaloneScene : MonoBehaviour
{
    // The prefab for the data Magnet that will be instantiated
    public GameObject MagnetHolder;
    public GameObject PointHolder; 
    // Public Dropdown menus
    public Dropdown XCoordinateDropDown;
    public Dropdown YCoordinateDropDown;
    public Dropdown ZCoordinateDropDown;
    public Dropdown MagnetNameDropDown;
    // Public Slider Objects
    public Slider MagnetismStrengthSlider;
    public Slider SpringStrengthSlider;
    // Public Toggle Objects
    public Toggle MagnetActiveToggle;
    public Toggle AllowMagnetismToggle;
    // Private class members
    private string m_fileName;
    private List<string> m_magnetNameList;
    private int childMagnetCount = 0;
    private bool m_debug = true;
    private int m_currentMagnetDropdownPosition = -1;
    // Start is called before the first frame update
    private bool m_lastMagnetActiveToggleStatus;
    private bool m_lastAllowMagnetismToggleStatus;
    private float m_lastMagnetismStrengthSliderValue;
    private float m_lastSpringStrengthSliderValue = 0;

    void Start()
    {
        // Disable VR 
        XRSettings.enabled = false;
        // Create a new list
        m_magnetNameList = new List<string>();
    }
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (HasDataFileBeenUpdated())
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
            // Set Min and Max Value for Slider. I had to put it here because the Slider was
            MagnetismStrengthSlider.minValue = -1;
            MagnetismStrengthSlider.maxValue = 1;
            SpringStrengthSlider.minValue = 0;
            SpringStrengthSlider.maxValue = 50;
            SpringStrengthSlider.value = 25;
        }
        // Set Magnet Stuff
        if (MagnetNameDropDown != null) {
            if (m_currentMagnetDropdownPosition != MagnetNameDropDown.value) {

                // Find child Magnet
                foreach (Transform childMagnet in MagnetHolder.transform) {
                    // If able to find child object
                    if (childMagnet.name == MagnetNameDropDown.options[MagnetNameDropDown.value].text)
                    {
                        MagnetAttributes attributes = childMagnet.GetComponent<MagnetAttributes>();
                        m_currentMagnetDropdownPosition = MagnetNameDropDown.value;
                        // Set new Values
                        MagnetActiveToggle.isOn = attributes.MagnetActive;
                        AllowMagnetismToggle.isOn = attributes.AllowMagnetism;
                        MagnetismStrengthSlider.value = attributes.MagnetismStrength;
                        // Store Last Values Stored
                        m_lastMagnetActiveToggleStatus = MagnetActiveToggle.isOn;
                        m_lastAllowMagnetismToggleStatus = AllowMagnetismToggle.isOn;
                        m_lastMagnetismStrengthSliderValue = MagnetismStrengthSlider.value;
                    }
                } 
            } else if ((m_lastMagnetActiveToggleStatus != MagnetActiveToggle.isOn) ||
                        (m_lastAllowMagnetismToggleStatus != AllowMagnetismToggle.isOn) ||
                        (m_lastMagnetismStrengthSliderValue != MagnetismStrengthSlider.value)) {
                // Set New Values for 
                MagnetAttributes attributes = MagnetHolder.transform.
                    GetChild(m_currentMagnetDropdownPosition).
                    GetComponent<MagnetAttributes>();
                // Set new Values
                attributes.MagnetActive = MagnetActiveToggle.isOn;
                attributes.AllowMagnetism = AllowMagnetismToggle.isOn;
                attributes.MagnetismStrength = MagnetismStrengthSlider.value;
                // Store Last Values Stored
                m_lastMagnetActiveToggleStatus = MagnetActiveToggle.isOn;
                m_lastAllowMagnetismToggleStatus = AllowMagnetismToggle.isOn;
                m_lastMagnetismStrengthSliderValue = MagnetismStrengthSlider.value;
            }
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
        if (childMagnetCount != MagnetHolder.transform.childCount) {
            childMagnetCount = MagnetHolder.transform.childCount;
            return true;
        }
        // Check file name has changed
        if (m_fileName != "")
        {
            // return true;
        }
        return false;
    }
}
