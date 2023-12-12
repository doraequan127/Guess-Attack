using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shatter : MonoBehaviour
{
	private Rigidbody[] ribs;
	[SerializeField]
	private float minForce = 2, maxForce = 3;
	private float force;

	[SerializeField] float fallMinDuration = 3;
	[SerializeField] float fallMaxDuration = 5;
	
	
	void OnEnable()
	{
		ribs = this.GetComponentsInChildren<Rigidbody>();
		Vector3 direction = Vector3.zero;
		foreach (var rib in ribs)
		{

			direction = rib.transform.position - this.transform.position;
			direction.Normalize();
			force = Random.Range(minForce, maxForce);

			rib.AddForce(direction * force, ForceMode.Impulse);
		}
	}

	public void AllowTrigger()
	{
		foreach (Rigidbody rib in ribs)
		{
			rib.GetComponent<Collider>().isTrigger = true;
		}
	}
}

