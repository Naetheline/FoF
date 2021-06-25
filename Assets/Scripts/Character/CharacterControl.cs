using System;
using UnityEngine;



    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

    public class CharacterControl : MonoBehaviour
    {
        public UnityEngine.AI.NavMeshAgent agent { get; private set; }             // the navmesh agent required for the path finding
       
        public Vector3 targetPosition;   // target to aim for

    [SerializeField]
    private Vector3 offsetRotation;

    private Animator anim;

    private float m_ForwardAmount;

        private void Start()
        {
            // get the components on the object we need ( should not be null due to require component so no need to check )
            agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
            anim = GetComponentInChildren<Animator>();

            agent.updateRotation = true;
            agent.updatePosition = true;
        }


        private void Update()
        {
            if (targetPosition != null)
                agent.SetDestination(targetPosition);
        Move(agent.desiredVelocity);
           
                
        }
    private void LateUpdate()
    {
        this.transform.Rotate(offsetRotation);
    }

    void Move(Vector3 move)
    {

        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);

        // move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        // m_TurnAmount = Mathf.Atan2(move.x, move.z);

        m_ForwardAmount = move.z;


        // send input and other state parameters to the animator
        UpdateAnimator(move);
    }

    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        anim.SetFloat("Speed", m_ForwardAmount, 0.1f, Time.deltaTime);
        //m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        //m_Animator.SetBool("Crouch", m_Crouching);
        //m_Animator.SetBool("OnGround", m_IsGrounded);

        // don't use that while airborne
        anim.speed = 1;
        
    }


    public void SetTarget(Vector3 target)
        {
        this.agent.ResetPath();
            this.targetPosition = target;
        }

    public void AttackAnim()
    {
        anim.SetTrigger("Attack");
    }





    }

