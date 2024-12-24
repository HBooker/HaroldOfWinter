using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSeller : MonoBehaviour
{
    // public bool testInputEnabled = false;
    public Mesh baseMesh;
    public Mesh updateMesh;
    public int snowMax = 0;
    public int giftsMax = 0;
    public int lightsMax = 0;
    public float interactDist = 4.0f;

    private Ingredients ingredients;
    private MeshRenderer interactPromptRenderer;
    private GameObject playerObj;
    private PlayerController playerController;
    private bool canInteract = false;

    // Start is called before the first frame update
    void Start()
    {
        ingredients = new Ingredients(
            snowMax,
            giftsMax,
            lightsMax);

        LoadIngredients();

        interactPromptRenderer = transform.Find("InteractPrompt").GetComponent<MeshRenderer>();
        interactPromptRenderer.enabled = false;

        playerObj = GameObject.FindWithTag("player");
        playerController = playerObj.GetComponent<PlayerController>();
    }

    private void LoadIngredients()
    {
        for (int i = 0; i < snowMax; ++i)
        {
            ingredients.AddIngredient(Ingredients.Type.Snow);
        }

        for (int i = 0; i < giftsMax; ++i)
        {
            ingredients.AddIngredient(Ingredients.Type.Gifts);
        }

        for (int i = 0; i < lightsMax; ++i)
        {
            ingredients.AddIngredient(Ingredients.Type.Lights);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!testInputEnabled)
        //    return;

        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    AddIngredient(Ingredients.Type.Snow);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    AddIngredient(Ingredients.Type.Gifts);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    AddIngredient(Ingredients.Type.Lights);
        //}
    }

    public bool AddIngredient(Ingredients.Type ing)
    {
        if (ingredients.AddIngredient(ing))
        {
            //ApplyMeshStates();
            return true;
        }
        return false;
    }

    public bool RemoveIngredient(Ingredients.Type ing)
    {
        if (ingredients.RemoveIngredient(ing))
        {
            //ApplyMeshStates();
            //playerObj.SendMessage("RemoveIngredient", ing);
            return true;
        }
        return false;
    }

    //private void ApplyMeshStates()
    //{
    //    bool updated = ;

    //    if (snowed)
    //    {
    //        GetComponent<MeshFilter>().sharedMesh = treeSnowMesh;
    //    }
    //    else
    //    {
    //        GetComponent<MeshFilter>().sharedMesh = treeMesh;
    //    }
    //}

    public void OnHeartbeat()
    {
        canInteract = false;

        if (ingredients.snowCount > 0)
        {
            canInteract = true;
        }
        else if (ingredients.giftsCount > 0
            && playerController.HasIngredient(Ingredients.Type.Snow))
        {
            canInteract = true;
        }
        else if (ingredients.lightsCount > 0
            && playerController.HasIngredient(Ingredients.Type.Gifts))
        {
            canInteract = true;
        }

        float dist = (playerObj.transform.position - transform.position).magnitude;
        if (canInteract && dist <= interactDist)
        {
            interactPromptRenderer.enabled = true;
            playerController.SetInteractTarget(gameObject);
        }
        else
        {
            interactPromptRenderer.enabled = false;
            //playerController.SetInteractTarget(null);
            // could cause problems if multiple interactable objects are within
            // interact distance of the player at one time, but that will not
            // happen in this small prototype
        }
    }

    public void OnInteract()
    {
        if (!canInteract)
        {
            return;
        }
        //Debug.Log($"SellerOnInteract\nsnow: {}\ngifts: {}\nlights: {}");

        if (ingredients.lightsCount > 0
            && playerController.RemoveIngredient(Ingredients.Type.Gifts))
        {
            if (!playerController.AddIngredient(Ingredients.Type.Lights))
            {
                Debug.LogError("Failed to add Lights ingredient to player");
            }

            if (!RemoveIngredient(Ingredients.Type.Lights))
            {
                Debug.LogError("Failed to remove Lights ingredient from seller");
            }

            foreach (MeshFilter mesh in GetComponentsInChildren<MeshFilter>())
            {
                mesh.sharedMesh = updateMesh;
            }
        }
        else if (ingredients.giftsCount > 0
            && playerController.RemoveIngredient(Ingredients.Type.Snow))
        {
            if (!playerController.AddIngredient(Ingredients.Type.Gifts))
            {
                Debug.LogError("Failed to add Gifts ingredient to player");
            }

            if (!RemoveIngredient(Ingredients.Type.Gifts))
            {
                Debug.LogError("Failed to remove Gifts ingredient from seller");
            }

            GetComponent<MeshFilter>().sharedMesh = updateMesh;
        }
        else if (ingredients.snowCount > 0
            && playerController.AddIngredient(Ingredients.Type.Snow))
        {
            if (!RemoveIngredient(Ingredients.Type.Snow))
            {
                Debug.LogError("Failed to remove Snow ingredient from seller");
            }

            GetComponent<MeshFilter>().sharedMesh = updateMesh;
        }
        else
        {
            //GetComponent<MeshFilter>().sharedMesh = baseMesh;
            return;
        }

        canInteract = false;
    }
}
