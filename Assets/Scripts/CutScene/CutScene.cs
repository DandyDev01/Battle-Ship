using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CutScene : MonoBehaviour
{
	[SerializeField] new private string name = string.Empty;
	[SerializeField] private string description = string.Empty;
	[SerializeField] private string id = string.Empty;
	[SerializeField] private SceneSegment[] segments = null;

	public string Name { get { return name; } }
	public string Description { get { return description; } }
	public string Id { get { return id; } }
	public bool IsCompleted { get; private set; }
	public bool IsRunning { get; private set; }

	public Action OnComplete;

	public SceneSegment GetSegment(int index)
	{
		if (index < 0 || index >= segments.Length)
			throw new System.Exception("invalid index");

		return segments[index];
	}

	public void Play()
	{
		IsCompleted = false;
		IsRunning = true;

		StartCoroutine(PlayCoroutine());
	}

	private IEnumerator PlayCoroutine()
	{
		foreach (SceneSegment segment in segments)
		{
			segment.Run();
			yield return new WaitForSceneSegment(segment);
		}

		IsCompleted = true;
		IsRunning = false;
	}
}
