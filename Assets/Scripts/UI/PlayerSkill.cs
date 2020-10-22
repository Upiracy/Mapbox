using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkill : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int CDTime = 5;
    [SerializeField] Image buttonMask = null;
    [SerializeField] Button button = null;
    private bool skillAvailable = true;

    void Start()
    {
        buttonMask.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!skillAvailable)
        {
            if (buttonMask.fillAmount <= 0f) {
                buttonMask.fillAmount = 0f; 
                skillAvailable = true;
                button.interactable = true;
            }
            else
            {
                buttonMask.fillAmount -= Time.deltaTime/(float)CDTime;
            }
        }
    }
    public void OnPress()
    {
        buttonMask.fillAmount = 1f;
        skillAvailable = false;
        button.interactable = false;
    }
}
