using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speedX = 5.0f;
    public float speedZ = 5.0f;
    public float rotateAngle = 3.0f;
    //public bool testInputEnabled = false;
    public const int snowMax = 31;
    public const int giftsMax = 9;
    public const int lightsMax = 3;
    //public float treeApplyDistance = 5.0f;
    //public float promptRenderDistance = 2.0f;
    public float interactDist = 2.5f;

    private Rigidbody rb;
    private Ingredients ingredients;
    private TextMeshProUGUI snowCountText;
    private TextMeshProUGUI giftsCountText;
    private TextMeshProUGUI lightsCountText;
    //private GameObject treeObject;
    private TreeController treeController;
    private MeshRenderer[] interactRenderers;

    //private Dictionary<int, GameObject> interactTargets;
    private GameObject interactTarget = null;
    //private GameObject interactTarget = null;

    // Start is called before the first frame update
    void Start()
    {
        //treeObject = GameObject.Find("tree");
        treeController = GameObject.Find("tree").GetComponent<TreeController>();
        rb = GetComponent<Rigidbody>();

        ingredients = new Ingredients(
            snowMax, 
            giftsMax, 
            lightsMax);
       
        snowCountText = GameObject.Find("SnowCountText").GetComponent<TextMeshProUGUI>();
        giftsCountText = GameObject.Find("GiftsCountText").GetComponent<TextMeshProUGUI>();
        lightsCountText = GameObject.Find("LightsCountText").GetComponent<TextMeshProUGUI>();
        UpdateUI();

        GameObject[] interacts = GameObject.FindGameObjectsWithTag("interact");
        interactRenderers = new MeshRenderer[interacts.Length];
        for (int i = 0; i < interacts.Length; ++i)
        {
            interactRenderers[i] = interacts[i].GetComponent<MeshRenderer>();
            interactRenderers[i].enabled = false;
        }

        //interactTargets = new Dictionary<int, GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //foreach (var interact in interactRenderers)
        //{
        //    float dist = (interact.transform.position - transform.position).magnitude;

        //    if (dist <= promptRenderDistance)
        //    {
        //        if (!interact.enabled)
        //        {
        //            interact.enabled = true;
        //            //interactTargets.Add(interact.GetInstanceID(), interact.gameObject);
        //            interactTarget = interact.gameObject;
        //        }
        //    }
        //    else
        //    {
        //        if (interact.enabled)
        //        {
        //            interact.enabled = false;
        //            //interactTargets.Remove(interact.GetInstanceID());
        //            interactTarget = null;
        //        }
        //    }
        //}

        if (interactTarget != null)
        {
            float dist = (interactTarget.transform.position - transform.position).magnitude;
            if (dist > interactDist)
            {
                interactTarget = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (interactTarget != null)
            {
                interactTarget.SendMessage("OnInteract");
                //Ingredients.Type ing = GetNextAvailableIngredient();
                ////GameObject target = interactTargets.First().Value;
                //if (ing != Ingredients.Type.None)
                //{
                //    interactTarget.SendMessageUpwards("AddIngredient", ing);
                //    //treeController.AddIngredient(ing);
                //    //RemoveIngredient(ing);
                //}
            }
        }

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

        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    RemoveIngredient(Ingredients.Type.Snow);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    RemoveIngredient(Ingredients.Type.Gifts);
        //}

        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    RemoveIngredient(Ingredients.Type.Lights);
        //}
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rb.rotation *= Quaternion.AngleAxis(rotateAngle * -1.0f, Vector3.up);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.rotation *= Quaternion.AngleAxis(rotateAngle, Vector3.up);
        }

        Vector3 velocity = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            velocity = transform.forward * speedZ;
        }

        if (Input.GetKey(KeyCode.S))
        {
            velocity = transform.forward * speedZ * -1.0f;
        }

        rb.velocity = velocity;
    }

    public bool HasIngredient(Ingredients.Type ing)
    {
        switch (ing)
        {
            case Ingredients.Type.Snow:
                return ingredients.snowCount > 0;
            case Ingredients.Type.Gifts:
                return ingredients.giftsCount > 0;
            case Ingredients.Type.Lights:
                return ingredients.lightsCount > 0;
            default:
                return false;
        }
    }

    public void SetInteractTarget(GameObject obj)
    {
        interactTarget = obj;
    }

    public bool CanAddIngredient(Ingredients.Type ing)
    {
        return ingredients.CanAddIngredient(ing);
    }

    public bool AddIngredient(Ingredients.Type ing)
    {
        if (ingredients.AddIngredient(ing))
        {
            UpdateUI();
            return true;
        }
        return false;
    }

    public bool RemoveIngredient(Ingredients.Type ing)
    {
        if (ingredients.RemoveIngredient(ing))
        {
            UpdateUI();
            return true;
        }
        return false;
    }

    private void UpdateUI()
    {
        snowCountText.text = $"{ingredients.snowCount}";
        giftsCountText.text = $"{ingredients.giftsCount}";
        lightsCountText.text = $"{ingredients.lightsCount}";
    }

    private Ingredients.Type GetNextAvailableIngredient()
    {
        if (ingredients.snowCount > 0)
        {
            return Ingredients.Type.Snow;
        }

        if (ingredients.giftsCount > 0)
        {
            return Ingredients.Type.Gifts;
        }

        if (ingredients.lightsCount > 0)
        {
            return Ingredients.Type.Lights;
        }

        return Ingredients.Type.None;
    }
}
