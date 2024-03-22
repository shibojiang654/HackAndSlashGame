using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    #region GameObject_variables
    [SerializeField]
    [Tooltip("health pack")]
    private GameObject healthpack;
    #endregion

    private void Awake() {
        transform.position = new Vector2(Random.Range(-24.0f, 8.0f), Random.Range(-12.0f, 5.0f));
    }

    #region Chest_functions
    IEnumerator DestroyChest() {
        yield return new WaitForSeconds(.3f);
        Instantiate(healthpack, transform.position, transform.rotation);
        Destroy(this.gameObject);
    }

    public void Interact() {
        StartCoroutine("DestroyChest");
    }
    #endregion
}
