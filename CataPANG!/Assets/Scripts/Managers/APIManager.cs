/* 
 * Resume of this project.
 * Copyright (C) Ricardo Ruiz Anaya 2017
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class APIManager : MonoBehaviour {

	/*void Start()
	{
		callPOST ("quique", 1000);
	}*/

	public void refreshApi(){
		StartCoroutine("respuesta");//invocacion de la rutina
	}
	string cadena = "";
	// Use this for initialization

	IEnumerator respuesta()
	{
		WWW web = new WWW("http://test.opileak.es:3000/api/v1/ranking");//direccion del servicio
		yield return web;//invocamos el servicio
		//yield return new WaitForSeconds(1f);//esperamos un tiempo la respuesta
		cadena = web.text;//agregamos la respuesta a una cadena de texto
	}


	void OnGUI()
	{
		//impresion de resultados

		if (cadena != "")
		{
			JSONObject j = new JSONObject(cadena);//descomponemos en json
			//extraemos cada uno delos campos


			for (int i = 0; i < this.transform.childCount; i++) {
				transform.GetChild (i).GetChild (1).GetComponent<Text> ().text = j[i].GetField("id").ToString().Replace("\"", "");
				transform.GetChild (i).GetChild (2).GetComponent<Text> ().text = j[i].GetField("score").ToString().Replace("\"", "");
			}
		}
	}

	public void callPOST(string name, long score){
		string data = "{\"id\":\""+name+"\",\"score\":"+score.ToString()+"}";

		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Content-Type", "application/json");

		//byte[] b = System.Text.Encoding.UTF8.GetBytes();
		byte[] pData = System.Text.Encoding.ASCII.GetBytes(data.ToCharArray());

		///POST
		WWW api = new WWW("http://test.opileak.es:3000/api/v1/ranking", pData, headers);

		StartCoroutine(WaitForWWW(api));

	}

	IEnumerator WaitForWWW(WWW www)
	{
		yield return www;

		string txt = "";

		if (string.IsNullOrEmpty(www.error))
			txt = www.text; //text of success
		else
			txt = www.error; //error

	}
}