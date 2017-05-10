using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemigo : MonoBehaviour {

	#region Setting Attributes

	[Tooltip("Vidas que tiene el enemigo.")] [SerializeField] byte lifes = 3;

	[Tooltip("Fuerza de repulsión al impactar.")] [SerializeField] float repulsion = 100f;

	[Tooltip("Escala de la primera vida eliminada.")] [SerializeField] Vector3 escala2 = new Vector3 (0.75f, 0.75f, 0.75f);

	[Tooltip("Escada de la segunda vida eliminada.")] [SerializeField] Vector3 escala1 = new Vector3 (0.5f, 0.5f, 0.5f);

	[Tooltip("True == left, False == right.")] [SerializeField] bool? desplazamiento = false;

	[Tooltip("Velocidad constante a la que se desplaza el enemigo.")] [SerializeField] float Speed = 1f;

	[Tooltip("Factor para suavizar el movimiento del enemigo.")] [SerializeField] float SmoothingFactor = 1f;

	//[Tooltip("Componente texto de la puntuación.")] [SerializeField] Text score;

	public bool trigged = false;

	public delegate void UpdateDestroy();

	public event UpdateDestroy Destroying;

	Rigidbody2D rg;

	Vector3 cvel;
	Vector3 tvel;

	#endregion

	#region Unity Methods

	void Awake () {
		rg = GetComponent<Rigidbody2D> ();
	}

	void Start () {
		if (desplazamiento != null) {
			if (desplazamiento == true) {
				rg.AddForce (new Vector3 (0.5f, 0f, 0f) * repulsion * Time.deltaTime);
			} else {
				rg.AddForce (new Vector3 (-0.5f, 0f, 0f) * repulsion * Time.deltaTime);
			}
		}
	}

	void LateUpdate () {
		cvel = transform.GetComponent<Rigidbody2D> ().velocity;
		tvel = cvel.normalized * Speed;
		transform.GetComponent<Rigidbody2D> ().velocity = Vector3.Lerp(cvel, tvel, Time.deltaTime * SmoothingFactor);
	}

	#endregion

	#region Other Methods


	public void Divide () {
		/*

		CharControl.instance.AddScore();
		//score.text = CharControl.instance.sco.ToString ();
		if (lifes == 1) {
			if (FindObjectsOfType<Enemigo> ().Length == 1) {
				CharControl.instance.Victory();
			}
			Destroy (gameObject);
		}
		if (lifes > 1) {
			Vector3 xy = rg.transform.localPosition;
			//GameObject izq = Instantiate (gameObject);


			var globals = GameObject.Find("GlobalAssets").GetComponent<GlobalAssets>();

			//GameObject izq = Instantiate<GameObject> (globals.ServerEntityPrototype);
			//izq.transform.SetParent(FindObjectOfType<MyServer>().transform);
			GameObject netw = Instantiate<GameObject> (globals.NetworkEntityStatePrototype);
			GameObject izq = netw.GetComponent<NetworkEntityState> ();
			netw.transform.SetParent (izq.transform);
			netw.GetComponent<NetworkEntityState> ().PrefabType = PrefabType.Npc;
			izq.name = netw.GetComponent<NetworkEntityState>().PrefabType.ToString();

			izq.transform.SetParent(FindObjectOfType<MyServer>().transform);
			netw.GetComponent<NetworkEntityState> ().InstanceNewEnemy ();


			izq.GetComponentInParent<Enemigo> ().desplazamiento = null;
			izq.transform.localPosition = new Vector3(xy.x + 0.62f, xy.y, 0f);
			izq.GetComponentInParent<Rigidbody2D> ().AddForce (new Vector3 (1f, 2f, 0f) * repulsion * Time.deltaTime, ForceMode2D.Impulse);
			rg.Sleep ();
			rg.transform.localPosition = new Vector2(xy.x - 0.62f, xy.y);;
			rg.AddForce (new Vector3 (-1f, 2f, 0f) * repulsion * Time.deltaTime, ForceMode2D.Impulse);

			if (lifes == 3) {
				transform.localScale = escala2;
				izq.transform.localScale = escala2;
			}

			if (lifes == 2) {
				transform.localScale = escala1;
				izq.transform.localScale = escala1;
			}

			lifes--;
			izq.GetComponent<Enemigo> ().lifes = lifes;
		} 
		*/
	}



	void OnDisable () {
		if (trigged) {
			Destroying();
		}
	}

	#endregion

}
