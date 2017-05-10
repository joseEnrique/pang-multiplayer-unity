using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitaVidas: MonoBehaviour {

	[SerializeField] Text lifes;

	Rigidbody2D rg;
	CapsuleCollider2D bc;

	void Awake () {
		rg = CharControl.instance.GetComponent<Rigidbody2D> ();
		bc = CharControl.instance.GetComponent<CapsuleCollider2D> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag("Enemy")){
			rg.isKinematic = true;
			bc.enabled = false;
			if (CharControl.instance.lifesPlayer-- == 1) {
				CharControl.instance.Lose ();
			}
			lifes.text = CharControl.instance.lifesPlayer.ToString();
			other.GetComponent<Enemigo> ().trigged = true;
			other.GetComponent<Enemigo> ().Destroying += Recollision;
		}
	}
		
	void OnTriggerExit2D (Collider2D other) {
		if (other.CompareTag ("Enemy")) {
			bc.enabled = true;
			rg.isKinematic = false;
		}
	}

	void Recollision () {
			bc.enabled = true;
			rg.isKinematic = false;
	}

}
