using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Diagnostics;

public class GetCoords : MonoBehaviour
{
	private GameObject obj;
	private string coords = "";
	Stopwatch sw = new Stopwatch();
	private string full_path;
	
	private void Awake()
	{
		//create file for output
		DateTime now = DateTime.Now;
		string strDate = now.ToString("O");
		string filename = "example_test" + ".txt";
		//create file address -- documents folder for now
		string path = "C:/Users/HFVRl/Documents/Eye Data/";
		full_path = path + filename;
		File.Create(full_path).Dispose();
		string title = "Eye Tracking Data for Subject: " + strDate + "\r\n";
		File.WriteAllText(full_path, title);
		//start stopwatch
		sw.Start();
		obj = GameObject.Find("tracking_sphere");
	}
	
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coords = sw.ElapsedMilliseconds + ", " + obj.transform.position.x.ToString() + ", " + obj.transform.position.y.ToString() + ", " + obj.transform.position.z.ToString() + "\r\n";
		UnityEngine.Debug.Log(coords);
		File.AppendAllText(full_path, coords);
    }
}
