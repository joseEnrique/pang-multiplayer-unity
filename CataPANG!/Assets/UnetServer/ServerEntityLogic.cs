using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System;
using UnityEngine.UI;

public class ServerEntityLogic : MonoBehaviour
{


	#region Setting Attributes

	[Tooltip("Vidas que tiene el enemigo.")] [SerializeField] public byte lifes = 3;

	[Tooltip("Fuerza de repulsión al impactar.")] [SerializeField] float repulsion = 100f;

	[Tooltip("Escala de la primera vida eliminada.")] [SerializeField] Vector3 escala2 = new Vector3 (0.75f, 0.75f, 0.75f);

	[Tooltip("Escada de la segunda vida eliminada.")] [SerializeField] Vector3 escala1 = new Vector3 (0.5f, 0.5f, 0.5f);

	[Tooltip("True == left, False == right.")] [SerializeField] public bool? desplazamiento = false;

	[Tooltip("Velocidad constante a la que se desplaza el enemigo.")] [SerializeField] float Speed = 1f;

	[Tooltip("Factor para suavizar el movimiento del enemigo.")] [SerializeField] float SmoothingFactor = 1f;

	//[Tooltip("Componente texto de la puntuación.")] [SerializeField] Text score;

	public bool trigged = false;

	public delegate void UpdateDestroy();

	public event UpdateDestroy Destroying;

	Rigidbody2D rg;

	Vector3 cvel;
	Vector3 tvel;
	NetworkEntityState _networkState;
	private Vector3 _startPos;

	public Vector3 DestinationPos;
	#endregion


	//#region Unity Methods
	void Awake () {
		rg = GetComponent<Rigidbody2D> ();
	}

	void Start () {
		_networkState = this.GetComponentInChildren<NetworkEntityState> ();
		_networkState.PrefabType = PrefabType.Npc;
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

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.CompareTag ("Player")) {
			if(other.gameObject.GetComponentInChildren<NetworkEntityState>().netId.Value == 3){
				int ar = int.Parse(GameObject.Find ("Lifes1").GetComponent<Text> ().text);
				ar--;
				if (ar > 0) {
					GameObject temp = GameObject.Find ("Lifes1");
					temp.GetComponent<Text> ().text = ar.ToString ();
				} else {
					GameObject temp = GameObject.Find ("Lifes1");
					temp.GetComponent<Text> ().text = ar.ToString ();
					Debug.Log ("Player1 , " + GameObject.Find ("Score1").GetComponent<Text> ().text);
					var globals = FindObjectOfType<GlobalAssets>();
					globals.gameObject.GetComponent<APIManager>().callPOST("Player1", long.Parse(GameObject.Find ("Score1").GetComponent<Text> ().text));
					GetComponentInChildren<NetworkEntityState> ().Derro ();
					CancelInvoke ();



					//AQUÍ



					GameObject.Find ("GameObject").GetComponent<SceneMan> ().re ();
					//Time.timeScale = 0f;
					//LOSE
				}
			} else if(other.gameObject.GetComponentInChildren<NetworkEntityState>().netId.Value == 4){
				int ar = int.Parse(GameObject.Find ("Lifes2").GetComponent<Text> ().text);
				ar--;
				if(ar > 0){
					GameObject temp = GameObject.Find ("Lifes2");
					temp.GetComponent<Text> ().text = ar.ToString();
				} else {
					GameObject temp = GameObject.Find ("Lifes2");
					temp.GetComponent<Text> ().text = ar.ToString ();
					Debug.Log ("Player2 , " + GameObject.Find ("Score2").GetComponent<Text> ().text);
					var globals = FindObjectOfType<GlobalAssets>();
					globals.gameObject.GetComponent<APIManager>().callPOST("Player2", long.Parse(GameObject.Find ("Score2").GetComponent<Text> ().text));
					GetComponentInChildren<NetworkEntityState> ().Derro ();
					CancelInvoke ();


					// AQUÍ

					GameObject.Find ("GameObject").GetComponent<SceneMan> ().re ();
					//Time.timeScale = 0f;
					//LOSE
				}
			}
			this.GetComponentInChildren<NetworkEntityState> ().invokeCanvas ();
		}
	}

	public void Divide () {
		var globals = FindObjectOfType<GlobalAssets>();
		//initialize all server-controlled entities here
		/*var npc = Instantiate<GameObject>(globals.NetworkEntityStatePrototype);

		npc.GetComponent<NetworkEntityState>().PrefabType = PrefabType.Npc;

		NetworkServer.Spawn(npc);

		CharControl.instance.AddScore();*/
		//score.text = CharControl.instance.sco.ToString ();
		if (lifes == 1) {
			if (FindObjectsOfType<ServerEntityLogic> ().Length == 1) {
				globals.gameObject.GetComponent<APIManager>().callPOST("Player1", long.Parse(GameObject.Find ("Score1").GetComponent<Text> ().text));
				globals.gameObject.GetComponent<APIManager>().callPOST("Player2", long.Parse(GameObject.Find ("Score2").GetComponent<Text> ().text));
				GetComponentInChildren<NetworkEntityState> ().Victo ();
				CancelInvoke ();



				//AQUÍ



				GameObject.Find ("GameObject").GetComponent<SceneMan> ().re ();
			}
			Destroy (gameObject);
		}
		if (lifes > 1) {
			Vector3 xy = rg.transform.localPosition;
			//GameObject izq = Instantiate (gameObject);



			//GameObject izq = Instantiate<GameObject> (globals.ServerEntityPrototype);
			//izq.transform.SetParent(FindObjectOfType<MyServer>().transform);
			GameObject netw = Instantiate<GameObject> (globals.NetworkEntityStatePrototype);
			//GameObject izq = netw.GetComponent<NetworkEntityState> ();
			//netw.transform.SetParent (izq.transform);
			netw.GetComponent<NetworkEntityState> ().PrefabType = PrefabType.Npc;
			//izq.name = netw.GetComponent<NetworkEntityState>().PrefabType.ToString();

			//izq.transform.SetParent(FindObjectOfType<MyServer>().transform);
			//netw.GetComponent<NetworkEntityState> ().InstanceNewEnemy ();


			NetworkServer.Spawn(netw);


			netw.GetComponentInParent<ServerEntityLogic> ().desplazamiento = null;
			netw.transform.parent.transform.localPosition = new Vector3(xy.x + 0.62f, xy.y, 0f);
			netw.GetComponentInParent<Rigidbody2D> ().AddForce (new Vector3 (1f, 2f, 0f) * repulsion * Time.deltaTime, ForceMode2D.Impulse);
			rg.Sleep ();
			rg.transform.localPosition = new Vector2(xy.x - 0.62f, xy.y);
			rg.AddForce (new Vector3 (-1f, 2f, 0f) * repulsion * Time.deltaTime, ForceMode2D.Impulse);

			if (lifes == 3) {
				transform.localScale = escala2;
				netw.transform.parent.transform.localScale = escala2;
			}

			if (lifes == 2) {
				transform.localScale = escala1;
				netw.transform.parent.transform.localScale = escala1;
			}

			lifes--;
			netw.GetComponentInParent<ServerEntityLogic> ().lifes = lifes;

		} 
	}


	void OnDisable () {
		if (trigged) {
			Destroying();
		}
	}

}

