using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Test : MonoBehaviour 
{
	public Texture2D texture;
	public MeshRenderer meshRenderer;
	
	void Start() 
	{
		ColorPallet pallet 	  			   = new ColorPallet();
		Color[] textureColors 			   = texture.GetPixels();
		List<ushort> texturePalletedColors = new List<ushort>();
		string colorsText 	  			   = "Colors: \n";
		
		foreach(Color color in textureColors)
			texturePalletedColors.Add(pallet.GetColorIndex(color));
		
		using(FileStream fileStream = File.Create(Application.dataPath + "test.bytes"))
		{
			using (BinaryWriter writer = new BinaryWriter(fileStream))
			{
				string palletColors = "Pallet Colors: \n";
				
				foreach(Color color in pallet.PalletColors)
				{
					palletColors += color + "\n";
					
					Color32 color32 = color;
					
					writer.Write((byte)color32.a);
					writer.Write((byte)color32.r);
					writer.Write((byte)color32.g);
					writer.Write((byte)color32.b);
				}
				
				foreach(ushort palletedColorIndex in texturePalletedColors)
					writer.Write((ushort)palletedColorIndex);
				
				Debug.Log("Pallet size = " + pallet.PalletColors.Count);
				Debug.Log(palletColors);
			}
		}
		
		Color[] newColors = new Color[textureColors.Length];
		
		for(int x = 0; x < textureColors.Length; x++)
			newColors[x] = pallet.PalletColors[texturePalletedColors[x]];
		
		Texture2D newTexture = new Texture2D(texture.width, texture.height, TextureFormat.ARGB32, false);
		newTexture.SetPixels(newColors);
		newTexture.Apply(false, false);
		
		meshRenderer.material.mainTexture = newTexture;
		
		//foreach(KeyValuePair<Color, int> pair in colors)
			//colorsText += "Color = " + pair.Key + " count = " + pair.Value + " \n";
		
		//Debug.Log("Colors count = " + colors.Count + colorsText);
		//Debug.Log(colorsText);
	}
}
