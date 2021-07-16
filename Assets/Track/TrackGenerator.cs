using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackGenerator : PresetGenerator
{
    public float rotationAccuracy = 0.1f;

    override protected void OnNewSection() {
        // Gotta match up the pieces
        if (currentSections.Count > 1) {
            GameObject newest = currentSections[currentSections.Count - 1];
            GameObject secondNewest = currentSections[currentSections.Count - 2];
            MoveTogether(newest.GetComponent<TrackSection>(), secondNewest.GetComponent<TrackSection>());
        }
    }

    private void MoveTogether(TrackSection moveable, TrackSection still) {
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

        // Debug.Log((stillAttachPoint - stillReferencePoint) + ", " + (moveableAttachPoint - moveableReferencePoint));
        // Debug.Log(stillAngle + ", " + moveableAngle);
        float angleChange = stillAngle - moveableAngle;
        moveable.transform.eulerAngles += Vector3.forward * angleChange;
        }

        {
        // Translate
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
