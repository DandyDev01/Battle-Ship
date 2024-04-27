using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnActorSceneSegmentAction : SceneSegmentAction
{
	[SerializeField] private Transform spawnTransform = null;
	[SerializeField] private GameObject actorToSpawnPrefab = null;

	public override void Execute()
	{
		IsCompleted = false;
		Instantiate(actorToSpawnPrefab, spawnTransform.position, Quaternion.identity);
		IsCompleted = true;
	}
}
