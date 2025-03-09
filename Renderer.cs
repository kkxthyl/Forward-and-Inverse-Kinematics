using UnityEngine;

private void UpdateSkeletonPosition()
    {
        // Update each JointSphere's position
        for (int i = 0; i < Actor.Joints.Count; i++){
            JointSpheres[i].position = Actor.Joints[i].GlobalPosition;
        }

        // Update each bone's position and rotation
        for (int i = 1; i < Bones.Count; i++) { 
            Joint currentJoint = Actor.Joints[i];
            Joint parentJoint = currentJoint.GetParent();
            Vector3 parentPos = parentJoint.GlobalPosition;
            Vector3 currentPos = currentJoint.GlobalPosition;
            Vector3 bone = currentPos - parentPos;

            
            Bones[i].position =   ((currentPos - parentPos)/2) + parentPos;
            if(bone != Vector3.zero) {
                Bones[i].rotation = Quaternion.FromToRotation(Vector3.up, bone);
            }
        }
        
        ApplyActorScale();
    }