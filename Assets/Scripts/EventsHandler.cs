using UnityEngine;
using System.Collections.Generic;

public class EventsHandler : MonoBehaviour
{
    public static EventsHandler Instance { get; private set; }

    [HideInInspector] public OverworldPlayerBehaviour playerController;
    [HideInInspector] public List<OverworldPartyMember> partyFollowers = new List<OverworldPartyMember>();

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<OverworldPlayerBehaviour>();

        partyFollowers.AddRange(
            FindObjectsByType<OverworldPartyMember>(FindObjectsSortMode.None)
        );
    }

    private void Update()
    {
        SaveAndLoad();
    }

    private void SaveAndLoad()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveSystem.Save();
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            SaveSystem.Load();
        }
    }
}
