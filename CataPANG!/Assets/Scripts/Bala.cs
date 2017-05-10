using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bala : MonoBehaviour {

	// Hacer un pull de objetos

	[Tooltip("Velocidad a la cual se desplaza la bala.")] [SerializeField] int velocidad = 4;

	public uint idnet;

	void Update () {
		transform.position += Vector3.up * Time.deltaTime * velocidad;
		transform.localScale += Vector3.up * Time.deltaTime * 2 * velocidad;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.CompareTag ("Enemy")) {
			if(idnet==3){
				int ar = int.Parse(GameObject.Find ("Score1").GetComponent<Text> ().text) + 100;
				GameObject temp = GameObject.Find ("Score1");
				temp.GetComponent<Text> ().text = ar.ToString();	
			}else if(idnet==4){
				int ar = int.Parse(GameObject.Find ("Score2").GetComponent<Text> ().text) +100;
				GameObject temp = GameObject.Find ("Score2");
				temp.GetComponent<Text> ().text = ar.ToString();
			}
			other.GetComponentInChildren<NetworkEntityState> ().invokeCanvas ();
			other.GetComponent<ServerEntityLogic> ().Divide ();

			Destroy (gameObject);
			//Destruir en clientes.
		}
		if (other.CompareTag ("Floor")) {
			Destroy (gameObject);
			//Destruir en clientes.
		}
	}

}
