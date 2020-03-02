using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FenyxBrasil.Photon
{
	public class PlayerController : MonoBehaviour
	{


		public float playerSpeed = 5f;

		Rigidbody2D rigiBD2d;



		void Start()
		{
			rigiBD2d = GetComponent<Rigidbody2D>();
		}




		void Update()
		{
			PlayerMove(); 
			PlayerTurn();
		}



		void PlayerMove()
		{
			var x = Input.GetAxis("Horizontal");
			var y = Input.GetAxis("Vertical");

			rigiBD2d.velocity = new Vector2(x, y);
		}



		void PlayerTurn()
		{
			Vector3 mousePosition = Input.mousePosition;

			mousePosition = Camera.main.WorldToScreenPoint(mousePosition);

			Vector2 direction = new Vector2
				(
				mousePosition.x - transform.position.x, 
				mousePosition.y - transform.position.y
				);

			transform.up = direction;

		}


	}
}
