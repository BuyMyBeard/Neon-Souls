using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceFloor : MonoBehaviour
{
    [SerializeField] List<Material> tileColors;
    List<MeshRenderer>[] tiles;
    int currentIndex = 0;

    [SerializeField] float offsetDelay;
    [SerializeField] float changeDelay;
    private void Awake()
    {
        tiles = new List<MeshRenderer>[tileColors.Count];
        for (int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new List<MeshRenderer>();
        }
        foreach (var renderer in GetComponentsInChildren<MeshRenderer>())
        {
            int index = tileColors.FindIndex(r => renderer.sharedMaterial.name == r.name);
            if (index != -1) tiles[index].Add(renderer);
        }
    }

    IEnumerator Start()
    {
        yield return new WaitForSeconds(offsetDelay);
        while (true)
        {
            yield return new WaitForSeconds(changeDelay);
            currentIndex++;
            for (int i = 0; i < tileColors.Count; i++)
            {
                foreach (var tile in tiles[i])
                {
                    tile.material = tileColors[(i + currentIndex) % tileColors.Count];
                }
            }
        }
    }
}
