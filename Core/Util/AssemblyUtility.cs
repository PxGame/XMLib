using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class AssemblyUtility
{
    public static List<Type> FindAllSubclass<T>()
    {
        List<Type> results = new List<Type>();

        Assembly[] assemblys = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblys)
        {
            Type[] types = assembly.GetTypes();
            foreach (var type in types)
            {
                if (!type.IsAbstract && type.IsSubclassOf(typeof(T)))
                {
                    results.Add(type);
                }
            }
        }

        return results;
    }
}