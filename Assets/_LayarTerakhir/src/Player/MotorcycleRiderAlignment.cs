using System.Linq;
using UnityEngine;

namespace LayarTerakhir.Player
{
    // The riding clip supplies the posture; analytic limb alignment fits the actual scooter.
    public sealed class MotorcycleRiderAlignment : MonoBehaviour
    {
        private Transform hips,spine;private Transform[] bones;
        private Transform motor,seat;private Transform[] grips,feet;
        private Transform Bone(string name)=>bones.FirstOrDefault(b=>b.name==name);
        public void Fit(Transform vehicle)
        {
            if(!vehicle)return;
            if(bones==null){bones=GetComponentsInChildren<Transform>(true);hips=Bone("Hips");spine=Bone("Spine");}
            if(!hips)return;
            // Sample the current legacy blend before fitting, so capture/pause
            // frames cannot accumulate an extra torso bend on the previous fit.
            var animation=GetComponent<Animation>();if(animation)animation.Sample();
            var head=Bone("Head");var authoredHead=head?head.rotation:Quaternion.identity;
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
            if(spine)
            {
                var authored=spine.rotation;
                for(float lean=0;lean<=28;lean+=2)
                {
                    spine.rotation=Quaternion.AngleAxis(lean,motor.right)*authored;
                    bool reachable=true;
                    foreach(string side in new[]{"L","R"})
                    {
                        var upper=Bone("UpperArm_"+side);var elbow=Bone("Forearm_"+side);var wrist=Bone("Hand_"+side);
                        if(!upper||!elbow||!wrist)continue;
                        var grip=grips.OrderBy(g=>Vector3.Distance(g.position,wrist.position)).First();
                        var target=grip.position-motor.forward*.055f-Vector3.up*.01f;
                        float reach=Vector3.Distance(upper.position,elbow.position)+Vector3.Distance(elbow.position,wrist.position);
                        if(Vector3.Distance(upper.position,target)>reach-.015f)reachable=false;
                    }
                    if(reachable)break;
                }
            }
            // Fitting the torso to handlebars must retain the clip's head
            // direction; otherwise the extra lean points the face at the floor.
            if(head)head.rotation=authoredHead;
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
        internal static void Solve(Transform root,Transform middle,Transform end,Vector3 target,Vector3 pole)
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
