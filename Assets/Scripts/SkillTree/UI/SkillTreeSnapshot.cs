using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeSnapshot : MonoBehaviour
{
	private SaveSkillTree _snapshot;
	private SkillMenu _menu;

	void Awake()
	{
		_menu = GetComponent<SkillMenu>();
		//LoadSnapshotFromDataBase();
	}

	public void SaveSnapshot()
	{
		_snapshot = _menu._skillTree.GetSnapshot();
		SaveSnapshotToDataBase();
	}

	public void LoadSnapshot()
	{
		if (_snapshot != null)
		{
			_menu._skillTree.LoadSnapshot(_snapshot);
			SkillCategory[] categories = _menu._skillTree.GetCategories();
			_menu.ShowCategory(categories[0]);
		}
	}


	/* SAVE SYSTEM TEMPORAIRE */

	public void SaveSnapshotToDataBase()
	{
		// NOT IMPLEMENTED YET
	}

	public void LoadSnapshotFromDataBase()
	{
		// NOT IMPLEMENTED YET
		LoadSnapshot();
	}
	/*
	protected override T Deserialize<T>(string dataToDeserialize)
	{
		T savedObject;
		try
		{
			savedObject = JsonConvert.DeserializeObject<T>(dataToDeserialize);
		}
		catch (Exception exception)
		{
			throw new SavedDataLoadException($"The {_saveName} save file isn't of type {typeof(T)}.\n{exception.ToString()}");
		}
		return savedObject;
	}

	protected override string Serialize<T>(T dataToSerialize)
	{
		return JsonConvert.SerializeObject(dataToSerialize);
	}
	*/
	/* SAVE SYSTEM TEMPORAIRE */
}