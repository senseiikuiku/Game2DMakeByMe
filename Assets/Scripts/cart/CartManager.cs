using UnityEngine;

public abstract class CartManager : MonoBehaviour
{
    [SerializeField] protected GameObject[] itemCartPrefabs; // Mảng chứa các đối tượng trong giỏ hàng
    [SerializeField] protected int priceItem; // Giá của mỗi mặt hàng trong giỏ hàng
    [SerializeField] protected int indexItem; // Chỉ mục của mặt hàng trong giỏ hàng
    [SerializeField] protected Transform itemCartSpawnPoint; // Điểm xuất hiện của mặt hàng trong giỏ hàng




    protected void IndexItem()
    {
        int requiredScore = (indexItem == 3) ? 15 : 10;
        if (GameManager.Instance.score >= requiredScore)
        {
            switch (indexItem)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    Instantiate(itemCartPrefabs[indexItem], itemCartSpawnPoint.position, Quaternion.identity);
                    GameManager.Instance.AddScore(-priceItem); // Trừ điểm khi mua mặt hàng
                    break;

                default:
                    break;
            }
        }
    }

    public void OnButtonClick()
    {
        IndexItem();
    }

}
