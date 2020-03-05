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
        private float SpeedMove = 3, speedRun = 5;
        [SerializeField]
        private float SmoothRotation = 3.5f;
        public CharacterController CharacterSc;
        [SerializeField]
        private Animator anim;
        [SerializeField]
        private GameObject Camera;
        [SerializeField]
        private Transform EyesPlayer;

        public Transform test;

        [Header("Configuration Rotation Mesh")]
        [SerializeField]
        private Transform TorcoMesh;

        //
        PhotonView photonV;
        Camera cam;
        private Vector3 moveDirection = Vector3.zero;
        Vector3 rotationToPos = Vector3.zero;

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
            CheckMoviment();
        }

        void CheckMoviment() {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");
            float tempAngle = Mathf.Atan2(v, h);
            h *= Mathf.Abs(Mathf.Cos(tempAngle));
            v *= Mathf.Abs(Mathf.Sin(tempAngle));
            moveDirection = new Vector3(h, 0, v);
            moveDirection = transform.TransformDirection(moveDirection);
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveDirection *= speedRun;
                if (v > 0)
                {
                    anim.SetFloat("Speed", 1);
                }
                else {
                    anim.SetFloat("Speed", -1);
                }
            }
            else {
                moveDirection *= SpeedMove;
                if (v > 0)
                {
                    anim.SetFloat("Speed", 0.5f);
                }
                else {
                    anim.SetFloat("Speed", -0.5f);
                }
            }

            if (moveDirection != Vector3.zero)
            {
                CharacterSc.Move(moveDirection * Time.deltaTime);
            }
            else {
                anim.SetFloat("Speed", 0);
            }
            
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
            }
        }

        //private void OnAnimatorIK(int layerIndex)
        //{
        //    if (anim)
        //    {
        //        //anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);

        //        //
        //        //anim.SetIKPosition(AvatarIKGoal.RightHand, test.position);
        //        Transform t = anim.GetBoneTransform(HumanBodyBones.Spine);
        //        t.LookAt(EyesPlayer);
        //    }
        //}
    }
}
