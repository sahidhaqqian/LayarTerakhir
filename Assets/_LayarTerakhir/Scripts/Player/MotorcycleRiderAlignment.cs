using System.Linq;
using UnityEngine;

namespace LayarTerakhir.Player
{
    // The riding clip supplies the posture; analytic limb alignment fits the actual scooter.
    public sealed class MotorcycleRiderAlignment : MonoBehaviour
    {
        private Transform hips;private Transform[] bones;
        private Transform motor,seat;private Transform[] grips,feet;
        private Transform Bone(string name)=>bones.FirstOrDefault(b=>b.name==name);
        public void Fit(Transform vehicle)
        {
            if(!vehicle)return;
            if(bones==null){bones=GetComponentsInChildren<Transform>(true);hips=Bone("Hips");}
            if(!hips)return;
            if(motor!=vehicle)
            {
                motor=vehicle;var contacts=vehicle.GetComponentsInChildren<Transform>(true).Where(t=>!t.IsChildOf(transform)).ToArray();
                seat=contacts.FirstOrDefault(t=>t.name=="RiderSeatSurface");
                grips=contacts.Where(t=>t.name.StartsWith("RiderGrip")).ToArray();
                feet=contacts.Where(t=>t.name.StartsWith("RiderFoot")).ToArray();
            }
            if(!seat||grips.Length<2)return;
            var seatPoint=seat.position+Vector3.up*.075f;
            transform.position+=seatPoint-hips.position;
            foreach(string side in new[]{"L","R"})
            {
                var hand=Bone("Hand_"+side);var foot=Bone("Foot_"+side);if(!hand||!foot)continue;
                var grip=grips.OrderBy(g=>Vector3.Distance(g.position,hand.position)).First();
                float sign=Mathf.Sign(Vector3.Dot(grip.position-motor.position,motor.right));
                Solve(Bone("UpperArm_"+side),Bone("Forearm_"+side),hand,grip.position-motor.forward*.055f-Vector3.up*.01f,
                    motor.position+motor.right*sign*.48f+motor.forward*.08f+Vector3.up*.65f);
                var board=feet.OrderBy(f=>Vector3.Distance(f.position,foot.position)).FirstOrDefault();
                if(board)Solve(Bone("Thigh_"+side),Bone("Shin_"+side),foot,board.position+Vector3.up*.085f,
                    motor.position+motor.right*sign*.24f+motor.forward*.32f+Vector3.up*.5f);
            }
        }
        private static void Solve(Transform root,Transform middle,Transform end,Vector3 target,Vector3 pole)
        {
            if(!root||!middle||!end)return;
            var endRotation=end.rotation;
            float upper=Vector3.Distance(root.position,middle.position),lower=Vector3.Distance(middle.position,end.position);
            Vector3 delta=target-root.position;float distance=Mathf.Clamp(delta.magnitude,.02f,upper+lower-.001f);var axis=delta.normalized;
            float projection=(distance*distance+upper*upper-lower*lower)/(2*distance);
            Vector3 normal=Vector3.ProjectOnPlane(pole-root.position,axis).normalized;
            var joint=root.position+axis*projection+normal*Mathf.Sqrt(Mathf.Max(0,upper*upper-projection*projection));
            root.rotation=Quaternion.FromToRotation(middle.position-root.position,joint-root.position)*root.rotation;
            middle.rotation=Quaternion.FromToRotation(end.position-middle.position,target-middle.position)*middle.rotation;
            end.rotation=endRotation;
        }
    }
}
