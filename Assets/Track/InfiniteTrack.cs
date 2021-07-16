using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteTrack : PresetGenerator, Track
{
    public float rotationAccuracy = 0.1f;   // distance from end of track to reference point to get angle
    public Transform car;   // Used for current section calculations

    new protected void Update() {
        base.Update();
    }

    // ============================================================================================
    // Track
    // ============================================================================================

    public bool OnTrack(Vector3 point)
    {
        return false;
    }

    // ============================================================================================
    // Preset Generator overrides
    // ============================================================================================

    /// <summary>
    /// Called when a new section is created.
    /// If there are at least 2 sections, move the last one (highest index) such that the start
    /// mates up with the end of the second to last.
    /// </summary>
    override protected void OnNewSection()
    {
        // Gotta match up the pieces
        if (currentSections.Count > 1)
        {
            GameObject newest = currentSections[currentSections.Count - 1];
            GameObject secondNewest = currentSections[currentSections.Count - 2];
            MoveTogether(newest.GetComponent<TrackSection>(), secondNewest.GetComponent<TrackSection>());
        }
    }

    /// <summary>
    /// Moves the moveable TrackSection such that the start of it mates up with the end of the still TrackSection.
    /// Rotation and Translation
    /// </summary>
    /// <param name="moveable"></param>
    /// <param name="still"></param>
    private void MoveTogether(TrackSection moveable, TrackSection still)
    {
        // Rotate
        {
            Vector3 stillAttachPoint = still.GetPoint(1f);
            Vector3 stillReferencePoint = still.GetPoint(1f - rotationAccuracy);
            Vector3 moveableAttachPoint = moveable.GetPoint(0f);
            Vector3 moveableReferencePoint = moveable.GetPoint(rotationAccuracy);
            Vector3 stillDir = (stillAttachPoint - stillReferencePoint).normalized;
            Vector3 moveableDir = (moveableReferencePoint - moveableAttachPoint).normalized;
            float stillAngle = Mathf.Rad2Deg * Mathf.Atan2(stillDir.y, stillDir.x);
            float moveableAngle = Mathf.Rad2Deg * Mathf.Atan2(moveableDir.y, moveableDir.x);
            float angleChange = stillAngle - moveableAngle;
            moveable.transform.eulerAngles += Vector3.forward * angleChange;
        }
        // Translate
        {
            Vector3 stillAttachPoint = still.GetPoint(1f);
            Vector3 moveableAttachPoint = moveable.GetPoint(0f);
            Vector3 distance = stillAttachPoint - moveableAttachPoint;
            moveable.transform.position += distance;
        }
        // TODO this but smarter
        moveable.Validate();
        still.Validate();
    }

}
