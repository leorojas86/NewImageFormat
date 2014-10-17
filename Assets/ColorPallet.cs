using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorPallet
{
	#region Constants
	
	private const float MAXIMUM_COLOR_VARIANCE = 0.02f;
	
	#endregion
	#region Variables
	
	private List<Color> _palletColors = new List<Color>();
	
	#endregion
	
	#region Properties
	
	public List<Color> PalletColors
	{
		get { return _palletColors; }
	}
	
	#endregion
	
	public ushort GetColorIndex(Color color)
	{
		ushort colorIndex = FindEquivalentColorIndex(color);
		
		if(colorIndex == _palletColors.Count)
		{
			_palletColors.Add(color);
			
			if(_palletColors.Count == ushort.MaxValue)
				Debug.LogError("Error: ushort pallet size exceded.");
		}
		
		return colorIndex;
	}
	
	private ushort FindEquivalentColorIndex(Color color)
	{
		ushort currentIndex = 0;
		
		foreach(Color currentColor in _palletColors)
		{
			if(IsSimmilarChannel(color.a, currentColor.a) &&
			   IsSimmilarChannel(color.r, currentColor.r) &&
			   IsSimmilarChannel(color.g, currentColor.g) &&
			   IsSimmilarChannel(color.b, currentColor.b) )
			{
				return currentIndex;
			}
			
			currentIndex++;
		}
		
		return (ushort)_palletColors.Count;
	}
	
	private static bool IsSimmilarChannel(float channel, float other)
	{
		return channel < other + MAXIMUM_COLOR_VARIANCE && 
			   channel > other - MAXIMUM_COLOR_VARIANCE;
	}
}
