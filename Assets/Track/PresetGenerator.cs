using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Generic script to infinitely generate things.
/// Takes preset sections and generates and destorys them as needed according to the current
/// section and buffer sections.
/// </summary>
public abstract class PresetGenerator : MonoBehaviour
{
    public GameObject[] sectionPrefabs;     // Prefabs
    public List<GameObject> currentSections;    // Instantiated tracks. Put ones already in scene here in inspector
    protected int currentSectionsStart = 0;
    public int forwardBufferSections = 2;
    public int currentSection = 0;

    protected void Start()
    {
        currentSectionsStart = currentSections.Count;
    }

    // Update is called once per frame
    protected void Update()
    {
        UpdateSections();
    }

    void UpdateSections()
    {
        while (currentSectionsStart - forwardBufferSections < currentSection)
        {
            currentSectionsStart += 1;
            GameObject newSection = Instantiate(RandomSection(), transform);
            currentSections.Add(newSection);
            OnNewSection();
        }
    }

    GameObject RandomSection()
    {
        return sectionPrefabs[RandomInt(sectionPrefabs.Length)];
    }

    int RandomInt(int maxExclusive)
    {
        float i = Random.Range(0, maxExclusive);
        if (i == maxExclusive) i = 0;
        return Mathf.FloorToInt(i);
    }

    protected abstract void OnNewSection();
}
