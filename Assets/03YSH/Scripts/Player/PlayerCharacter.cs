using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using CharacterController;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter Instance { get { return instance; } }
    public WeaponManager weaponManager { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public Rigidbody rigidbody { get; private set; }
    public Animator animator { get; private set; }
    public GameObject effectGenerator;
    public Quaternion targetRotation;

    private static PlayerCharacter instance;

    #region #Ä³¸¯ÅÍ ½ºÅÈ

    public float MaxHp      { get { return maxHp; } }
    public float CurrentHp  { get { return currentHp; } }
    public float MoveSpeed  { get { return moveSpeed; } }
    public int DashCount    { get { return dashCount; } }

    [Header("Ä³¸¯ÅÍ ½ºÅÈ")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int dashCount;
    #endregion

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            weaponManager = new WeaponManager();
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
            DontDestroyOnLoad(gameObject);
            return;
        }
        DestroyImmediate(gameObject);
    }

    private void Start()
    {
        InitStateMachine();
    }

    private void Update()
    {
        stateMachine?.UpdateState();
    }

    private void FixedUpdate()
    {
        stateMachine?.FixedUpdateState();
    }

    public void OnUpdateStat(float maxHp, float currentHp, float moveSpeed, int dashCount)
    {
        this.maxHp = maxHp;
        this.currentHp = currentHp;
        this.moveSpeed = moveSpeed; 
        this.dashCount = dashCount;
    }

    private void InitStateMachine()
    {
        PlayerCharacterController controller = GetComponent<PlayerCharacterController>();
        stateMachine = new StateMachine(StateName.MOVE, new MoveState(controller));
        stateMachine.AddState(StateName.ATTACK, new AttackState(controller));
    }
}
