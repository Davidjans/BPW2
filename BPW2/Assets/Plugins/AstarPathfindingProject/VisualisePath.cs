using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace Pathfinding
{
	public class VisualisePath : MonoBehaviour
	{
		public LayerMask mask;

		private LineRenderer renderer;

		private AIPath ai;

		public bool permitMove = false;

		private Vector3 LastPos = Vector3.zero;

		private Seeker m_Seeker;

		Camera cam;

		// Start is called before the first frame update
		void Start()
		{
			m_Seeker = GetComponent<Seeker>();
			cam = Camera.main;
			SetNewPath();
			renderer = GetComponent<LineRenderer>();
			//ai = GetComponent<AIPath>();
		}

		// Update is called once per frame
		void Update()
		{
			//if (permitMove)
			//{
			//	ai.maxSpeed = 4;
			//	if (ai.remainingDistance < ai.endReachedDistance)
			//	{
			//		permitMove = false;
			//	}
			//}
			//else
			//{
			//	ai.maxSpeed = 0;
			//}

			//if (Input.GetKeyDown(KeyCode.A))
			//	permitMove = !permitMove;
		}

		private void SetNewPath()
		{
			CancelInvoke("SetNewPath");
			Invoke("SetNewPath", 1f);
			RaycastHit hit;
			
			Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask);
			m_Seeker.StartPath(transform.position, hit.point, OnPathComplete);
		}

		public void OnPathComplete(Path p)
		{
			if (p.error)
			{
				Invoke("SetNewPath", 0.1f);
			}
			else
			{
				Path path = m_Seeker.GetCurrentPath();
				var buffer = new List<Vector3>();
				buffer = path.vectorPath;
				renderer.positionCount = buffer.Count;
				renderer.SetPositions(buffer.ToArray());
				Invoke("SetNewPath",0.1f);
			}
		}
	}
}
