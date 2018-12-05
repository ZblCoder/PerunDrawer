using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace PerunDrawer
{
	public class Utilities : MonoBehaviour
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
