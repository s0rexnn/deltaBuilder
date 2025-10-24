using UnityEngine;

public class OverworldMenuBehaviour : MonoBehaviour
{
    private enum MenuState { ITEM, STAT, CELL }
    private enum MenuOption { O_ITEM, O_STAT, O_CELL }

    private MenuState currentState = MenuState.ITEM;
    private MenuOption currentOption = MenuOption.O_ITEM;

    [Header("UI Elements")]
    public Transform UTPanel_A;
    public Transform UTPanel_B;
    public Transform UTPanel_C;
    public Transform SOUL;

    [Header("Soul Positions")]
    public Transform SOUL_ItemPosition;
    public Transform SOUL_StatPosition;
    public Transform SOUL_CellPosition;

    private bool menu_Active = false;

    private void Awake()
    {
        ChangeState(MenuState.ITEM);
        UTPanel_A.gameObject.SetActive(false);
        UTPanel_B.gameObject.SetActive(false);
        UTPanel_C.gameObject.SetActive(false);
        SOUL.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateState();
        HandleNavigation();
    }

    private void HandleNavigation()
    {
        if (Input.GetKeyDown(KeyCode.C) && !menu_Active)
        {
            menu_Active = true;
            UTPanel_A.gameObject.SetActive(true);
            UTPanel_B.gameObject.SetActive(true);
            SOUL.gameObject.SetActive(true);
        }

        int index = (int)currentOption;
        index = Mathf.Clamp(index, 0, 2);

        if (menu_Active)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
                index++;

            else if (Input.GetKeyDown(KeyCode.UpArrow))
                index--;

            index = Mathf.Clamp(index, 0, 2);
            currentOption = (MenuOption)index;

            if (Input.GetKeyDown(KeyCode.X))
            {
                menu_Active = false;
                UTPanel_A.gameObject.SetActive(false);
                UTPanel_B.gameObject.SetActive(false);
                UTPanel_C.gameObject.SetActive(false);
            }

            switch (currentOption)
            {
                case MenuOption.O_ITEM:
                    SOUL.position = SOUL_ItemPosition.position;
                    ChangeState(MenuState.ITEM);
                    break;
                case MenuOption.O_STAT:
                    SOUL.position = SOUL_StatPosition.position;
                    ChangeState(MenuState.STAT);
                    break;
                case MenuOption.O_CELL:
                    SOUL.position = SOUL_CellPosition.position;
                    ChangeState(MenuState.CELL);
                    break;
            }
        }
    }

    private void ChangeState(MenuState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
    }

    private void UpdateState() { }
}
