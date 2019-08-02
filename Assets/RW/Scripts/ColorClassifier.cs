/*-----------------------------------------------------------------------------                    
 * 3 Dimensional Multivariate Data Visualization
 *-----------------------------------------------------------------------------
 *      Class:          ColorClassifier
 *      Description:         
 *-----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                  
 *      Date:           7/11/2019
 *      Notes:           
 *                      
 *      Revision History:
 *      
 *      (7/11/2019) - Software Refactor - This file was generated to handle the
 *                    color of the magnets and also the color correlation for 
 *                    each of the data points if the color classifier is 
 *                    enabled
 *      (7/21/2019) - Adding color alteration based off specified values.  
 *      (8/02/2019) - Finished comments for file
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
using UnityEngine;

public class ColorClassifier : MonoBehaviour
{
    public bool Active = false;
    private string m_PreviousMagnet = "";
    private float m_PreviousValue = float.NaN;
    private string m_PreviousCutOffOption = "";
    public string PreviousMagnet { get => m_PreviousMagnet;
                                   set => m_PreviousMagnet = value; }
    public float PreviousValue { get => m_PreviousValue;
                                 set => m_PreviousValue = value; }
    public string PreviousCutOffOption { get => m_PreviousCutOffOption;
                                         set => m_PreviousCutOffOption = value;
                                       }

    /// <summary>
    /// Used to generate a random color based of the min and max values.
    /// </summary>
    /// <param name="minimum"> Minimum range value to produce a color</param>
    /// <param name="maximum"> Maxium range value to produce a color.</param>
    /// <returns></returns>
    public static Color NextRandomColor (float minimum, 
                                         float maximum, 
                                         float alpha)
    {
        float minFloatColorValue = 0.0f;
        float maxFloatColorValue = 1.0f;
        // If the color values passed in are out of range, return a color value 
        // equal to white.
        if ( ( minFloatColorValue < minimum && minimum < maxFloatColorValue) ||
             (minFloatColorValue < maximum && maximum < maxFloatColorValue))
        {
            return new Color(0,0,0,1);
        }
        // If the minimum is higher than the max value, return a color of 
        // black. This decision was more so visually debugging purposes. 
        if (minimum > maximum)
        {
            return new Color(1,1,1,1);
        }
        // Set the R,G,B,A
        float r = Random.Range(minimum, maximum);
        float g = Random.Range(minimum, maximum);
        float b = Random.Range(minimum, maximum);
        return new Color(r,g,b, alpha); 
    }
    /// <summary>
    /// Used to select the points that should be altered based off the values 
    /// passed in.This function uses the attributes of the selected magnet to 
    /// change the color of points to either the color of the derived from the
    /// magnet or white, which means the point has not met the requirements for
    /// for color change. 
    /// </summary>
    /// <param name="pointHolderTransform">Parent point holder transform. Used to
    /// access the children data points.
    /// </param>
    /// <param name="targetColor">The target color of the point. For the purpose
    /// of this application, it is assumed that the color passed in correlates
    /// to a specific magnet.
    /// </param>
    /// <param name="magnetName">The name of the magnet in which the color 
    /// has be derived from.
    /// </param>
    /// <param name="direction">The direction which the user wants the points 
    /// in the graph to be altered. The two choice are either above or below the
    /// input cutoff threshold
    /// </param>
    /// <param name="cutOff">The cutoff point for color change. This value 
    /// should be between the minimum and maximum value of the associated 
    /// magnet value
    /// </param>
    public void AlterScatterPlotColorsBasedOnInput(Transform pointHolderTransform, 
                                                          Color targetColor, 
                                                          string magnetName, 
                                                          string direction,
                                                          float cutOff)
    {
        bool alterationsHadSomeEffect = false;
        foreach (Transform childDataPoint in pointHolderTransform)
        {
            // Get value based on magnet name passed in.
            float value = childDataPoint.GetComponent<ParticleAttributes>()
                .KeyValue(magnetName);

            if (direction == "Above")
            {
                if (value >= cutOff)
                {
                    childDataPoint.GetComponent<Renderer>().material.color 
                    = targetColor;
                    alterationsHadSomeEffect = true;
                }
                else
                {
                    childDataPoint.GetComponent<Renderer>().material.color 
                    = Color.white;
                }
            }
            else if (direction == "Below")
            {
                if (value <= cutOff)
                {
                    childDataPoint.GetComponent<Renderer>().material.color 
                    = targetColor;
                    alterationsHadSomeEffect = true;
                }
                else
                {
                    childDataPoint.GetComponent<Renderer>().material.color 
                    = Color.white;
                }
            }
        }
        // If any of the particles color changed, store the values for later. 
        // There needs to be a way of knowing when an alteration has occurred 
        // so we don't continuous make alterations the scene.
        if (alterationsHadSomeEffect == true)
        {
            PreviousMagnet = magnetName;
            PreviousValue = cutOff;
            PreviousCutOffOption = direction;
        }
    }
}
