using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SceneSegment : MonoBehaviour
{
    [SerializeField] private SceneSegmentAction[] actions = null;

    public bool IsCompleted { get; private set; }
    public bool IsRunning { get; private set; }

    public void Run()
	{
		IsRunning = true;
		IsCompleted = false;

		StartCoroutine(RunCoroutine());
	}

    private IEnumerator RunCoroutine()
	{
		foreach (SceneSegmentAction action in actions)
		{
			action.Execute();
			yield return new WaitForSegmentAction(action);
		}

		IsRunning = false;
		IsCompleted = true;
	}
}
