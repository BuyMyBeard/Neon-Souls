using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDisplay : MonoBehaviour
{
    [SerializeField] GameObject potion;
    [SerializeField] float spacing = 10;
    Stack<GameObject> displayedPotions = new Stack<GameObject>();

    public int PotionCount
    {
        get => displayedPotions.Count;
        set
        {
            Clear();
            Add(value);
        }
    }

    public void Clear()
    {
        while (displayedPotions.Count > 0)
            Destroy(displayedPotions.Pop());
    }
    public void AddOne()
    {
        GameObject instance = Instantiate(potion);
        instance.transform.SetParent(transform, false);
        instance.transform.position = transform.position + spacing * displayedPotions.Count * Vector3.right; 
        displayedPotions.Push(instance);
    }
    public void Add(int count)
    {
        for (int i = 0; i < count; i++)
            AddOne();
    }
    public void RemoveOne()
    {
        if (displayedPotions.TryPop(out GameObject removedPotion))
            Destroy(removedPotion);
    }


}
