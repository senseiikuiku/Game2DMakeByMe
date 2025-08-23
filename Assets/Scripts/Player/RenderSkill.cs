using UnityEngine;
using UnityEngine.UI;

public class RenderSkill : MonoBehaviour
{
    [SerializeField] private GameObject[] skillPrefabs; // Mảng chứa các prefab kỹ năng

    private PlayerController playerController;
    private Image image;

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        image = GetComponent<Image>();
    }

    private void Update()
    {
        RenderSkills();
    }

    private void RenderSkills()
    {
        if (playerController == null)
            return;

        image.enabled = true;

        switch (playerController.skill)
        {
            case 1:
                image.sprite = (playerController.bulletLevel == 1)
                    ? skillPrefabs[0].GetComponent<SpriteRenderer>().sprite
                    : skillPrefabs[1].GetComponent<SpriteRenderer>().sprite;
                break;

            case 2:
                image.sprite = (playerController.slashLevel == 1)
                    ? skillPrefabs[2].GetComponent<SpriteRenderer>().sprite
                    : skillPrefabs[3].GetComponent<SpriteRenderer>().sprite;
                break;

            default:
                image.enabled = false;
                break;
        }
    }


}
