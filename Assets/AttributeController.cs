using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class AttributeController : MonoBehaviour
{
	[Size(10, 10, 10)]
	public Transform trans;

	[Size(5, 3)]
	public RectTransform rect;

	private void Start()
	{
		BindingFlags bind = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		MonoBehaviour[] monoBehaviours = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);

		foreach (MonoBehaviour monoBehaviour in monoBehaviours)
		{
			Type type = monoBehaviour.GetType();

			IEnumerable<FieldInfo> sizeAttachedFields = type.GetFields(bind).Where(x => x.HasAttribute<SizeAttribute>());

			foreach (FieldInfo fieldInfo in sizeAttachedFields)
			{
				SizeAttribute att = fieldInfo.GetCustomAttribute<SizeAttribute>();
				object value = fieldInfo.GetValue(monoBehaviour);

				if (value is RectTransform rect)
				{
					rect.sizeDelta = att.scale;
				}
				else if (value is Transform trans)
				{
					trans.localScale = att.scale;
				}
				else
				{
					Debug.LogError("½ÇÆÐ");
				}
			}
		}
	}
}

[AttributeUsage(AttributeTargets.Field, Inherited = true)]
public class SizeAttribute : Attribute
{
	public Vector3 scale;

	public SizeAttribute(float x = 1f, float y = 1f, float z = 0f)
	{
		scale = new Vector3(x, y, z);
	}
}
