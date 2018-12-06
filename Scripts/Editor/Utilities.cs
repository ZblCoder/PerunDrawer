using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.AnimatedValues;
using UnityEngine;

namespace PerunDrawer
{
	public static class Utilities
	{
		
		public static List<Attribute> GetAttrib(Type classType, string fieldName)
		{
			FieldInfo info = classType.GetField(fieldName);
			if (info != null)
				return info.GetCustomAttributes(false).Cast<Attribute>().ToList();
			return null;
		}
	}
}
