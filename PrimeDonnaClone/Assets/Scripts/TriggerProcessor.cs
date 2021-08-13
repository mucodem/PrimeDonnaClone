using PrimeDonna.Player;
using UnityEngine;
using UnityEngine.UI;

public class TriggerProcessor : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] float fillSpeed;
    [SerializeField] float fillAmount;

    [Header("References")]
    public Image circle;
    [SerializeField] GameObject canvas;

    Vector3 desiredRot; //circle rotation adjust
    Player player;

    private void Start()
    {
        player = GetComponent<Player>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Music"))
        {
            Destroy(other.gameObject);
            IncreaseAngle();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            player.RagdollAdjust(true);
        }
    }

    private void Update()
    {
        canvas.transform.position = transform.position;

        if (circle.fillAmount < 1)
            CircleUpdate();
    }

    void CircleUpdate() // update circle rotation and fill Amount
    {
        circle.fillAmount += Time.deltaTime * fillSpeed;

        desiredRot = new Vector3(0, 0, -90f + circle.fillAmount * 360f / 2);
        circle.transform.localEulerAngles = desiredRot;
    }

    void IncreaseAngle() // if triggers with music sign increase angle
    {
        circle.fillAmount += fillAmount;
    }

    public void ResetAngle()
    {
        circle.fillAmount = 0f;
    }
}
