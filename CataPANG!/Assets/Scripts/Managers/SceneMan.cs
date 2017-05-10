
/* 
 * Resume of this project.
 * Copyright (C) Ricardo Ruiz Anaya 2017
 */

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneMan : MonoBehaviour {

	public void LoadAScene(int sce){
		SceneManager.LoadScene (sce);
	}

	public void Quitt(){
		Application.Quit();
	}

	public void re(){
		SceneManager.LoadScene (1);
	}
}
