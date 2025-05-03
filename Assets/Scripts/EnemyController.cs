using UnityEngine;
using Cinemachine;
using System;

public class EnemyController : MonoBehaviour
{
    public enum EnemyState { Traverse, AtkCastle, Chase, AtkPlayer };

    public EnemyState currentState;

    public EnemySO stats;
    public CinemachineDollyCart cart;

    public bool canChase;
    public bool isAttacking;

    float cooldown = 0; 

    public float atkRate = 0;
    public float atkPower =0;
    public int maxHp = 0;
    public int curHp = 0;
    [SerializeField] GameObject poof;

    void OnAwake()
    {
        setPath(GameManager.Instance.pathManager.path);
        Debug.Log(GameManager.Instance.pathManager.path);
    }

    void Start()
    {
        Debug.Log(GameManager.Instance.pathManager.path);
        GetComponent<Animator2D>().animations[0] = stats.anim1;
        GetComponent<Animator2D>().animations[1] = stats.anim2;
        setMoveSpeed(5);
        setHp(1);
        setAtk(1);
        setAtkRate();

        curHp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking)
        {
            BeginAtk();
        }
        GetComponent<Animator2D>().SetAnimation(isAttacking?1:0);
    }

    public void ChangeState(EnemyState state)
    {
        currentState = state;    
        Debug.Log("Game State changed to: " + currentState);
        CheckState();

    }

    private void CheckState()
    {
        switch (currentState)
        {
            case EnemyState.Traverse:
                HandleTraverse();
                break;
            case EnemyState.AtkCastle:
                HandleAtkCastle();
                break;
            case EnemyState.Chase:
                HandleChase();
                break;
            case EnemyState.AtkPlayer:
                HandleAtkPlayer();
                break;
        }
    }

    private void HandleTraverse()
    {
        cart.enabled = true;
    }

    private void HandleAtkCastle()
    {
        isAttacking = true;
    }

    private void HandleChase()
    {
        throw new NotImplementedException();
    }

    private void HandleAtkPlayer()
    {
        throw new NotImplementedException();
    }

    void setMoveSpeed(float buffVal)
    {
        cart.m_Speed = buffVal * stats.speed;

    }

    void setAtk(float buffVal)
    {
        atkPower = buffVal * stats.atk;

    }

    void setHp(float buffVal)
    {

        maxHp = Mathf.RoundToInt(buffVal * stats.HP);

    }

    void setAtkRate()
    {
        atkRate = stats.speed;
    }

    void doAbility(Ability ability)
    {
        ability.Activate();
    }

    void Attack(float damage)
    {
        if (currentState == EnemyState.AtkCastle)
        {
            GameManager.Instance.CastleTakeDamage(damage);
            return;
        }
        if(currentState == EnemyState.AtkPlayer)
        {
            GameManager.Instance.PlayerTakeDamage(damage);
            return;
        }
    }

    void BeginAtk()
    {
        cooldown += Time.deltaTime;
        if (cooldown >= atkRate)
        {
            cooldown = 0f; //Reset cooldown
            Attack(atkPower);
        }
    }

        void setPath(CinemachinePath path)
    {
        cart.m_Path = path;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Castle"))
        {
            Debug.Log("Attacking"); 
            ChangeState(EnemyState.AtkCastle);
            isAttacking = true;

        }
        else
        {
            Debug.Log("Colliding with: " + other.gameObject.tag);
        }
    }


    public void TakeDamage(int damage)
    {
        curHp -= damage;
        if(curHp <= 0)
        {
            GameManager.Instance.enemyCount--;
            Destroy(gameObject);
        }
        Instantiate(poof, transform.position, transform.rotation);
    }
}