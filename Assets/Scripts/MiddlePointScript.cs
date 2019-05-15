using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Debug = System.Diagnostics.Debug;

public class MiddlePointScript : MonoBehaviour
{

    private MeshRenderer _meshRenderer;
    private int BlueUnitsCount = 0;
    private int RedUnitsCount = 0;

    private int time = 5;

    private SphereCollider cld;

    private List<Collider> collided;

    public TextMesh TimerText;
    public Material RedMaterial;
    public Material BlueMaterial;

    public static float CapturedPointCoefficient = 0.5f;
    public static bool IsCaptured = false;
    public static bool IsCapturing = false;

    public static string Owner = "Neutral";
    private string PrevOwner = "";

    private const int CapturingTime = 6;

    // Use this for initialization
    void Start()
    {
        
        TimerText.transform.rotation = Camera.main.transform.rotation;
        _meshRenderer = GetComponent<MeshRenderer>();
         
        cld = GetComponent<SphereCollider>();

        collided = new List<Collider>();

        TimerText.text = "";
    }


    private void Update()
    {
        BlueUnitsCount = 0;
        RedUnitsCount = 0;

        for (int i = 0; i < collided.Count; i++)
        {
            if (i >= collided.Count) continue;
            if (collided[i] == null ||
                Vector3.Distance(transform.position, collided[i].transform.position) > cld.radius + 1)
                collided.RemoveRange(i, 1);
            if (i >= collided.Count) continue;
            if (collided[i] != null)
            {
                if (collided[i].tag.Contains("BlueBody")) BlueUnitsCount++;
                if (collided[i].tag.Contains(("RedBody"))) RedUnitsCount++;
            }
        }

        if (IsCapturing && RedUnitsCount > 0 && BlueUnitsCount > 0)
        {
            Owner = PrevOwner;
            IsCapturing = false;
            TimerText.text = "";
            CancelInvoke();
        }

        if (BlueUnitsCount == 0 && RedUnitsCount > 0 && Owner != "Red" && !IsCapturing)
        {
            
            PrevOwner = Owner;
            time = CapturingTime;
            IsCapturing = true;
            Owner = "Red";
            IsCaptured = false;
            InvokeRepeating("UpdatePoint", 0, 1);
        }

        if (RedUnitsCount == 0 && BlueUnitsCount > 0 && Owner != "Blue" && !IsCapturing)
        {
            PrevOwner = Owner;
            time = CapturingTime;
            IsCapturing = true;
            Owner = "Blue";
            IsCaptured = false;
            InvokeRepeating("UpdatePoint", 0, 1);
        }

    }

    private void UpdatePoint()
    {
        time -= 1;
        TimerText.text = time.ToString();
        if (time == 0)
        {
            TimerText.text = "";

            IsCaptured = true;
            IsCapturing = false;

            if (Owner == "Red")
            {
                _meshRenderer.material = RedMaterial;
                GameInfo.RedPlayer.IncomeCoefficient += CapturedPointCoefficient;
                if (GameInfo.BluePlayer.IncomeCoefficient > 1.1f)
                    GameInfo.BluePlayer.IncomeCoefficient -= CapturedPointCoefficient;
            }
            else
            {
                _meshRenderer.material = BlueMaterial;
                GameInfo.BluePlayer.IncomeCoefficient += CapturedPointCoefficient;
                if (GameInfo.RedPlayer.IncomeCoefficient > 1.1f)
                    GameInfo.RedPlayer.IncomeCoefficient -= CapturedPointCoefficient;
            }

            CancelInvoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("BlueBody")) collided.Add(other);
        if (other.tag.Contains(("RedBody"))) collided.Add(other);
    }
}