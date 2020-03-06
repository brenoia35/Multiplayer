using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

namespace Survivor.Sistema {
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PhotonView))]
    public class PlayerMoviment : MonoBehaviour
    {
        [Header("Configuration Player")]
        [SerializeField]
        private float SpeedMove = 3, speedRun = 5, gravity = -12;
        [SerializeField]
        private float SmoothRotation = 3.5f;
        public CharacterController CharacterSc;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private GameObject Camera;
        [SerializeField]
        private Transform EyesPlayer;
        public float speedSmoothTime = 0.1f;

        [Space(20), Header("Anim Ik")]
        public AnimationIk Ik;
        float speedSmoothVelocity;

        //
        PhotonView photonV;
        Camera cam;
        private Vector3 moveDirection = Vector3.zero;
        Vector3 rotationToPos = Vector3.zero;

        float currentSpeed, velocityY;
        public Vector2 inputDir { get; private set; }

        private void Start()
        {
            CharacterSc = GetComponent<CharacterController>();
            photonV = GetComponent<PhotonView>();
            anim = GetComponent<Animator>();
            if (!photonV.IsMine)
            {
                this.enabled = false;
                return;
            }
            else {
                GameObject ob = PhotonNetwork.Instantiate(Camera.name, Camera.transform.position, Camera.transform.rotation, 0);
                ob.GetComponent<TopDownCamera>().target = this.gameObject.transform;
                cam = ob.GetComponent<Camera>();
            }
        }

        private void Update()
        {
            if (!photonV.IsMine) return;
            CheckMouse();
            Inputs();
        }

        void Inputs() {
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            inputDir = input.normalized;
            bool running = Input.GetKey(KeyCode.LeftShift);
            //if (Pb.isDead) return;
            Move(inputDir, running);
            float animationSpeedPercent = ((running) ? currentSpeed / speedRun : currentSpeed / SpeedMove * .5f);
            if (inputDir.y > 0)
            {
                anim.SetFloat("Speed", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
            }
            else {
                anim.SetFloat("Speed", -animationSpeedPercent, speedSmoothTime, Time.deltaTime);
            }
        }

        void Move(Vector2 inputDir, bool running)
        {

            float targetSpeed = ((running) ? speedRun : SpeedMove) * inputDir.magnitude;
            currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

            velocityY += Time.deltaTime * gravity;
            Vector3 velocity;
            if (inputDir.y > 0)
            {
                velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
            }
            else {
                velocity = (-transform.forward) * currentSpeed + Vector3.up * velocityY;
            }
            CharacterSc.Move(velocity * Time.deltaTime);
            currentSpeed = new Vector2(CharacterSc.velocity.x, CharacterSc.velocity.z).magnitude;

            if (CharacterSc.isGrounded)
            {
                velocityY = 0;
            }
        }

        float GetModifiedSmoothTime(float smoothTime)
        {
            if (CharacterSc.isGrounded)
            {
                return smoothTime;
            }
            return smoothTime / 2;
        }

        void CheckMouse() {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)){
                rotationToPos = hit.point;
                rotationToPos.y = transform.position.y;
            }
            if (rotationToPos != Vector3.zero) {
                transform.rotation = Quaternion.Slerp(transform.rotation, 
                                                        Quaternion.LookRotation(rotationToPos - transform.position), 
                                                        SmoothRotation * Time.deltaTime);
                Ik.Look(hit.point);
            }
        }
    }
}
