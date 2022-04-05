using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(StringBounds2DDictionary))]
[CustomPropertyDrawer(typeof(StringGameObjectDictionary))]
[CustomPropertyDrawer(typeof(StringIntDictionary))]
[CustomPropertyDrawer(typeof(TileTypeTileListDictionary))]
[CustomPropertyDrawer(typeof(TitlTypeSpriteDictionary))]
public class AnySerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer { }

[CustomPropertyDrawer(typeof(TileListStorage))]
public class AnySerializableStorageDictionaryPropertyDrawer : SerializableDictionaryStoragePropertyDrawer { }
