using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
public class NetworkEntityState : NetworkBehaviour
{

    [SyncVar]
    public PrefabType PrefabType;

	/*
	public GameObject instServer(){
		var globals = FindObjectOfType<GlobalAssets>();

		var serverEnt = Instantiate<GameObject> (globals.ServerEntityPrototype);
		transform.SetParent(serverEnt.transform);
		serverEnt.transform.SetParent(FindObjectOfType<MyServer>().transform);
		serverEnt.name = PrefabType.ToString();

		InvokeRepeating("UpdateClientPosition", 0, .1f);

		return serverEnt;
	}

	*/
	/*
	public void instClient(){
		var globals = FindObjectOfType<GlobalAssets>();

		var clientEnt = Instantiate<GameObject> (globals.EnemyClientPrefab);
		ConfigureClientPrefab (clientEnt);
	}
	*/



	public void Victo(){
		Victorius ();
	}

	public void Derro(){
		Derrotius ();
	}


    public override void OnStartServer()
    {
		
        base.OnStartServer();
        var globals = FindObjectOfType<GlobalAssets>();

		if (PrefabType == PrefabType.Canvas) {
			var serverEnt = Instantiate<GameObject> (globals.Canvas);
			transform.SetParent(serverEnt.transform);
			serverEnt.transform.SetParent(FindObjectOfType<MyServer>().transform);
			serverEnt.name = PrefabType.ToString();
		}

		if (PrefabType == PrefabType.Npc) {
			var serverEnt = Instantiate<GameObject> (globals.ServerEntityPrototype);
			transform.SetParent(serverEnt.transform);
			serverEnt.transform.SetParent(FindObjectOfType<MyServer>().transform);
			serverEnt.name = PrefabType.ToString();
		} else if (PrefabType == PrefabType.Player) {
			var serverEnt = Instantiate<GameObject> (globals.ClientEntityPrefab);
			//serverEnt.GetComponent<CharControl> ().enabled = false;
			//Destroy (serverEnt.GetComponent<Rigidbody2D> ());
			transform.SetParent(serverEnt.transform);
			serverEnt.transform.SetParent(FindObjectOfType<MyServer>().transform);
			serverEnt.name = PrefabType.ToString();
		}
		
		InvokeRepeating("UpdateClientPosition", 0, .001f);

    }

	public void invokeCanvas(){
		Invoke("UpdateCanvas", 0f);
	}


	[Server]
	private void UpdateCanvas()
	{
		string sco1 = GameObject.Find ("Score1").GetComponent<Text>().text;
		string sco2 = GameObject.Find ("Score2").GetComponent<Text>().text;
		string lif1 = GameObject.Find ("Lifes1").GetComponent<Text>().text;
		string lif2 = GameObject.Find ("Lifes2").GetComponent<Text>().text;
		RpcCanvas(sco1,sco2,lif1,lif2);
	}

	[ClientRpc]
	private void RpcCanvas(string sco1, string sco2, string lif1, string lif2)
	{
		GameObject.Find ("Score1").GetComponent<Text> ().text = sco1;
		GameObject.Find ("Score2").GetComponent<Text> ().text = sco2;
		GameObject.Find ("Lifes1").GetComponent<Text> ().text = lif1;
		GameObject.Find ("Lifes2").GetComponent<Text> ().text = lif2;
	}

