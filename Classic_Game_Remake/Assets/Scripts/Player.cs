using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public AudioSource source;

    public AudioClip triggerSound;

    public Projectile LaserPrefab;

    public float speed = 5.0f;

    private bool _LaserActive;

    private void Update()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position += Vector3.left * this.speed * Time.deltaTime;

        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position += Vector3.right * this.speed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {

        if (!_LaserActive) {
            Projectile projectile = Instantiate(this.LaserPrefab, this.transform.position, Quaternion.identity);
            source.PlayOneShot(triggerSound);
            projectile.destroyed += laserDestroyed;
            _LaserActive = true;
        }
    }

    private void laserDestroyed() {
        _LaserActive = false;

    }

    private void OnTriggerEnter2D(Collider2D other) {     
        if (other.gameObject.layer == LayerMask.NameToLayer("Invader") || other.gameObject.layer == LayerMask.NameToLayer("Missile")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

}
