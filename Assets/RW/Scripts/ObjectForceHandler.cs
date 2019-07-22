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

using System.Threading;
using UnityEngine;

public class ObjectForceHandler : MonoBehaviour
{
    Thread ChildThread = null;
    EventWaitHandle ChildThreadWait = new EventWaitHandle(true, EventResetMode.ManualReset);
    EventWaitHandle MainThreadWait = new EventWaitHandle(true, EventResetMode.ManualReset);

    private float springConstant = 2.0f;

    void ChildThreadLoop()
    {
        ChildThreadWait.Reset();
        ChildThreadWait.WaitOne();

        while (true)
        {
            ChildThreadWait.Reset();

            // Do Update

            // Debug.Log("testing threads");

            WaitHandle.SignalAndWait(MainThreadWait, ChildThreadWait);
        }
    }
    // Object which will contain instantiated prefabs in hiearchy
    public GameObject PointHolder;
    // Object which will contain instantiated prefabs in hiearchy
    public GameObject MagnetHolder;
    // Start is called before the first frame update
    void Start()
    {

        InvokeRepeating("UseMagnets", 2.0f, 0.1f);
        InvokeRepeating("SpringBackToOrigin", 2.0f, 0.1f);
    }

    void Awake()
    {
        //ChildThread = new Thread(ChildThreadLoop);
        //ChildThread.Start();
    }

    void Update()
    {
        //MainThreadWait.Reset();
        // WaitHandle.SignalAndWait(ChildThreadWait, MainThreadWait);
    }
    /// <summary>
    /// Uses the magnets.
    /// </summary>
    private void UseMagnets()
    {
        if ((MagnetHolder == null) || (MagnetHolder.transform.childCount == 0))
        {
            return;
        }
        foreach (Transform childMagnet in MagnetHolder.transform)
        {
            if (childMagnet.GetComponent<MagnetAttributes>().MagnetActive)
            {
                    childMagnet.GetComponent<MagnetAttributes>().LastPosition = childMagnet.transform.position;
                    UseMagneticForces();
            }
        }
    }
    /// <summary>
    /// Apply force to the Particle points based on whether or not the magnet is active. If the magnet is not active
    /// or doesn't have a positive or negative force on the Particle, you will not visual see any changes on the
    /// Particle itself.
    /// </summary>
    private void UseMagneticForces()
    {
        if ((MagnetHolder == null) || (MagnetHolder.transform.childCount == 0))
        {
            return;
        }
        if ((PointHolder == null) || (PointHolder.transform.childCount == 0))
        {
            return;
        }
        foreach (Transform childMagnet in MagnetHolder.transform)
        {
            if (childMagnet.GetComponent<MagnetAttributes>().MagnetActive)
            {
                foreach (Transform childDataPoint in PointHolder.transform)
                {
                    Vector3 direction = (childMagnet.GetComponent<MagnetAttributes>().CalculateDirection(childDataPoint.position)).normalized;
                    float dataPointValue = childDataPoint.GetComponent<ParticleAttributes>().KeyValue(childMagnet.name);
                    if (childMagnet.GetComponent<MagnetAttributes>().MagnetVisible)
                    {
                        childDataPoint.GetComponent<Rigidbody>().AddForce(direction *
                        childMagnet.GetComponent<MagnetAttributes>().CalculateAttractionForce(dataPointValue));
                    }
                }
            }
        }
    }
    /// <summary>
    /// Springs the back to origin.
    /// </summary>
    private void SpringBackToOrigin()
    {
        if ((PointHolder == null) || (PointHolder.transform.childCount == 0))
        {
            return;
        }
        Vector3 distanceAndDirection;
        foreach (Transform childDataPoint in PointHolder.transform)
        {
            if (childDataPoint.position != childDataPoint.GetComponent<ParticleAttributes>().OriginLocation)
            {
                // Calculate the magnitude and direction.
                distanceAndDirection = ( childDataPoint.GetComponent<ParticleAttributes>().OriginLocation - childDataPoint.position );
                childDataPoint.gameObject.GetComponent<Rigidbody>().AddForce(distanceAndDirection * springConstant);


            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="objectPosition1"></param>
    /// <param name="objectPosition2"></param>
    /// <returns></returns>
    private float DistanceBeteenGameObjects (Vector3 objectPosition1, Vector3 objectPosition2)
    {
        return Vector3.Distance(objectPosition1, objectPosition2);
    }
}
