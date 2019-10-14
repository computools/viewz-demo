using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

	public float damage = 10f;
	public float range = 100f;
	public float fireRate = 15f;
	public float impactForce = 60f;
	public Camera fpsCam;

	public ParticleSystem flash;

	public GameObject impactEffect;

	private float nextTimeToFire = 0f;

	float rate = 0.0f;

	public Animator anim;

	// Update is called once per frame
	void Update () 
	{
        
		if(Input.GetMouseButton(0) && Time.time >= nextTimeToFire
           || Input.GetKey("joystick button 15") && Time.time >= nextTimeToFire)
		{
			nextTimeToFire = Time.time + 1f/fireRate;
			Shoot();
		}
		else
		{
			var em = flash.emission;
			em.enabled = false;
			em.rateOverTime = 0.0f;
		}

		if(Input.GetMouseButtonUp(0) || !Input.GetKey("joystick button 15"))
		{
			anim.SetBool("Shoot",false);
		}
	}

	void Shoot()
	{
		flash.Play();
		var em = flash.emission;
		em.enabled = true;
		em.rateOverTime = 15.0f;
		anim.SetBool("Shoot",true);
		RaycastHit hit;
        
        if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hit))
        {
			Target target  = hit.transform.GetComponent<Target>();

			if(target != null)
			{
				target.TakeDamage(damage);
			}

			if(hit.rigidbody!=null)
			{
				hit.rigidbody.AddForce(-hit.normal * impactForce);
			}

			GameObject impactGO = Instantiate(impactEffect,hit.point,Quaternion.LookRotation(hit.normal));
			Destroy(impactGO,2.0f);
		}
			
	}

}