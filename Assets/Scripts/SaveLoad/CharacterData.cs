using System;
using System.Collections.Generic;

[Serializable]
public class CharacterData
{
	public String stageProgress;	// latest stage available
	public Dictionary<String, int> scoreRecord;
	public CheckPoint checkpoint;
}

[Serializable]
public struct CheckPoint
{
	// Position
	public String stage;
	public int checkPointId;

	// Record
	public float time;
	public List<ItemColor> keys;
	public Dictionary<ItemColor, int> jewellarys;
}

/*
Save Data
- Level select scene
	1. level progress e,g. 1-1, 1-2, 3-1
	-> (episode, stage)
	2. score record of each level
	-> Dictionary(level, score)
- Start game menu scene & each level scene
	3. CheckPoint 
	-> (level, checkPointId)
 */