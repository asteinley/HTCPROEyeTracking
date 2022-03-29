//========= Copyright 2018, HTC Corporation. All rights reserved. ===========
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

namespace ViveSR
{
    namespace anipal
    {
        namespace Eye
        {
            public class OutputEyeData_GazeRay : MonoBehaviour
            {
				
				Stopwatch sw = new Stopwatch();
				private string t2p;
				private string eye_look;
				private float gaze_x;
				private float gaze_y;
				private float gaze_z;
				private long time;
				private string path;
				private string strDate;
				private string full_path;
				private string filename;
				private string hit_pos;
				private Vector3 GazeDirectionCombined;
				private string test;
				
                public int LengthOfRay = 25;
                [SerializeField] private LineRenderer GazeRayRenderer;
                private static EyeData eyeData = new EyeData();
                private bool eye_callback_registered = false;
                
				private void Awake()
				{
					//create file for output
					DateTime now = DateTime.Now;
					string strDate = now.ToString("O");
					filename = "example_test" + ".txt";
					//create file address -- documents folder for now
					path = "C:/Users/HFVRl/Documents/Eye Data/";
					full_path = path + filename;
					File.Create(full_path).Dispose();
					string title = "Eye Tracking Data for Subject: " + strDate + "\r\n";
					File.WriteAllText(full_path, title);
					//start stopwatch
					sw.Start();
				}
				
				private void Start()
                {
                    if (!SRanipal_Eye_Framework.Instance.EnableEye)
                    {
                        enabled = false;
                        return;
                    }
                    Assert.IsNotNull(GazeRayRenderer);
                }

                private void Update()
                {
                    if (SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.WORKING &&
                        SRanipal_Eye_Framework.Status != SRanipal_Eye_Framework.FrameworkStatus.NOT_SUPPORT) return;

                    if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == true && eye_callback_registered == false)
                    {
                        SRanipal_Eye.WrapperRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
                        eye_callback_registered = true;
                    }
                    else if (SRanipal_Eye_Framework.Instance.EnableEyeDataCallback == false && eye_callback_registered == true)
                    {
                        SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
                        eye_callback_registered = false;
                    }

                    Vector3 GazeOriginCombinedLocal, GazeDirectionCombinedLocal;

                    if (eye_callback_registered)
                    {
                        if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else if (SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else if (SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal, eyeData)) { }
                        else return;
                    }
                    else
                    {
                        if (SRanipal_Eye.GetGazeRay(GazeIndex.COMBINE, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else if (SRanipal_Eye.GetGazeRay(GazeIndex.LEFT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else if (SRanipal_Eye.GetGazeRay(GazeIndex.RIGHT, out GazeOriginCombinedLocal, out GazeDirectionCombinedLocal)) { }
                        else return;
                    }

                    Vector3 GazeDirectionCombined = Camera.main.transform.TransformDirection(GazeDirectionCombinedLocal);
                    GazeRayRenderer.SetPosition(0, Camera.main.transform.position - Camera.main.transform.up * 0.05f);
                    GazeRayRenderer.SetPosition(1, Camera.main.transform.position + GazeDirectionCombined * LengthOfRay);
					
					//Update each frame to print eye data to text file
					eye_look = GazeDirectionCombined.ToString();
					gaze_x = GazeDirectionCombined.x;
					gaze_y = GazeDirectionCombined.y;
					gaze_z = GazeDirectionCombined.z;
					time = sw.ElapsedMilliseconds;
					
					t2p = (time + ", " + gaze_x + ", " + gaze_y + ", " + gaze_z); 
					t2p += (", ");
					Shoot();
					t2p += hit_pos;
					t2p += "\r\n";
					File.AppendAllText(full_path, t2p);
				}
                private void Release() {
                    if (eye_callback_registered == true)
                    {
                        SRanipal_Eye.WrapperUnRegisterEyeDataCallback(Marshal.GetFunctionPointerForDelegate((SRanipal_Eye.CallbackBasic)EyeCallback));
                        eye_callback_registered = false;
                    }
                }
                private static void EyeCallback(ref EyeData eye_data)
                {
                    eyeData = eye_data;
                }
				
				void Shoot()
				{
					RaycastHit hit;
					if (Physics.Raycast(Camera.main.transform.position, GazeDirectionCombined, out hit))
					{
						hit_pos = (hit.point).ToString("F3");
					}
				}
				
            }
        }
    }
}