    public override void OnStartClient()
    {
        base.OnStartClient();
        var globals = FindObjectOfType<GlobalAssets>();

        //var clientEnt = Instantiate<GameObject>(globals.ClientEntityPrefab);

		if (PrefabType == PrefabType.Canvas) {
		
			var clientEnt = Instantiate<GameObject> (globals.Canvas);
			ConfigureClientPrefab (clientEnt);
		}

		if (PrefabType == PrefabType.Npc) {
			var clientEnt = Instantiate<GameObject> (globals.ServerEntityPrototype);
			//clientEnt.GetComponent<Enemigo> ().enabled = false;
			//clientEnt.GetComponent<Collider2D> ().enabled = false;
			//Destroy (clientEnt.GetComponent<Rigidbody2D>());
			ConfigureClientPrefab (clientEnt);
		} else if (PrefabType == PrefabType.Player) {
			if (!this.GetComponentInChildren<NetworkEntityState> ().isLocalPlayer) {
				var clientEnt = Instantiate<GameObject> (globals.ClientEntityPrefab);
				ConfigureClientPrefab (clientEnt);
			}/* else {
				var clientEnt = Instantiate<GameObject> (globals.PlayerClientPrefab);
				ConfigureClientPrefab (clientEnt);
			}*/
		}

    }
    private void ConfigureClientPrefab(GameObject clientEnt)
    {

        SetIgnoreAllServerObjectCollision(clientEnt.transform);

        transform.SetParent(clientEnt.transform);
        clientEnt.transform.SetParent(FindObjectOfType<MyClient>().transform);

        clientEnt.name = PrefabType.ToString();
    }
    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        var globals = FindObjectOfType<GlobalAssets>();

        var clientEnt = Instantiate<GameObject>(globals.PlayerClientPrefab);

		clientEnt.GetComponent<CharControl> ().sli = globals.slid;
		//clientEnt.GetComponent<CharControl> ().score = globals.scor;


		globals.jum.onClick.AddListener (() => clientEnt.GetComponent<CharControl>().ToJump());
		globals.sho.onClick.AddListener (() => clientEnt.GetComponent<CharControl>().Shoot());


        //At the time of OnStartClient, we couldn't know that this was a LocalPlayer.
        //We do now so we simply remove the old prefab and put in the new one with the e.g. camera.
        var oldParent = this.transform.parent;
        ConfigureClientPrefab(clientEnt);
        Destroy(oldParent.gameObject);

        InvokeRepeating("UpdatePlayerServerPosition", 0, .3f);
    }

	public void Shooting(){
		ShootPlayerServer ();
	}

    [Client]
    private void UpdatePlayerServerPosition()
    {
		if (isLocalPlayer) {
			CmdServerMove (transform.parent.position, this.netId.Value);
		}
    }

    [Command]
	public void CmdServerMove(Vector3 position, uint id)
	{	
		//Suavizar llamando a un método con lerp.
		transform.parent.position = position;
    }

	[Server]
	private void Victorius()
	{
		RpcVictorius ();
	}

	[ClientRpc]
	public void RpcVictorius()
	{	
		GameObject.Find ("GameObject").GetComponent<SceneMan> ().LoadAScene (2);
	}

	[Server]
	private void Derrotius()
	{
		RpcDerrotius ();
	}

	[ClientRpc]
	public void RpcDerrotius()
	{	
		GameObject.Find ("GameObject").GetComponent<SceneMan> ().LoadAScene (3);
	}

	[Client]
	private void ShootPlayerServer(){
		CmdServerShoot (netId.Value);
	}

	[Command]
	private void CmdServerShoot(uint id){
		this.GetComponentInParent<ClientEntityLogic> ().Shoot (id);
	}

    [Server]
    private void UpdateClientPosition()
    {
		RpcClientMove(transform.parent.position, transform.parent.localScale);
    }

    [ClientRpc]
	private void RpcClientMove(Vector3 position, Vector3 scale)
    {
		if (!isLocalPlayer){
			transform.parent.position = position;
			transform.parent.localScale = scale;
		}
	}

	[Server]
	public void UpdateClientShoot(){
		RpcClientShoot (netId.Value);
	}

	[ClientRpc]
	private void RpcClientShoot(uint id){
		//if ((!isLocalPlayer) && (PrefabType == PrefabType.Player)) {
		this.GetComponentInParent<ClientEntityLogic> ().Shoot (id);
		//}
	}




    [Client]
    private void SetIgnoreAllServerObjectCollision(Transform ignoreWith)
    {
        var server = FindObjectOfType<MyServer>();
        if (server)
        {
            foreach (var collider in server.GetComponentsInChildren<Collider>())
            {
                foreach (var selfCollider in ignoreWith.GetComponentsInChildren<Collider>())
                {
                    Physics.IgnoreCollision(collider, selfCollider);
                }
            }
        }
    }

    void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }

}
