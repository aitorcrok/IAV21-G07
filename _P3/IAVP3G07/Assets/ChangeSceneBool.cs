using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;
namespace BehaviorDesigner.Runtime.Tasks.Opera
{
    [TaskCategory("Opera")]
    [TaskIcon("Assets/Behavior Designer Tutorials/Tasks/Editor/{SkinColor}SeekIcon.png")]
    public class ChangeSceneBool : Action
    {
        [Tooltip("SceneVariables")]
        public SharedGameObject sceneVariables;
        SceneVariables sV;
        [Tooltip("VariableName")]
        public SharedString variableName;
        [Tooltip("Value")]
        public SharedBool value;
        public override void OnStart()
        {
            sV = sceneVariables.Value.GetComponent<SceneVariables>();
            Variables.Object(sV).Set(variableName.Value, value);
        }
    }
}