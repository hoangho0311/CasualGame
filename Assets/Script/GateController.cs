using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GateType { multiplyType, additionType};
public class GateController : MonoBehaviour
{
    public int gateValue;
    public TMPro.TextMeshProUGUI gateText;
    public GateType gateType;
    GatesHolderController gatesHolderController;

    bool hasGateUsed;
    PlayerController PlayerController;
    GameObject playerSpawnGO;
    // Start is called before the first frame update
    void Start()
    {
        AddGateValueAndSymbol();
    }

    private void Awake()
    {
        playerSpawnGO = GameObject.FindGameObjectWithTag("PlayerSpawner");
        PlayerController = playerSpawnGO.GetComponent<PlayerController>();
        gatesHolderController = transform.parent.gameObject.GetComponent<GatesHolderController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player" && hasGateUsed==false)
        {
            hasGateUsed = true;
            PlayerController.SpawnPlayer(gateValue, gateType) ;
            gatesHolderController.CloseGates();
            Destroy(gameObject);
        }
    }

    void AddGateValueAndSymbol()
    {
        switch (gateType)
        {
            case GateType.multiplyType:
                gateText.text = "x" + gateValue.ToString();

                break;
            case GateType.additionType:
                gateText.text = "+" + gateValue.ToString();

                break;
            default:
                break;
        }
    }
}
