using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using CharacterController;
using UnityEditor.Animations;
using UnityEngine.UIElements;

public enum CharacterType
{
    Warrior = 0,
    Archer = 1,
    Wizard = 2,
}

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter Instance { get { return instance; } }
    public WeaponManager weaponManager { get; private set; }
    public StateMachine stateMachine { get; private set; }
    public new Rigidbody rigidbody { get; private set; }
    public Animator animator { get; set; }
    public AnimatorController animatorCon { get; set; }

    // public SkinnedMeshRenderer skinnedMeshRenderer;

    public GameObject effectGenerator;

    public CharacterType characterClass;

    [Header("Character Meshs")]
    public Mesh[] classMesh;
    [Header("Character AnimatorControllers")]
    public RuntimeAnimatorController[] classControllers;
    [Header("Character WeaponObjects")]
    public GameObject[] weaponObjects;


    private static PlayerCharacter instance;

    #region #ĳ���� ����

    public float MaxHp { get { return maxHp; } }
    public float CurrentHp { get { return currentHp; } }
    public float MoveSpeed { get { return moveSpeed; } }
    public int DashCount { get { return dashCount; } }

    [Header("ĳ���� ����")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float currentHp;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int dashCount;
    #endregion

    [Header("�߻�ü �߻���ġ")]
    public GameObject firePoint;

    [Header("���� �ݶ��̴�")]
    public BoxCollider attackRange;

    public PlayerCharacterController PCC;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            weaponManager = new WeaponManager();
            rigidbody = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();            
            return;
        }
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
        stateMachine.AddState(StateName.DASH, new DashState(controller));
    }
}
