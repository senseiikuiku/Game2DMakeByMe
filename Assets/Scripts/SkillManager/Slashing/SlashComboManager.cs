using UnityEngine;

public abstract class SlashComboManager : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] protected GameObject[] slashPrefabs;         // Các đòn slash combo
    [SerializeField] protected Transform slashSpawnPoint;         // Vị trí xuất chiêu
    [SerializeField] protected float slashDelay = 0.3f;           // Delay giữa các đòn combo
    [SerializeField] protected float comboResetTime = 1.0f;       // Thời gian reset nếu không tiếp tục combo
    [SerializeField] protected float slashLifetime = 0.3f;        // Thời gian tồn tại của slash

    protected int currentSlashIndex = 0;                          // Chỉ số đòn combo hiện tại
    protected float lastSlashTime = -Mathf.Infinity;              // Thời điểm đòn combo cuối cùng

    protected PlayerController playerController; // Tham chiếu đến PlayerController để tương tác
    protected AudioManager audioManager; // Tham chiếu đến AudioManager để phát âm thanh
    /// <summary>
    /// Gọi hàm này từ PlayerController khi muốn tung đòn slash
    /// </summary>
    /// 

    protected virtual void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>(); // Tìm PlayerController trong cảnh
        audioManager = FindAnyObjectByType<AudioManager>(); // Tìm AudioManager trong cảnh
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }

    public void Launch(Vector2 direction)
    {
        if (slashPrefabs.Length == 0 || slashSpawnPoint == null)
            return;

        float timeSinceLastSlash = Time.time - lastSlashTime; // Tính thời gian kể từ đòn slash cuối cùng

        // Reset combo nếu quá lâu không nhấn
        if (timeSinceLastSlash > comboResetTime)
        {
            ResetCombo();
        }

        // Nếu đủ delay, thực hiện slash
        if (timeSinceLastSlash >= slashDelay)
        {
            Slash(direction);
        }
    }

    protected virtual void Slash(Vector2 direction) { }

    protected void ResetCombo()
    {
        currentSlashIndex = 0;
    }
}
