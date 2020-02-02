using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour, new()
{
	private static T instance = null;

	internal Singleton() { }

	public static T Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();
				if (instance == null)
				{
					GameObject obj = new GameObject();
					obj.name = typeof(T).Name;
					obj.hideFlags = HideFlags.DontSave;
					instance = obj.AddComponent<T>();
				}
			}
			return instance;
		}
	}
}
