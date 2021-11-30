using UnityEngine;
using UnityEngine.SceneManagement;

public class Invaders : MonoBehaviour {

    public AudioSource source;

    public AudioClip triggerSound;

    public Invader[] prefabs;

    int rows = 5;

    int columns = 6;

    public AnimationCurve speed;

    public int amountKilled { get; private set; }

    private int totalInvaders = 30;

    public float percentKilled => (float)this.amountKilled / (float)this.totalInvaders;

    public int amountAlive => this.totalInvaders - this.amountKilled;

    public Projectile misslePrefab;

    private Vector3 _direction = Vector2.right;

    private void Awake()
    {
        for (int row = 0; row < this.rows; row++) {
            float width = 5.0f * (this.columns - 1);
            float height = 3.5f * (this.rows - 1);
            Vector2 centering = new Vector2(-width / 2, -height / 2);
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 3.5f), 0.0f);

            for (int col = 0; col < this.columns; col++) {
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += InvaderKilled;
                Vector3 position = rowPosition;
                position.x += col * 5.0f;
                invader.transform.localPosition = position;
            }
        }
    }

    private void Start() {
        // Fire rate for the Invaders
        InvokeRepeating(nameof(MissleAttack),0.175f, 0.175f);
    }

    private void Update() {
        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;

        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach (Transform invader in this.transform) {

            if (!invader.gameObject.activeInHierarchy) {
                continue;
            }

            if (_direction == Vector3.right && invader.position.x >= (rightEdge.x - 1.6f)) {
                AdvanceRow();

            }
            else if (_direction == Vector3.left && invader.position.x <= (leftEdge.x + 1.6f)) {
                AdvanceRow();
            }
        }
    }

    private void AdvanceRow() {
        _direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;

    }

    private void MissleAttack()
    {
        foreach (Transform invader in this.transform)
        {

            if (!invader.gameObject.activeInHierarchy)
            {
                continue;
            }

            if (Random.value < 0.1f)
            {
                Instantiate(this.misslePrefab, invader.position, Quaternion.identity);
                source.PlayOneShot(triggerSound);
                break;
            }
        }
    }
    private void InvaderKilled(){
        this.amountKilled++;
        if (this.amountKilled == 30)
        {
            SceneManager.LoadScene("GameWon");
        }
    }
}