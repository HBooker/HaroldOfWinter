using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    public Mesh treeMesh;
    public Mesh treeSnowMesh;
    public Mesh treeLightsMesh;
    public Mesh treeSnowLightsMesh;
    // public bool testInputEnabled = false;
    public const int snowMax = 10;
    public const int giftsMax = 6;
    public const int lightsMax = 3;
    public float interactDist = 4.0f;
    private bool canInteract = false;

    private GameObject[] gifts;
    private Ingredients ingredients;
    private MeshRenderer interactPromptRenderer;
    private GameObject playerObj;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        ingredients = new Ingredients(
            snowMax, 
            giftsMax, 
            lightsMax);

        gifts = GameObject.FindGameObjectsWithTag("gift");
        for (int i = 0; i < ingredients.giftsMax; ++i)
        {
            SetGiftRendering(gifts[i], false);
        }

        interactPromptRenderer = transform.Find("InteractPrompt").GetComponent<MeshRenderer>();
        interactPromptRenderer.enabled = false;

        playerObj = GameObject.FindWithTag("player");
        playerController = playerObj.GetComponent<PlayerController>();
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

    public bool CanAddIngredient(Ingredients.Type ing)
    {
        return ingredients.CanAddIngredient(ing);
    }

    public bool AddIngredient(Ingredients.Type ing)
    {
        if (ingredients.AddIngredient(ing))
        {
            ApplyMeshStates();
            return true;
        }
        return false;
    }

    public bool RemoveIngredient(Ingredients.Type ing)
    {
        if (ingredients.RemoveIngredient(ing))
        {
            ApplyMeshStates();
            //playerObj.SendMessage("RemoveIngredient", ing);
            return true;
        }
        return false;
    }

    private void ApplyMeshStates()
    {
        bool snowed = ingredients.snowCount >= ingredients.snowMax;
        bool lighted = ingredients.lightsCount >= ingredients.lightsMax;

        if (snowed && lighted)
        {
            GetComponent<MeshFilter>().sharedMesh = treeSnowLightsMesh;
        }
        else if (snowed)
        {
            GetComponent<MeshFilter>().sharedMesh = treeSnowMesh;
        }
        else if (lighted)
        {
            GetComponent<MeshFilter>().sharedMesh = treeLightsMesh;
        }
        else
        {
            GetComponent<MeshFilter>().sharedMesh = treeMesh;
        }

        for (int i = 0; i < ingredients.giftsMax; ++i)
        {
            SetGiftRendering(gifts[i], i < ingredients.giftsCount);
        }
    }

    private void SetGiftRendering(GameObject gift, bool enabled)
    {
        MeshRenderer[] renderers = gift.GetComponentsInChildren<MeshRenderer>();
        foreach(MeshRenderer r in renderers)
        {
            r.enabled = enabled;
        }
    }

    public void OnHeartbeat()
    {
        canInteract = false;

        if (ingredients.snowCount < ingredients.snowMax 
            && playerController.HasIngredient(Ingredients.Type.Snow))
        {
            canInteract = true;
        }
        else if (ingredients.giftsCount < ingredients.giftsMax
            && playerController.HasIngredient(Ingredients.Type.Gifts))
        {
            canInteract = true;
        }
        else if (ingredients.lightsCount < ingredients.lightsMax
            && playerController.HasIngredient(Ingredients.Type.Lights))
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
        if (ingredients.lightsCount < ingredients.lightsMax 
            && playerController.RemoveIngredient(Ingredients.Type.Lights))
        {
            if (!AddIngredient(Ingredients.Type.Lights))
            {
                Debug.LogError("Failed to add Lights ingredient to tree");
            }
        }
        else if (ingredients.giftsCount < ingredients.giftsMax 
            && playerController.RemoveIngredient(Ingredients.Type.Gifts))
        {
            if (!AddIngredient(Ingredients.Type.Gifts))
            {
                Debug.LogError("Failed to add Gifts ingredient to tree");
            }
        }
        else if (ingredients.snowCount < ingredients.snowMax
            && playerController.RemoveIngredient(Ingredients.Type.Snow))
        {
            if (!AddIngredient(Ingredients.Type.Snow))
            {
                Debug.LogError("Failed to add Snow ingredient to tree");
            }
        }
    }
}
