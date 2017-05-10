using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClientEntityLogic : MonoBehaviour
{

	#region Setting Attributes

	NetworkEntityState _networkState;

	[Tooltip("Prefab de la bala.")] [SerializeField] GameObject bala;

	[Tooltip("Offset de donde tiene que aparecer la bala.")] [SerializeField] float offset = 1f;

	#endregion

	#region Unity Methods

	void Awake () {
		_networkState = this.GetComponentInChildren<NetworkEntityState>();
		_networkState.PrefabType = PrefabType.Player;
	}

	public void Shoot (uint id) {
		GameObject ba = Instantiate(bala);
		ba.GetComponent<Bala> ().idnet = id;
		ba.transform.position = new Vector3(transform.position.x, transform.position.y + offset, 0.2f);
		this.GetComponentInChildren<NetworkEntityState> ().UpdateClientShoot();
	}

	#endregion

}
