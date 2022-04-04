using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ClimateManagement;

[Serializable]
public class StringBounds2DDictionary : SerializableDictionary<string, BoundsInt2D> { }

[Serializable]
public class StringGameObjectDictionary : SerializableDictionary<string, GameObject> { }

[Serializable]
public class StringIntDictionary : SerializableDictionary<string, int> { }

[Serializable]
public class TileTypeTileListDictionary : SerializableDictionary<TileType, List<Tile>, TileListStorage> { }

[Serializable]
public class TileListStorage : SerializableDictionary.Storage<List<Tile>> { }

