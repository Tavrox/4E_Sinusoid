using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Environment : MonoBehaviour {

	public enum types
	{
		stone,
		aura,
		electric,
		touchy,
		wood,
		fragile,
		remote,
		mobile,
		reverb,
		floorboard,
		stalactite,
	};
	public types typeList;
	public Dictionary<string, types> dicoEnv = new Dictionary<string, types>();
	public string typeImport;

	public void Start()
	{
		dicoEnv.Add("stone", types.stone);
		dicoEnv.Add("aura", types.aura);
		dicoEnv.Add("electric", types.electric);
		dicoEnv.Add("touchy", types.touchy);
		dicoEnv.Add("wood", types.wood);
		dicoEnv.Add("fragile", types.fragile);
		dicoEnv.Add("remote", types.remote);
		dicoEnv.Add("mobile", types.mobile);
		dicoEnv.Add("reverb", types.reverb);
		dicoEnv.Add("floorboard", types.floorboard);
		dicoEnv.Add("stalactite", types.stalactite);
		
		if (typeImport == "" || typeImport == null)
		{
			typeImport = "stone";
		}
		typeImport = typeImport.ToLower();

		adaptCollider();

	}

	public void adaptCollider()
	{
		foreach (KeyValuePair<string, types> _obj in dicoEnv)
		{
			if(typeImport == _obj.Key)
			{
				typeList = _obj.Value;
			}
		}
	}
}
