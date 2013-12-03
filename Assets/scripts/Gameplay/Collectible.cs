using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour {
	
//	[SerializeField] private Player player;
	public enum Specs {HealthPotion, PowerUpKnife, PowerUpAxe,PowerUpShield};
	[SerializeField] private Specs Specif;
	public int regenValue = 20;

	void OnTriggerEnter(Collider coll)
	{
//		player = GameObject.FindWithTag("Player").GetComponent<Player>();
		if(coll.gameObject.CompareTag("Player"))
		{
			switch (Specif)
			{
				case (Specs.HealthPotion) :
				{
					break;
				}
			}
		}
		Destroy(this.gameObject);
	}
	
	private void fade()
	{
		
	}
}
