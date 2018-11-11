using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaterCustomization : MonoBehaviour
{
	public Button maleButton, femaleButton, finalizeButton;
	public Transform customizePoint;
	public Transform spawnPoint;

	private bool gender;

	public bool Gender
	{
		get
		{
			return gender;
		}
		set
		{
			gender = value;
			GenarateCharater();
		}
	}

	private bool finalize = false;

	public GameObject[] MaleCharaterModles;
	private GameObject activeMaleCharaterModlesObject;
	public GameObject[] FemaleCharaterModles;
	private GameObject activeCharaterModelObject;
	private int selectedIndex = 0;




	public CharaterAttack currentCustomizableCharacter;

	// Use this for initialization
	void Start()
	{
		finalize = false;
		
		//activeCharaterModelObject = FemaleCharaterModles[FemaleCharaterModles.Length];
		//activeMaleCharaterModlesObject = MaleCharaterModles[FemaleCharaterModles.Length];
	}

	// Update is called once per frame
	void Update()
	{

	}

    /// <summary>
    /// Finds the active charater and sets it position to a set location
    /// </summary>
	public void MoveAvtiveCharaterModle()
	{
		finalize = true;
		currentCustomizableCharacter.ableToAttack = true;

		if (finalize == true)
		{
			if (gender == true)
			{
				//activeMaleCharaterModlesObject = GameObject.FindGameObjectWithTag("Male");
				currentCustomizableCharacter.transform.position = spawnPoint.transform.position;
				currentCustomizableCharacter.transform.rotation = spawnPoint.transform.rotation;
			}

			if (gender == false)
			{
				//activeFemaleCharaterModlesObject = GameObject.FindGameObjectWithTag("Female");
				currentCustomizableCharacter.transform.position = spawnPoint.transform.position;
				currentCustomizableCharacter.transform.rotation = spawnPoint.transform.rotation;
			}
			//activeMaleCharaterModlesObject.transform.position = spawnPoint.transform.position;
			//activeFemaleCharaterModlesObject.transform.position = spawnPoint.transform.position;
		}
		else if (finalize == false)
		{
			//activeMaleCharaterModlesObject.transform.position = customizePoint.transform.position;
			//activeFemaleCharaterModlesObject.transform.position = customizePoint.transform.position;
		}
	}

    /// <summary>
    /// Sets the current weapon for the charter
    /// </summary>
    /// <param name="weapon">Prefab Weapon</param>
	public void AssignWeaponToCharacter(Weapon weapon)
	{
		if (currentCustomizableCharacter != null)
		{
			currentCustomizableCharacter.weapon = weapon;
			currentCustomizableCharacter.EquippedBoolRepeat();
		}
	}

    /// <summary>
    /// Sets the current Magic for the charter
    /// </summary>
    /// <param name="magic">Prefab partica System</param>
    public void AssignMagicToCharacter(Magic magic)
	{
		if (currentCustomizableCharacter != null)
		{
			currentCustomizableCharacter.magicType = magic;
		}
	}

    /// <summary>
    /// This creates and destroys a prefab instanticated into the scene and cycles through a list to determin what is instanticted and destroyed
    /// </summary>
	public void GenarateCharater()
	{
		
		foreach (Transform obj in customizePoint)
		{
			Destroy(obj.gameObject);
		}

		if (gender == true)
		{
			selectedIndex %= MaleCharaterModles.Length;
			activeCharaterModelObject = Instantiate(MaleCharaterModles[selectedIndex], customizePoint.position, customizePoint.rotation, customizePoint);


		}
		else
		{
			selectedIndex %= FemaleCharaterModles.Length;
			activeCharaterModelObject = Instantiate(FemaleCharaterModles[selectedIndex], customizePoint.position, customizePoint.rotation, customizePoint);
		}


		CharaterAttack attack = activeCharaterModelObject.GetComponent<CharaterAttack>();
		if (attack != null)
		{
			currentCustomizableCharacter = attack;
		}

	}

    /// <summary>
    /// This is the Line up system used to cycle through the diffrent skins for the genders
    /// this is done by increasing and index and this index is tied to a list which has all the prefab gameobjects
    /// </summary>
	public void NextModelInList()
	{
		selectedIndex++;
		GenarateCharater();
	}
	public void PreviousModelInList()
	{
		selectedIndex--;
		if (selectedIndex < 0)
		{
			selectedIndex = (gender) ? MaleCharaterModles.Length - 1 : FemaleCharaterModles.Length - 1;
		}
		GenarateCharater();
	}

    public void DestroyActiveCharater()
    {
        Destroy(currentCustomizableCharacter.gameObject);
    }
}
