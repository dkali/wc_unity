using UnityEngine;
using System.Collections;

public class character_control : MonoBehaviour {

	private Animator anim;
	private Hashtable ht; //table to match direction from inputs ( "horizont, vertical", degree )

	private int direction = 135;
	private int oldDirection = 135;
	private string oldMove;

	private float groundSpeed = 0.11f;

	private GameObject leftJoystick;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		ht = new Hashtable();
		initHT(ht);
		leftJoystick = GameObject.Find ("Left Joystick");
	}
	
	// Update is called once per frame
	void Update () {
		float fHorizont = 0;
		float fVertical = 0;
#if UNITY_IPHONE || UNITY_ANDROID
		fHorizont = ((leftJoystick.transform.position.x>=0) ? ((leftJoystick.transform.position.x==0) ? 0 : 1) : -1);
		fVertical = ((leftJoystick.transform.position.y>=0) ? ((leftJoystick.transform.position.y==0) ? 0 : 1) : -1);
#else
		fHorizont = Input.GetAxisRaw("Horizontal");
		fVertical = Input.GetAxisRaw("Vertical");
#endif

		// are we moving?
		string move = fHorizont.ToString() + "," + fVertical.ToString();

		if ( ht.ContainsKey(move))
		{
			direction = (int)ht[move];

			anim.SetInteger("nLook", direction);

			//move player
			anim.SetBool("bMove", true);
			movePlayer( fHorizont, fVertical );

			if ( oldMove != move )
			{
				anim.SetTrigger("movementTrigger");
				flipIfNeeded(direction);
			}

		}
		 else 
		{
			anim.SetBool("bMove", false);
		}

		oldDirection = direction;
		oldMove = move;
	}

	void initHT( Hashtable table )
	{
		//( "horizont, vertical", degree )
		table.Add("0,1", 0 );
		table.Add("1,1", 45 );
		table.Add("1,0", 90 );
		table.Add("1,-1", 135 );
		table.Add("0,-1", 180 );
		table.Add("-1,-1", 225 );
		table.Add("-1,0", 270 );
		table.Add("-1,1", 315 );
	}

	void flipIfNeeded( int dir )
	{
//		print ( "old dir " + oldDirection.ToString() + "; dir " + dir.ToString() );

		if ( ( oldDirection >= 0 && oldDirection <= 180 && dir > 180 && dir < 360 ) ||
		     ( oldDirection > 180 && oldDirection < 360 && dir >= 0 && dir <= 180 ) )
		{
			flip();
		}
	}

	void flip()
	{
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	void movePlayer( float fHorizont, float fVertical )
	{
		float x = 0;
		float y = 0;

		if (fHorizont != 0)
		{
			x = groundSpeed;
		}
		if (fVertical != 0)
		{
			y = groundSpeed;
		}

		if (fHorizont != 0 && fVertical != 0 )
		{
			x -= 0.02f;
			y -= 0.02f;
		}

		if (fHorizont < 0)
			x *= -1;
		if (fVertical < 0)
			y *= -1;
			
		transform.Translate(new Vector3(x, y, 0) * 0.1f);
	}
}
