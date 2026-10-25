using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class retinaProDataRuntime
{
	public static string atlasFolder = "UI/Atlases/";

	public bool loaded;

	public List<retinaProRuntimeDevice> deviceList;

	public List<retinaProRuntimeAtlas> atlasList;

	public retinaProDataRuntime()
	{
		deviceList = new List<retinaProRuntimeDevice>();
		atlasList = new List<retinaProRuntimeAtlas>();
		loaded = loadSettings();
	}

	~retinaProDataRuntime()
	{
	}

	public bool loadSettings()
	{
		lock (retinaProFileLock.fileLock)
		{
			TextAsset textAsset = Resources.Load("RetinaProData", typeof(TextAsset)) as TextAsset;
			if (textAsset == null)
			{
				return false;
			}
			if (textAsset.bytes == null)
			{
				return false;
			}
			if (textAsset.bytes.Length <= 0)
			{
				return false;
			}
			XMLParser xMLParser = new XMLParser();
			XMLNode nodes = xMLParser.Parse(textAsset.text);
			if (nodes == null)
			{
				Debug.LogWarning("Could not parse retinaProData.txt");
				return false;
			}
			if (!loadDevices(ref nodes))
			{
				Debug.LogWarning("Could not parse devices");
				return false;
			}
			if (!loadAtlases(ref nodes))
			{
				Debug.LogWarning("Could not parse atlases");
				return false;
			}
		}
		return true;
	}

	private bool loadDevices(ref XMLNode nodes)
	{
		XMLNodeList nodeList = nodes.GetNodeList("retinaProData>0>ArrayOfRetinaProDevice>0>retinaProDevice");
		if (nodeList == null)
		{
			return false;
		}
		foreach (XMLNode item in nodeList)
		{
			retinaProRuntimeDevice retinaProRuntimeDevice2 = new retinaProRuntimeDevice();
			foreach (DictionaryEntry item2 in item)
			{
				if (item2.Key.ToString().CompareTo("name") == 0)
				{
					XMLNodeList xMLNodeList = (XMLNodeList)item2.Value;
					foreach (XMLNode item3 in xMLNodeList)
					{
						foreach (DictionaryEntry item4 in item3)
						{
							if (item4.Key.ToString().CompareTo("_text") == 0)
							{
								retinaProRuntimeDevice2.name = item4.Value.ToString();
							}
						}
					}
				}
				else if (item2.Key.ToString().CompareTo("pixelSize") == 0)
				{
					XMLNodeList xMLNodeList2 = (XMLNodeList)item2.Value;
					foreach (XMLNode item5 in xMLNodeList2)
					{
						foreach (DictionaryEntry item6 in item5)
						{
							if (item6.Key.ToString().CompareTo("_text") == 0)
							{
								string s = item6.Value.ToString();
								float.TryParse(s, out retinaProRuntimeDevice2.pixelSize);
							}
						}
					}
				}
				else if (item2.Key.ToString().CompareTo("screens") == 0)
				{
					XMLNodeList xMLNodeList3 = (XMLNodeList)item2.Value;
					foreach (XMLNode item7 in xMLNodeList3)
					{
						retinaProRuntimeScreen retinaProRuntimeScreen2 = new retinaProRuntimeScreen();
						foreach (DictionaryEntry item8 in item7)
						{
							if (item8.Key.ToString().CompareTo("width") == 0)
							{
								XMLNodeList xMLNodeList4 = (XMLNodeList)item8.Value;
								foreach (XMLNode item9 in xMLNodeList4)
								{
									foreach (DictionaryEntry item10 in item9)
									{
										if (item10.Key.ToString().CompareTo("_text") == 0)
										{
											string s2 = item10.Value.ToString();
											int.TryParse(s2, out retinaProRuntimeScreen2.width);
										}
									}
								}
							}
							else if (item8.Key.ToString().CompareTo("height") == 0)
							{
								XMLNodeList xMLNodeList5 = (XMLNodeList)item8.Value;
								foreach (XMLNode item11 in xMLNodeList5)
								{
									foreach (DictionaryEntry item12 in item11)
									{
										if (item12.Key.ToString().CompareTo("_text") == 0)
										{
											string s3 = item12.Value.ToString();
											int.TryParse(s3, out retinaProRuntimeScreen2.height);
										}
									}
								}
							}
							else
							{
								if (item8.Key.ToString().CompareTo("useForBothLandscapePortrait") != 0)
								{
									continue;
								}
								XMLNodeList xMLNodeList6 = (XMLNodeList)item8.Value;
								foreach (XMLNode item13 in xMLNodeList6)
								{
									foreach (DictionaryEntry item14 in item13)
									{
										if (item14.Key.ToString().CompareTo("_text") == 0)
										{
											string value = item14.Value.ToString();
											bool.TryParse(value, out retinaProRuntimeScreen2.useForBothLandscapePortrait);
										}
									}
								}
							}
						}
						retinaProRuntimeDevice2.screens.Add(retinaProRuntimeScreen2);
					}
				}
				else if (item2.Key.ToString().CompareTo("rootWidth") == 0)
				{
					XMLNodeList xMLNodeList7 = (XMLNodeList)item2.Value;
					foreach (XMLNode item15 in xMLNodeList7)
					{
						foreach (DictionaryEntry item16 in item15)
						{
							if (item16.Key.ToString().CompareTo("_text") == 0)
							{
								string s4 = item16.Value.ToString();
								int.TryParse(s4, out retinaProRuntimeDevice2.rootWidth);
							}
						}
					}
				}
				else if (item2.Key.ToString().CompareTo("rootHeight") == 0)
				{
					XMLNodeList xMLNodeList8 = (XMLNodeList)item2.Value;
					foreach (XMLNode item17 in xMLNodeList8)
					{
						foreach (DictionaryEntry item18 in item17)
						{
							if (item18.Key.ToString().CompareTo("_text") == 0)
							{
								string s5 = item18.Value.ToString();
								int.TryParse(s5, out retinaProRuntimeDevice2.rootHeight);
							}
						}
					}
				}
				else if (item2.Key.ToString().CompareTo("rootAuto") == 0)
				{
					XMLNodeList xMLNodeList9 = (XMLNodeList)item2.Value;
					foreach (XMLNode item19 in xMLNodeList9)
					{
						foreach (DictionaryEntry item20 in item19)
						{
							if (item20.Key.ToString().CompareTo("_text") == 0)
							{
								string value2 = item20.Value.ToString();
								bool.TryParse(value2, out retinaProRuntimeDevice2.rootAuto);
							}
						}
					}
				}
				else
				{
					if (item2.Key.ToString().CompareTo("rootUseBothPortLand") != 0)
					{
						continue;
					}
					XMLNodeList xMLNodeList10 = (XMLNodeList)item2.Value;
					foreach (XMLNode item21 in xMLNodeList10)
					{
						foreach (DictionaryEntry item22 in item21)
						{
							if (item22.Key.ToString().CompareTo("_text") == 0)
							{
								string value3 = item22.Value.ToString();
								bool.TryParse(value3, out retinaProRuntimeDevice2.rootUseBothPortLand);
							}
						}
					}
				}
			}
			deviceList.Add(retinaProRuntimeDevice2);
		}
		return true;
	}

	private bool loadAtlases(ref XMLNode nodes)
	{
		XMLNodeList nodeList = nodes.GetNodeList("retinaProData>0>ArrayOfRetinaProAtlas>0>retinaProAtlas");
		if (nodeList == null)
		{
			return false;
		}
		foreach (XMLNode item in nodeList)
		{
			retinaProRuntimeAtlas retinaProRuntimeAtlas2 = new retinaProRuntimeAtlas();
			foreach (DictionaryEntry item2 in item)
			{
				if (item2.Key.ToString().CompareTo("name") == 0)
				{
					XMLNodeList xMLNodeList = (XMLNodeList)item2.Value;
					foreach (XMLNode item3 in xMLNodeList)
					{
						foreach (DictionaryEntry item4 in item3)
						{
							if (item4.Key.ToString().CompareTo("_text") == 0)
							{
								retinaProRuntimeAtlas2.name = item4.Value.ToString();
							}
						}
					}
				}
				else
				{
					if (item2.Key.ToString().CompareTo("isFont") != 0)
					{
						continue;
					}
					XMLNodeList xMLNodeList2 = (XMLNodeList)item2.Value;
					foreach (XMLNode item5 in xMLNodeList2)
					{
						foreach (DictionaryEntry item6 in item5)
						{
							if (item6.Key.ToString().CompareTo("_text") == 0)
							{
								string value = item6.Value.ToString();
								bool.TryParse(value, out retinaProRuntimeAtlas2.isFont);
							}
						}
					}
				}
			}
			atlasList.Add(retinaProRuntimeAtlas2);
		}
		return true;
	}
}
