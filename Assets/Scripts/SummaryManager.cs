using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SummaryManager : MonoBehaviour
{
    public static SummaryManager Instance { get;  private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private GameObject summaryPanel;
    [SerializeField] private TextMeshProUGUI summaryText,summaryTitle,summaryType;

    public void ShowSummary(string title, string type,string description)
    {
        summaryTitle.text = title;
        summaryType.text = type;
        summaryText.text = description;
        summaryPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void HideSummary()
    {
        summaryPanel.SetActive(false);
        Time.timeScale = 1;
    }
}
