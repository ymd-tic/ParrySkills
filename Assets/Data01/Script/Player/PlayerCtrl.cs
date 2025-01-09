using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    //-----SerializeField------------------------------------------------------------
    [Header("�ő�HP")]            �@[SerializeField] private float maxHp = 10;
    [Header("�ő�X�^�~�i")]        [SerializeField] private float maxStamina = 10;
    [Header("���C���J����")]        [SerializeField] private Camera mainCamera;
    [Header("�ړ����x")]            [SerializeField] private float moveSpeed;
    [Header("�����]�����x")]        [SerializeField] private float rotSpeed;
    [Header("��𑬓x")]            [SerializeField] private float rollingSpeed;
    [Header("�X�^�~�i�񕜑��x")]    [SerializeField] private float staminaHealSpeed;
    [Header("�X�^�~�i�g�p��")]      [SerializeField] private float staminaUseValue;
    [Header("HP�Q�[�W")]            [SerializeField] private Slider hpGage;
    [Header("�X�^�~�i�Q�[�W")]      [SerializeField] private Slider staminaGage;
    [Header("�p���B�����蔻��")]    [SerializeField] public BoxCollider parryCollider;
    [Header("���̋O�ՃG�t�F�N�g")]  [SerializeField] public ParticleSystem[] slashEfects = new ParticleSystem[4];


    //-----privateField--------------------------------------------------------------
    private Vector3 moveVector; // �ړ�����
    private Vector3 rollingVector; // ������
    private float horizontal; // X��
    private float vertical;   // Z��
    private float curAtackState = 0; // ���݂̍U���i��
    private bool isCanCombo = false; // �R���{�\��
    private bool isCanAtack = true;  // �U���o���邩
    private bool isCanRolling = false;
    private bool isNowRolling = false;  // ������Ă��邩
    private CharacterController characterController;
    private SkillCtrl skillController;
    private Coroutine rollingCoroutine;
    private Coroutine parryCoroutine;
    private PlayerHitManager playerHitManager;
    private Generic.ParamateValue hpValue;
    private Generic.ParamateValue staminaValue;

    //-----publicField---------------------------------------------------------------
    [System.NonSerialized] public Animator animator;
    [System.NonSerialized] public int atackPower = 1;


    //-----staticField---------------------------------------------------------------



    //-----protectedField------------------------------------------------------------

    #region �V�X�e��

    private void Awake()
    {
        hpValue = new Generic.ParamateValue(maxHp, maxHp, 0);
        staminaValue = new Generic.ParamateValue(maxHp, maxStamina, 0);
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        skillController = GetComponent<SkillCtrl>();
        animator = GetComponent<Animator>();
        playerHitManager = GetComponent<PlayerHitManager>();
        hpGage.value = hpValue.cur / hpValue.max;
        staminaGage.value = staminaValue.cur / staminaValue.max;
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage")) return;

        // ��𒆂���Ȃ���΃X�^�~�i��
        if (!isNowRolling)
        {
            staminaValue.cur = Mathf.Clamp(staminaValue.cur += staminaHealSpeed * Time.deltaTime, staminaValue.min, staminaValue.max);
            staminaGage.value = staminaValue.cur / staminaValue.max;
        }

        MovePosition(); // �ړ�
        MoveRolling();  // ���
    }
    #endregion


    #region InputSystem�֘A

    /// <summary>
    /// �ړ��̌Ăяo��
    /// </summary>
    /// <param name="_context"></param>
    public void OnMove(InputAction.CallbackContext _context)
    {
        horizontal = _context.ReadValue<Vector2>().x;
        vertical = _context.ReadValue<Vector2>().y;

        float moveAmount = Mathf.Abs(horizontal) + Mathf.Abs(vertical); // ���͂��Ă��邩��0�`1�܂łŊi�[
        animator.SetFloat("Run", moveAmount);
    }

    /// <summary>
    /// �ʏ�U���̌Ăяo��
    /// </summary>
    /// <param name="_context"></param>
    public void OnAtack(InputAction.CallbackContext _context)
    {
        if (!_context.performed) { return; } // �u�ԓI�ɉ����Ă��Ȃ�������return
        if (!isCanAtack) { return; }        // �U�����\����Ȃ�������return
        if (isNowRolling) { return; }          // ��𒆂�������return
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Damage")) { return; }

        curAtackState = animator.GetFloat("Atack");

        if (isCanCombo)
        {
            if (curAtackState >= 4)
            {
                //animator.SetFloat("Atack", 0f);
                isCanAtack = false;
                StartCoroutine(ComboEnd());
                return;
            }

            isCanCombo = false;
            curAtackState += 1f;



            animator.SetFloat("Atack", curAtackState);
        }
        else
        {
            if (curAtackState == 0f)
            {
                curAtackState += 1f;
                animator.SetFloat("Atack", 1f);
            }
        }

        atackPower = (int)curAtackState * 10;

        atackPower += skillController.GetAtackBuff();

        // �G���߂��ɂ�����G�̕�������
        if (AreaManager.enemyList.Count != 0)
        {
            Vector3 nearEnemy = AreaManager.NearEnemy(this.gameObject);

            if (Vector3.Distance(transform.position, nearEnemy) <= 4)
            {
                transform.LookAt(nearEnemy);
            }
        }
    }

    /// <summary>
    /// ����̌Ăяo��
    /// </summary>
    /// <param name="_context"></param>
    public void OnRolling(InputAction.CallbackContext _context)
    {
        if (!_context.performed) { return; }
        if (staminaValue.cur < staminaUseValue) { return; }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage")) { return; }

        if (isCanRolling)
        {
            isCanRolling = false;
            StopCoroutine(rollingCoroutine);
            SetRolling();
            animator.CrossFade("Rolling", 0.1f, 0, 0f);
            return;
        }

        if (!isNowRolling)
        {
            SetRolling();
            animator.SetTrigger("Rolling");
        }
    }

    #endregion


    #region �v���C���[�̐���

    #region �ړ��E��]

    /// <summary>
    /// �v���C���[�̈ړ�
    /// </summary>
    void MovePosition()
    {
        // �J�����̑O����
        Vector3 cameraForward = Vector3.Scale(mainCamera.transform.forward, new Vector3(1, 0, 1)).normalized;

        // �v���C���[�̑O����
        moveVector = cameraForward * vertical + mainCamera.transform.right * horizontal;

        if (animator.GetFloat("Atack") != 0 || isNowRolling) { return; }

        // �L�����N�^�[�R���g���[���[�ňړ�
        characterController.Move(moveSpeed * moveVector * Time.deltaTime);

        // �v���C���[�������Ă�����
        if (moveVector != Vector3.zero)
        {
            MoveRotation(moveVector);
        }
    }

    /// <summary>
    /// �v���C���[����͕����ɏ��X�ɉ�]������
    /// </summary>
    /// <param name="_moveVector"></param> �v���C���[�̐i�s����
    void MoveRotation(Vector3 _moveVector)
    {
        // ��]����p�x���v�Z
        Quaternion deg = Quaternion.LookRotation(_moveVector);

        // ��]�ɑ��x��t����
        transform.rotation = Quaternion.Lerp(transform.rotation, deg, Time.deltaTime * rotSpeed);
    }

    #endregion


    #region ���

    /// <summary>
    /// ������̏����ݒ�
    /// </summary>
    void SetRolling()
    {
        rollingCoroutine = StartCoroutine(RollingCoroutine());
        ParryColliderOff();
        StartCoroutine(new Generic.CalcuRation().ValueFluctuation(-staminaUseValue, staminaGage, staminaValue));
        animator.SetFloat("Atack", 0);
        // �ړ����Ă��Ȃ������琳�ʁA���Ă���ړ�����
        if (vertical == 0 && horizontal == 0)
        {
            rollingVector = transform.forward;
        }
        else
        {
            rollingVector = moveVector;
        }
        transform.rotation = Quaternion.LookRotation(rollingVector, Vector3.up);

    }

    /// <summary>
    /// ������ɑO���Ɉړ�
    /// </summary>
    void MoveRolling()
    {
        if (!isNowRolling) { return; }

        characterController.Move(rollingVector * rollingSpeed * Time.deltaTime);
    }

    #endregion


    #region ���̑�

    /// <summary>
    /// �_���[�W���󂯂�
    /// </summary>
    /// <param name="_damage"></param>
    public void TakeDamage(float _damage)
    {
        Debug.Log(_damage);
        // HP��0�ȉ��ɂȂ�����
        if (hpValue.cur + _damage <= hpValue.min)
        {
            SceneController.GameFinish(SceneController.GameEndStatus.OVER);
        }

        animator.SetTrigger("Damage");
        animator.SetFloat("Atack", 0f);
        ParryColliderOff();
        StartCoroutine(new Generic.CalcuRation().ValueFluctuation(_damage,hpGage,hpValue));
    }

    /// <summary>
    /// �p���B�R���C�_�[��Off
    /// </summary>
    private void ParryColliderOff()
    {
        parryCollider.enabled = false;
        StopCoroutine(ParryColliderCoroutine());
    }

    /// <summary>
    /// �v���C���[HP�𑝌�������
    /// </summary>
    /// <param name="_value">�����l</param>
    public void HpFluctuation(float _value)
    {
        StartCoroutine(new Generic.CalcuRation().ValueFluctuation(_value, hpGage, hpValue));
    }
    #endregion

    #endregion


    #region �A�j���[�V����Event

    /// <summary>
    /// �U�����I�������R���{�ł����Ԃɂ���
    /// </summary>
    void AttackEnd()
    {
        isCanCombo = true;
    }

    /// <summary>
    /// �U�����[�V�������I�������U������߂�
    /// </summary>
    void AttackAnimationEnd()
    {
        isCanCombo = false;
        animator.SetFloat("Atack", 0f);
        animator.ResetTrigger("Rolling");
    }

    /// <summary>
    /// �U�����肪�o����G�t�F�N�g���o��
    /// </summary>
    void SlashEfectr()
    {
        if(animator.GetFloat("Atack") == 0f) {return;}

        slashEfects[(int)animator.GetFloat("Atack") - 1].Play();
        parryCoroutine = StartCoroutine(ParryColliderCoroutine());
    }

    #endregion


    #region�@�R���[�`��

    /// <summary>
    /// �����蔻����ēx�o��
    /// </summary>
    /// <returns></returns>
    IEnumerator RollingCoroutine()
    {
        float rollingTime = 0.5f;  // �A�����ĉ���o���鎞��
        float continuousRolingTime = 0.6f; // �������ړ��ł��鎞��
        isCanRolling = false;
        isNowRolling = true;
        playerHitManager.enabled = false;

        yield return new WaitForSeconds(rollingTime);
        isCanRolling = true;
        playerHitManager.enabled = true;

        yield return new WaitForSeconds(continuousRolingTime - rollingTime);
        isCanRolling = false;
        isNowRolling = false;
    }

    /// <summary>
    /// �R���{�U���̏I�����������x�U�����o����܂ł̏���
    /// </summary>
    /// <returns></returns>
    IEnumerator ComboEnd()
    {
        float canAtackTime = 0f; // �U�����o����悤�ɂȂ�܂ł̎���
        yield return new WaitForSeconds(canAtackTime);
        isCanAtack = true;
    }

    /// <summary>
    /// �G�t�F�N�g���o���^�C�~���O�Ńp���B������o��
    /// </summary>
    /// <returns></returns>
    IEnumerator ParryColliderCoroutine()
    {
        float parryOnTime = 0.2f; // �p���B�̓����蔻�肪ON�ɂȂ鎞��
        parryCollider.enabled = true;
        yield return new WaitForSeconds(parryOnTime);
        parryCollider.enabled = false;
    }

    #endregion
}
