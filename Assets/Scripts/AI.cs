using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AI : MonoBehaviour
{
	private float fpsTargetDistance;
	public float enemyLookDistance;
	public float attackDistance;
	private Transform enemyTarget;
	private Rigidbody rbBody;
	Rigidbody theRigidedbody;
	private float deathdelay;
	private NavMeshAgent agent;
	public float damping;
	private Animator anim;
	public float doAttackDistance = 1;
	public string[] singleAttack;
	public float timeBetweenAttacks;
	private bool Dead = false;
    public Image healthBar;

    // Use this for initialization
    void Start()
	{
        maxhealth = health;

		enemyTarget = FindObjectOfType<player>().transform;
		rbBody = GetComponent<Rigidbody>();
		agent = GetComponent<NavMeshAgent>();
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
	void FixedUpdate()
	{
        //The Health for our charater is converted into a value between 0 and 1 so that it is bale to control the fill amount for the Sprite image used to display health
        healthBar.fillAmount = health / maxhealth;

        //find the Location within thw world space and sets that as a Vector for the caluculation of the distance from the Target
        fpsTargetDistance = Vector3.Distance(enemyTarget.position, transform.position);

        //Testes that are checked to determine if the player and the enemy meet any requirements to perform diffrent actions.
		if (fpsTargetDistance < enemyLookDistance && Dead == false)
		{
			lookAtPlayer();
			GetComponent<WanderingAI>().StopAgent();
			rbBody.isKinematic = true;
		}
		else
		{
			GetComponent<WanderingAI>().ResumeAgent();
			rbBody.isKinematic = false;
		}
		if (fpsTargetDistance < attackDistance && Dead == false)
		{
			attackPlease();
		}

		/*deathdelay -= Time.deltaTime;
		if (deathdelay <= 0 && Dead == true)
		{
			Destroy(this.gameObject);
		}*/
	}

    /// <summary>
    /// Look at player sets the rotation of the object to face the target
    /// </summary>
	void lookAtPlayer()
	{
		Quaternion rotation = Quaternion.LookRotation(enemyTarget.position - transform.position);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
		Debug.Log("I see you");
	}

    /// <summary>
    /// This sets the objects Nav Mesh agents position to be the tergets position and will in turn cause the object to mover towards that position
    /// </summary>
	void attackPlease()
	{
		agent.destination = enemyTarget.position;
		GetComponent<WanderingAI>().ResumeAgent();
		if (fpsTargetDistance <= doAttackDistance && singleAttackCoroutine == null)
		{
			singleAttackCoroutine = StartCoroutine(SingleAttack());
			Debug.Log("Attack");
		}
	}

	[Space]
	[SerializeField] private float health;
    private float maxhealth;

    [Space]
	[SerializeField] private float timer = 4;

    /// <summary>
    /// This controlles the health and dammage of the object,
    /// if an object of a spicific tag enters the trigger zone then it will do the assoucicated task
    /// in this case its finding an object of type withing the scene that being a weapon and getting its damage value
    /// and subtracting it from the health of the object
    /// </summary>
    /// <param name="other"></param>
	private void OnTriggerEnter(Collider other)
	{
		print("Collision");

		if (other.gameObject.CompareTag("Weapon"))
		{
			Weapon weapon = FindObjectOfType<CharaterAttack>().weapon;
			if (fpsTargetDistance >= weapon.minRange && fpsTargetDistance <= weapon.maxRange)
			{
				health -= weapon.damage;
				Debug.Log("Weapon delt " + weapon.damage + " Damage");
				Debug.Log("Boss Health " + health);
                print("Collision");
            }
			else
			{
				Debug.Log("Missed");
			}
		}

		Death();
	}

	private Coroutine singleAttackCoroutine;

    /// <summary>
    /// This triggers the random attacks that the object has set up in its animator
    /// </summary>
    /// <returns>Random String that triggers and animation</returns>
	IEnumerator SingleAttack()
	{
		anim.SetTrigger(singleAttack[Random.Range(0, singleAttack.Length)]);
		yield return new WaitForSecondsRealtime(timeBetweenAttacks);
		singleAttackCoroutine = null;
	}

    /// <summary>
    /// Finds a object of type in this case magic and finds it damage value and deducts that value from the objects health this is done for every partical.
    /// </summary>
	private void OnParticleCollision()
	{
		Magic magic = FindObjectOfType<CharaterAttack>().magicType;
		health -= magic.damage;
		Debug.Log("you hit the boss for " + magic.damage + "and his heath is on " + health);
		Death();
	}

    public GameObject manaOrb;
    public GameObject healthOrb;
    private int randomNumber;

    /// <summary>
    /// Death determins if the objects health is less that 0 and triggers a death animation
    /// When this method is called it also chooses a random number between 0 and 5 and if that number is either a 2 or a 3 it will instantiate the selected game object.
    /// once this action has been completer it starts a count down for the object to be destroyed and sets the Nav Mesh agent to stop all movement.
    /// </summary>
	void Death()
	{
		if (health <= 0 && !Dead)
		{
            randomNumber = Random.Range(0, 5);
            if (randomNumber == 2)
            {
                Instantiate<GameObject>(manaOrb, this.transform.position, Quaternion.identity);
            }
            else if (randomNumber == 4)
            {
                Instantiate<GameObject>(healthOrb, this.transform.position, Quaternion.identity);
            }
            else
            {
                print("Poof");
            }

			Dead = true;
			GetComponent<WanderingAI>().StopAgent();

			//deathdelay = timer;
			//CapsuleCollider c = GetComponent<CapsuleCollider>();
			//c.enabled = false;
			Destroy(gameObject, timer);
			anim.SetTrigger("Death");
		}
	}
}
