using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    public static CursorController instance;

    public Camera MainCamera;
    public Vector3 MousePosWorld;

    public Transform CursorArrow;
    public Color CursorColorValid;
    public Color CursorColorInvalid;
    public Material CursorMaterial;
    public float CursorCooldownTime;
    public float DrainSpeed = 0.3f;
    public float FillSpeed = 0.2f;

    public float CursorFuel;
    public float CursorFuelMax = 1;
    public AnimationCurve ColorBlendCurve;

    private float CursorAnimTime = 0;
    private bool IsMouseDown;
    private float CursorFuel01;
    private bool HasFuel = true;

    // Use this for initialization
    void Awake() {
        instance = this;
        CursorFuel = CursorFuelMax;
    }

    public void Restart() {
        CursorFuel = 1;
        CursorMaterial.SetColor("_Color", CursorColorValid);
    }
	
    // Update is called once per frame
    void Update() {
        // mouse input
        Plane plane = new Plane(Vector3.up, 0);
        float dist;
        Ray ray = MainCamera.ScreenPointToRay(Input.mousePosition);
        if (plane.Raycast(ray, out dist)) {
            MousePosWorld = ray.GetPoint(dist);
            transform.position = MousePosWorld;
        }

        CursorFuel01 = CursorFuel / CursorFuelMax;

        HasFuel = CursorFuel > 0;

        if (CursorFuel01 > 0 && CursorFuel01 < CursorFuelMax) {
            float e = ColorBlendCurve.Evaluate(CursorFuel01);
            CursorMaterial.SetColor("_Color", Color.Lerp(CursorColorInvalid, CursorColorValid, e));
        }

        if (Input.GetMouseButtonDown(0)) {
            OnMouseDown();
        }
        
        if (Input.GetMouseButtonUp(0)) {
            OnMouseUp();
        }

        if (IsMouseDown) {
            OnMouseDownExecute();
        } else {
            OnMouseUpExecute();
        }
    }

    private void OnMouseDown() {
        IsMouseDown = true;

        if (HasFuel) GameManager.Instance.StartFollow();

        // reset cursor animation
        CursorAnimTime = 0;
        Vector3 newPos = CursorArrow.position;
        newPos.y = 0;
        CursorArrow.position = newPos;
    }

    private void OnMouseUp() {
        IsMouseDown = false;
        GameManager.Instance.EndFollow();
    }

    private void OnMouseDownExecute() {
        DrainFuel();
    }

    private void DrainFuel() {
        // drain fuel
        if (CursorFuel01 > 0) {
            CursorFuel -= Time.deltaTime * DrainSpeed;

            if (CursorFuel <= 0) OnEmptiedOut();
        }
    }

    private void OnMouseUpExecute() {

        // fill fuel
        if (CursorFuel01 < CursorFuelMax) {
            CursorFuel += Time.deltaTime * FillSpeed;

            if (CursorFuel >= CursorFuelMax) OnFilledUp();
        }

        AnimateCursor();
    }

    private void OnFilledUp() {
        CursorFuel = CursorFuelMax;
        CursorFuel01 = 1;
        CursorMaterial.SetColor("_Color", CursorColorValid);
//        Debug.Log("OnFilledUp");
    }

    private void OnEmptiedOut() {
        CursorFuel = 0;
        CursorFuel01 = 0;
        CursorMaterial.SetColor("_Color", CursorColorInvalid);
        GameManager.Instance.EndFollow();
//        Debug.Log("OnEmptiedOut");
    }

    private void AnimateCursor() {
        float yOffset = 1;
        CursorAnimTime += Time.deltaTime;
        float y = Mathf.Sin(CursorAnimTime * 5) * 0.5f + 0.5f + yOffset;
        CursorArrow.localPosition = Vector3.up * y;
    }
}
