using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PresetGenerator : MonoBehaviour
{
    public GameObject[] sections;
    public List<GameObject> currentSections;
    protected int currentSectionsStart = 0;
    public int forwardBufferSections = 2;
    public int rearBufferSections = 2;
    public int currentSection = 0;

    void Start() {
        currentSectionsStart = currentSections.Count;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSections();
    }

    void UpdateSections() {
        while (currentSectionsStart - forwardBufferSections < currentSection) {
            currentSectionsStart += 1; 
            GameObject newSection = Instantiate(RandomSection(), transform);
            currentSections.Add(newSection);
            OnNewSection();
        }
    }

    GameObject RandomSection() {
        return sections[RandomInt(sections.Length)];
    }

    int RandomInt(int maxExclusive) {
        float i = Random.Range(0, maxExclusive);
        if (i == maxExclusive) i = 0;
        return Mathf.FloorToInt(i);
    }

    protected abstract void OnNewSection();
}
