using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Movement
    public Animator animator;
    public float moveSpeed = 5f;
    private Vector3 moveInput;

    public Transform firePoint;
    public GameObject bulletPrefab;
    public int ammoNumber;

    private readonly int maxAmmoInPistolMag = 10;
    private int ammoInPistolMag;
    private readonly int maxAmmoInRifleMag = 25;
    private int ammoInRifleMag;
    private readonly int maxAmmoInShotgunMag = 21;
    private int ammoInShotgunMag;


    public Text ammoText;
    public SpriteRenderer shootEffect;
    private float currentShootEffectTime;
    public float shootEffectDuration;
    public AudioClip shootSound;
    public AudioClip deadSound;
    public AudioClip reloadSound;

    public float reloadTime = 1f;
    private float currentReloadTime = 0f;
    bool isReloading = false;
    public Text reloadText;

    public Text goldText;

    [HideInInspector]
    public int weaponSelected = 0;

    [HideInInspector]
    public bool HaveRifle = false;
    private bool RifleShooting = false;
    private readonly float rifleShootDecay = 0.1f;
    private float currentRifleTime = 0f;

    [HideInInspector]
    public bool HaveShotgun = false;

    [HideInInspector]
    public int goldNumber;

    public ParticleSystem bloodSplash;
    public GameObject uiShop;
    public Text weaponText;
    public Text howToPlayText;

    // Start is called before the first frame update
    void Start()
    {
        uiShop.SetActive(false);
        goldNumber = 0;
        ammoInPistolMag = maxAmmoInPistolMag;
        ammoInRifleMag = maxAmmoInRifleMag;
        ammoInShotgunMag = maxAmmoInShotgunMag;
        moveInput = new Vector3(0f, 0f, 0f);
        shootEffect.enabled = false;
        reloadText.enabled = false;
        weaponText.text = "Weapon selected: Pistol";
    }

    // Update is called once per frame
    void Update()
    {      
        switch(weaponSelected)
        {
            case 0:
                ammoText.text = "x " + ammoInPistolMag.ToString() + "/" + ammoNumber.ToString();
                break;
            case 1:
                ammoText.text = "x " + ammoInRifleMag.ToString() + "/" + ammoNumber.ToString();
                break;
            case 2:
                ammoText.text = "x " + ammoInShotgunMag.ToString() + "/" + ammoNumber.ToString();
                break;
        }
        
        goldText.text = "GOLD: " + goldNumber.ToString();
        PlayerInput();
        AnimationCheck();
        if(currentShootEffectTime >= shootEffectDuration)
        {
            shootEffect.enabled = false;
            currentShootEffectTime = 0;
        }
        else
        {
            currentShootEffectTime += 1 * Time.deltaTime;
        }

        if(RifleShooting == true)
        {
            currentRifleTime += 1 * Time.deltaTime;
            if(currentRifleTime > rifleShootDecay)
            {
                Shoot(ref ammoInRifleMag);
                currentRifleTime = 0f;
            }       
        }
        IfReloading();
    }

    private void FixedUpdate()
    {
        this.transform.position += new Vector3(moveInput.x * Time.fixedDeltaTime, moveInput.y * Time.fixedDeltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Zombie"))
        {
            FindObjectOfType<GameMaster>().GameOver();
            AudioSource.PlayClipAtPoint(deadSound, transform.position);
            reloadText.enabled = false;
            Destroy(gameObject);
            Instantiate(bloodSplash, transform.position, transform.rotation);
        }
    }

    void PlayerInput()
    {
        moveInput = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, Input.GetAxis("Vertical") * moveSpeed, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch(weaponSelected)
            {
                case 0:
                    Shoot(ref ammoInPistolMag);
                    break;
                case 1:
                    RifleShooting = true;
                    break;
                case 2:
                    Shoot(ref ammoInShotgunMag);
                    break;
                default:
                    break;
            }
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            RifleShooting = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(ammoNumber > 0 && isReloading == false)
            {
                if (weaponSelected == 0 && ammoInPistolMag < maxAmmoInPistolMag)
                {
                    AudioSource.PlayClipAtPoint(reloadSound, transform.position);
                    isReloading = true;
                }
                else if (weaponSelected == 1 && ammoInRifleMag < maxAmmoInRifleMag)
                {
                    AudioSource.PlayClipAtPoint(reloadSound, transform.position);
                    isReloading = true;
                }
                else if (weaponSelected == 2 && ammoInShotgunMag < maxAmmoInShotgunMag)
                {
                    AudioSource.PlayClipAtPoint(reloadSound, transform.position);
                    isReloading = true;
                }
            }            
        }
        if((ammoInPistolMag == 0 || ammoInRifleMag == 0 || ammoInShotgunMag == 0) && ammoNumber > 0 && isReloading == false)
        {
            AudioSource.PlayClipAtPoint(reloadSound, transform.position);
            isReloading = true;
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            if(uiShop.activeSelf == true)
            {
                uiShop.SetActive(false);
            }
            else
            {
                uiShop.SetActive(true);
            }    
        }
        if(isReloading == false)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                weaponSelected = 0;
                weaponText.text = "Weapon selected: Pistol";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && HaveRifle == true)
            {
                weaponSelected = 1;
                weaponText.text = "Weapon selected: Rifle";
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && HaveShotgun == true)
            {
                weaponSelected = 2;
                weaponText.text = "Weapon selected: Shotgun";
            }
        }    
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            if(howToPlayText.enabled == true)
            {
                howToPlayText.enabled = false;
            }
            else
            {
                howToPlayText.enabled = true;
            }
        }
    }

    private void AnimationCheck()
    {
        // Run animation check
        if(Mathf.Abs(moveInput.x) > 0.01 || Mathf.Abs(moveInput.y) > 0.01)
        {
            animator.SetBool("IsRunning", true);
        }
        else
        {
            animator.SetBool("IsRunning", false);
        }

        // Rotation check
        if(moveInput.x > 0)
        {
            this.transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if(moveInput.x < 0)
        {
            this.transform.rotation = new Quaternion(0, 180, 0, 0);
        }
    }

    private void Shoot(ref int ammo)
    { 
        if (ammo > 0 && isReloading == false && weaponSelected != 2)
        {
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            --ammo;
            shootEffect.enabled = true;
        }       
        if (ammo > 0 && isReloading == false && weaponSelected == 2)
        {
            AudioSource.PlayClipAtPoint(shootSound, transform.position);
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            firePoint.position -= new Vector3(0f, 0.1f, 0f);
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            firePoint.position += new Vector3(0, 0.2f, 0f);
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            firePoint.position -= new Vector3(0f, 0.1f, 0f);
            ammo -= 3;
            shootEffect.enabled = true;
        }
    }

    private void Reload(int maxAmmo, ref int ammo)
    {    
        int reloadedAmmo = maxAmmo - ammo;
        if (ammoNumber < reloadedAmmo)
        {
            reloadedAmmo = ammoNumber;
            ammoNumber = 0;
            ammo += reloadedAmmo;
        }
        else
        {
            ammoNumber -= reloadedAmmo;
            ammo += reloadedAmmo;
        }             
        reloadText.enabled = false;
        isReloading = false;
    }

    private void IfReloading()
    {
        if (isReloading == true)
        {
            reloadText.enabled = true;
            if (currentReloadTime < reloadTime / 3f)
            {
                reloadText.text = "Reloading.";
            }
            else if (currentReloadTime >= reloadTime / 3f && currentReloadTime <= reloadTime / 2f)
            {
                reloadText.text = "Reloading..";
            }
            else if (currentReloadTime > reloadTime / 2f && currentReloadTime <= reloadTime)
            {
                reloadText.text = "Reloading...";
            }
            else if (currentReloadTime >= reloadTime)
            {
                switch(weaponSelected)
                {
                    case 0:
                        Reload(maxAmmoInPistolMag, ref ammoInPistolMag);
                        break;
                    case 1:
                        Reload(maxAmmoInRifleMag, ref ammoInRifleMag);
                        break;
                    case 2:
                        Reload(maxAmmoInShotgunMag, ref ammoInShotgunMag);
                        break;
                    default:
                        break;
                }
                currentReloadTime = 0f;
            }
            currentReloadTime += 1 * Time.deltaTime;
        }
    }

    public void AddGold(int gold)
    {
        goldNumber += gold;
    }

    public int GetGold()
    {
        return goldNumber;
    }
}
