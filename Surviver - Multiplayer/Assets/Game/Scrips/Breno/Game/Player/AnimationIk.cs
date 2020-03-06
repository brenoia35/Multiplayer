using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

namespace Survivor.Sistema {
    public class AnimationIk : MonoBehaviour
    {
        public Animator Anim;
        public Transform RightArm, LefthArm;
        public Transform Weapon;

        public float IkWeight { get; private set; }

        Vector3 pos;

        private void Start()
        {
            SetIkValue(1, 1);
        }

        void SetIkValue(int Layer, float Weight) {
            IkWeight = Weight;
            OnAnimatorIK(Layer);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            if (!GetComponent<PhotonView>().IsMine) return;
            Anim.SetIKPositionWeight(AvatarIKGoal.RightHand, IkWeight);
            Anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, IkWeight);
            Anim.SetIKPosition(AvatarIKGoal.RightHand, RightArm.position);
            Anim.SetIKPosition(AvatarIKGoal.LeftHand, LefthArm.position);
            Anim.SetLookAtWeight(IkWeight);
            Anim.SetLookAtPosition(pos);
        }

        public void Look(Vector3 Position) {
            if (!Weapon) return;
            pos = Position;
            Weapon.LookAt(Position);
        }
    }
}