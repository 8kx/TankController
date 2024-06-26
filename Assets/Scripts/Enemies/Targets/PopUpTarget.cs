using UnityEngine;

public class PopUpTarget : EnemyBase
{
    public float popUpTime = 2f;
    public float hideTime = 3f;
    private bool isVisible = false;
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (isVisible && timer >= popUpTime)
        {
            Hide();
        }
        else if (!isVisible && timer >= hideTime)
        {
            Show();
        }
    }

    private void Show()
    {
        isVisible = true;
        timer = 0f;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    private void Hide()
    {
        isVisible = false;
        timer = 0f;
        transform.position = new Vector3(transform.position.x, -2, transform.position.z);
    }

    public override void Hit()
    {
        Debug.Log("Target hit!");
        Hide();
        // Additional hit logic can be added here
    }
}
