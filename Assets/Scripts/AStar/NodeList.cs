using UnityEngine;
using System.Collections;

public class NodeList
{
	private ArrayList list = new ArrayList ();

	public Node GetFirstNode ()
	{
		return (Node) list [0];
	}

	public bool Contain (Node node)
	{
		return list.Contains (node);
	}

	public void Push (Node node)
	{
		list.Add (node);
		list.Sort ();
	}

	public void Pop (Node node)
	{
		list.Remove (node);
		list.Sort ();
	}

}