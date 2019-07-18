/*----------------------------------------------------------------------------
 * 3 Dimensional Multivariate Data Visualization
 *----------------------------------------------------------------------------
 *      Class:          LoadMainScene
 *      Description:    Loads main scene of application.
 *----------------------------------------------------------------------------
 *      Author:         Ronald H. Baker (Brown University Masters Student)                     
 *      Date:           7/11/2019
 *      Notes:           
 *                      
 *      Revision History:
 *      
 *      (7/11/2019) - Software Refactor - File was just commented for refactor
 *----------------------------------------------------------------------------
 * This program is free software: you can redistribute it and/or modify it 
 * under the terms of the GNU General Public License as published by the Free 
 * Software Foundation, either version 3 of the License, or (at your option) any 
 * later version.
 * 
 * This program is distributed in the hope that it will be useful, but WITHOUT 
 * ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS 
 * FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.
 */
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoadMainScene : MonoBehaviour
{
    int m_MainSceneIndex = 0;
    /// <summary>
    /// Loads Main Scene. 
    /// </summary>
    public void LoadScene()
    {
        SceneManager.LoadScene(m_MainSceneIndex);
    }
}
