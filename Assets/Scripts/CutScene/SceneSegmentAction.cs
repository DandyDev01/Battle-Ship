using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneSegmentAction : MonoBehaviour
{
	public bool IsCompleted { get; protected set; }

	public abstract void Execute();
}
