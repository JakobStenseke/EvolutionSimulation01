	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	public class CellScript : MonoBehaviour {
	
		// STATS
		public string ID;
		public int GenerationID;
		public int Strength;
		public int Health;
		public int Rage;
		public int Sexuality;
		public int Perception;
		public int Age;
		public int AgeOfDeath;
		public int Gender;

		// BORDERS
		float maxX = 20f;
		float minX = -20f;
		float maxY = 20f;
		float minY = -20f;

		int HealthTimer;
		
		// DIRECTIONS
		private float RandomX;
		private float RandomY;

		// MISC
		public Collider2D[] colliders;
		private float radius;
		public float moveSpeed;
		private Vector2 movement;

		public Collider2D SelectedNeighbor;
		public int SelectedNeighborNumber;
		public bool MoveTowards;

		public GameObject CellBaby;
		public GameObject StrengthColor;
		public GameObject SexRageColor;


		void Start () {

			if (GenerationID == 0) {
			
			GenerationID = 1;

			//name the cell
			string st = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
			char c1 = st[Random.Range(0, st.Length)];
			ID = c1.ToString();

			}

			Age = 0;
			AgeOfDeath = Random.Range(60, 100);
			MoveTowards = false;
			Collider2D MotherColl = GetComponent<Collider2D>();
			MotherColl.enabled = false;

			//Immune the first second
			Invoke ("NoImmunity", 1f);

		
			gameObject.name = ID.ToString ();
			
			//initial values randomly
			if (moveSpeed == 0) {
				moveSpeed = Random.Range (2f, 15f);
			}
			if (Health == 0) {
				Health = Random.Range (1, 11);
			}
			if (Strength == 0) {
				Strength = Random.Range (1, 11);
			}
			if (Rage == 0) {
				Rage = Random.Range (0, 11);
			}
			if (Sexuality == 0) {
				Sexuality = Random.Range (0, 11);
			}
			if (Perception == 0) {
			Perception = Random.Range (0, 11);
			}

			if (Random.value > 0.5f) {
				Gender = 0;
			} else {
				Gender = 1;
			}

			transform.localScale = new Vector3(Health, Health, 0);

			InvokeRepeating ("Move", 0, 0.2f);
			InvokeRepeating ("Aging", 0.4f, 0.4f);
			
		}

		void Update () {

		if (MoveTowards == false) { 
			transform.Translate (RandomX * moveSpeed * Time.deltaTime, RandomY * moveSpeed * Time.deltaTime, 0);
		} 
		else {

			if (Sexuality + Rage / 2 > 10) {

				if (SelectedNeighbor == null) {
					float step = moveSpeed * Time.deltaTime;
					transform.position = Vector3.MoveTowards (transform.position, SelectedNeighbor.transform.position, step);
				}
			}
			else {
				if (SelectedNeighbor == null) {
					float step = moveSpeed * Time.deltaTime;
					transform.position = Vector3.MoveTowards (transform.position, transform.position - SelectedNeighbor.transform.position, step);
				}
			}
		}

			// make sure the position is inside the borders

			if (transform.position.x >= maxX){
				RandomX = -1f;
			}
			if (transform.position.x <= minX) {
				RandomX = 1f;
			}
			if (transform.position.y >= maxY){
				RandomY = -1f;
			}
			if (transform.position.y <= minY) {
				RandomY = 1f;
			}


		StrengthColor.gameObject.GetComponent<Renderer>().material.color = new Color (1f,0f,0f, Strength / 10f);
		SexRageColor.gameObject.GetComponent<Renderer>().material.color = new Color (0f,0f, Rage / 10f, Sexuality / 10f);

		}


		void Move (){

		int Random1 = Random.Range(0, Perception);

		//If the perception is developed, the cell will more likely move towards other cells
		if (Random1 >= 5) {

			MoveTowards = true;
			radius = Perception / 3f;
			colliders = Physics2D.OverlapCircleAll (transform.position, radius); // creates an array of all the overlapps

			if (colliders.Length > 1) {

				SelectedNeighbor = colliders [1];
				if (SelectedNeighbor == gameObject || SelectedNeighbor == null ) {
					SelectedNeighborNumber = Random.Range (0, colliders.Length);
					SelectedNeighbor = colliders [SelectedNeighborNumber];
				}

			
			} 
			else 
			{
				MoveTowards = false;
				RandomX = Random.Range(-1f,1f);
				RandomY = Random.Range(-1f,1f);
			}
		} 
		else
		{
			MoveTowards = false;
		RandomX = Random.Range(-1f,1f);
		RandomY = Random.Range(-1f,1f);

		}
	}

		void Aging (){

		Age += 1;

		if (Age > AgeOfDeath) {
			Debug.Log (ID + " died of age, " + Age + " years old");
				Destroy (gameObject);
			}

				HealthTimer += 1;
				if (HealthTimer > 10){
					Health -= 1;
					transform.localScale = new Vector3 (Health, Health, 0);
					HealthTimer = 0;
				}

				if (Health == 0){
					Debug.Log (ID + " starved to death, " + Age + " years old");
			Destroy (gameObject);
		}
	}

			void OnTriggerEnter2D (Collider2D other)
			{

				//Invoke ("FightEnable", 0.1f);
				CellScript otherCell = other.gameObject.GetComponent<CellScript> ();

			//	if ((Gender == 0 && Gender != otherCell.Gender) && (Random.value > 0.1f)) {
			if (Sexuality > Rage) {

						//Fighting = false;
						Collider2D MotherColl = GetComponent<Collider2D>();
						MotherColl.enabled = false;

						GameObject man = GameObject.Find ("Manager");
						ManagerScript manScript = man.GetComponent<ManagerScript> ();
						
						//spawn a baby with values inherited in a random range between the values of mother and father
						manScript.BirthPosition = transform.position + new Vector3(-RandomX * 2, -RandomY * 2, 0.0f);
						manScript.ChildHealth = Random.Range(otherCell.Health, Health);
						manScript.ChildRage = Random.Range(otherCell.Health, Health);
						manScript.ChildStrength = Random.Range (otherCell.Strength, Strength);
						manScript.ChildSexuality = Random.Range (otherCell.Sexuality, Sexuality);
						manScript.ChildSpeed = Random.Range (otherCell.moveSpeed, moveSpeed);
						manScript.ChildPerception = Random.Range (otherCell.Perception, Perception);
						manScript.ChildID = ID + otherCell.ID;
						manScript.ChildGenID = GenerationID += 1;
						manScript.SpawnChild ();
						Debug.Log (ID + " gave birth to " + otherCell.ID + "'s baby: " + manScript.ChildID + " generation " + manScript.ChildGenID);

						// transform the mother so it won't get eaten by the kid or the other way around
						Vector3 afterbirthPos = transform.position + new Vector3(RandomX * 2, RandomY * 2, 0.0f);
						transform.position = afterbirthPos;
						Sexuality -= 1;
						Rage += 1;
						Invoke ("NoImmunity", 0.5f);
				
				}
			else {
			
				if (Strength > otherCell.Strength) {
				Health += otherCell.Health / 2;
				//Strength += 1;
				Debug.Log (ID + " killed " + otherCell.ID);
				if (Health < 100) {
					transform.localScale = new Vector3 (Health, Health, 0);
				} else {
					Health = 100;
					transform.localScale = new Vector3 (Health, Health, 0);
				}
				Destroy (other.gameObject);
				Rage -= 1;
				Sexuality += 1;
				}
			}
		}

		void NoImmunity(){

			Collider2D MotherColl = GetComponent<Collider2D>();
			MotherColl.enabled = true;

		}

		//Method for future use
		void FightEnable(){

			//Collider2D MotherColl = GetComponent<Collider2D>();
			//MotherColl.enabled = true;
			//	Fighting = true;
		}
	}
