using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class ARGameController : MonoBehaviour
{
    private DatabaseReference reference;
    public ARRaycastManager raycastManager;
    public GameObject cluePrefab;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                reference = FirebaseDatabase.DefaultInstance.RootReference;
                Debug.Log("Firebase initialized.");
            }
            else
            {
                Debug.LogError("Could not resolve Firebase dependencies.");
            }
        });

        if (raycastManager == null)
            raycastManager = FindObjectOfType<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                PlaceClue(hitPose.position);
            }
        }
    }

    void PlaceClue(Vector3 position)
    {
        GameObject clue = Instantiate(cluePrefab, position, Quaternion.identity);
        clue.SetActive(true);
        Debug.Log("Clue placed at: " + position);
        SyncClueData(position);
    }

    void SyncClueData(Vector3 position)
    {
        Dictionary<string, object> clueData = new Dictionary<string, object>();
        clueData["x"] = position.x;
        clueData["y"] = position.y;
        clueData["z"] = position.z;

        reference.Child("Clues").Push().SetValueAsync(clueData).ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
                Debug.Log("Clue synced to Firebase: " + position);
            else
                Debug.LogError("Failed to sync clue: " + task.Exception);
        });
    }

    public void RetrieveClueData()
    {
        reference.Child("Clues").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (var child in snapshot.Children)
                {
                    float x = float.Parse(child.Child("x").Value.ToString());
                    float y = float.Parse(child.Child("y").Value.ToString());
                    float z = float.Parse(child.Child("z").Value.ToString());
                    Debug.Log("Clue position retrieved: (" + x + ", " + y + ", " + z + ")");
                    // Optional: Instantiate cluePrefab at retrieved position
                }
            }
            else
            {
                Debug.LogError("Error retrieving clues: " + task.Exception);
            }
        });
    }
}
