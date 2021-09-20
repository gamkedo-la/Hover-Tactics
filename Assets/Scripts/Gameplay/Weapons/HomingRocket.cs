using UnityEngine;
using UnityEngine.Assertions;

public class HomingRocket : MonoBehaviour
{
    public float damage = 0.5f;
    public float rocketSpeed;
    public float rotateSpeed;
    public float targetRange;
    public GameObject meshObject;
    public float destroyDelay = 5.0f;

    [Header("Effects")]
    public string explosionTag;
    public SoundFxKey explosionSound;

    private Rigidbody rb;
    private Collider projectileCollider;
    private AudioSource audioSource;
    private Transform target;
    private float destroyTimer = 0.0f;

    void OnEnable()
    {
        destroyTimer = destroyDelay;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        projectileCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        target = FindClosestEnemy();

        Assert.IsNotNull(rb, "Rigidbody is null!");
        Assert.IsNotNull(projectileCollider, "Projectile Collider is null!");
        Assert.IsNotNull(audioSource, "Audio Source is null!");
    }

	private void Update()
	{
        target = FindClosestEnemy();

        if(destroyTimer <= 0.0f)
            DisableObject();
        else
            destroyTimer -= Time.deltaTime;
    }

	void FixedUpdate()
    {
        if(target == gameObject.transform)
		{
            rb.velocity = transform.forward * rocketSpeed;
        }
        else
		{
            Vector3 direction = target.position - rb.position;
            direction.Normalize();

            Vector3 rotationAmount = Vector3.Cross(transform.forward, direction);
            rb.angularVelocity = rotationAmount * rotateSpeed;

            rb.velocity = transform.forward * rocketSpeed;
        }
    }

    Transform FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance && curDistance <= targetRange)
            {
                closest = go;
                distance = curDistance;
            }
        }
        if(closest == null)
		{
            return gameObject.transform;
		}
        else
		{
            return closest.transform;
        }
    }

	private void OnCollisionEnter(Collision coll)
	{
        DestroyEffects(coll);
        meshObject.SetActive(false);
        projectileCollider.enabled = false;
        Invoke("DisableObject", 1.0f);

        AbstractTakeDamage canTakeDamage = coll.transform.GetComponent<AbstractTakeDamage>();
        if(canTakeDamage) canTakeDamage.TakeDamage(GetDamage());
    }

    private Damage GetDamage() { return new Damage() { Value = -damage }; }

    void DestroyEffects(Collision coll)
    {
        ContactPoint contact = coll.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        ObjectPooler.instance.SpawnFromPool(explosionTag, transform.position, rot);
        SoundFXManager.PlayOneShot(explosionSound, audioSource);
    }

    void DisableObject()
    {
        destroyTimer = destroyDelay;
        meshObject.SetActive(true);
        projectileCollider.enabled = true;
        gameObject.SetActive(false);
    }
}
