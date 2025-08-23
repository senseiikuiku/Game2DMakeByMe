using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource backGroundAudioSource;
    [SerializeField] private AudioSource effectAudioSource;

    [SerializeField] private AudioClip backGroundClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip keyClip;
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private AudioClip enemyDieClip;
    [SerializeField] private AudioClip gameOverClip;
    [SerializeField] private AudioClip eatFoodLv1Clip;
    [SerializeField] private AudioClip eatFoodLv2Clip;
    [SerializeField] private AudioClip pickUpSkill;
    [SerializeField] private AudioClip fireBulletLv1;
    [SerializeField] private AudioClip fireBulletLv2;
    [SerializeField] private AudioClip explosionLv2;
    [SerializeField] private AudioClip comboSlashingLv1;
    [SerializeField] private AudioClip comboSlashingLv2;
    [SerializeField] private AudioClip swordSlash;
    [SerializeField] private AudioClip slashBoss;
    [SerializeField] private AudioClip strikeBoss;
    [SerializeField] private AudioClip fireBulletBoss;
    [SerializeField] private AudioClip explosionBoss;
    [SerializeField] private AudioClip jumpBoss;
    [SerializeField] private AudioClip hurtBoss;
    [SerializeField] private AudioClip dashBoss;
    [SerializeField] private AudioClip defBoss;
    [SerializeField] private AudioClip walkBoss;
    [SerializeField] private AudioClip dieBoss;

    void Start()
    {
        backGroundAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1f); // Lấy giá trị âm lượng nhạc nền từ PlayerPrefs ở bên VolumeSettings, mặc định là 1f
        effectAudioSource.volume = PlayerPrefs.GetFloat("EffectVolume", 1f);

        PlayBackGroundMusic();
    }



    public void PlayBackGroundMusic()
    {
        backGroundAudioSource.clip = backGroundClip;
        backGroundAudioSource.Play();
    }

    public void PlayCoinSound()
    {
        effectAudioSource.PlayOneShot(coinClip);
    }

    public void PlayKeySound()
    {
        effectAudioSource.PlayOneShot(keyClip);
    }

    public void PlayJumpSound()
    {
        effectAudioSource.PlayOneShot(jumpClip);
    }

    public void PlayEnemyOrTrapHitSound()
    {
        effectAudioSource.PlayOneShot(enemyHitClip);
    }

    public void PlayEnemyDieSound()
    {
        effectAudioSource.PlayOneShot(enemyDieClip);
    }

    public void PlayGameOverSound()
    {
        effectAudioSource.PlayOneShot(gameOverClip);
    }

    public void PlayEatFoodLv1()
    {
        effectAudioSource.PlayOneShot(eatFoodLv1Clip);
    }

    public void PlayEatFoodLv2()
    {
        effectAudioSource.PlayOneShot(eatFoodLv2Clip);
    }

    public void PlayPickUpSkill()
    {
        effectAudioSource.PlayOneShot(pickUpSkill);
    }

    public void PlayFireBulletLv1()
    {
        effectAudioSource.PlayOneShot(fireBulletLv1);
    }

    public void PlayFireBulletLv2()
    {
        effectAudioSource.PlayOneShot(fireBulletLv2);
    }

    public void PlayExplosionLv2()
    {
        effectAudioSource.PlayOneShot(explosionLv2);
    }

    public void PlayComboSlashingLv1()
    {
        effectAudioSource.PlayOneShot(comboSlashingLv1);
    }

    public void PlayComboSlashingLv2()
    {
        effectAudioSource.PlayOneShot(comboSlashingLv2);
    }

    public void PlaySwordSlash()
    {
        effectAudioSource.PlayOneShot(swordSlash);
    }

    public void PlaySlashBoss()
    {
        effectAudioSource.PlayOneShot(slashBoss);
    }

    public void PlayStrikeBoss()
    {
        effectAudioSource.PlayOneShot(strikeBoss);
    }

    public void PlayFireBulletBoss()
    {
        effectAudioSource.PlayOneShot(fireBulletBoss);
    }

    public void PlayExplosionBoss()
    {
        effectAudioSource.PlayOneShot(explosionBoss);
    }

    public void PlayJumpBoss()
    {
        effectAudioSource.PlayOneShot(jumpBoss);
    }

    public void PlayHurtBoss()
    {
        effectAudioSource.PlayOneShot(hurtBoss);
    }

    public void PlayDashBoss()
    {
        effectAudioSource.PlayOneShot(dashBoss);
    }

    public void PlayDefBoss()
    {
        effectAudioSource.PlayOneShot(defBoss);
    }

    public void PlayWalkBoss()
    {
        effectAudioSource.PlayOneShot(walkBoss);
    }

    public void PlayDieBoss()
    {
        effectAudioSource.PlayOneShot(dieBoss);
    }
}
