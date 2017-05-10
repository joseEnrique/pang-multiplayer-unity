using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharControl : Singleton<CharControl> {
		
	#region Setting Attributes

	[Tooltip("Velocidad a la que se desplaza el player.")] [SerializeField] float Speed = 10f;

	[Tooltip("Velocidad a la que se salta el player.")] [SerializeField] float Jump = 1000f;

	[Tooltip("Variable para controlar si se encuentra tocando algún suelo.")] [SerializeField] bool OnGround = false;

	[Tooltip("Prefab de la bala.")] [SerializeField] GameObject bala;

	[Tooltip("Offset de donde tiene que aparecer la bala.")] [SerializeField] float offset = 1f;

	[Tooltip("Vidas que tiene el personaje.")] [SerializeField] public int lifesPlayer = 3;

	[Tooltip("")] [SerializeField] public Slider sli;

	//[Tooltip("Componente texto de la puntuación.")] [SerializeField] public Text score;

	public int sco = 0;

	Rigidbody2D rg;

	#endregion

	#region Unity Methods

	void Awake () {
		rg = GetComponent<Rigidbody2D> ();
		sli = FindObjectOfType<GlobalAssets>().slid;
		//score = GameObject.Find ("ScoreText").GetComponent<Text> ();
	}

	void Update () {
		rg.velocity = new Vector3 (Speed * sli.value, rg.velocity.y, rg.velocity.x);
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.collider.CompareTag ("Floor") && collision != null) {
			OnGround = true;
		}
	}

	void OnCollisionExit2D (Collision2D collision) {
		if (collision.collider.CompareTag ("Floor") && collision != null) {
			OnGround = false;
		}
	}

	#endregion

	#region Other Methods

	/*public void AddScore(){
		sco += 100;
		score.text = sco.ToString ();
	}
*/
	public void Move(float val) {
		rg.velocity = new Vector3 (Speed * val, rg.velocity.y, rg.velocity.x);
	}

	public void ToJump() {
		if (OnGround) {
			rg.velocity = new Vector3 (rg.velocity.x, Jump, rg.velocity.x);
			transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + Vector3.up, Jump * Time.deltaTime);
		}
	}

	public void Shoot () {
		GameObject ba = Instantiate(bala);
		ba.transform.position = new Vector3(transform.position.x, transform.position.y + offset, 0.2f);
		this.GetComponentInChildren<NetworkEntityState> ().Shooting ();
	}

	public void Lose () {
		Debug.Log ("Perdistes");
		Time.timeScale = 0f;
	}
		
	public void Victory () {
		Debug.Log ("Victoria");
		Time.timeScale = 0f;
	}

	#endregion


}
