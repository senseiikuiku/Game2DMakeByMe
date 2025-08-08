using UnityEngine;

public class SlashComboLv1 : SlashComboManager
{
    protected override void Start()
    {

    }
    protected override void Slash(Vector2 direction)
    {
        GameObject slash = Instantiate(
            slashPrefabs[currentSlashIndex],
            slashSpawnPoint.position, slashPrefabs[currentSlashIndex].transform.rotation
        );

        // Lật slash theo hướng nhân vật đang quay mặt
        Vector3 slashScale = slash.transform.localScale;
        slashScale.x = slashScale.x * playerController.facingDirection;
        slash.transform.localScale = slashScale;

        // Lật góc quay của slash theo hướng nhân vật đang quay mặt
        Vector3 slashRotation = slash.transform.rotation.eulerAngles;
        slashRotation.z = slashRotation.z * playerController.facingDirection; // Lật theo hướng nhân vật
        slash.transform.rotation = Quaternion.Euler(slashRotation);

        // Phá huy slash sau thời gian tồn tại
        Destroy(slash, slashLifetime);

        lastSlashTime = Time.time;
        currentSlashIndex++;

        if (currentSlashIndex >= slashPrefabs.Length)
        {
            currentSlashIndex = 0;
        }
    }

}
