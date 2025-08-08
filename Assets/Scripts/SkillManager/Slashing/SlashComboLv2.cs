using UnityEngine;

public class SlashComboLv2 : SlashComboManager
{
    [SerializeField] private GameObject swordWave; // Hiệu ứng sóng kiếm
    [SerializeField] private float swordWaveSpeed = 15f; // Tốc độ sóng kiếm
    [SerializeField] private Transform swordWaveSpawnPoint;         // Vị trí xuất chiêu



    protected override void Slash(Vector2 direction)
    {
        if (slashPrefabs.Length == 0 || slashSpawnPoint == null)
            return;
        GameObject slash = Instantiate(
            slashPrefabs[currentSlashIndex],
            slashSpawnPoint.position,
            Quaternion.identity);

        // Lật slash theo hướng nhân vật đang quay mặt
        Vector3 slashScale = slash.transform.localScale;
        slashScale.x = slashScale.x * playerController.facingDirection; // Lật theo hướng nhân vật
        slash.transform.localScale = slashScale;

        // Lật góc quay của slash theo hướng nhân vật đang quay mặt
        Vector3 slashRotation = slash.transform.rotation.eulerAngles;
        slashRotation.z = slashRotation.z * playerController.facingDirection; // Lật theo hướng nhân vật
        slash.transform.rotation = Quaternion.Euler(slashRotation);

        Destroy(slash, slashLifetime); // Hủy đòn chém sau một thời gian nhất định

        lastSlashTime = Time.time; // Cập nhật thời gian của đòn chém cuối cùng

        bool isLastSlash = currentSlashIndex == slashPrefabs.Length - 1; // Kiểm tra xem có phải là đòn chém cuối cùng không

        currentSlashIndex++; // Tăng chỉ số đòn combo

        if (currentSlashIndex >= slashPrefabs.Length)
        {
            currentSlashIndex = 0; // Reset chỉ số nếu đã đạt đến cuối mảng

            if (isLastSlash && swordWave != null)
            {
                // Lật vị trí xuất chiêu sóng kiếm theo hướng nhân vật đang quay mặt
                Vector3 waveSpawnScale = swordWaveSpawnPoint.localScale;
                waveSpawnScale.x = waveSpawnScale.x * playerController.facingDirection; // Lật theo hướng nhân vật
                swordWaveSpawnPoint.localScale = waveSpawnScale;

                // Tao hiệu ứng sóng kiếm nếu là đòn chém cuối cùng
                GameObject wave = Instantiate(swordWave, swordWaveSpawnPoint.position, Quaternion.identity);
                audioManager?.PlaySwordSlash(); // Phát âm thanh chém


                // Lật sóng kiếm theo hướng nhân vật đang quay mặt
                Vector3 waveScale = wave.transform.localScale;
                waveScale.x = waveScale.x * playerController.facingDirection; // Lật theo hướng nhân vật
                wave.transform.localScale = waveScale;

                // Thiết lập vận tốc cho sóng kiếm
                Rigidbody2D waveRb = wave.GetComponent<Rigidbody2D>();
                if (waveRb != null)
                {
                    waveRb.linearVelocity = new Vector2(swordWaveSpeed * playerController.facingDirection, 0f);
                }
            }
        }
    }
}
