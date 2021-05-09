using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

namespace BehaviorDesigner.Runtime.Tasks.Opera
{
    [TaskCategory("Opera")]
    [TaskIcon("Assets/Behavior Designer Tutorials/Tasks/Editor/{SkinColor}SeekIcon.png")]
    public class GetSceneBool : Action
    {
        [Tooltip("SceneVariables")]
        public SharedGameObject sceneVariables;
        [Tooltip("VariableName")]
        public SharedString variableName;
        [RequiredField]
        public SharedBool storeResult;
        SceneVariables sv;
        public override void OnStart()
        {
            sv = sceneVariables.Value.GetComponent<SceneVariables>();
            storeResult = Variables.Object(sv).Get<bool>(variableName.Value);
        }
    }
}