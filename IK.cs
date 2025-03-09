using System.Collections.Generic;
using UnityEngine;

public static class InverseKinematics
{
    public static void ApplyIK(Actor actor, Vector3 targetPosition)
    {
        float threshold = 5.0f;
        int iterations = 10; 
        Joint currentJoint = actor.GetIKEndEffector().GetParent();
        Joint endEffector = actor.GetIKEndEffector();

        for (int i = 0; i < iterations; i++){
            while (currentJoint.GetParent() != null){
                endEffector = actor.GetIKEndEffector();
                Vector3 targetVector = (currentJoint.GlobalPosition - targetPosition);
                Vector3 endEffectorVector = (  currentJoint.GlobalPosition - endEffector.GlobalPosition);
                Debug.Log("error: " +     Vector3.Distance(endEffector.GlobalPosition, targetPosition));

                if (Vector3.Distance(endEffector.GlobalPosition, targetPosition) < threshold){
                    break;
                }

                if (float.IsNaN(endEffectorVector.x) || float.IsNaN(endEffectorVector.y) || float.IsNaN(endEffectorVector.z) || float.IsNaN(targetVector.x) || float.IsNaN(targetVector.y) || float.IsNaN(targetVector.z)){
                    break;
                }
                Debug.Log("end effector vector: " + endEffectorVector);
                Debug.Log("target vector: " + targetVector);

                Quaternion rotation = Quaternion.FromToRotation( endEffectorVector, targetVector);

                if (currentJoint.GetParent() != null){
                    Quaternion localQuatPrev = currentJoint.LocalQuaternion;
                    Quaternion globalQuatNew = rotation * currentJoint.GetParent().GlobalQuaternion * localQuatPrev;
                    Quaternion localQuatNew = Quaternion.Inverse(currentJoint.GetParent().GlobalQuaternion) * globalQuatNew;
                    currentJoint.LocalQuaternion = localQuatNew;
                }
                
                currentJoint.GlobalPosition =  currentJoint.GetParent().GlobalPosition + currentJoint.GetParent().GlobalQuaternion * currentJoint.LocalPosition;

                foreach (Joint child in actor.GetRootJoint().GetChildren()){
                    UpdateJointPositions(actor, child);
                }
            
                currentJoint = currentJoint.GetParent();
            }
        
        }
        
    }

    public static void UpdateJointPositions(Actor actor, Joint joint)
    {
        if (joint == null) {
            return;
        }

        if (joint.GetParent() != null){
            Joint parentJoint = joint.GetParent();
            joint.GlobalQuaternion = parentJoint.GlobalQuaternion * joint.LocalQuaternion;
            joint.GlobalPosition = parentJoint.GlobalPosition + parentJoint.GlobalQuaternion * joint.LocalPosition;
        }
        else {
            joint.GlobalQuaternion = joint.LocalQuaternion;
        }
        foreach (Joint child in joint.GetChildren())
        {
            UpdateJointPositions(actor, child);
        }
        return;
    }

}
