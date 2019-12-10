using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPress : MonoBehaviour
{
  // if Player hits the press point on the switch, turn switch on
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			// tell the switch to be turned on
			this.GetComponentInParent<Switch>().TurnedOn();
		}
	}
}
