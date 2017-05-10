using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum PrefabType
{
    Player,
    Npc,
    SomeOtherNpc,
	Canvas
}
public class GlobalAssets : MonoBehaviour
{
    public GameObject NetworkEntityStatePrototype;

    public GameObject ServerEntityPrototype;

    public GameObject ClientEntityPrefab;

    public GameObject PlayerClientPrefab;

	public GameObject EnemyClientPrefab;

	public GameObject Canvas;

	public Slider slid;

	public Button jum;

	public Button sho;

	public Text scor;
}
