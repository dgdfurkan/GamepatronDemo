using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
	public CharacterController controller; //Contoller componenti
	public Animator anim; //Animator componenti
	public GameObject KameraObjesi; //Kamera objesi (Kamera oyuncunun sa� ve sol hareketlerine g�re takip edecek. Sahnedeki kamera transformunun t�m de�erleri sabit kalacak, yaln�zca position.x de�eri karakterin position.x de�erini takip edecek.)
	public GameObject mermiPrefab; //Duvar ya da npc objelerine t�kland���nda spawnlanacak olan mermi prefab�

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		AnimasyonVeRotasyon();
		YerCekimiVeZiplama();
		KameraTakip();
		KarakterHareket();
	}

	void AnimasyonVeRotasyon()
	{
	}

	void YerCekimiVeZiplama()
	{
	}

	void KameraTakip()
	{
	}

	void KarakterHareket()
	{

	}
}
