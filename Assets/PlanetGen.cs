using System.Numerics;
using Unity.Mathematics;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class PlanetGen : MonoBehaviour
{
    private float width = 20;
    private float length = 20;

    [SerializeField] private float scale = 2;
    [SerializeField] private GameObject planetPrefab;

    void Start()
    {
        GeneratePlanets();
    }

    private void GeneratePlanets()
    {
        for (float x = 0; x < width; x++)
        {
            for(float z = 0; z < length; z++)
            {   
                UnityEngine.Vector3 pos = new UnityEngine.Vector3(x * scale, 0f, z * scale);
                GameObject gen = Instantiate(planetPrefab, pos, UnityEngine.Quaternion.identity);

                gen.transform.SetParent(this.transform);

            }
    }
    }

    private float noiseGeneration()
    {
        return 0f;
    }
}
