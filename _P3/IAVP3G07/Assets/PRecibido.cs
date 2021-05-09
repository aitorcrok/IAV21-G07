using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Opera
{
    [TaskCategory("Opera")]
    [TaskIcon("Assets/Behavior Designer Tutorials/Tasks/Editor/{SkinColor}SeekIcon.png")]
    public class ComparePosition : Conditional
    {
        [Tooltip("The first Transform")]
        public SharedTransform firstVector3;
        [Tooltip("The second Transform")]
        public SharedTransform secondVector3;
        [Tooltip("The distance")]
        [RequiredField]
        public SharedFloat storeResult;

        public override TaskStatus OnUpdate()
        {
            storeResult.Value = Vector3.Distance(firstVector3.Value.position, secondVector3.Value.position);
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            firstVector3 = null;
            secondVector3 = null;
            storeResult = 0;
        }
    }
}