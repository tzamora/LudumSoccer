using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class UnityExtensions
{
	public static void Hide(this GameObject gameObject)
	{
		if(gameObject.transform.renderer != null)
		{
			gameObject.renderer.enabled = false;
			
			gameObject.SetActive(false);
		}
		
		foreach (Transform child in gameObject.transform)
		{
			if(child.transform.renderer != null)
			{
				child.renderer.enabled = false;
				
				child.gameObject.SetActive(false);
			}
		}
	}
	
	public static void Show(this GameObject gameObject)
	{
		if(gameObject.transform.renderer != null)
		{
			gameObject.transform.renderer.enabled = true;
		
			gameObject.SetActive(true);
		}
		
		foreach (Transform child in gameObject.transform)
		{
			if(child.transform.renderer != null)
			{
				child.transform.renderer.enabled = true;
				
				child.gameObject.SetActive(true);
			}
		}
	}
	
	public static bool isTouchingWall (this CharacterController characterController)
	{
		return (characterController.collisionFlags & CollisionFlags.Sides) != 0;
	}
	
	static readonly System.Random random = new System.Random ();

	public static void Shuffle<T> (this IList<T> list)
	{
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = random.Next (n + 1);
			T value = list [k];
			list [k] = list [n];
			list [n] = value;
		}
	}
	
	/// <summary>
	/// Taken from http://stackoverflow.com/questions/9033/hidden-features-of-c/407325#407325
	/// instead of doing (enum & value) == value you can now use enum.Has(value)
	/// </summary>
	/// <typeparam name="T">Type of enum</typeparam>
	/// <param name="type">The enum value you want to test</param>
	/// <param name="value">Flag Enum Value you're looking for</param>
	/// <returns>True if the type has value bit set</returns>
	public static bool Has<T> (this System.Enum type, T value)
	{
		return (((int)(object)type & (int)(object)value) == (int)(object)value);
	}
	
	
	
	//
	//checks if a renderer is visible from a specified camera
	//
	public static bool IsVisibleFrom(this Renderer renderer, Camera camera)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
	}
	
	public static T Pop<T>(this List<T> theList)
    {
         var local = theList[theList.Count - 1];
         theList.RemoveAt(theList.Count - 1);
         return local;
    }
      
    public static void Push<T>(this List<T> theList, T item)
    {
        theList.Add(item);
    }
	
	/// <summary>
	/// Spawns the child game object in the center of the specified parent
	/// </summary>
	/// <returns>
	/// The new game object.
	/// </returns>
	/// <param name='parent'>
	/// Parent.
	/// </param>
	/// <param name='prefab'>
	/// Prefab.
	/// </param>
	public static GameObject SpawnChildGameObject(this GameObject parent, GameObject prefab)
	{
		GameObject go = (GameObject) Spawner.Spawn( prefab, Vector3.zero, Quaternion.identity);
		
		go.transform.parent = parent.transform;
		
		go.transform.localPosition = Vector3.zero;
		
		return go;
	}
	
	public static void ClearChildren(this GameObject go)
	{
		if(go == null)
		{
			return;
		}
		
		var children = new List<GameObject>();
		
		foreach (Transform child in go.transform) 
		{
			children.Add(child.gameObject);	
		}
		
		children.ForEach(child => Object.Destroy(child));
	}
	
	public static void SetX(this Vector3 vector, float x)
	{
		vector = new Vector3(x, vector.y, vector.z);
	}
	
	public static void SetY(this Vector3 vector, float y)
	{
		vector = new Vector3(vector.x, y, vector.z);
	}
	
	public static void SetZ(this Vector3 vector, float z)
	{
		vector = new Vector3(vector.x, vector.y, z);
	}
	
	public static string TextWrapper(this string text)
	{
        string result="", next="";
 
		int charByLine = 70, cursor = 0, charCount = 0;
 
		bool breakLine = false;
 
        while(cursor<text.Length)
		{					
			result += (next = (text[cursor++])+"");
			
			charCount++;
			
			if(((next = ((next==" ") && breakLine) ? "\n" :""))=="\n") breakLine= (charCount=0)!=0;
			
			result += next;
			
			breakLine = ((charCount >= charByLine-1) || breakLine);
		}
        
		return result;
	}
}
