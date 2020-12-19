using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    // OBJECT REFERENCES
    GameObject shootPoint;
    GameObject bulletPrefab;

    [SerializeField]
    public float DelayAfterShot=2f;

    public void Start()
    {
        shootPoint = transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject;
        bulletPrefab = Resources.Load("Prefab/Bullets/Rocket") as GameObject;
    }
    public void Shoot()
    {
        GameObject.Find("GameUI").transform.Find("Panel").gameObject.SetActive(true);
        GameObject.Find("Timer").GetComponent<TurnTimer>().DeactivateTimer();
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
        bullet.GetComponent<Bullet>().playerWhoShot = this.gameObject;
        Invoke("DelayedEndTurn", DelayAfterShot);
    }
    private void DelayedEndTurn()
    {
        GameObject.Find("UI").gameObject.GetComponent<GameController>().EndTurn();
        GameObject.Find("GameUI").transform.Find("Panel").gameObject.SetActive(false);
    }
    
}
