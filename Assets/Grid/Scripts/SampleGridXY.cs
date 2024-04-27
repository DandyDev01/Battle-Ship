using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
	public enum cellType { Empty = 0, Ship, Pin };

	public class SampleGridXY : MonoBehaviour
	{
		[SerializeField] private Vector3 oragin = Vector3.zero;
		[SerializeField] private int columns = 1;
		[SerializeField] private int rows = 1;
		[SerializeField] private float cellSize = 1;

		public GridXY<cellType> Grid { get; private set; }

		public int Columns { get { return columns; } }
		public int Rows { get { return rows; } }
		public float CellSize { get { return cellSize; } }
		public Vector3 Oragin { get { return oragin; } }	

		public void Awake()
		{
			Grid = new GridXY<cellType>(oragin, columns, Rows, CellSize);
		}
	}
}
