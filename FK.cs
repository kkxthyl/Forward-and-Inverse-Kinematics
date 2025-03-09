using UnityEngine;

public static class ForwardKinematics
{
    public static void UpdateJointPositions(Actor actor, float[] frameData)
    {
        Joint root = actor.GetRootJoint();
        root.GlobalPosition = new Vector3(frameData[0], frameData[1], frameData[2]);
        root.LocalPosition = Vector3.zero;
        root.LocalQuaternion = eulerToQuat(new Vector3(frameData[3], frameData[4], frameData[5]), root.RotateOrder);
        root.GlobalQuaternion = root.LocalQuaternion;

        int index =6;
        for (int i = 1; i < actor.Joints.Count; i++){
            Joint joint = actor.Joints[i];
            if (joint.Name != "Site"){
                joint.LocalQuaternion = eulerToQuat(new Vector3(frameData[index], frameData[index+1], frameData[index+2]), joint.RotateOrder);

                if (joint.RotateOrder == Joint.RotationOrder.XYZ  || joint.RotateOrder == Joint.RotationOrder.XZY || joint.RotateOrder == Joint.RotationOrder.YXZ || joint.RotateOrder == Joint.RotationOrder.YZX || joint.RotateOrder == Joint.RotationOrder.ZXY || joint.RotateOrder == Joint.RotationOrder.ZYX){ 
                    index += 3;
                }
            }

            Joint parentJoint = joint.GetParent(); 
            joint.GlobalQuaternion = parentJoint.GlobalQuaternion * joint.LocalQuaternion;
            joint.GlobalPosition = parentJoint.GlobalPosition + parentJoint.GlobalQuaternion * joint.LocalPosition;

        }
        
    }
    private static Quaternion eulerToQuat (Vector3 euler, Joint.RotationOrder order){
    
        switch (order){
            case Joint.RotationOrder.XYZ:
                Quaternion x = Quaternion.AngleAxis(euler.x, Vector3.right);
                Quaternion y = Quaternion.AngleAxis(euler.y, Vector3.up);
                Quaternion z = Quaternion.AngleAxis(euler.z, Vector3.forward);

                return z * y * x;
            case Joint.RotationOrder.XZY:
                 x = Quaternion.AngleAxis(euler.x, Vector3.right);
                 z = Quaternion.AngleAxis(euler.z, Vector3.up);
                 y = Quaternion.AngleAxis(euler.y, Vector3.forward);

                return y * z * x;
            case Joint.RotationOrder.YXZ:
                 y = Quaternion.AngleAxis(euler.y, Vector3.right);
                 x = Quaternion.AngleAxis(euler.x, Vector3.up);
                 z = Quaternion.AngleAxis(euler.z, Vector3.forward);

                return z * x * y;
            case Joint.RotationOrder.YZX:
                 y = Quaternion.AngleAxis(euler.y, Vector3.right);
                 z = Quaternion.AngleAxis(euler.z, Vector3.up);
                 x = Quaternion.AngleAxis(euler.x, Vector3.forward);

                return x * z * y;
            case Joint.RotationOrder.ZXY:
                 z = Quaternion.AngleAxis(euler.z, Vector3.right);
                 x = Quaternion.AngleAxis(euler.x, Vector3.up);
                 y = Quaternion.AngleAxis(euler.y, Vector3.forward);

                return y * x * z;
            case Joint.RotationOrder.ZYX:
                 z = Quaternion.AngleAxis(euler.z, Vector3.right);
                 y = Quaternion.AngleAxis(euler.y, Vector3.up);
                 x = Quaternion.AngleAxis(euler.x, Vector3.forward);

                return x * y * z;
            default:
                return Quaternion.identity;
        }
    }



}
