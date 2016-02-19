using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Animator))]
[RequireComponent (typeof(AudioClip))]
[RequireComponent (typeof (AudioSource))]
public class SimpleCharacterController : MonoBehaviour {

    /// <summary>
    /// Every character should have these things
    /// </summary>
    public AudioClip[] audioClips;
    protected Animator anim;
    protected AudioSource audioPlayer;

    //CHARCTER VELOCITY INFORMATION
    [HideInInspector]
    public Vector3 velocity;
    protected float velocityXSmoothing;

    //PHYSICS AND GRAVITY INFORMATION
    public float jumpHeight = 4;
    public float jumpTime = .4f;
    public float moveSpeed = 6;

    //PUBLICALLY ACCESSIBLE VARIABLES FOR OTHER SCRIPTS
    [HideInInspector]
    public bool isControllable;
    [HideInInspector]
    public bool maintainVelocity;
    [HideInInspector]
    public bool facingRight;
    [HideInInspector]
    public Controller2D controller;

    [HideInInspector]
    public float gravityModifier = 1;  //For weapons and internal control purposes. 

    protected CharacterStats playerStats;



    // Use this for initialization
    /// <summary>
    /// Whenever creating a movement controller, use this as a base
    /// </summary>
    protected virtual void Start () {
        //Initialize Variables
        isControllable = true;
        maintainVelocity = false;
        facingRight = true;

        //Get components
        audioPlayer = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
        controller = GetComponent<Controller2D>();
        playerStats = GetComponent<CharacterStats>();

    }


}
