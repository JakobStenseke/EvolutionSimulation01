using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour {

	public GameObject CellBaby;
	public Button GenerateButton;
	public int Year;
	public Text YearText;
	public Text PopulationText;

	public int spawnAmount = 10; // Can be used to change the number of agents spawned in Generate()
	public int NumberOfCells;

	public int ChildHealth;
	public int ChildRage;
	public int ChildStrength;
	public int ChildSexuality;
	public int ChildPerception;
	public float ChildSpeed;
	public string ChildID;
	public int ChildGenID;

	public Vector3 BirthPosition;

	// Use this for initialization
	void Start () {

		Button btn = GenerateButton.GetComponent<Button> ();
		btn.onClick.AddListener (Generate);

		InvokeRepeating ("NewYear", 0.4f, 0.4f);

		Time.timeScale = 0;

	}
	
	// Update is called once per frame
	//Remake this into a for loop -- if int < 10, spawn at position
	void Generate () {

		Time.timeScale = 0.5f;

		for (int i = 0; i < spawnAmount; i++) 
		{
			Vector3 spawnPosition = new Vector3 (Random.Range (-20, 20), Random.Range (-20, 20), 0);
			Instantiate (CellBaby, spawnPosition, Quaternion.identity);
		}

	}

	public void SpawnChild (){

		GameObject child = (GameObject)Instantiate(CellBaby, BirthPosition, transform.rotation);
		child.GetComponent<CellScript> ().Health = ChildHealth;
		child.GetComponent<CellScript> ().Rage = ChildRage;
		child.GetComponent<CellScript> ().Strength = ChildStrength;
		child.GetComponent<CellScript> ().moveSpeed = ChildSpeed;
		child.GetComponent<CellScript> ().Perception = ChildPerception;
		child.GetComponent<CellScript> ().ID = ChildID;
		child.GetComponent<CellScript> ().GenerationID = ChildGenID;
	}

	void NewYear (){
		Year += 1;

		YearText.text = Year.ToString ();
		NumberOfCells = GameObject.FindGameObjectsWithTag ("Cell").Length;

		PopulationText.text = NumberOfCells.ToString ();
	}

}
