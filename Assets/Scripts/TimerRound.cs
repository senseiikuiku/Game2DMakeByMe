using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class TimerRound : MonoBehaviour
{
    [SerializeField] private float timerRound = 100;
    [SerializeField] private TextMeshProUGUI timerText;
    private Image timer;

    private float currentTime;

    private void Awake()
    {
        timer = GetComponent<Image>();
    }

    private void Start()
    {
        currentTime = timerRound;
        //UpdateTimerUI();
    }

    void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // giảm theo thời gian thực
            if (currentTime < 0) currentTime = 0;

            UpdateTimerUI();

            if (currentTime <= 0)
            {
                GameManager.Instance.GameOver(); // kết thúc game
            }
        }
    }

    private void UpdateTimerUI()
    {
        // Cập nhật text (làm tròn xuống)
        timerText.text = Mathf.CeilToInt(currentTime).ToString();

        // Thanh image giảm dần
        if (timer != null)
        {
            timer.fillAmount = currentTime / timerRound;
        }
    }
}
