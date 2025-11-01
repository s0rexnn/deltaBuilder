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

   public void DisableMovement() // For cutscenes and dialogues
    {
        if (playerController != null)
        {
            playerController.SetIdleAnimation();
            playerController.enabled = false;
            playerController.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        }
        if (partyFollowers != null)
        {
            foreach (var member in partyFollowers)
            {
                member.SetIdleAnimation();
                member.enabled = false;
            }
        }
    }
    
    public void EnableMovement()  // For cutscenes and dialogues
    {
        if (playerController != null)
        {
            playerController.enabled = true;
        }
        if (partyFollowers != null)
        {
            foreach (var member in partyFollowers)
            {
                member.enabled = true;
            }
        }
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
