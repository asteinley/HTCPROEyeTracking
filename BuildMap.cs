using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using System.Collections.Generic;

public class BuildMap : MonoBehaviour
{
	List<string> list = new List<string>();
	public GameObject prefab;
	private string line;
	
	private Gradient gradient;
    private GradientColorKey[] colorKey;
    private GradientAlphaKey[] alphaKey;
	
	
	private void Awake()
	{
		//create file address -- documents folder for now
		string path = "C:/Users/HFVRl/Documents/Eye Data/example_test.txt";
		//var fileContent = string.Empty;
		//var filePath = string.Empty;

		/* using (OpenFileDialog openFileDialog = new OpenFileDialog())
		{
			openFileDialog.InitialDirectory = "c:\\";
			openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
			openFileDialog.FilterIndex = 2;
			openFileDialog.RestoreDirectory = true;

			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				//Get the path of specified file
				filePath = openFileDialog.FileName;

				//Read the contents of the file into a stream
				var fileStream = openFileDialog.OpenFile();
	
				using (StreamReader reader = new StreamReader(fileStream))
				{
				fileContent = reader.ReadToEnd();
				}
			}
		} */
	}
	
	// Start is called before the first frame update
	void Start()
    {
        // Read the file and display it line by line.  
		System.IO.StreamReader file =
			new System.IO.StreamReader(@"C:/Users/HFVRl/Documents/Eye Data/example_test.txt");  
		
		while((line = file.ReadLine()) != null)  
		{
			list.Add(line);
		}
		
		string [] lines = list.ToArray();
		
		//string[] sp1 = lines[1].Split(new char[] {','});
		string[] sp = lines[lines.Length-1].Split(new char[] {','});

		float end = float.Parse(sp[0]);
		
		
		//create violet
		Color purple = new Color(0.294f, 0f, 0.50f, 1f);
		Color orange = new Color(1f, 0.55f, 0f, 1f);
		
		//create gradient!
		gradient = new Gradient();

        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[7];
        colorKey[0].color = purple;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.blue;
        colorKey[1].time = 0.16f;
		colorKey[2].color = Color.cyan;
        colorKey[2].time = 0.32f;
		colorKey[3].color = Color.green;
        colorKey[3].time = 0.48f;
		colorKey[4].color = Color.yellow;
        colorKey[4].time = 0.64f;
		colorKey[5].color = orange;
        colorKey[5].time = 0.80f;
		colorKey[6].color = Color.red;
        colorKey[6].time = 1.0f;

        // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 1.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
		
		
		for (int i = 0; i < lines.Length; i++)
		{
			if (i == 0){
				continue;
			}
			string[] split = lines[i].Split(new char[] {','});
			
			Vector3 position    = new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3]));
			Quaternion rotation = new Quaternion(1, 1, 1, 1);
			GameObject obj      = Instantiate(prefab, position, rotation) as GameObject;
			
			//Color col = new Color(float.Parse("0"), (float.Parse(split[0])/end), float.Parse("0"), float.Parse("1"));
		
			float rn = float.Parse(split[0]);
			var rend = obj.GetComponent<Renderer>();
			
			rend.material.SetColor("_Color", gradient.Evaluate(rn/end));
			
			/* if (float.Parse(split[0]) <= (end/5)){
				rend.material.SetColor("_Color", Color.blue);
			}else if (float.Parse(split[0]) <= (end/5)*2){
				rend.material.SetColor("_Color", Color.cyan);
			}else if (float.Parse(split[0]) <= (end/5)*3){
				rend.material.SetColor("_Color", Color.green);
			}else if (float.Parse(split[0]) <= (end/5)*4){
				rend.material.SetColor("_Color", Color.yellow);
			}else{
				rend.material.SetColor("_Color", Color.red);
			} */
			
		}
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}